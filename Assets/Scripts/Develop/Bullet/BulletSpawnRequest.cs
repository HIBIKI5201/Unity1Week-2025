using Unity.Entities;
using Unity.Mathematics;

public readonly struct BulletSpawnRequest : IComponentData
{
    public BulletSpawnRequest(
        int index,
        float3 pos,
        float3 dir)
    {
        PrefabIndex = index;
        Position = pos;
        Direction = dir;
    }

    public readonly int PrefabIndex;
    public readonly float3 Position;
    public readonly float3 Direction;
}
