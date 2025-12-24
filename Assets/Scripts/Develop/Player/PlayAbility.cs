using UnityEngine;

public class PlayAbility : MonoBehaviour
{

}

public enum AbilityType
{
    None = 0,
    SpeedBoost = 1 << 0,    //1
    Shield = 1 << 1,    //2
    DoubleDamage = 1 << 2   //4
}
