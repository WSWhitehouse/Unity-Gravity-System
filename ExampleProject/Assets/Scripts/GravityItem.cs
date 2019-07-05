using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Gravity System/Gravity Item"), RequireComponent(typeof(Rigidbody))]
public class GravityItem : MonoBehaviour
{
    public Vector3 Up = Vector3.up;
    public int ActiveFieldCount;
    public float CurrentDistance = Mathf.Infinity;
    public List<IGravitySource> CurrentGravitySource = new List<IGravitySource>();

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}