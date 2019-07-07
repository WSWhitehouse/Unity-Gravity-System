using System.Collections.Generic;
using UnityEngine;

namespace DoctorWolfy121.GravitySystem
{
    [AddComponentMenu("Gravity System/Gravity Item 2D"), RequireComponent(typeof(Rigidbody2D))]
    public class GravityItem2D : MonoBehaviour
    {
        [Tooltip("Up direction of this item")]
        public Vector2 Up = Vector2.up;
        [Tooltip("How many Gravity Sources are affecting this object")]
        public int ActiveFieldCount;
        [Tooltip("The current distance to a Gravity Source")]
        public float CurrentDistance = Mathf.Infinity;
        [Tooltip("Rotates this object so down always faces towards the Gravity Source (Useful for a player controller)")]
        public bool RotateToGround = false;
        public List<IGravitySource2D> CurrentGravitySource = new List<IGravitySource2D>();

        public Rigidbody2D Rigidbody2D { get; private set; }

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}