using SymphonyFrameWork.System;
using UnityEngine;

public class InGameInit : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AblityMap _abilityMap;
    [SerializeField] private float _cameraMoveSpeed = 2f;
    private Camera _camera;
    private AbilityRepository _abilityRepository;
    private AudioManager _audioManager;

    private void Awake()
    {
        if (!ServiceLocator.TryGetInstance(out _abilityRepository))
        {
            _abilityRepository = new AbilityRepository();
            ServiceLocator.RegisterInstance(_abilityRepository, ServiceLocator.LocateType.Locator);
        }
        // ScriptableObject で定義したマップがあれば登録
        if (_abilityMap != null && _abilityMap.Entries != null)
        {
            foreach (var e in _abilityMap.Entries)
            {
                _abilityRepository.RegisterMapping(e.EnemyId, e.Ablity);
            }
        }
        _cameraMover.Init(_cameraMoveSpeed);
        _camera = ServiceLocator.GetInstance<Camera>();
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _playerController.Init(_config, _camera, _cameraMover);
        _audioManager.StopAllAudioIfPlaying();
        _audioManager.PlayBGM("InGame");
    }
}
