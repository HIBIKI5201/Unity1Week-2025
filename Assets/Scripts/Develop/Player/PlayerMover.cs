using UnityEngine;

public class PlayerMover
{
    public PlayerMover(PlayerConfig config, Transform transform)
    {
        _config = config;
        _transform = transform;
    }

    public void OnMove(Vector2 inputVector, float deltaTimed)
    {
        Vector3 moveVector = new Vector3(inputVector.x, inputVector.y, 0);
        _transform.position += moveVector * _config.MoveSpeed * deltaTimed;
    }

    private readonly PlayerConfig _config;
    private readonly Transform _transform;
}
