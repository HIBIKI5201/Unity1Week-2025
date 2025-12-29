using SymphonyFrameWork.System;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct PlayerHitResult : IComponentData
{
    // マーカーコンポーネント（IComponentData に参照型を含めない）
}

public class PlayerCollision
{
    public PlayerCollision(EntityManager em, Transform transform, PlayerConfig config, Func<bool> isGhostActive = null)
    {
        _transform = transform;
        _entityManager = em;
        _config = config;
        _isGhostActive = isGhostActive;

        // 弾クエリ：弾本体 + 発生元 ID（EnemySorce） + Transform
        _bulletQuery = _entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<BulletEntity>(),
            ComponentType.ReadOnly<EnemyBullet>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<EnemySource>(),
            ComponentType.Exclude<PlayerHitResult>()
        );

        // 敵本体クエリ：EnemyEntity + Transform
        _enemyQuery = _entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<EnemyEntity>(),
            ComponentType.ReadOnly<LocalTransform>()
        );

        // ServiceLocator から AbilityRepository を参照（存在しなければ null）
        if (!ServiceLocator.TryGetInstance<AbilityRepository>(out _abilityRepository))
            _abilityRepository = null;
    }

    private PlayerConfig _config;
    private Transform _transform;
    private EntityManager _entityManager;
    private EntityQuery _bulletQuery;
    private EntityQuery _enemyQuery;
    private readonly Func<bool> _isGhostActive;
    private AbilityRepository _abilityRepository;

    public bool LateUpdate()
    {
        // ゴースト（無敵）なら当たり判定をスキップする
        if (_isGhostActive != null && _isGhostActive())
        {
            return false;
        }

        bool anyHit = false;

        // --- 弾との当たり判定 ---
        int bulletCount = _bulletQuery.CalculateEntityCount();
        if (bulletCount > 0)
        {
            var bullets = _bulletQuery.ToEntityArray(Allocator.TempJob);
            var transforms = _bulletQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            var bulletData = _bulletQuery.ToComponentDataArray<BulletEntity>(Allocator.TempJob);
            var enemySources = _bulletQuery.ToComponentDataArray<EnemySource>(Allocator.TempJob);
            var hitResults = new NativeArray<bool>(bulletCount, Allocator.TempJob);

            try
            {
                var job = new PlayerBulletHitJob
                {
                    BulletTransforms = transforms,
                    BulletData = bulletData,
                    PlayerPos = (float3)_transform.position,
                    PlayerRadius = _config.CollisionRadius,
                    HitResults = hitResults
                };
                JobHandle handle = job.Schedule(bulletCount, 64);
                handle.Complete();

                for (int i = 0; i < bulletCount; i++)
                {
                    if (hitResults[i])
                    {
                        // マーカーを付けて以降の判定対象から除外
                        _entityManager.AddComponent<PlayerHitResult>(bullets[i]);
                        anyHit = true;

                        // 発生元 EnemyId を AbilityRepository に登録（初回のみ反映される）
                        if (_abilityRepository != null)
                        {
                            int enemyId = enemySources[i].EnemyId;
                            _abilityRepository.RegisterHitOnce(enemyId);
                        }
                    }
                }
            }
            finally
            {
                bullets.Dispose();
                transforms.Dispose();
                bulletData.Dispose();
                enemySources.Dispose();
                hitResults.Dispose();
            }
        }

        // --- 敵本体との当たり判定 ---
        int enemyCount = _enemyQuery.CalculateEntityCount();
        if (enemyCount > 0)
        {
            var enemyTransforms = _enemyQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            var enemyData = _enemyQuery.ToComponentDataArray<EnemyEntity>(Allocator.TempJob);

            try
            {
                float3 playerPos = (float3)_transform.position;
                float playerRadius = _config.CollisionRadius;

                for (int i = 0; i < enemyCount; i++)
                {
                    float distSq = math.distancesq(playerPos, enemyTransforms[i].Position);
                    float hitRadius = playerRadius + enemyData[i].Radius;
                    if (distSq <= hitRadius * hitRadius)
                    {
                        anyHit = true;
                        // 敵本体の ID を登録（初回のみ）
                        if (_abilityRepository != null)
                        {
                            _abilityRepository.RegisterHitOnce(enemyData[i].Id);
                        }
                    }
                }
            }
            finally
            {
                enemyTransforms.Dispose();
                enemyData.Dispose();
            }
        }

        return anyHit;
    }
}