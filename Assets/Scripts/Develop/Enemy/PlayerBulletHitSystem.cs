using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(DamageApplySystem))]
public partial struct PlayerBulletHitSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyEntity>();
        state.RequireForUpdate<PlayerBullet>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
        var em = state.EntityManager;

        var bulletQuery = SystemAPI.QueryBuilder()
            .WithAll<LocalTransform, BulletEntity, PlayerBullet>()
            .Build();

        var bulletTransforms =
            bulletQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var bulletData =
            bulletQuery.ToComponentDataArray<BulletEntity>(state.WorldUpdateAllocator);
        var bulletEntities =
            bulletQuery.ToEntityArray(state.WorldUpdateAllocator);

        foreach (var (enemyTransform, enemyData, enemyEntity)
            in SystemAPI
                .Query<RefRO<LocalTransform>, RefRO<EnemyEntity>>()
                .WithEntityAccess())
        {
            float3 enemyPos = enemyTransform.ValueRO.Position;
            float enemyRadius = enemyData.ValueRO.Radius;

            int totalDamage = 0;
            var hitBullets = new List<Entity>(4);
            // ローカルで同フレーム中の弾ごとのヒット履歴を保持（ECB の反映前に判定を重複させないため）
            var localHitSets = new Dictionary<Entity, HashSet<Entity>>();
            //ヒット検出
            for (int i = 0; i < bulletEntities.Length; i++)
            {
                var bulletEntity = bulletEntities[i];
                float3 bulletPos = bulletTransforms[i].Position;
                float bulletRadius = bulletData[i].Radius;

                float hitDist = enemyRadius + bulletRadius;
                float distSq = math.distancesq(enemyPos, bulletPos);

                if (distSq <= hitDist * hitDist)
                {
                    bool alreadyHit = false;
                    // 永続バッファに既に記録があるかチェック（過去フレームで当たっていないか）
                    if (em.HasComponent<HitTarget>(bulletEntity))
                    {
                        var buf = em.GetBuffer<HitTarget>(bulletEntity);
                        for (int bi = 0; bi < buf.Length; bi++)
                        {
                            if (buf[bi].Target == enemyEntity)
                            {
                                alreadyHit = true;
                                break;
                            }
                        }
                    }
                    // 同フレーム内で既に記録済みかチェック（ECB の反映前の追加分を考慮）
                    if (!alreadyHit && localHitSets.TryGetValue(bulletEntity, out var set))
                    {
                        if (set.Contains(enemyEntity)) alreadyHit = true;
                    }

                    if (alreadyHit)
                        continue;

                    // ヒットを集計
                    totalDamage += bulletData[i].Damage;
                    hitBullets.Add(bulletEntity);
                    // ローカル履歴に追加
                    if (!localHitSets.TryGetValue(bulletEntity, out var localSet))
                    {
                        localSet = new HashSet<Entity>();
                        localHitSets[bulletEntity] = localSet;
                    }
                    localSet.Add(enemyEntity);
                }
            }

            // 集計後に一度だけイベントを発行
            if (totalDamage > 0)
            {
                ecb.AddComponent(enemyEntity, new DamageEvent
                {
                    Value = totalDamage
                });

                // ヒットごとに独立したイベントエンティティを作成し、弾のバッファへヒット履歴を追記する
                for (int i = 0; i < hitBullets.Count; i++)
                {
                    var hitBullet = hitBullets[i];
                    var evt = ecb.CreateEntity();
                    ecb.AddComponent(evt, new HitEvent
                    {
                        Bullet = hitBullet,
                        Target = enemyEntity,
                        Position = enemyPos
                    });

                    // 永続バッファが存在すればそこに追加して、以降のフレームでも同一敵を無視できるようにする
                    if (em.HasComponent<HitTarget>(hitBullet))
                    {
                        ecb.AppendToBuffer(hitBullet, new HitTarget { Target = enemyEntity });
                    }
                }
            }
        }

        ecb.Playback(state.EntityManager);

        // 配列の解放
        bulletTransforms.Dispose();
        bulletData.Dispose();
        bulletEntities.Dispose();
    }
}
