using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBuffer : MonoBehaviour
{
    private const string PLAYER_MOVE = "Move";

    public InputAction PlayerMove => PlayerMove;
    private InputAction _playerMove;

    private void Awake()
    {
        if(TryGetComponent<PlayerInput>(out var playerInput))
        {
            _playerMove = playerInput.actions[PLAYER_MOVE];
        }
    }
}
