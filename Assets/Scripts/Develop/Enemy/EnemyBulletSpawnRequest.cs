using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public readonly struct EnemyBulletSpawnRequest : IComponentData
{
    public EnemyBulletSpawnRequest(int index,float3 pos,int horming)
    {
        Id = index;
        Position = pos;
        Horming = horming;
    }

    public readonly int Id;
    public readonly float3 Position;
    public readonly int Horming;
}
