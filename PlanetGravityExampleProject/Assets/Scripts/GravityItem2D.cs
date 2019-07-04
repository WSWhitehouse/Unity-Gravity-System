using UnityEngine;

public class GravityItem2D : MonoBehaviour
{
    public Vector2 Up = Vector2.up;
    public int ActiveFieldCount;
    public float CurrentDistance = Mathf.Infinity;
    public GravitySource2D CurrentGravitySource;
}