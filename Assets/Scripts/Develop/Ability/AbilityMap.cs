using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityMap", menuName = "ScriptableObjects/AbilityMap", order = 0)]
public class AbilityMap : ScriptableObject
{
    public AbilityEntry[] Entries;
}

[Serializable]
public enum AbilityType
{
    None,
    Ghost,
    Penetration
}

[Serializable]
public class AbilityEntry
{
    public int EnemyId;
    public AbilityType Ability;
}
