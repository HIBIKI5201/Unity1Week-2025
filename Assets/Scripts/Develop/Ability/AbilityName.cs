using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AblityName",menuName =("ScriptableObjects/AblityName"))]
public class AbilityName : ScriptableObject
{
    [SerializeField] private AbilityTypes[] AbilityEntries;
    private Dictionary<AbilityType, string> _cache;

    private void OnEnable()
    {
        _cache = new Dictionary<AbilityType, string>();
        foreach (var e in AbilityEntries)
        {
            if (!_cache.ContainsKey(e.Ability))
                _cache.Add(e.Ability, e.Name);
        }
    }

    public string GetName(AbilityType type)
    {
        if (_cache != null && _cache.TryGetValue(type, out var name))
            return name;

        return type.ToString();
    }
}

[Serializable]
public class AbilityTypes
{
    public string Name;
    public AbilityType Ability;
}
