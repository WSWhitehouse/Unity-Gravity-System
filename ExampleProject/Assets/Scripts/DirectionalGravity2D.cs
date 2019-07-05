using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Gravity System/Gravity Sources/Directional Gravity 2D")]
public class DirectionalGravity2D : MonoBehaviour, IGravitySource2D
{
    [SerializeField, Tooltip("The direction of gravity.")]
    private Vector2 gravityDirection = Vector2.down;

    [SerializeField,
     Tooltip("How much gravity force to apply to objects within range")]
    private float gravityStrength = 9.8f;

    [SerializeField, Space(5), Tooltip("Enable Debug rays and lines to help visualise the gravity.")]
    private bool enableDebug;

    public float GravityStrength => gravityStrength;

    public Collider2D[] GravityColliders { get; private set; }

    public List<GravityItem2D> ItemsInRange { get; } = new List<GravityItem2D>();

    private const float MaxRaycastDistance = 100.0f;

    private void Awake()
    {
        GravityColliders = GetComponents<Collider2D>();

        if (GravityColliders == null || GravityColliders.Length == 0)
        {
            Debug.LogWarning("GravitySource has no colliders, will not be functional.");
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        var item = c.GetComponent<GravityItem2D>();
        if (item == null || ItemsInRange.Contains(item)) return;

        ItemsInRange.Add(item);

        ++item.ActiveFieldCount;
        item.CurrentGravitySource.Add(this);
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        var item = c.GetComponent<GravityItem2D>();
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
        // Iterate over each object within range of our gravity
        for (int i = 0; ItemsInRange != null && i < ItemsInRange.Count; ++i)
        {
            if (ItemsInRange[i] == null || ItemsInRange[i].Rigidbody2D.gravityScale <= 0)
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

                RaycastHit2D[] raycastHit = new RaycastHit2D[0];
                var raycastInt = gravityCollider.Raycast(toCollider, raycastHit, MaxRaycastDistance);
                if (raycastInt > 0)
                {
                    if (enableDebug)
                    {
                        Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.red);
                        Debug.DrawRay(raycastHit[0].point, raycastHit[0].normal * 2, Color.red);
                        gravityRay = new Ray(item.transform.position, -raycastHit[0].normal);
                    }

                    // Set our new ray to point in the opposite direction of this normal, to raycast 'down' towards the closest point on the plane formed by the normal

                    // Update gravity direction guess if this was a closer hit
                    var dist = Vector2.Distance(raycastHit[0].point, gravityRay.origin);
                    if (dist < closestHit)
                    {
                        gravityDir = -raycastHit[0].normal;
                        closestHit = dist;
                    }
                }

                // Raycast a second time onto the collider with the refined 'down' direction
                if (raycastInt > 0)
                {
                    raycastInt = gravityCollider.Raycast(raycastHit[0].normal, raycastHit, MaxRaycastDistance);

                    if (raycastInt > 0)
                    {
                        if (enableDebug)
                        {
                            Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.green);
                            Debug.DrawRay(raycastHit[0].point, raycastHit[0].normal * 2, Color.green);
                        }

                        var dist = Vector2.Distance(raycastHit[0].point, gravityRay.origin);
                        if (dist < closestHit)
                        {
                            gravityDir = -raycastHit[0].normal;
                            closestHit = dist;
                        }
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

                item.Up = Vector2.Lerp(item.Up, -gravityDir.normalized, Time.deltaTime * 2.0f);

                // Calculate force
                var force = gravityDir.normalized * GravityStrength;
                var distRatio =
                    Mathf.Clamp01(closestHit / Vector2.Distance(transform.position, item.transform.position));

                // Gravity gets scaled up with distance because games
                force *= 1.0f + distRatio;
                item.Rigidbody2D.AddForce(force * item.Rigidbody2D.mass);
            }
        }
    }
}