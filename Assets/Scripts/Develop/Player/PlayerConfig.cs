using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = -1)]
public class PlayerConfig : ScriptableObject
{
    public float CollisionRadius => _collisionRadius;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _collisionRadius = 1.5f;
}
