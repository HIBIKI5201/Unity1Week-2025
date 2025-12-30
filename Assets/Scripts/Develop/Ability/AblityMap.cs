using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityMap", menuName = "ScriptableObjects/AbilityMap", order = 0)]
public class AblityMap : ScriptableObject
{
    public AblityEntry[] Entries;
}

[Serializable]
public enum AbilityType
{
    None,
    Ghost,
    Penetration
}

[Serializable]
public class AblityEntry
{
    public int EnemyId;
    public AbilityType Ablity;
}
