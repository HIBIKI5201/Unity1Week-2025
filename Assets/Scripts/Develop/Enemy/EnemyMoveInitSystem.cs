using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// プレイヤーを追従する弾の速度方向を更新する System
/// </summary>
[BurstCompile]
public partial struct EnemyBulletHomingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // プレイヤー位置を取得する
        float3 playerPos = SystemAPI.GetSingleton<PlayerPosition>().Position;

        foreach (var (move, transform) in
                 SystemAPI.Query<RefRW<MoveEntity>, RefRO<LocalTransform>>()
                     .WithAll<HomingBulletTag>())
        {
            // プレイヤー方向ベクトルを計算する
            float3 direction = playerPos - transform.ValueRO.Position;

            // ゼロ長ベクトルを回避する
            if (math.lengthsq(direction) < 0.0001f)
            {
                continue;
            }

            // 向きを更新し、速度の大きさを維持する
            direction = math.normalize(direction);
            float speed = math.length(move.ValueRO.Velocity);
            move.ValueRW.Velocity = direction * speed;
        }
    }
}