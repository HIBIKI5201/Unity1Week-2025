using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AblityName",menuName =("ScriptableObjects/AblityName"))]
public class AblityName : ScriptableObject
{
    [SerializeField] private AblityTypes[] AblityEntries;
    private Dictionary<AblityType, string> _cache;

    private void OnEnable()
    {
        _cache = new Dictionary<AblityType, string>();
        foreach (var e in AblityEntries)
        {
            if (!_cache.ContainsKey(e.Ability))
                _cache.Add(e.Ability, e.Name);
        }
    }

    public string GetName(AblityType type)
    {
        if (_cache != null && _cache.TryGetValue(type, out var name))
            return name;

        return type.ToString();
    }
}

[Serializable]
public class AblityTypes
{
    public string Name;
    public AblityType Ability;
}
