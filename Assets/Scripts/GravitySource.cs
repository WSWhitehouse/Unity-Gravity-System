using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
  [Tooltip("How much gravity force to apply to objects within range")]
  public float Gravity = 9.8f;

  [Tooltip("The maximum distance from the surface of the gravity source that is still affected by gravity")]
  public float Radius = 5.0f;

  [Tooltip("List of colliders to use as gravity sources, will be raycasted against")]
  [SerializeField]
  private Collider[] gravityColliders;

  // How far should raycasts go, make this the maximum distance you need gravity to affect objects from
  private const float kRaycastDistance = 100.0f;

  private List<Rigidbody> objectsInRange = new List<Rigidbody>();

  private void OnDrawGizmos()
  {
    if (Camera.current == null)
      return;

    // Visualize gravity radius 
    Gizmos.color = Color.blue;
    for (var i = 0; gravityColliders != null && i < gravityColliders.Length; ++i)
    {
      var col = gravityColliders[i];
      var raycastFrom = col.transform.position + Camera.current.transform.up * 1000.0f;
      var raycastDir = (col.transform.position - raycastFrom).normalized;
      var ray = new Ray(raycastFrom, raycastDir);
      RaycastHit hitInfo;
      if (col.Raycast(ray, out hitInfo, 2000.0f))
      {
        Gizmos.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal * Radius);
      }
    }
  }

  private void Start()
  {
    if (gravityColliders == null || gravityColliders.Length == 0)
    {
      Debug.LogWarning("GravitySource has no colliders, will not be functional");
    }
  }

  private void OnTriggerStay(Collider c)
  {
    var rb = c.GetComponent<Rigidbody>();
    if (rb != null && !objectsInRange.Contains(rb))
    {
      objectsInRange.Add(rb);

      var item = rb.GetComponent<GravityItem>() ?? rb.gameObject.AddComponent<GravityItem>();
      ++item.ActiveFieldCount;
    }
  }

  private void OnTriggerExit(Collider c)
  {
    var rb = c.GetComponent<Rigidbody>();
    if (rb != null && objectsInRange.Contains(rb))
    {
      objectsInRange.Remove(rb);

      var item = rb.GetComponent<GravityItem>() ?? rb.gameObject.AddComponent<GravityItem>();
      --item.ActiveFieldCount;
      item.CurrentDistance = Mathf.Infinity;
      item.CurrentGravitySource = null;
    }
  }

  private void FixedUpdate()
  {
    // Iterate over each object within range of our gravity
    for (var i = 0; objectsInRange != null && i < objectsInRange.Count; ++i)
    {
      if (objectsInRange[i] == null || !objectsInRange[i].useGravity)
        continue;

      // Calculate initial gravity direction, just towards the gravity source transform
      var rb = objectsInRange[i];
      var gravityDir = (transform.position - rb.transform.position).normalized;

      // Find out which of our child colliders is closest
      var closestHit = Mathf.Infinity;
      for (var j = 0; j < gravityColliders.Length; ++j)
      {
        // Step 1, raycast in general direction of collider to find a normal of the 
        // surface
        RaycastHit hitInfo;
        var raycastTo = gravityColliders[j].transform.position;
        var toCollider = (raycastTo - rb.transform.position).normalized;
        var gravityRay = new Ray(rb.transform.position, toCollider);
        if (gravityColliders[j].Raycast(gravityRay, out hitInfo, kRaycastDistance))
        {
          // Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.red);
          // Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.red);

          // Now, set our new ray to point in the opposite direction of this normal, 
          // to raycast 'down' towards the closest point on the plane formed by the normal
          gravityRay = new Ray(rb.transform.position, -hitInfo.normal);

          // Update gravity direction guess if this was a closer hit
          var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
          if (dist < closestHit)
          {
            gravityDir = -hitInfo.normal;
            closestHit = dist;
          }
        }

        // Raycast a second time onto the collider with the refined 'down' direction
        if (gravityColliders[j].Raycast(gravityRay, out hitInfo, kRaycastDistance))
        {
          // Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.green);
          // Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.green);
          var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
          if (dist < closestHit)
          {
            gravityDir = -hitInfo.normal;
            closestHit = dist;
          }
        }
      }

      // Debug.DrawRay(rb.transform.position, gravityDir * 2, Color.blue);

      // Now apply gravity if we are the closest source (only 1 source at a time applies gravity)
      var item = rb.GetComponent<GravityItem>();
      if (item.CurrentGravitySource == this || closestHit < item.CurrentDistance)
      {
        // Update tracking vars 
        item.CurrentDistance = closestHit;
        item.CurrentGravitySource = this;
        item.Up = Vector3.Lerp(item.Up, -gravityDir.normalized, Time.deltaTime * 2.0f);
        
        // Calculate force
        var force = gravityDir.normalized * Gravity;
        var distRatio = Mathf.Clamp01(closestHit / Radius);

        // Gravity gets scaled up with distance because games
        force *= 1.0f + distRatio;
        rb.AddForce(force * rb.mass);
      }
    }
  }
}