using SymphonyFrameWork.System;
using UnityEngine;

public class PlayerDead
{
    public PlayerDead(PlayerConfig playerConfig)
    {
        _sceneName = playerConfig.TitleName;
        _undeadName = playerConfig.InGameName;
    }

    private readonly string _sceneName;
    private readonly string _undeadName;

    public void OnDead()
    {
        if (ServiceLocator.TryGetInstance<AbilityRepository>(out var repository))
        {
            repository.GetAndConsumeMappedAbilities();
        }

        SceneLoader.UnloadScene(_undeadName);
        Debug.Log("PlayerDead: Load Title Scene");
        SceneLoader.LoadScene(_sceneName);
        Debug.Log("PlayerDead: Title Scene Loaded");
    }
}
