using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
public partial struct MoveSystem : ISystem
{
    public void OnCreate(ref SystemState state){}
    public void OnDestroy(ref SystemState state){}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (move,xform) in
                 SystemAPI.Query<RefRO<Bullet>,RefRW<LocalTransform>>() )
        {
            ProcessMove(ref state ,move,xform);
        }
    }

    private void ProcessMove(ref SystemState state, RefRO<Bullet> move, RefRW<LocalTransform> xform)
    {
        xform.ValueRW.Position = xform.ValueRW.Position + move.ValueRO.Direction * move.ValueRO.Speed * SystemAPI.Time.DeltaTime ;
    }
}
