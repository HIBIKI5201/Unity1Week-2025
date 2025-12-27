using SymphonyFrameWork.System;
using Unity.Cinemachine;
using UnityEngine;

public class InGameInit : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _cameraMoveSpeed = 2f;
    private Camera _camera;

    private void Awake()
    {
        _cameraMover.Init(_cameraMoveSpeed);
        _camera = ServiceLocator.GetInstance<Camera>();
        _playerController.Init(_config, _camera, _cameraMover);
    }
}
