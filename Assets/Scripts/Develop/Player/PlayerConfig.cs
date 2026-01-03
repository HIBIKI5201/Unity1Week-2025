using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = -1)]
public class PlayerConfig : ScriptableObject
{
    public float CollisionRadius => _collisionRadius;
    public float AttackSpeed => _attackSpeed;
    public float MoveSpeed => _moveSpeed;
    public float GhostTime => _ghostTime;
    public float GhostAbilityCoolTime => _ghostAbilityCoolTime;
    public int PenetrationCount => _penetrationCount;
    public string TitleName => _titleName;
    public string InGameName => _inGameName;
    public Material GhostMaterial => _ghostMaterial;
    public float ShotCoolTime => _shotCoolTime;
    public float ShotUpdateCoolTime => _shotUpdateCoolTime;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _collisionRadius = 1.5f;
    [SerializeField] private float _ghostTime = 0.5f;
    [SerializeField] private float _ghostAbilityCoolTime = 1f;
    [SerializeField] private int _penetrationCount = 1;
    [SerializeField] private string _titleName = string.Empty;
    [SerializeField] private string _inGameName = string.Empty;
    [SerializeField] private Material _ghostMaterial;
    [SerializeField] private float _shotCoolTime = 0.2f;
    [SerializeField] private float _shotUpdateCoolTime = 0.2f;
}
