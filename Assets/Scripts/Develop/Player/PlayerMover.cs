using UnityEngine;

public class PlayerMover
{
    public PlayerMover(PlayerConfig config, Transform transform, Collider collider, Camera camera)
    {
        _config = config;
        _transform = transform;
        _collider = collider;
        _camera = camera;
    }

    public void OnMove(Vector2 inputVector, Vector3 cameraScrollVelocity, float deltaTimed)
    {
        Vector3 inputMove = new Vector3(inputVector.x, inputVector.y, 0f) * _config.MoveSpeed;
        Vector3 scrollMove = new Vector3(0f, cameraScrollVelocity.y, 0f);
        Vector3 nextPosition = _transform.position + (inputMove + scrollMove) * deltaTimed;
        nextPosition = ClampPositionToCamera(nextPosition);
        nextPosition.z = _transform.position.z;
        _transform.position = nextPosition;
    }

    private readonly PlayerConfig _config;
    private readonly Transform _transform;
    private readonly Collider _collider;
    private readonly Camera _camera;

    private Vector3 ClampPositionToCamera(Vector3 position)
    {
        float depth = Mathf.Abs(_camera.transform.position.z - position.z);
        // カメラの表示範囲（ワールド座標）
        Vector3 min = _camera.ViewportToWorldPoint(new Vector3(0, 0, depth));
        Vector3 max = _camera.ViewportToWorldPoint(new Vector3(1, 1, depth));
        // プレイヤーの当たり判定サイズ
        Vector3 extents = _collider.bounds.extents;

        position.x = Mathf.Clamp(position.x, min.x + extents.x, max.x - extents.x);
        position.y = Mathf.Clamp(position.y, min.y + extents.y, max.y - extents.y);

        return position;
    }
}