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
        // EntityManager を取得する
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // PlayerPosition 用の Entity を生成する
        _playerEntity = _entityManager.CreateEntity(typeof(PlayerPosition));

        // 初期位置を設定する
        _entityManager.SetComponentData(
            _playerEntity,
            new PlayerPosition
            {
                Position = transform.position
            });
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
        // World が破棄済みの場合は何もしない
        if (!World.DefaultGameObjectInjectionWorld.IsCreated)
        {
            return;
        }

        // PlayerPosition Entity を明示的に破棄する
        if (_entityManager.Exists(_playerEntity))
        {
            _entityManager.DestroyEntity(_playerEntity);
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

