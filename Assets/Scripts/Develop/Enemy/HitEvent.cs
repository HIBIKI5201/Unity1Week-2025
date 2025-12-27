using Unity.Entities;
using Unity.Mathematics;

public struct HitEvent : IComponentData
{
    public Entity Bullet;
    public Entity Target;
    public float3 Position;
}