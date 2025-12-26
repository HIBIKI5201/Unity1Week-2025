using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
            Entity hitBullet = Entity.Null;

            for (int i = 0; i < bulletEntities.Length; i++)
            {
                float3 bulletPos = bulletTransforms[i].Position;
                float bulletRadius = bulletData[i].Radius;

                float hitDist = enemyRadius + bulletRadius;
                float distSq = math.distancesq(enemyPos, bulletPos);

                if (distSq <= hitDist * hitDist)
                {
                    totalDamage += bulletData[i].Damage;
                    hitBullet = bulletEntities[i];
                    ecb.DestroyEntity(bulletEntities[i]);
                    break;
                }
            }

            if (totalDamage > 0)
            {
                ecb.AddComponent(enemyEntity, new DamageEvent
                {
                    Value = totalDamage
                });

                ecb.AddComponent(enemyEntity, new HitEvent
                {
                    Bullet = hitBullet,
                    Target = enemyEntity,
                    Position = enemyPos
                });
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
