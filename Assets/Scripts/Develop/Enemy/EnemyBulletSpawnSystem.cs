using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyBulletSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonBuffer<BulletEnemyPrefabElement>(out var prefabBuffer))
        {
            return;
        }

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // リクエストを取得。
        foreach (var (request, entity)
                 in SystemAPI.Query<RefRO<EnemyBulletSpawnRequest>>()
                     .WithEntityAccess())
        {
            int id = request.ValueRO.Id;

            Entity prefab = Entity.Null;
            foreach (var element in prefabBuffer)
            {
                // ID が一致する Prefab を検索する
                if (element.Id == id)
                {
                    prefab = element.Prefab;
                    break;
                }
            }

// Prefab が見つかった場合のみ Entity を生成する
            if (prefab != Entity.Null)
            {
                // EntityCommandBuffer を使って弾 Entity を生成する
                Entity bullet = ecb.Instantiate(prefab);
                ecb.AddComponent(bullet, new EnemySource
                {
                    EnemyId = id
                });
                ecb.SetComponent(
                    bullet,
                    LocalTransform.FromPosition(
                        request.ValueRO.Position
                        ));
                // ホーミング指定がある場合、追従タグを付与する
                if (request.ValueRO.Horming == 1)
                {
                    ecb.AddComponent<HomingBulletTag>(bullet);
                }
            }


            // リクエスト消費
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}