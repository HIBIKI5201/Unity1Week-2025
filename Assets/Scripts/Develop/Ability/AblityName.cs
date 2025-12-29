using System;
using UnityEngine;

[CreateAssetMenu(fileName ="AblityName",menuName =("ScriptableObjects/AblityName"))]
public class AblityName : ScriptableObject
{
    public AblityTypes[] AblityEntries;
}

[Serializable]
public class AblityTypes
{
    public string Name;
    public AbilityType Ability;
}
