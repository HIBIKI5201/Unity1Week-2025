using Unity.Entities;
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
    EntityManager _entityManager;
    EntityQuery _bulletQuery;

    public bool LateUpdate()
    {
        var playerPos = (float3)_transform.position;

        using (var bullets = _bulletQuery.ToEntityArray(Unity.Collections.Allocator.Temp))
        using (var transforms = _bulletQuery.ToComponentDataArray<LocalTransform>(Unity.Collections.Allocator.Temp))
        using (var bulletData = _bulletQuery.ToComponentDataArray<Bullet>(Unity.Collections.Allocator.Temp))
        {
            Debug.Log($"Bullet Count : {bullets.Length}");
            for (int i = 0; i < bullets.Length; i++)
            {
                float dist = math.distance(playerPos, transforms[i].Position);
                float hitRadius = _config.CollisionRadius + bulletData[i].Radius;
                Debug.Log(dist + (" ") + hitRadius);
                if (dist <= hitRadius)
                {
                    // 当たった事実だけを付与
                    _entityManager.AddComponent<Hit>(bullets[i]);
                    return true;
                }
            }
        }
        return false;
    }
}
