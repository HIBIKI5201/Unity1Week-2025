using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアクティブおよびパッシブアビリティを管理するコンポーネント。
/// </summary>
public class AbilityManager
{
    private IActiveAbility _active;
    private readonly List<IPassiveAbility> _passives = new();

    /// <summary>
    /// アクティブアビリティを設定。
    /// 既に設定されているアクティブは Deactivate() を呼んで安全に停止する。
    /// </summary>
    /// <param name="ability">設定するアクティブアビリティ（null で解除）</param>
    public void SetActive(IActiveAbility ability)
    {
        if (_active != null)
        {
            // 古いアクティビティを安全に停止してから差し替える
            _active.Deactivate();
        }

        _active = ability;
    }

    /// <summary>
    /// パッシブアビリティを追加。null または既に追加済みのインスタンスは無視する。
    /// </summary>
    /// <param name="ability">追加するパッシブアビリティ</param>
    public void AddPassive(IPassiveAbility ability)
    {
        if (ability == null) return;
        if (_passives.Contains(ability)) return;
        _passives.Add(ability);
    }

    /// <summary>
    /// 指定のパッシブアビリティを削除（同一インスタンスを比較して削除）。
    /// </summary>
    /// <param name="ability">削除するパッシブアビリティ</param>
    public void RemovePassive(IPassiveAbility ability)
    {
        if (ability == null) return;
        _passives.Remove(ability);
    }

    /// <summary>
    /// アクティブアビリティを発動。
    /// </summary>
    public void Activate()
    {
        if (_active?.CanActivate == true)
            _active.Activate();
    }

    /// <summary>
    /// フレーム毎の更新処理を行います。アクティブアビリティの Tick を呼び出す。
    /// </summary>
    /// <param name="dt">経過時間（秒）</param>
    public void Tick(float dt)
    {
        _active?.Tick(dt);
    }

    /// <summary>
    /// 弾発射時の BulletContext を構築し、登録されたパッシブアビリティに変更を適用する。
    /// </summary>
    public BulletContext BuildBulletContext(int index, Unity.Mathematics.float3 pos, Unity.Mathematics.float3 forward)
    {
        var ctx = new BulletContext
        {
            Index = index,
            Position = pos,
            Forward = forward,
            Count = 1,
            Penetration = 0
        };

        // 各パッシブで発射時のコンテキストを変更する
        foreach (var p in _passives)
            p.OnShoot(ref ctx);

        return ctx;
    }

    /// <summary>
    /// ヒット時にパッシブアビリティへ通知を行う。
    /// </summary>
    /// <param name="ctx">ヒット情報（参照で受け渡され、パッシブで変更される可能性があります）</param>
    public void OnHit(ref HitContext ctx)
    {
        // 各パッシブにヒット情報を渡して処理させる
        foreach (var p in _passives)
            p.OnHit(ref ctx);
    }
}