using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

/// <summary>
/// 敵の弾とプレイヤーの当たり判定を行うシステム
/// </summary>
[BurstCompile]
public partial struct HitEnemyBulletToPlayerSystem : ISystem
{
    /// <summary>
    /// 毎フレーム呼ばれ、敵弾がプレイヤーに当たったかを判定する
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        // プレイヤー情報がなければ処理しない
        if (!SystemAPI.TryGetSingleton<PlayerInfo>(out var player))
        {
            return;
        }

        float3 playerPos = player.Position;
        float playerRadius = player.Radius;

        // Simulation 終了時に実行される ECB を取得
        var ecb =
            SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, bullet, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<Bullet>>()
                     .WithAll<EnemyBullet>()
                     .WithEntityAccess())
        {
            // 距離の二乗を計算
            float distanceSq =
                math.distancesq(transform.ValueRO.Position, playerPos);

            // 当たり判定半径の合計
            float hitRadius = bullet.ValueRO.Radius + playerRadius;

            // 球同士の当たり判定
            if (distanceSq <= hitRadius * hitRadius)
            {
                // 敵弾を削除
                ecb.DestroyEntity(entity);
                
            }
            
        }
        ecb.Playback(state.EntityManager);

        // バッファを破棄
        ecb.Dispose();
    }
}