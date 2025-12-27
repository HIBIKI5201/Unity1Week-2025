using Unity.Entities;
using Unity.Mathematics;

public readonly struct BulletSpawnRequest : IComponentData
{
    public BulletSpawnRequest(int index,float3 pos,float3 dir,int penetration)
    {
        PrefabIndex = index;
        Position = pos;
        Direction = dir;
        Penetration = penetration;
    }

    public readonly int PrefabIndex;
    public readonly int Penetration;
    public readonly float3 Position;
    public readonly float3 Direction;
}
