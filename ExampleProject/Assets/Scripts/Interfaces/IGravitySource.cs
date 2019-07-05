using System.Collections.Generic;
using UnityEngine;

public interface IGravitySource
{
    float GravityStrength { get; }
    List<GravityItem> ItemsInRange { get; }
    Collider[] GravityColliders { get; }
}