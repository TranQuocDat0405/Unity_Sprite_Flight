using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }


    public GameState CurrentState { get; private set; }

    public static event Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
    }

    private void Start()
    {
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;


        switch(newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
        onGameStateChanged?.Invoke(newState);
    }
}