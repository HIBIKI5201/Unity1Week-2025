using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public struct PlayerBulletHitJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<LocalTransform> BulletTransforms;
    [ReadOnly] public NativeArray<Bullet> BulletData;
    [ReadOnly] public float3 PlayerPos;
    [ReadOnly] public float PlayerRadius;

    public NativeArray<bool> HitResults;

    public void Execute(int index)
    {
        float dist = math.distance(PlayerPos, BulletTransforms[index].Position);
        float hitRadius = PlayerRadius + BulletData[index].Radius;

        HitResults[index] = dist <= hitRadius;
    }
}
