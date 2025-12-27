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
        Vector3 inputMove = new Vector3(inputVector.x, 0f, inputVector.y) * _config.MoveSpeed;
        Vector3 scrollMove = new Vector3(0f, 0f, cameraScrollVelocity.z);
        Vector3 nextPosition = _transform.position + (inputMove + scrollMove) * deltaTimed;
        nextPosition = ClampPositionToCamera(nextPosition);
        nextPosition.y = _transform.position.y;
        _transform.position = nextPosition;
    }

    private readonly PlayerConfig _config;
    private readonly Transform _transform;
    private readonly Collider _collider;
    private readonly Camera _camera;

    private Vector3 ClampPositionToCamera(Vector3 position)
    {
        float depth = Vector3.Dot(position - _camera.transform.position, _camera.transform.forward);
        // カメラの表示範囲（ワールド座標）
        Vector3 min = _camera.ViewportToWorldPoint(new Vector3(0, 0, depth));
        Vector3 max = _camera.ViewportToWorldPoint(new Vector3(1, 1, depth));
        // プレイヤーの当たり判定サイズ
        Vector3 extents = _collider.bounds.extents;

        position.x = Mathf.Clamp(position.x, min.x + extents.x, max.x - extents.x);
        position.z = Mathf.Clamp(position.z, min.z + extents.z, max.z - extents.z);

        return position;
    }
}