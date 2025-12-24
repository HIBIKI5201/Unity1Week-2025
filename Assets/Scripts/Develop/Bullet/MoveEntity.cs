using Unity.Entities;
using Unity.Mathematics;

public struct MoveEntity : IComponentData
{
    public float3 Velocity;
}