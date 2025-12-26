using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private bool _ghostAbilirty;
    [SerializeField] private bool _penetrationAbility;
    private InputBuffer _inputBuffer;
    private PlayerMover _playerMover;
    private PlayerAttacker _playerAttacker;
    private PlayerCollision _playerCollision;
    private AbilityManager _abilityManager;
    private Vector2 _moveDirection;
    private EntityManager _em;

    // 実際に追加したインスタンス参照を保持しておく
    private GhostAbility _ghostInstance;
    private PenetrationAbility _penetrationInstance;

    // 前フレームのフラグ（変更検出用）
    private bool _prevGhostFlag;
    private bool _prevPenetrationFlag;


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
        AbilityBridge.Manager = _abilityManager;
        // 初期フラグに応じて追加
        SyncAbilities(true);
        // 初期の前フレーム値を記録
        _prevGhostFlag = _ghostAbilirty;
        _prevPenetrationFlag = _penetrationAbility;
    }

    private void OnDestroy()
    {
        UnRegistrantion();
    }

    private void Update()
    {
        // インスペクタでの切り替えを反映（実行中にオン/オフできる）
        if (_ghostAbilirty != _prevGhostFlag || _penetrationAbility != _prevPenetrationFlag)
        {
            SyncAbilities(false);
            _prevGhostFlag = _ghostAbilirty;
            _prevPenetrationFlag = _penetrationAbility;
        }

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

    // フラグに基づいてアビリティを追加・削除する（startUp フラグ isStartUp）
    private void SyncAbilities(bool isStartUp)
    {
        // ゴーストアビリティ
        if (_ghostAbilirty)
        {
            if (_ghostInstance == null)
            {
                _ghostInstance = new GhostAbility(_config);
                _abilityManager.SetActive(_ghostInstance);
            }
            else if (isStartUp)
            {
                _abilityManager.SetActive(_ghostInstance);
            }
        }
        else
        {
            if (_ghostInstance != null)
            {
                // アクティブ解除
                _abilityManager.SetActive(null);
            }
        }

        // 貫通パッシブ
        if (_penetrationAbility)
        {
            if (_penetrationInstance == null)
            {
                _penetrationInstance = new PenetrationAbility();
                _abilityManager.AddPassive(_penetrationInstance);
            }
            else if (isStartUp)
            {
                // 既に追加済みなら何もしない
            }
        }
        else
        {
            if (_penetrationInstance != null)
            {
                _abilityManager.RemovePassive(_penetrationInstance);
                // インスタンスは保持しておいて再度有効化時に再利用可能とする
            }
        }
    }
}
