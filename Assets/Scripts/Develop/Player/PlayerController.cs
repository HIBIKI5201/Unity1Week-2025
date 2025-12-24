using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    private InputBuffer _inputBuffer;
    private PlayerMover _playerMover;
    private Vector2 _moveDirection;


    private void Start()
    {
        _inputBuffer = GetComponent<InputBuffer>();
        InitialRegistration();
        _playerMover = new PlayerMover(_config, transform);
    }

    private void OnDestroy()
    {
        UnRegistrantion();
    }

    private void Update()
    {
        _playerMover.OnMove(_moveDirection, Time.deltaTime);
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

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _moveDirection = Vector2.zero;
        }
    }
}
