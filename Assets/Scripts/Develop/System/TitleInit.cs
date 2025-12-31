using SymphonyFrameWork.System;
using TMPro;
using UnityEngine;

public class TitleInit : MonoBehaviour
{
    private AudioManager _audioManager;
    private void Awake()
    {
        if (!ServiceLocator.TryGetInstance<AbilityRepository>(out _))
        {
            ServiceLocator.RegisterInstance(new AbilityRepository(), ServiceLocator.LocateType.Locator);
        }
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _audioManager.StopAllAudioIfPlaying();
        _audioManager.PlayBGM("Title");
    }
}
