using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアクティブおよびパッシブアビリティを管理するコンポーネント。
/// </summary>
public class AbilityManager : MonoBehaviour
{
    private IActiveAbility _active;
    private readonly List<IPassiveAbility> _passives = new();

    /// <summary>
    /// アクティブアビリティを設定。
    /// </summary>
    /// <param name="ability">設定するアクティブアビリティ</param>
    public void SetActive(IActiveAbility ability)
    {
        _active = ability;
    }

    /// <summary>
    /// パッシブアビリティを追加。
    /// </summary>
    /// <param name="ability">追加するパッシブアビリティ</param>
    public void AddPassive(IPassiveAbility ability)
    {
        _passives.Add(ability);
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
    /// <param name="index">弾のインデックス</param>
    /// <param name="pos">発射位置</param>
    /// <param name="forward">発射方向（正規化ベクトル）</param>
    /// <returns>構築された BulletContext</returns>
    public BulletContext BuildBulletContext(
        int index,
        Unity.Mathematics.float3 pos,
        Unity.Mathematics.float3 forward)
    {
        var ctx = new BulletContext
        {
            Index = index,
            Position = pos,
            Forward = forward,
            Count = 1
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