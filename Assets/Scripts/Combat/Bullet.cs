using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IInteractable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime = 3f;

    private Vector3 _direction;
    private Coroutine _timerCoroutine;

    public event Action<Bullet> NeedsToReturn;

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;

        StopTimer();
        _timerCoroutine = StartCoroutine(ReturnAfterDelay());
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReturnToPool();
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        StopTimer();
        NeedsToReturn?.Invoke(this);
    }

    private void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    private void OnDisable()
    {
        StopTimer();
    }
}