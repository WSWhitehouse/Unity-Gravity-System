using UnityEngine;

public class GravityItem : MonoBehaviour
{
    public Vector3 Up = Vector3.up;
    public int ActiveFieldCount;
    public float CurrentDistance = Mathf.Infinity;
    public GravitySource CurrentGravitySource;
}
