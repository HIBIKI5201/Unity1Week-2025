using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct BulletSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonBuffer<BulletPrefabElement>(out var prefabBuffer))
        {
            return;
        }

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // リクエストを取得。
        foreach (var (request, entity)
            in SystemAPI.Query<RefRO<BulletSpawnRequest>>()
                        .WithEntityAccess())
        {
            int index = request.ValueRO.PrefabIndex;
            if (index < 0 || index >= prefabBuffer.Length)
                continue;

            Entity prefab = prefabBuffer[index].Prefab;
            Entity bullet = ecb.Instantiate(prefab);

            ecb.SetComponent(bullet, LocalTransform.FromPosition(
                request.ValueRO.Position
            ));

            // Penetration がある場合は弾にコンポーネントを追加
            int pen = request.ValueRO.Penetration;
            if (pen > 0)
            {
                ecb.AddComponent(bullet, new BulletPenetration { Remaining = pen });
            }

            // リクエスト消費
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}