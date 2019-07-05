using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityItem : MonoBehaviour
{
    public Vector3 Up = Vector3.up;
    public int ActiveFieldCount;
    public float CurrentDistance = Mathf.Infinity;
    public GravitySource CurrentGravitySource;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}
