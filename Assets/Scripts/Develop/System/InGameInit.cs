using SymphonyFrameWork.System;
using UnityEngine;

public class InGameInit : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AbilityMap _abilityMap;
    [SerializeField] private float _cameraMoveSpeed = 2f;
    private Camera _camera;
    private AblityRepository _abilityRepository;

    private void Awake()
    {
        _abilityRepository = ServiceLocator.GetInstance<AblityRepository>();
        // ScriptableObject で定義したマップがあれば登録
        if (_abilityMap != null && _abilityMap.Entries != null)
        {
            foreach (var e in _abilityMap.Entries)
            {
                _abilityRepository.RegisterMapping(e.EnemyId, e.Ability);
            }
        }
        _cameraMover.Init(_cameraMoveSpeed);
        _camera = ServiceLocator.GetInstance<Camera>();
        _playerController.Init(_config, _camera, _cameraMover);
    }
}
