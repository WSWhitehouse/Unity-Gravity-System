using System.Collections.Generic;
using UnityEngine;

namespace DoctorWolfy121.GravitySystem
{
    [AddComponentMenu("Gravity System/Gravity Sources/Directional Gravity")]
    public class DirectionalGravity : MonoBehaviour, IGravitySource
    {
        [SerializeField, Tooltip("The direction of gravity.")]
        private Vector3 gravityDirection = Vector3.down;

        [SerializeField] private float gravityStrength = 9.8f;

        [SerializeField] private bool enableGravity = true;

        [SerializeField, Space(5)] private bool enableDebug;

        public float GravityStrength => gravityStrength;

        public List<GravityItem> ItemsInRange { get; } = new List<GravityItem>();

        public Collider[] GravityColliders { get; private set; }

        public bool EnableGravity => enableGravity;

        private const float MaxRaycastDistance = 100.0f;

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
                var gravityDir = gravityDirection;

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

                    var distRatio =
                        Mathf.Clamp01(closestHit / Vector3.Distance(transform.position, item.transform.position));

                    // Gravity gets scaled up with distance because games
                    force *= 1.0f + distRatio;
                    item.Rigidbody.AddForce(force * item.Rigidbody.mass);
                }
            }
        }
    }
}