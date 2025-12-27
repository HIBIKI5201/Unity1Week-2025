using SymphonyFrameWork.System;
using UnityEngine;

public class PlayerDead
{
    public PlayerDead(PlayerConfig playerConfig)
    {
        _sceneName = playerConfig.TitleName;
    }

    private string _sceneName;

    public void OnDead()
    {
        SceneLoader.LoadScene(_sceneName);
    }
}
