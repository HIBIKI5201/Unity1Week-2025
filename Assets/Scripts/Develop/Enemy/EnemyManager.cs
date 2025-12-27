using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private int _health = 10;

    [SerializeField] private int _id;
    private Entity _entity;
    private EntityManager _em;

    private float _deltaTime;
    void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;

        _entity = _em.CreateEntity(
            typeof(LocalTransform)
        );
        _em.AddComponentData(_entity, new EnemyEntity()
        {
            Radius = _radius,
            Id = _id
        });
        _em.AddComponentData(_entity, new HealthEntity() { Value = _health });
    }

    void Update()
    {
        if (!_em.Exists(_entity)) return;

        _em.SetComponentData(_entity, LocalTransform.FromPosition(transform.position));

        if (_em.HasComponent<DeadEvent>(_entity))
        {
            Destroy(gameObject);
        }

        _deltaTime += Time.deltaTime;
        if (_deltaTime >= 1f)
        {
            ShootBullet();
            _deltaTime = 0;
        }

    }
    /// <summary>
    ///  エネミーの現在地から弾を発射する。
    /// </summary>
    private void ShootBullet()
    {
        EnemyBulletContext enemyContext = new EnemyBulletContext
        {
            Id = _id,
            Position = transform.position,
            Forward = transform.forward,
        };
        BulletShootHelper.ShootEnemy(_em, enemyContext);
    }


    private void OnDestroy()
    {
        _em.DestroyEntity(_entity);
    }
}

public struct EnemyEntity : IComponentData
{
    public float Radius;
    public int Id;
}

public struct HealthEntity : IComponentData
{
    public int Value;
}

public struct DeadEvent : IComponentData
{
}