using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityItem2D : MonoBehaviour
{
    public Vector2 Up = Vector2.up;
    public int ActiveFieldCount;
    public float CurrentDistance = Mathf.Infinity;
    public List<IGravitySource2D> CurrentGravitySource = new List<IGravitySource2D>();

    public Rigidbody2D Rigidbody2D { get; private set; }

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
}