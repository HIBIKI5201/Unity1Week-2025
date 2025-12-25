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

    public void OnMove(Vector2 inputVector, Vector3 camraScrollVelocity, float deltaTimed)
    {
        Vector3 inputMove = new Vector3(inputVector.x, 0, inputVector.y * _config.MoveSpeed);
        Vector3 nextPosition = _transform.position + (inputMove + camraScrollVelocity) * deltaTimed;
        nextPosition = ClampPositionToCamera(nextPosition);
        _transform.position = nextPosition;
    }

    private readonly PlayerConfig _config;
    private readonly Transform _transform;
    private readonly Collider _collider;
    private readonly Camera _camera;

    private Vector3 ClampPositionToCamera(Vector3 position)
    {
        // カメラの表示範囲（ワールド座標）
        Vector3 min = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector3 max = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        // プレイヤーの当たり判定サイズ
        Vector3 extents = _collider.bounds.extents;

        position.x = Mathf.Clamp(position.x, min.x + extents.x, max.x - extents.x);
        position.z = Mathf.Clamp(position.z, min.z + extents.z, max.z - extents.z);

        return position;
    }
}