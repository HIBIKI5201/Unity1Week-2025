using System.Collections.Generic;
using UnityEngine;

public class AbilityRepository
{
    private readonly object _sync = new();
    private readonly Dictionary<int, bool> _hitAbility = new();

    /// <summary>
    /// 敵ID に対して「一度だけ」ヒット登録します。
    /// 既に登録済みなら false を返し、未登録なら登録して true を返します。
    /// </summary>
    public bool RegisterHitOnce(int enemyId)
    {
        lock (_sync)
        {
            if (_hitAbility.ContainsKey(enemyId))
                return false;

            _hitAbility[enemyId] = true;
            return true;
        }
    }


    /// <summary>
    /// 指定の敵ID が既に登録されているかを返します。
    /// </summary>
    public bool HasRegistered(int enemyId)
    {
        lock (_sync)
        {
            return _hitAbility.ContainsKey(enemyId);
        }
    }

    /// <summary>
    /// 登録済みの敵ID の読み取り用コピーを返します。
    /// </summary>
    public IReadOnlyCollection<int> Snapshot()
    {
        lock (_sync)
        {
            return new List<int>(_hitAbility.Keys);
        }
    }

    /// <summary>
    /// 指定の敵ID の登録を解除します（必要なら）。
    /// </summary>
    public bool Remove(int enemyId)
    {
        lock (_sync)
        {
            return _hitAbility.Remove(enemyId);
        }
    }

    /// <summary>
    /// 全登録をクリアします。
    /// </summary>
    public void Clear()
    {
        lock (_sync)
        {
            _hitAbility.Clear();
        }
    }
}
