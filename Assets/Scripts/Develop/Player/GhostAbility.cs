using UnityEngine;

/// <summary>
/// プレイヤーのゴースト（無敵）アビリティを表します。
/// PlayerConfig を参照してアクティブ時間とクールダウン時間を取得するように変更しました。
/// </summary>
public class GhostAbility : IActiveAbility
{
    /// <summary>
    /// コンストラクタ。PlayerConfig からタイミング値を初期化。
    /// </summary>
    /// <param name="config">プレイヤー設定（null の場合はデフォルト値を使用）</param>
    public GhostAbility(PlayerConfig config)
    {
        _config = config;
        _cooldownTimer=_config.GhostAbilityCoolTime;
    }
    private readonly PlayerConfig _config;
    private float _timer;
    private float _limitTimer;
    private float _cooldownTimer;
    private bool _active;
    private bool _cooling;

    /// <summary>
    /// 発動可能かを返します（現在アクティブでも冷却中でもない場合）。
    /// </summary>
    public bool CanActivate => !_active && !_cooling;

    /// <summary>
    /// 現在アクティブ状態かどうかを返します（外部から無敵状態のチェックに利用可能）。
    /// </summary>
    public bool IsActive => _active;

    /// <summary>
    /// アビリティを発動します。アクティブ時間のカウントを開始。
    /// </summary>
    public void Activate(float time)
    {
        _active = true;
        _timer = time;
        _limitTimer = time + _config.GhostTime;
    }

    /// <summary>
    /// 毎フレームの時間経過を処理します。アクティブ終了後にクールダウンを開始。
    /// </summary>
    /// <param name="dt">経過時間（秒）</param>
    public void Tick(float dt)
    {
        if (_active)
        {
            if (_timer <= _limitTimer)
            {
                _active = false;
                _cooling = true;
            }
        }
        else
        {

        }
    }
}