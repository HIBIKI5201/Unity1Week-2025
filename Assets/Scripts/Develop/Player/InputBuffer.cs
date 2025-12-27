using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBuffer : MonoBehaviour
{
    private const string PLAYER_MOVE = "Move";
    private const string PLAYER_ATTACK = "Attack";
    private const string PLAYER_ABILITY = "Ability";

    public InputAction PlayerMove => _playerMove;
    public InputAction PlayerAttack => _playerAttack;
    public InputAction PlayerAbility => _playerAbility;
    private InputAction _playerMove;
    private InputAction _playerAttack;
    private InputAction _playerAbility;

    private void Awake()
    {
        if(TryGetComponent<PlayerInput>(out var playerInput))
        {
            _playerMove = playerInput.actions[PLAYER_MOVE];
            _playerAttack = playerInput.actions[PLAYER_ATTACK];
            _playerAbility = playerInput.actions[PLAYER_ABILITY];
        }
    }
}
