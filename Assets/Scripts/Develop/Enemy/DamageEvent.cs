using Unity.Entities;
using UnityEngine;

public struct DamageEvent : IComponentData
{
    public Entity Target;
    public int Value;
}
