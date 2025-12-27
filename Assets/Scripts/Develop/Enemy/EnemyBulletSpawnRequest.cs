using Unity.Entities;
using Unity.Mathematics;

public readonly struct EnemyBulletSpawnRequest : IComponentData
{
    public EnemyBulletSpawnRequest(int index,float3 pos,float3 dir)
    {
        Id = index;
        Position = pos;
        Direction = dir;
    }

    public readonly int Id;
    public readonly float3 Position;
    public readonly float3 Direction;
}
