using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public Action OnGameReset;

    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameStateManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("StatsManager");
                    _instance = go.AddComponent<GameStateManager>();
                }
            }
            return _instance;
        }
    }

    public enum GameState
    {
        TUTO,
        IN_OFFICE,
        IN_BOSS_OFFICE,
        DEAD,
        FIRED //viré (le mauvais développeur)
    }

    private readonly GameState DEFAULT_GAME_STATE = GameState.IN_OFFICE;
    private GameState _currentGameState;
    public event Action<GameState, GameState> OnGameStatusUpdated;

    public GameState GameStatus
    {
        get { return _currentGameState; }
        set
        {
            GameState oldGameState = _currentGameState;
            _currentGameState = value;

            switch (_currentGameState)
            {
                case GameState.FIRED:
                case GameState.DEAD:
                    Time.timeScale = 0f;
                    break;
                case GameState.TUTO:
                    GameStatus = GameState.IN_OFFICE; //TODO faire le TUTO
                    break;
                default:
                    Time.timeScale = 1f;
                    break;
            }

            OnGameStatusUpdated?.Invoke(_currentGameState, oldGameState);
        }
    }

    public void ResetGame()
    {
        StatsManager.Instance.Reset();
        OnGameReset.Invoke();
        GameStatus = GameState.TUTO;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStatus = DEFAULT_GAME_STATE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
