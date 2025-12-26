using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = -1)]
public class PlayerConfig : ScriptableObject
{
    public float CollisionRadius => _collisionRadius;
    public float AttackSpeed => _attackSpeed;
    public float MoveSpeed => _moveSpeed;
    public float GhostTime => _ghostTime;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _collisionRadius = 1.5f;
    [SerializeField] private float _ghostTime = 0.5f;
}
