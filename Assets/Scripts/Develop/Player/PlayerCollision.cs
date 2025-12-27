using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct Hit : IComponentData { }

public class PlayerCollision
{
    public PlayerCollision(EntityManager em, Transform transform, PlayerConfig config, Func<bool> isGhostActive = null)
    {
        _transform = transform;
        _entityManager = em;
        _config = config;
        _isGhostActive = isGhostActive;
        _bulletQuery = _entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<BulletEntity>(),
            ComponentType.ReadOnly<EnemyBullet>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.Exclude<Hit>()
        );
    }

    private PlayerConfig _config;
    private Transform _transform;
    private EntityManager _entityManager;
    private EntityQuery _bulletQuery;
    private readonly Func<bool> _isGhostActive;

    public bool LateUpdate()
    {
        // ゴースト（無敵）なら当たり判定をスキップする
        if (_isGhostActive != null && _isGhostActive())
        {
            return false;
        }

        int count = _bulletQuery.CalculateEntityCount();
        if (count == 0) return false;
        //バレットのデータを取得
        var bullets = _bulletQuery.ToEntityArray(Allocator.TempJob);
        var transforms = _bulletQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        var bulletData = _bulletQuery.ToComponentDataArray<BulletEntity>(Allocator.TempJob);
        var hitResults = new NativeArray<bool>(count, Allocator.TempJob);
        //ジョブを生成、実行
        var job = new PlayerBulletHitJob
        {
            BulletTransforms = transforms,
            BulletData = bulletData,
            PlayerPos = (float3)_transform.position,
            PlayerRadius = _config.CollisionRadius,
            HitResults = hitResults
        };
        JobHandle handle = job.Schedule(count, 64);
        handle.Complete();
        //結果をもとに処理
        bool isHit = false;
        for (int i = 0; i < count; i++)
        {
            if (hitResults[i])
            {
                _entityManager.AddComponent<Hit>(bullets[i]);
                isHit = true;
            }
        }
        //メモリ開放
        bullets.Dispose();
        transforms.Dispose();
        bulletData.Dispose();
        hitResults.Dispose();

        return isHit;
    }
}
