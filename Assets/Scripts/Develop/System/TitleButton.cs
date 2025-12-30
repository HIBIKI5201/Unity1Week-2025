using SymphonyFrameWork.System;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    private AudioManager _audioManager;
    public void OnClick(string SEName)
    {
        _audioManager.PlaySE(SEName);
    }
    private void Awake()
    {
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
    }
}
