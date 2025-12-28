using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityMap", menuName = "Game/AbilityMap")]
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
