using System.Collections.Generic;
using UnityEngine;

namespace DoctorWolfy121.GravitySystem
{
    public interface IGravitySource
    {
        float GravityStrength { get; }
        List<GravityItem> ItemsInRange { get; }
        Collider[] GravityColliders { get; }
        bool EnableGravity { get; }
    }
}