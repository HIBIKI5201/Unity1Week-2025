using System.Collections;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _spawnAmount = 5;
    [SerializeField] private float _intervalSeconds = 0.5f;

    /// <summary>
    /// Signal Receiver から呼ばれるスポーン開始
    /// </summary>
    public void StartSpawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// 設定された内容で敵を一定間隔で生成する
    /// </summary>
    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            Instantiate(
               _enemyPrefab,
                transform.position,
                Quaternion.identity);

            yield return new WaitForSeconds(_intervalSeconds);
        }
    }
}
