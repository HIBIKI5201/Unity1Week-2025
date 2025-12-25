using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    private InputBuffer _inputBuffer;
    private PlayerMover _playerMover;
    private PlayerCollision _playerCollision;
    private Vector2 _moveDirection;
    private EntityManager _em;


    private void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _inputBuffer = GetComponent<InputBuffer>();
        InitialRegistration();
        _playerMover = new PlayerMover(_config, transform);
        _playerCollision = new PlayerCollision(_em, transform, _config);
    }

    private void OnDestroy()
    {
        UnRegistrantion();
    }

    private void Update()
    {
        _playerMover.OnMove(_moveDirection, Time.deltaTime);

        if (Mouse.current.leftButton.isPressed)
        {
            _em.Shoot(0, transform.position, transform.forward);
        }
    }

    private void LateUpdate()
    {
        if (_playerCollision.LateUpdate())
        {
            Debug.Log("Dead");
        }
    }

    private void InitialRegistration()
    {
        _inputBuffer.PlayerMove.performed += OnMove;
        _inputBuffer.PlayerMove.canceled += OnMove;
    }

    private void UnRegistrantion()
    {
        _inputBuffer.PlayerMove.performed -= OnMove;
        _inputBuffer.PlayerMove.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

}
