using Unity.Entities;
using Unity.Mathematics;

public struct BulletEntity : IComponentData
{
    public float3 Direction;
    public float Speed;
    public float Radius;
}

