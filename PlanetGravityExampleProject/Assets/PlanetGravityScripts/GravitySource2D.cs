using UnityEngine;
using System.Collections.Generic;

public class GravitySource2D : MonoBehaviour
{
    [Tooltip("How much gravity force to apply to objects within range")]
    public float gravity = 9.8f;

    [Tooltip("The maximum distance from the surface of the gravity source that is still affected by gravity")]
    public float radius = 5.0f;

    [SerializeField, Space(5), Tooltip("Enable Debug rays and lines to help visualise the gravity.")]
    private bool enableDebug;

    private Collider2D[] _gravityColliders;

    private const float MaxRaycastDistance = 100.0f;

    private List<Rigidbody2D> _objectsInRange = new List<Rigidbody2D>();

    private void OnDrawGizmos()
    {
        if (!enableDebug) return;

        if (Camera.current == null)
            return;

        // Visualize gravity radius 
        Gizmos.color = Color.magenta;
        for (int i = 0; _gravityColliders != null && i < _gravityColliders.Length; ++i)
        {
            var col = _gravityColliders[i];
            DrawLine(col, transform.up);
            DrawLine(col, transform.right);
        }
    }

    private void DrawLine(Collider2D collider, Vector3 dir)
    {
        var raycastFrom = collider.transform.position + dir * 1000.0f;
        //Vector2 raycastFrom2D = raycastFrom;
        var raycastDir = (collider.transform.position - raycastFrom).normalized;
        //var ray = new Ray(raycastFrom, raycastDir);
        //RaycastHit2D[] hitInfo = new RaycastHit2D[0];
        //collider.Raycast(raycastFrom2D, hitInfo, 2000.0f);

        Gizmos.DrawLine(raycastFrom, raycastDir * (-radius * 2));
    }

    private void Awake()
    {
        _gravityColliders = GetComponents<Collider2D>();

        if (_gravityColliders == null || _gravityColliders.Length == 0)
        {
            Debug.LogWarning("GravitySource has no colliders, will not be functional.");
        }

        SetItemsInRadius();
    }

    private void SetItemsInRadius()
    {
        var overlapColliders = Physics2D.OverlapCircleAll(transform.position, radius);


        foreach (var c in overlapColliders)
        {
            var component = c.GetComponent<GravityItem2D>();
            if (component != null)
            {
                OnTriggerEnter2D(c);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        var rb = c.GetComponent<Rigidbody2D>();
        if (rb == null || _objectsInRange.Contains(rb)) return;

        _objectsInRange.Add(rb);

        var item = rb.GetComponent<GravityItem2D>() ?? rb.gameObject.AddComponent<GravityItem2D>();
        ++item.ActiveFieldCount;
        item.CurrentGravitySource = this;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        var rb = c.GetComponent<Rigidbody2D>();
        if (rb == null || !_objectsInRange.Contains(rb)) return;

        _objectsInRange.Remove(rb);

        var item = rb.GetComponent<GravityItem2D>() ?? rb.gameObject.AddComponent<GravityItem2D>();
        --item.ActiveFieldCount;
        item.CurrentDistance = Mathf.Infinity;
        item.CurrentGravitySource = null;
    }

    private void FixedUpdate()
    {
        // Iterate over each object within range of our gravity
        for (int i = 0; _objectsInRange != null && i < _objectsInRange.Count; ++i)
        {
            if (_objectsInRange[i] == null || _objectsInRange[i].gravityScale <= 0)
                continue;

            // Calculate initial gravity direction, just towards the gravity source transform
            var rb = _objectsInRange[i];
            var gravityDir = (transform.position - rb.transform.position).normalized;

            // Find out which of our child colliders is closest
            var closestHit = Mathf.Infinity;
            foreach (var gravityCollider in _gravityColliders)
            {
                // Skips this collider if it isn't a trigger
                if (!gravityCollider.isTrigger) continue;

                // Raycast in general direction of collider to find a normal of the surface
                var raycastTo = gravityCollider.transform.position;
                var toCollider = (raycastTo - rb.transform.position).normalized;
                var gravityRay = new Ray(rb.transform.position, toCollider);

                RaycastHit2D[] raycastHit = new RaycastHit2D[0];
                var raycastInt = gravityCollider.Raycast(toCollider, raycastHit, MaxRaycastDistance);
                if (raycastInt > 0) //Physics.Raycast(gravityRay, out var hitInfo, MaxRaycastDistance))
                {
                    if (enableDebug)
                    {
                        Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.red);
                        Debug.DrawRay(raycastHit[0].point, raycastHit[0].normal * 2, Color.red);
                        gravityRay = new Ray(rb.transform.position, -raycastHit[0].normal);
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

                    if (raycastInt > 0) //Physics.Raycast(gravityRay, out hitInfo, MaxRaycastDistance))
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
                Debug.DrawRay(rb.transform.position, gravityDir * 2, Color.blue);
            }

            // Now apply gravity if we are the closest source (only 1 source at a time applies gravity)
            var item = rb.GetComponent<GravityItem2D>();
            if (item.CurrentGravitySource == this || closestHit < item.CurrentDistance)
            {
                // Update tracking vars 
                item.CurrentDistance = closestHit;
                item.CurrentGravitySource = this;
                item.Up = Vector2.Lerp(item.Up, -gravityDir.normalized, Time.deltaTime * 2.0f);

                // Calculate force
                var force = gravityDir.normalized * gravity;
                var distRatio = Mathf.Clamp01(closestHit / radius);

                // Gravity gets scaled up with distance because games
                force *= 1.0f + distRatio;
                rb.AddForce(force * rb.mass);
            }
        }
    }
}