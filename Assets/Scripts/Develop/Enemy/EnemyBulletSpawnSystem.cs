using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 敵弾の生成リクエストを処理し、生成時に一度だけ
/// プレイヤー方向へ初期速度と回転を設定する System
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyBulletSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // 弾 Prefab バッファを取得する
        if (!SystemAPI.TryGetSingletonBuffer<BulletEnemyPrefabElement>(out var prefabBuffer))
        {
            return;
        }

        // プレイヤー位置を取得する
        float3 playerPos = SystemAPI.GetSingleton<PlayerPosition>().Position;

        // 構造変更用 EntityCommandBuffer
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // 弾生成リクエストを処理する
        foreach (var (request, requestEntity)
                 in SystemAPI.Query<RefRO<EnemyBulletSpawnRequest>>()
                     .WithEntityAccess())
        {
            int id = request.ValueRO.Id;

            // ID に対応する弾 Prefab を検索する
            Entity prefab = Entity.Null;
            foreach (var element in prefabBuffer)
            {
                if (element.Id == id)
                {
                    prefab = element.Prefab;
                    break;
                }
            }

            // Prefab が見つからなければ何もしない
            if (prefab == Entity.Null)
            {
                ecb.DestroyEntity(requestEntity);
                continue;
            }

            // 弾 Entity を生成する（この時点ではまだ未実体）
            Entity bullet = ecb.Instantiate(prefab);

            // 発生元の敵 ID を設定する
            ecb.AddComponent(bullet, new EnemySource
            {
                EnemyId = id
            });

            // プレイヤー方向ベクトルを計算する
            float3 direction = playerPos - request.ValueRO.Position;

            // ゼロ長ベクトルを回避する
            if (math.lengthsq(direction) > 0.0001f)
            {
                // 正規化して進行方向を作る
                direction = math.normalize(direction);

                // Prefab から初期速度を取得する
                // ※ Prefab は実体なので EntityManager から読んでよい
                MoveEntity move = state.EntityManager.GetComponentData<MoveEntity>(prefab);

                // 速度の大きさを保持したまま向きだけ変更する
                float speed = math.length(move.Velocity);
                move.Velocity = direction * speed;

                // 生成される弾に速度を設定する
                ecb.SetComponent(bullet, move);

                // 見た目の回転も進行方向に合わせる
                quaternion rotation =
                    quaternion.LookRotationSafe(direction, math.up());

                ecb.SetComponent(
                    bullet,
                    LocalTransform.FromPositionRotation(
                        request.ValueRO.Position,
                        rotation));
            }
            else
            {
                // 方向が決められない場合は位置のみ設定する
                ecb.SetComponent(
                    bullet,
                    LocalTransform.FromPosition(request.ValueRO.Position));
            }

            // リクエスト Entity を消費する
            ecb.DestroyEntity(requestEntity);
        }

        // まとめて反映する
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
