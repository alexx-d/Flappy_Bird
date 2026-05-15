using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Bird _bird;
    [SerializeField] private ObjectPool<Bullet> _playerBulletPool;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndGameScreen _endGameScreen;

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        _bird.GameOver += OnGameOver;

        _enemyGenerator.EnemyKilled += _scoreCounter.Add;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClicked -= OnPlayButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
        _bird.GameOver -= OnGameOver;

        _enemyGenerator.EnemyKilled -= _scoreCounter.Add;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _endGameScreen.Close();
        _startScreen.Open();

        _bird.Init(_playerBulletPool);
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;

        _enemyGenerator.Reset();
        _playerBulletPool.Reset();

        _endGameScreen.Open();
    }

    private void OnRestartButtonClick()
    {
        _endGameScreen.Close();
        StartGame();
    }
    private void OnPlayButtonClick()
    {
        _startScreen.Close();
        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1;

        _enemyGenerator.Reset();
        _bird.Reset();
        _enemyGenerator.StartSpawn();
    }
}