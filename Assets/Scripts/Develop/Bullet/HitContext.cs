using Unity.Entities;
using Unity.Mathematics;

public struct HitContext
{
    public Entity Bullet;
    public Entity Target;
    public float3 Position;
    public EntityCommandBuffer ECB;
}
