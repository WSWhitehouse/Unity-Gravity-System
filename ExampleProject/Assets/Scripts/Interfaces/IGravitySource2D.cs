using System.Collections.Generic;
using UnityEngine;

public interface IGravitySource2D
{
    float GravityStrength { get; }
    List<GravityItem2D> ItemsInRange { get; }
    Collider2D[] GravityColliders { get; }
}