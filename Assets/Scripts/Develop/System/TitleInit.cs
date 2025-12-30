using SymphonyFrameWork.System;
using UnityEngine;

public class TitleInit : MonoBehaviour
{
    private void Awake()
    {
        if (!ServiceLocator.TryGetInstance<AblityRepository>(out _))
        {
            ServiceLocator.RegisterInstance(new AblityRepository(), ServiceLocator.LocateType.Locator);
        }
    }
}
