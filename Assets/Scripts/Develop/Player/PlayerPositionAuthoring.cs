using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Player の Transform を ECS に同期するためのブリッジ
/// </summary>
public sealed class PlayerPositionBridge : MonoBehaviour
{
    private Entity _playerEntity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // 既存の PlayerPosition Entity があるか確認
        var query = _entityManager.CreateEntityQuery(typeof(PlayerPosition));
        if (query.CalculateEntityCount() == 0)
        {
            // まだない場合だけ作成
            _playerEntity = _entityManager.CreateEntity(typeof(PlayerPosition));

            // 初期位置を設定
            _entityManager.SetComponentData(
                _playerEntity,
                new PlayerPosition
                {
                    Position = transform.position
                });
        }
        else
        {
            // 既存の PlayerPosition を取得
            _playerEntity = query.GetSingletonEntity();
        }
    }


    private void Update()
    {
        // 毎フレーム Player の Transform を ECS に流し込む
        _entityManager.SetComponentData(
            _playerEntity,
            new PlayerPosition
            {
                Position = transform.position
            });
    }

    private void OnDestroy()
    {
        if (!World.DefaultGameObjectInjectionWorld.IsCreated)
        {
            return;
        }
        if (_entityManager.Exists(_playerEntity) &&
        !_entityManager.HasComponent<PlayerDeadEvent>(_playerEntity))
        {
            _entityManager.AddComponent<PlayerDeadEvent>(_playerEntity);
        }

    }

}
/// <summary>
/// ECS 側から参照される Player の位置データ
/// </summary>
public struct PlayerPosition : IComponentData
{
    public float3 Position;
}

public struct PlayerDeadEvent : IComponentData
{
    public int dummy; // 値は使わない
}
