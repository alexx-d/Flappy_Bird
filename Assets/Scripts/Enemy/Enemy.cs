using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(CollisionHandler))]
public class Enemy : MonoBehaviour, IInteractable
{
    [SerializeField] private float _fireDelay = 2f;
    
    private Shooter _shooter;
    private CollisionHandler _handler;
    private Coroutine _shootingCoroutine;

    public event Action<Enemy> Died;

    private void Awake()
    {
        _shooter = GetComponent<Shooter>();
        _handler = GetComponent<CollisionHandler>();
    }

    private void OnEnable()
    {
        _handler.CollisionDetected += ProcessCollision;

        _shootingCoroutine = StartCoroutine(ShootCycle());
    }

    private void OnDisable()
    {
        _handler.CollisionDetected -= ProcessCollision;

        if (_shootingCoroutine is not null)
        {
            StopCoroutine(_shootingCoroutine);
        }  
    }

    public void Init(ObjectPool<Bullet> bulletPool)
    {
        if (_shooter == null)
        {
            _shooter = GetComponent<Shooter>();
        }

        _shooter.SetPool(bulletPool);
    }

    private IEnumerator ShootCycle()
    {
        yield return new WaitUntil(() => _shooter is not null && _shooter.HasPool);

        var wait = new WaitForSeconds(_fireDelay);

        while (enabled)
        {
            yield return wait;
            _shooter.Shoot();
        }
    }

    private void ProcessCollision(IInteractable interactable)
    {
        if (interactable is Bullet)
        {
            Died?.Invoke(this);
        }
    }
}