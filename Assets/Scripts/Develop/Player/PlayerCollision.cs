using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct Hit : IComponentData { }

public class PlayerCollision
{
    public PlayerCollision(EntityManager em, Transform transform, PlayerConfig config)
    {
        _transform = transform;
        _entityManager = em;
        _config = config;
        _bulletQuery = _entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<Bullet>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.Exclude<Hit>()
        );
    }

    private PlayerConfig _config;
    private Transform _transform;
    private EntityManager _entityManager;
    private EntityQuery _bulletQuery;

    public bool LateUpdate()
    {
        int count = _bulletQuery.CalculateEntityCount();
        if (count == 0) return false;
        //バレットのデータを取得
        var bullets = _bulletQuery.ToEntityArray(Allocator.TempJob);
        var transforms = _bulletQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        var bulletData = _bulletQuery.ToComponentDataArray<Bullet>(Allocator.TempJob);
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
