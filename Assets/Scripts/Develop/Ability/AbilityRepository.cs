using System.Collections.Generic;
using UnityEngine;

public class AbilityRepository : MonoBehaviour
{
    private readonly object _sync = new();
    private readonly HashSet<int> _registered = new();
    private readonly HashSet<int> _consumed = new();
    private readonly Dictionary<int, AbilityType> _mapping = new();

    /// <summary>
    /// 初回ヒットのみ登録する（既に登録/消費済みなら false）。
    /// </summary>
    public bool RegisterHitOnce(int enemyId)
    {
        lock (_sync)
        {
            if (_registered.Contains(enemyId) || _consumed.Contains(enemyId))
                return false;

            _registered.Add(enemyId);
            return true;
        }
    }

    /// <summary>
    /// enemyId -> AbilityType のマッピングを登録（起動時に ScriptableObject から呼ぶ）。
    /// </summary>
    public void RegisterMapping(int enemyId, AbilityType ability)
    {
        lock (_sync)
        {
            _mapping[enemyId] = ability;
        }
    }

    /// <summary>
    /// 未消費の登録済み enemyId をマッピングに従って列挙し、消費済みにマークして返す（1回限り）。
    /// </summary>
    public List<AbilityType> GetAndConsumeMappedAbilities()
    {
        lock (_sync)
        {
            var result = new List<AbilityType>();
            foreach (var id in _registered)
            {
                if (_consumed.Contains(id)) continue;
                if (_mapping.TryGetValue(id, out var ability) && ability != AbilityType.None)
                {
                    result.Add(ability);
                }
                _consumed.Add(id);
            }
            return result;
        }
    }

    /// <summary>
    /// 登録済み（履歴）を持っているか（反映済み／未反映を含む）。
    /// </summary>
    public bool HasRegistered(int enemyId)
    {
        lock (_sync)
        {
            return _registered.Contains(enemyId) || _consumed.Contains(enemyId);
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            _registered.Clear();
            _consumed.Clear();
            _mapping.Clear();
        }
    }
}
