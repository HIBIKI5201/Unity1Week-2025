using Unity.Entities;
using UnityEngine;

/// <summary>
/// 共有ヘルス用 Entity を生成する MonoBehaviour
/// </summary>
public sealed class EnemyHealthCreate : MonoBehaviour
{
    [SerializeField] private int _healthValue = 100;

    public Entity HealthEntity => _healthEntity;

    private Entity _healthEntity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // 共有ヘルス Entity を一度だけ生成
        _healthEntity = _entityManager.CreateEntity(typeof(HealthEntity));

        _entityManager.SetComponentData(_healthEntity, new HealthEntity
        {
            Value = _healthValue
        });
    }

    private void OnDestroy()
    {
        if (!World.DefaultGameObjectInjectionWorld.IsCreated)
        {
            return;
        }

        if (_entityManager.Exists(_healthEntity))
        {
           _entityManager.DestroyEntity(_healthEntity);
        }
    }

}