using SymphonyFrameWork.System;
using UnityEngine;

public class TitleInit : MonoBehaviour
{
    private void Awake()
    {
        if (!ServiceLocator.TryGetInstance<AbilityRepository>(out _))
        {
            ServiceLocator.RegisterInstance(new AbilityRepository(), ServiceLocator.LocateType.Locator);
        }
    }
}
