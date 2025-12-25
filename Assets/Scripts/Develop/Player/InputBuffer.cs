using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBuffer : MonoBehaviour
{
    private const string PLAYER_MOVE = "Move";
    private const string PLAYER_ATTACK = "Attack";

    public InputAction PlayerMove => _playerMove;
    public InputAction PlayerAttack => _playerAttack;
    private InputAction _playerMove;
    private InputAction _playerAttack;

    private void Awake()
    {
        if(TryGetComponent<PlayerInput>(out var playerInput))
        {
            _playerMove = playerInput.actions[PLAYER_MOVE];
            _playerAttack = playerInput.actions[PLAYER_ATTACK];
        }
    }
}
