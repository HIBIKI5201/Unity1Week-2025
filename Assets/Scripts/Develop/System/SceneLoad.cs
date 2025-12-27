using SymphonyFrameWork.System;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneLoader.LoadScene(sceneName);
    }

    public void UnloadScene(string sceneName)
    {
        SceneLoader.UnloadScene(sceneName);
    }
}
