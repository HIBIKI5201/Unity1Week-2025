using SymphonyFrameWork.System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer))]
public class PlayerController : MonoBehaviour
{
    [Header("Inspector フラグ（手動付与）")]
    [SerializeField] private bool _ghostAbilirty;
    [SerializeField] private bool _penetrationAbility;

    [Header("Repository マッピング (敵ID を指定)")]
    private int _ghostSourceEnemyId = -1;
    private int _penetrationSourceEnemyId = -1;

    private PlayerConfig _config;
    private Camera _camera;
    private CameraMover _cameraMover;
    private InputBuffer _inputBuffer;
    private PlayerMover _playerMover;
    private PlayerAttacker _playerAttacker;
    private PlayerCollision _playerCollision;
    private PlayerDead _playerDead;
    private AbilityManager _abilityManager;
    private AbilityRepository _abilityRepository;
    private Vector2 _moveDirection;
    private EntityManager _em;

    // アビリティインスタンス参照（ゴーストの状態チェックに使う）
    private GhostAbility _ghostInstance;
    private PenetrationAbility _penetrationInstance;
    // ランタイム同期用フラグ
    private bool _prevGhostFlag;
    private bool _prevPenetrationFlag;
    private bool _penetrationAdded;

    // Repository 連携用
    private bool _repoGhostApplied;
    private bool _repoPenetrationApplied;

    public void Init(PlayerConfig config, Camera camera, CameraMover cameraMover)
    {
        _config = config;
        _camera = camera;
        _cameraMover = cameraMover;
    }

    private void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _inputBuffer = GetComponent<InputBuffer>();
        Collider playerCollider = GetComponent<Collider>();
        InitialRegistration();

        // AbilityManager の準備
        _abilityManager = new AbilityManager();
        AbilityBridge.Manager = _abilityManager;

        // ServiceLocator から AbilityRepository を取得（System 側で登録されている前提）
        if (!ServiceLocator.TryGetInstance<AbilityRepository>(out _abilityRepository))
            _abilityRepository = null;

        // 初期同期（シリアライズ済みフラグに従ってアビリティを追加/設定する）
        SyncAbilities(true);
        _prevGhostFlag = _ghostAbilirty;
        _prevPenetrationFlag = _penetrationAbility;

        // 各種ユーティリティを初期化（PlayerCollision にはゴースト判定デリゲートを渡す）
        _playerMover = new PlayerMover(_config, transform, playerCollider, _camera);
        _playerAttacker = new PlayerAttacker(_em, _config);
        _playerCollision = new PlayerCollision(_em, transform, _config, () => _ghostInstance != null && _ghostInstance.IsActive);
        _playerDead = new PlayerDead(_config);
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
        ApplyRepositoryAbilities();
        // アビリティの時間経過処理を毎フレーム呼ぶ
        _abilityManager?.Tick(Time.deltaTime);
        _playerMover.OnMove(_moveDirection, _cameraMover.ScrollVelocity, Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (_playerCollision.LateUpdate())
        {
            Debug.Log("プレイヤーが死亡しました。");
            _playerDead?.OnDead();
        }
    }

    private void InitialRegistration()
    {
        _inputBuffer.PlayerMove.performed += OnMove;
        _inputBuffer.PlayerMove.canceled += OnMove;
        _inputBuffer.PlayerAttack.started += OnAttack;
        _inputBuffer.PlayerAbility.started += OnAbility;
        Debug.Log("Input登録完了");
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
        Debug.Log($"移動入力: {_moveDirection}");
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

    private void ApplyRepositoryAbilities()
    {
        if (_abilityRepository == null && _abilityRepository == null)
        {
            // もしシステムに別名で登録されている場合に備え、ServiceLocator から取得しておく
            ServiceLocator.TryGetInstance<AbilityRepository>(out _abilityRepository);
        }

        if (_abilityRepository == null) return;

        // ゴースト能力：指定された敵IDからヒット登録があれば一度だけ付与
        if (!_repoGhostApplied && _ghostSourceEnemyId >= 0 && _abilityRepository.HasRegistered(_ghostSourceEnemyId))
        {
            if (_ghostInstance == null) _ghostInstance = new GhostAbility(_config);
            _abilityManager.SetActive(_ghostInstance);
            _repoGhostApplied = true;
            Debug.Log($"Repository によりゴースト能力を付与（敵ID: {_ghostSourceEnemyId}）");
        }

        // 貫通能力：指定された敵IDからヒット登録があれば一度だけパッシブ追加
        if (!_repoPenetrationApplied && _penetrationSourceEnemyId >= 0 && _abilityRepository.HasRegistered(_penetrationSourceEnemyId))
        {
            if (_penetrationInstance == null) _penetrationInstance = new PenetrationAbility(_config.PenetrationCount);
            if (!_penetrationAdded)
            {
                _abilityManager.AddPassive(_penetrationInstance);
                _penetrationAdded = true;
            }
            _repoPenetrationApplied = true;
            Debug.Log($"Repository により貫通能力を付与（敵ID: {_penetrationSourceEnemyId}）");
        }
    }
}
