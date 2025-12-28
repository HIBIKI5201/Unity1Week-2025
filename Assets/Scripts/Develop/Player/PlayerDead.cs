using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDead
{
    public PlayerDead(PlayerConfig playerConfig)
    {
        _sceneName = playerConfig.TitleName;
        _undeadName = playerConfig.InGameName;
    }

    private string _sceneName;
    private string _undeadName;

    public void OnDead()
    {
        SceneLoader.UnloadScene(_undeadName);
        Debug.Log("PlayerDead: Load Title Scene");
        SceneLoader.LoadScene(_sceneName);
        Debug.Log("PlayerDead: Title Scene Loaded");
    }
}
