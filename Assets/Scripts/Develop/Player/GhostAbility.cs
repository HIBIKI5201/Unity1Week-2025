using UnityEngine;

/// <summary>
/// プレイヤーのゴースト（無敵）アビリティを表します。
/// PlayerConfig を参照してアクティブ時間とクールダウン時間を取得するように変更しました。
/// </summary>
public class GhostAbility : IActiveAbility
{
    private readonly PlayerConfig _config;

    private float _activeTime;
    private float _coolTime;

    private float _timer;
    private bool _active;
    private bool _cooling;

    /// <summary>
    /// コンストラクタ。PlayerConfig からタイミング値を初期化。
    /// </summary>
    /// <param name="config">プレイヤー設定（null の場合はデフォルト値を使用）</param>
    public GhostAbility(PlayerConfig config)
    {
        _config = config;
        // null チェックして安全に初期化
        _activeTime = _config != null ? _config.GhostTime : 3f;
        _coolTime = _config != null ? _config.GhostAbilityCoolTime : 5f;
    }

    /// <summary>
    /// 発動可能かを返します（現在アクティブでも冷却中でもない場合）。
    /// </summary>
    public bool CanActivate => !_active && !_cooling;

    /// <summary>
    /// アビリティを発動します。アクティブ時間のカウントを開始。
    /// </summary>
    public void Activate()
    {
        _active = true;
        _timer = _activeTime;
    }

    /// <summary>
    /// 毎フレームの時間経過を処理します。アクティブ終了後にクールダウンを開始。
    /// </summary>
    /// <param name="dt">経過時間（秒）</param>
    public void Tick(float dt)
    {
        if (_active)
        {
            _timer -= dt;
            if (_timer <= 0f)
            {
                _active = false;
                _cooling = true;
                _timer = _coolTime;
            }
        }
        else if (_cooling)
        {
            _timer -= dt;
            if (_timer <= 0f)
                _cooling = false;
        }
    }
}