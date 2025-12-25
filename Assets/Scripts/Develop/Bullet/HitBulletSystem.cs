using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

/// <summary>
/// 敵の弾とプレイヤーの当たり判定を行うシステム
/// </summary>
[BurstCompile]
public partial struct HitBulletSystem : ISystem
{
    /// <summary>
    /// 毎フレーム呼ばれ、当たった弾を削除する
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        // Simulation 終了時に実行される ECB を取得
        var ecb =
            SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, bullet, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<BulletEntity>>()
                     .WithAll<Hit>()
                     .WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }
    }
}