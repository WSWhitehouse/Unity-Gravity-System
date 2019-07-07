using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace DoctorWolfy121.GravitySystem
{
    [AddComponentMenu("Gravity System/Gravity Sources/Planet Gravity")]
    public class PlanetGravity : MonoBehaviour, IGravitySource
    {
        [FormerlySerializedAs("gravity")]
        [SerializeField, Tooltip("How much gravity force to apply to objects within range")]
        private float gravityStrength = 9.8f;

        [SerializeField] private bool enableGravity = true;

        public float radius = 5.0f;

        [SerializeField, Space(5), Tooltip("Enable Debug rays and lines to help visualise the gravity.")]
        private bool enableDebug;

        private const float MaxRaycastDistance = 100.0f;

        public float GravityStrength => gravityStrength;

        public List<GravityItem> ItemsInRange { get; } = new List<GravityItem>();

        public Collider[] GravityColliders { get; private set; }

        public bool EnableGravity => enableGravity;

        private void OnDrawGizmos()
        {
            if (!enableDebug) return;

            // Visualize gravity radius 
            Gizmos.color = Color.magenta;
            for (int i = 0; GravityColliders != null && i < GravityColliders.Length; ++i)
            {
                var col = GravityColliders[i];
                DrawLine(col, transform.up);
                DrawLine(col, transform.right);
                DrawLine(col, transform.forward);
            }
        }

        private void DrawLine(Collider collider, Vector3 dir)
        {
            var raycastFrom = collider.transform.position + dir * 1000.0f;
            var raycastDir = (collider.transform.position - raycastFrom).normalized;
            var ray = new Ray(raycastFrom, raycastDir);
            if (collider.Raycast(ray, out var hitInfo, 2000.0f))
            {
                Gizmos.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal * (-radius * 2));
            }
        }

        private void Awake()
        {
            GravityColliders = GetComponents<Collider>();

            if (GravityColliders == null || GravityColliders.Length == 0)
            {
                Debug.LogWarning("GravitySource has no colliders, will not be functional.");
            }
        }

        private void OnTriggerStay(Collider c)
        {
            var item = c.GetComponent<GravityItem>();
            if (item == null || ItemsInRange.Contains(item)) return;

            ItemsInRange.Add(item);

            ++item.ActiveFieldCount;
            item.CurrentGravitySource.Add(this);
        }

        private void OnTriggerExit(Collider c)
        {
            var item = c.GetComponent<GravityItem>();
            if (item == null || !ItemsInRange.Contains(item)) return;

            ItemsInRange.Remove(item);

            --item.ActiveFieldCount;
            if (item.CurrentGravitySource.Contains(this))
            {
                item.CurrentDistance = Mathf.Infinity;
                item.CurrentGravitySource.Remove(this);
            }
        }

        private void FixedUpdate()
        {
            if (!EnableGravity) return;
            
            // Iterate over each object within range of our gravity
            for (int i = 0; ItemsInRange != null && i < ItemsInRange.Count; ++i)
            {
                if (ItemsInRange[i] == null || !ItemsInRange[i].Rigidbody.useGravity)
                    continue;

                // Calculate initial gravity direction, just towards the gravity source transform
                var item = ItemsInRange[i];
                var gravityDir = (transform.position - item.transform.position).normalized;

                // Find out which of our child colliders is closest
                var closestHit = Mathf.Infinity;
                foreach (var gravityCollider in GravityColliders)
                {
                    // Skips this collider if it isn't a trigger
                    if (!gravityCollider.isTrigger) continue;

                    // Raycast in general direction of collider to find a normal of the surface
                    var raycastTo = gravityCollider.transform.position;
                    var toCollider = (raycastTo - item.transform.position).normalized;
                    var gravityRay = new Ray(item.transform.position, toCollider);
                    if (gravityCollider.Raycast(gravityRay, out var hitInfo, MaxRaycastDistance))
                    {
                        if (enableDebug)
                        {
                            Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.red);
                            Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.red);
                        }

                        // Set our new ray to point in the opposite direction of this normal, to raycast 'down' towards the closest point on the plane formed by the normal
                        gravityRay = new Ray(item.transform.position, -hitInfo.normal);

                        // Update gravity direction guess if this was a closer hit
                        var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
                        if (dist < closestHit)
                        {
                            gravityDir = -hitInfo.normal;
                            closestHit = dist;
                        }
                    }

                    // Raycast a second time onto the collider with the refined 'down' direction
                    if (gravityCollider.Raycast(gravityRay, out hitInfo, MaxRaycastDistance))
                    {
                        if (enableDebug)
                        {
                            Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.green);
                            Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.green);
                        }

                        var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
                        if (dist < closestHit)
                        {
                            gravityDir = -hitInfo.normal;
                            closestHit = dist;
                        }
                    }
                }

                if (enableDebug)
                {
                    Debug.DrawRay(item.transform.position, gravityDir * 2, Color.blue);
                }

                // Now apply gravity if we are the closest source (only 1 source at a time applies gravity)
                if (item.CurrentGravitySource.Contains(this) || closestHit < item.CurrentDistance)
                {
                    // Update tracking vars 
                    item.CurrentDistance = closestHit;
                    if (!item.CurrentGravitySource.Contains(this))
                    {
                        item.CurrentGravitySource.Add(this);
                    }

                    item.Up = Vector3.Lerp(item.Up, -gravityDir.normalized, Time.deltaTime * 2.0f);

                    if (item.RotateToGround)
                    {
                        item.transform.up = -gravityDir.normalized;
                    }

                    // Calculate force
                    var force = gravityDir.normalized * GravityStrength;
                    var distRatio = Mathf.Clamp01(closestHit / radius);

                    // Gravity gets scaled up with distance because games
                    force *= 1.0f + distRatio;
                    item.Rigidbody.AddForce(force * item.Rigidbody.mass);
                }
            }
        }

        /*private void Update()
        {
            foreach (var item in ItemsInRange)
            {
                if (item.RotateToGround)
                {
                    item.transform.rotation = Quaternion.Euler(-item.Up);
                }
            }
        }*/
    }
}