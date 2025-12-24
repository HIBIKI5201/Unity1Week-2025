using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct BulletSpawnRequest : IComponentData
{
    public float3 Position;
    public float3 Direction;
}
