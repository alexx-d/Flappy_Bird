using System;
using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _lowerBound;
    [SerializeField] private float _upperBound;
    [SerializeField] private ObjectPool<Enemy> _enemyPool;
    [SerializeField] private ObjectPool<Bullet> _bulletPool;

    private Coroutine _spawnCoroutine;

    public event Action EnemyKilled;

    public void StartSpawn()
    {
        StopSpawn(); 
        _spawnCoroutine = StartCoroutine(GeneratePipes());
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine is not null)
        {
            StopCoroutine(_spawnCoroutine);
        }  
    }

    public void Reset()
    {
        StopSpawn();
        _enemyPool.Reset();
        _bulletPool.Reset();
    }

    private IEnumerator GeneratePipes()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled) 
        {
            Spawn();
            yield return wait;
        }
    }

    private void Spawn()
    {
        float spawnPositionY = UnityEngine.Random.Range(_upperBound, _lowerBound);
        Vector3 spawnPoint = new Vector3(transform.position.x, spawnPositionY, transform.position.z);

        Enemy enemy = _enemyPool.GetObject();
        enemy.Init(_bulletPool);

        enemy.transform.position = spawnPoint;
        enemy.gameObject.SetActive(true);

        enemy.Died -= OnEnemyDied;
        enemy.Died += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;

        EnemyKilled?.Invoke();

        _enemyPool.PutObject(enemy);
    }
}