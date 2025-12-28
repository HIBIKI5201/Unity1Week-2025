using System;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    private float _deltaTime;
    private void Update()
    {
        _deltaTime += Time.deltaTime;
        if (_deltaTime > 1)
        {
            Instantiate(_enemyPrefab);
            _deltaTime = 0;
        }
       
    }
}
