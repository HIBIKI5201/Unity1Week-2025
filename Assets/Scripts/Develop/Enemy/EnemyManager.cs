using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private EnemyHealthCreate _healthCreate;

    [SerializeField] private int _id;
    private Entity _entity;
    private EntityManager _em;

    private float _deltaTime;
    void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entity = _em.CreateEntity(typeof(LocalTransform));
        _em.AddComponentData(_entity, new EnemyEntity { Radius = _radius, Id = _id });
        _em.AddComponentData(_entity, new HealthRef { HealthEntity = _healthCreate.HealthEntity });
        
    }


    void Update()
    {
        if (!_em.Exists(_entity)) return;

        _em.SetComponentData(_entity, LocalTransform.FromPosition(transform.position));

        if (_em.HasComponent<DeadEvent>(_entity))
        {
            Destroy(_healthCreate.gameObject);
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



}

public struct EnemyEntity : IComponentData
{
    public float Radius;
    public int Id;
}



public struct DeadEvent : IComponentData
{
}



