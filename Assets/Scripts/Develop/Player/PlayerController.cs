using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraMover _cameraMover;
    private InputBuffer _inputBuffer;
    private PlayerMover _playerMover;
    private PlayerAttacker _playerAttacker;
    private PlayerCollision _playerCollision;
    private AbilityManager _abilityManager;
    private Vector2 _moveDirection;
    private EntityManager _em;


    private void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _inputBuffer = GetComponent<InputBuffer>();
        Collider playerCollider = GetComponent<Collider>();
        InitialRegistration();
        _playerMover = new PlayerMover(_config, transform, playerCollider, _camera);
        _playerAttacker = new PlayerAttacker(_em,_config);
        _playerCollision = new PlayerCollision(_em, transform, _config);
        _abilityManager = new AbilityManager();
        _abilityManager.AddPassive(new PenetrationAbility());
        AbilityBridge.Manager = _abilityManager;
    }

    private void OnDestroy()
    {
        UnRegistrantion();
    }

    private void Update()
    {
        // アビリティの時間経過処理を毎フレーム呼ぶ
        _abilityManager?.Tick(Time.deltaTime);
        _playerMover.OnMove(_moveDirection, _cameraMover.ScrollVelocity, Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (_playerCollision.LateUpdate())
        {
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Dead");
    }

    private void InitialRegistration()
    {
        _inputBuffer.PlayerMove.performed += OnMove;
        _inputBuffer.PlayerMove.canceled += OnMove;
        _inputBuffer.PlayerAttack.started += OnAttack;
        _inputBuffer.PlayerAbility.started += OnAbility;
    }

    private void UnRegistrantion()
    {
        _inputBuffer.PlayerMove.performed -= OnMove;
        _inputBuffer.PlayerMove.canceled -= OnMove;
        _inputBuffer.PlayerAttack.started -= OnAttack;
        _inputBuffer.PlayerAbility.started -= OnAbility;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        var ctx = _abilityManager.BuildBulletContext(0, transform.position, transform.forward);
        _playerAttacker?.OnAttack(ctx);
    }

    private void OnAbility(InputAction.CallbackContext context)
    {
        // アビリティ発動入力
        _abilityManager?.Activate(Time.time);
    }
}
