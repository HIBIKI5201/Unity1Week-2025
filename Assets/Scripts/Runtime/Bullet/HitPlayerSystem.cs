using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

[BurstCompile]
public partial struct HitPlayerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<PlayerInfo>(out var player))
        {
            Debug.LogWarning("No player found");
            return;
        }
        
        float3 playerPos = player.Position;
        float playerRadius = player.Radius;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        
        foreach (var(transform,bullet,entity)in 
                 SystemAPI.Query<RefRO<LocalTransform>,RefRO<Bullet>>()
                     .WithEntityAccess())
        {
            float distanceSq = math.distancesq(transform.ValueRO.Position, playerPos);
            float hitRadius = bullet.ValueRO.Radius +  playerRadius;

            if (distanceSq <= hitRadius * hitRadius)
            {
                ecb.DestroyEntity(entity);
            }
        }
        // 予約していた構造変更を一括実行
        ecb.Playback(state.EntityManager);

        // バッファを破棄
        ecb.Dispose();
        
    }
}
