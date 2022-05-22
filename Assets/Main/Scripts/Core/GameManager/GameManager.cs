using DG.Tweening;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _gameState;
    public GameState GameState => _gameState;

    public static event Action<GameState> OnGameStateChange;
    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;

        switch (gameState)
        {
            case GameState.Loading:
                PlayerDataHandler.HandleCoin(-SaveManager.Player.coin);
                DOTween.KillAll();
                ObjectPoolProxy.DismissAll();
                break;
            case GameState.Idle:
                break;
            case GameState.Gameplay:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }

        OnGameStateChange?.Invoke(_gameState);
    }
    private void Awake()
    {
    }
    private void Start()
    {
        SetGameState(GameState.Loading);
    }

    private void OnApplicationQuit()
    {
        SaveManager.Save();
    }
}

public enum GameState
{
    Loading = 0,
    Idle = 1,
    Gameplay = 2,
    PostGameplay = 3,
    Fight = 4,
    Win = 5,
    Lose = 6
}