using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


public class EnemySpawn:MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    private float _timer =  0;
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 0.5f)
        {
            Instantiate(_enemyPrefab);
            _timer = 0;
        }
    }
}