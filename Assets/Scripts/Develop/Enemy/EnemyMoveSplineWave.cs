using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyMoveSplineWave : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _waveAmplitude = 1f; // ウェーブの振幅
    [SerializeField] private float _waveFrequency = 1f; // ウェーブの周波数

    private float _splineLength = 0f;
    private float _t = 0f;
    private float _waveTime = 0f;

    private void Start()
    {
        if (_splineContainer == null) return;
        _splineLength = _splineContainer.CalculateLength();
    }
    
    private void Update()
    {
        if (_splineContainer == null || _t >= 1f) return;

        // Spline上の進行
        _t += (_speed / _splineLength) * Time.deltaTime;
        // 1を超えたら終了してDestroy
        if (_t >= 1f)
        {
            Destroy(gameObject); // オブジェクトを削除
            return;
        }
        _t = Mathf.Clamp01(_t);

        // Spline上の位置（float3）
        float3 position = _splineContainer.EvaluatePosition(_t);

        // 進行方向（float3）
        float3 forward = math.normalize(_splineContainer.EvaluatePosition(math.min(_t + 0.01f, 1f)) - position);

        // 右方向（float3）
        float3 right = math.normalize(math.cross(new float3(0, 1, 0), forward));

        // サイン波オフセット
        _waveTime += Time.deltaTime * _waveFrequency;
        float3 waveOffset = right * math.sin(_waveTime * math.PI * 2f) * _waveAmplitude;

        // Transformに代入するときだけVector3に変換
        transform.position = (Vector3)(position + waveOffset);

        transform.rotation = Quaternion.LookRotation((Vector3)right, Vector3.up);
    }

}