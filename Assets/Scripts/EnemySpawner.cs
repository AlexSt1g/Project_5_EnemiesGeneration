using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Enemy _prefab;    

    private float _repeatRate = 2f;
    private int _poolCapacity = 30;
    private int _poolMaxSize = 30;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (enemy) => EnableEnemy(enemy),
            actionOnRelease: (enemy) => DisableEnemy(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(RepeatGetEnemy(_repeatRate));
    }

    private IEnumerator RepeatGetEnemy(float repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (enabled)
        {
            GetEnemy();
            yield return wait;
        }
    }

    private void EnableEnemy(Enemy enemy)
    {        
        enemy.transform.position = GetSpawnPosition();
        enemy.SetMovementDirection(GetDirection());

        enemy.gameObject.SetActive(true);

        enemy.Died += ReleaseEnemy;
    }

    private Vector3 GetSpawnPosition()
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        return spawnPoint.position;
    }

    private Vector3 GetDirection()
    {
        float vectorComponentMaxValue = 1f;        

        return new Vector3(Random.Range(-vectorComponentMaxValue, vectorComponentMaxValue), 0, Random.Range(-vectorComponentMaxValue, vectorComponentMaxValue)).normalized;
    }

    private void DisableEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.Died -= ReleaseEnemy;
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
    }
}
