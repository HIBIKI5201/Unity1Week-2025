using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// 共有ヘルス用 Entity を生成する MonoBehaviour
/// </summary>
public sealed class EnemyHealthCreate : MonoBehaviour
{
    public void RegisterEnemy(Entity e)
    {
        _enemyEntities.Add(e);
    }

    [SerializeField] private int _healthValue = 100;

    public Entity HealthEntity => _healthEntity;
    private List<Entity> _enemyEntities = new();
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
        if (!World.DefaultGameObjectInjectionWorld.IsCreated) return;
        foreach (var e in _enemyEntities)
        {
            if (_entityManager.Exists(e))
                _entityManager.DestroyEntity(e);
        }

        _enemyEntities.Clear();
    }
}