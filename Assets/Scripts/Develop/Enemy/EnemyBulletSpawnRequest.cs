using Unity.Entities;
using Unity.Mathematics;

public readonly struct EnemyBulletSpawnRequest : IComponentData
{
    public EnemyBulletSpawnRequest(int index,float3 pos)
    {
        Id = index;
        Position = pos;
    }

    public readonly int Id;
    public readonly float3 Position;
}
