using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private float _radius;
    [SerializeField]
    private int _health = 10;

    private Entity _entity;
    private EntityManager _em;

    void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;

        _entity = _em.CreateEntity(
            typeof(LocalTransform)
        );
        _em.AddComponentData(_entity, new EnemyEntity() { Radius = _radius });
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
    }

    private void OnDestroy()
    {
        _em.DestroyEntity(_entity);
    }
}

public struct EnemyEntity : IComponentData
{
    public float Radius;
}

public struct HealthEntity : IComponentData
{
    public int Value;
}

public struct DeadEvent : IComponentData { }