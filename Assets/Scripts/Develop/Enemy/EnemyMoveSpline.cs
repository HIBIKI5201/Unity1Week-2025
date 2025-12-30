using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Splines;
public enum SplineType
{
    None,
    Wave,
    Boss,
    Group
}
public class EnemyMoveSpline : MonoBehaviour
{
    public SplineType SplineType => _currentType;
     private SplineCon _splineCon;
    [SerializeField] private float _speed = 5f;
    private SplineContainer _splineContainer;
    [SerializeField]
    private SplineType _currentType = SplineType.None;

    private float _splineLength = 0f;
    // 0〜1の範囲で進捗を管理。
    private float _t = 0f;

    private void Start()
    {
        _splineCon = FindAnyObjectByType<SplineCon>();
        if (_splineCon == null) return;
        _splineContainer = _currentType switch
        {
            SplineType.Wave => _splineCon.Wave,
            SplineType.Boss => _splineCon.Boss,
            SplineType.Group => _splineCon.Group,
            _ => null
        };
        Debug.Log(_currentType);
        _splineLength = _splineContainer.CalculateLength();
    }

    private void Update()
    {
        if (_splineContainer == null) return;
        if (_t >= 1f) return;
        // 正規化された進行度を加算
        _t += (_speed / _splineLength) * Time.deltaTime;
        _t = Mathf.Clamp01(_t);
        Vector3 position = _splineContainer.EvaluatePosition(_t);
        transform.position = position;
    }
}
