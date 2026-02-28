using System;
using UnityEngine;
using Unity.Cinemachine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;

    [SerializeField] private CinemachineCamera _defaultCam;
    [SerializeField] private CinemachineCamera _bossCam;

    public void EnterFocus()
    {
        _bossCam.Priority = 20;
        _defaultCam.Priority = 10;
    }

    public void ExitFocus()
    {
        _bossCam.Priority = 10;
        _defaultCam.Priority = 20;
    }

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
            OnGameStatusUpdated(_currentGameState, oldGameState);
        }
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
