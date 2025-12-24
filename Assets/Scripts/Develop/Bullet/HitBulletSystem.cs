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
    /// 毎フレーム呼ばれ、敵弾がプレイヤーに当たったかを判定する
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        

        // Simulation 終了時に実行される ECB を取得
        var ecb =
            SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, bullet, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<Bullet>>()
                     .WithAll<Hit>()
                     .WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
            
        }
        ecb.Playback(state.EntityManager);

        // バッファを破棄
        ecb.Dispose();
    }
}