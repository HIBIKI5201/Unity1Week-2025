using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public readonly struct EnemyBulletSpawnRequest : IComponentData
{
    public EnemyBulletSpawnRequest(int index,float3 pos,Quaternion quaternion)
    {
        Id = index;
        Position = pos;
        Direction = quaternion;
    }

    public readonly int Id;
    public readonly float3 Position;
    public readonly Quaternion Direction;
}
