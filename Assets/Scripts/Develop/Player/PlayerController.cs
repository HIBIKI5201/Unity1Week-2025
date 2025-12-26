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

    // アビリティインスタンス参照（ゴーストの状態チェックに使う）
    private GhostAbility _ghostInstance;
    private PenetrationAbility _penetrationInstance;
    // ランタイム同期用フラグ
    private bool _prevGhostFlag;
    private bool _prevPenetrationFlag;
    private bool _penetrationAdded;


    private void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _inputBuffer = GetComponent<InputBuffer>();
        Collider playerCollider = GetComponent<Collider>();
        InitialRegistration();

        // AbilityManager の準備
        _abilityManager = new AbilityManager();
        AbilityBridge.Manager = _abilityManager;

        // 初期同期（シリアライズ済みフラグに従ってアビリティを追加/設定する）
        SyncAbilities(true);
        _prevGhostFlag = _ghostAbilirty;
        _prevPenetrationFlag = _penetrationAbility;

        // 各種ユーティリティを初期化（PlayerCollision にはゴースト判定デリゲートを渡す）
        _playerMover = new PlayerMover(_config, transform, playerCollider, _camera);
        _playerAttacker = new PlayerAttacker(_em, _config);
        _playerCollision = new PlayerCollision(_em, transform, _config, () => _ghostInstance != null && _ghostInstance.IsActive);
    }

    private void OnDestroy()
    {
        UnRegistrantion();
    }

    private void Update()
    {
        // Inspector のフラグ変更を検出して同期（ランタイム反映）
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
        _abilityManager?.Activate();
        // 発動成功（ゴーストがアクティブになった）ならログ出力
        if (_ghostInstance != null && _ghostInstance.IsActive)
        {
            Debug.Log("ゴースト能力を発動しました。");
        }
    }

    // フラグに基づいてアビリティを追加・削除する（isStartUp: Start 時は true、ランタイム変更時は false）
    private void SyncAbilities(bool isStartUp)
    {
        // ゴーストアビリティ（アクティブ）
        if (_ghostAbilirty)
        {
            if (_ghostInstance == null)
            {
                _ghostInstance = new GhostAbility(_config);
            }
            // 常に AbilityManager に設定して有効化（Start/ランタイムどちらでも）
            _abilityManager.SetActive(_ghostInstance);
        }
        else
        {
            // フラグがオフならアクティブを解除
            _abilityManager.SetActive(null);
        }

        // 貫通パッシブ
        if (_penetrationAbility)
        {
            if (_penetrationInstance == null)
            {
                _penetrationInstance = new PenetrationAbility(_config.PenetrationCount);
                _abilityManager.AddPassive(_penetrationInstance);
                _penetrationAdded = true;
            }
            else if (!_penetrationAdded)
            {
                // 既にインスタンスがあるが未登録なら登録する
                _abilityManager.AddPassive(_penetrationInstance);
                _penetrationAdded = true;
            }
            // isStartUp 特別処理は不要（毎回登録状態を保証する実装）
        }
        else
        {
            if (_penetrationAdded && _penetrationInstance != null)
            {
                _abilityManager.RemovePassive(_penetrationInstance);
                _penetrationAdded = false;
                // インスタンスは保持（再利用可能）
            }
        }
    }
}
