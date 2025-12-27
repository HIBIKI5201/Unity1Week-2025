using UnityEngine;

/// <summary>
/// プレイヤーのゴースト（無敵）アビリティを表します。
/// PlayerConfig を参照してアクティブ時間とクールダウン時間を取得する。
/// </summary>
public class GhostAbility : IActiveAbility
{
    private readonly PlayerConfig _config;
    private float _activeRemaining;
    private float _cooldownRemaining;
    private bool _active;
    private bool _cooling;

    /// <summary>
    /// コンストラクタ。PlayerConfig からタイミング値を初期化。
    /// config が null の場合は安全なデフォルトを使用する。
    /// </summary>
    /// <param name="config">プレイヤー設定（null 許容）</param>
    public GhostAbility(PlayerConfig config)
    {
        _config = config;
        _activeRemaining = 0f;
        _cooldownRemaining = 0f;
        _active = false;
        _cooling = false;
    }

    private float GetGhostTime() => _config != null ? _config.GhostTime : 0.5f;
    private float GetCoolTime() => _config != null ? _config.GhostAbilityCoolTime : 1.0f;

    /// <summary>
    /// 発動可能かを返します（現在アクティブでも冷却中でもない場合）。
    /// </summary>
    public bool CanActivate => !_active && !_cooling;

    /// <summary>
    /// 現在アクティブ状態かどうかを返します（外部から無敵状態のチェックに利用可能）。
    /// </summary>
    public bool IsActive => _active;

    /// <summary>
    /// アビリティを発動する。
    /// </summary>
    public void Activate()
    {
        if (!CanActivate)
            return;

        _active = true;
        _activeRemaining = GetGhostTime();
        _cooldownRemaining = GetCoolTime();
        _cooling = false;
        Debug.Log("Ghost Ability Activated");
    }

    /// <summary>
    /// 毎フレームの時間経過処理。アクティブ終了後にクールタイムへ移行し、クール終了で再発動可能になる。
    /// </summary>
    /// <param name="dt">経過時間（秒）</param>
    public void Tick(float dt)
    {
        if (_active)
        {
            _activeRemaining -= dt;
            if (_activeRemaining <= 0f)
            {
                _active = false;
                _cooling = true;
                // クールダウンは Activate 時に設定済み
                Debug.Log("Ghost Ability Ended, entering cooldown");
            }
        }
        else if (_cooling)
        {
            _cooldownRemaining -= dt;
            if (_cooldownRemaining <= 0f)
            {
                _cooling = false;
                _cooldownRemaining = 0f;
                Debug.Log("Ghost Ability Cooldown Ended, can activate again");
            }
        }
    }

    /// <summary>
    /// アクティビティを強制解除する。
    /// Inspector でオフにしたり Manager が切り替える際に呼ばれる。
    /// 本実装では解除時に状態をリセットして即再利用可能な状態にする。
    /// </summary>
    public void Deactivate()
    {
        _active = false;
        _cooling = false;
        _activeRemaining = 0f;
        _cooldownRemaining = 0f;
        Debug.Log("Ghost Ability Deactivated and reset");
    }
}