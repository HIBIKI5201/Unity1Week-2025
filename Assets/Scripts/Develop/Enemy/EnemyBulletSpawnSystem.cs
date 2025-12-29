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
            Debug.Log("NoBuffer");
            return;
        }

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // リクエストを取得。
        foreach (var (request, entity)
                 in SystemAPI.Query<RefRO<EnemyBulletSpawnRequest>>()
                     .WithEntityAccess())
        {
            Debug.Log("YES0");
            int id = request.ValueRO.Id;

            Entity prefab = Entity.Null;
            foreach (var element in prefabBuffer)
            {
                Debug.Log("yes1");
                // ID が一致する Prefab を検索する
                if (element.Id == id)
                {
                    Debug.Log("YES1");
                    prefab = element.Prefab;
                    break;
                }
            }

// Prefab が見つかった場合のみ Entity を生成する
            if (prefab != Entity.Null)
            {
                Debug.Log("YES2");
                // EntityCommandBuffer を使って弾 Entity を生成する
                Entity bullet = ecb.Instantiate(prefab);
                ecb.AddComponent(bullet, new EnemySource
                {
                    EnemyId = id
                });
                ecb.SetComponent(
                    bullet,
                    LocalTransform.FromPositionRotation(
                        request.ValueRO.Position,
                        request.ValueRO.Direction));
            }


            // リクエスト消費
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}