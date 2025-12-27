using SymphonyFrameWork.System;
using System;
using UnityEngine;

public class BootSystemTrap : MonoBehaviour
{
    [SerializeField] private string _sceneName = "BootSystemTrapScene";
    private void Awake()
    {
        SceneLoader.LoadScene(_sceneName);
    }
}
