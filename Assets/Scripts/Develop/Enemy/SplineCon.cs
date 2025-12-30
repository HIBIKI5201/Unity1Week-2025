using UnityEngine;
using UnityEngine.Splines;

public class SplineCon : MonoBehaviour
{
    public SplineContainer Wave => _wave;
    public SplineContainer Boss => _boss;
    public SplineContainer Group => _group;

    private SplineContainer _wave;
    private SplineContainer _boss;
    private SplineContainer _group;

    private void Awake()
    { 
        _wave = FindObj("WaveSpline");
        _boss = FindObj("BossSpline");
        _group = FindObj("GroupSpline");
        Debug.Log("SplineContainer initialized.");

    }

    private SplineContainer FindObj(string name)
    {
        GameObject target = GameObject.Find(name);
        SplineContainer targetCon;
        if (target != null)
        {
            targetCon = target.GetComponent<SplineContainer>();
            return targetCon;
        }
        else
        {
            Debug.LogError("Cannot find object with name: " + name);
            return null;
        }

    }


}
