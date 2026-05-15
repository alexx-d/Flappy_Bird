using System;
using UnityEngine;

[RequireComponent(typeof(BirdMover))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(ScoreCounter))]
[RequireComponent(typeof(CollisionHandler))]
public class Bird : MonoBehaviour
{
    private BirdMover _birdMover;
    private PlayerInput _playerInput;
    private Shooter _shooter;
    private ScoreCounter _scoreCounter;
    private CollisionHandler _handler;

    public event Action GameOver;

    private void Awake()
    {
        _scoreCounter = GetComponent<ScoreCounter>();
        _handler = GetComponent<CollisionHandler>();
        _birdMover = GetComponent<BirdMover>();
        _playerInput = GetComponent<PlayerInput>();
        _shooter = GetComponent<Shooter>();
    }

    private void OnEnable()
    {
        _handler.CollisionDetected += ProcessCollision;

        _playerInput.Jumped += _birdMover.Jump;
        _playerInput.Attacked += _shooter.Shoot;
    }

    private void OnDisable()
    {
        _handler.CollisionDetected -= ProcessCollision;

        _playerInput.Jumped -= _birdMover.Jump;
        _playerInput.Attacked -= _shooter.Shoot;
    }

    private void ProcessCollision(IInteractable interactable)
    {
        if (interactable is Enemy || interactable is Bullet)
        {
            GameOver?.Invoke();
        }
    }

    public void Init(ObjectPool<Bullet> bulletPool)
    {
        _shooter.SetPool(bulletPool);
    }

    public void Reset()
    {
        _scoreCounter.Reset();
        _birdMover.Reset();
    }
}