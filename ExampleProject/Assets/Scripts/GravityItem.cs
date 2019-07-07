using System.Collections.Generic;
using UnityEngine;

namespace DoctorWolfy121.GravitySystem
{
    [AddComponentMenu("Gravity System/Gravity Item"), RequireComponent(typeof(Rigidbody))]
    public class GravityItem : MonoBehaviour
    {
        [Tooltip("Up direction of this item")]
        public Vector3 Up = Vector3.up;
        [Tooltip("How many Gravity Sources are affecting this object")]
        public int ActiveFieldCount;
        [Tooltip("The current distance to a Gravity Source")]
        public float CurrentDistance = Mathf.Infinity;
        [Tooltip("Rotates this object so down always faces towards the Gravity Source (Useful for a player controller)")]
        public bool RotateToGround = false;
        public List<IGravitySource> CurrentGravitySource = new List<IGravitySource>();

        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}