using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInfo : IComponentData
{
    public float3 Position;
    public float Radius;
}

public class PlayerInfoBridge : MonoBehaviour
{
    [SerializeField] private float _hitRadius;
    private EntityManager _entityManager;
    private Entity _playerEntity;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _playerEntity = _entityManager.CreateEntity(typeof(PlayerInfo));
    }

    private void Update()
    {
        _entityManager.SetComponentData(
            _playerEntity,
            new PlayerInfo
            {
                Position = transform.position,
                Radius = _hitRadius
            });
    }
}