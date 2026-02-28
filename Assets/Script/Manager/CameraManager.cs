using UnityEngine;
using Unity.Cinemachine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _defaultCam;
    [SerializeField] private CinemachineCamera _bossCam;

    private void HandleGameStatusUpdated(GameStateManager.GameState newgs, GameStateManager.GameState oldgs)
    {
        if (newgs == GameStateManager.GameState.IN_OFFICE)
        {
            _defaultCam.Priority = 100;
            _bossCam.Priority = 0;
        }
        else if (newgs == GameStateManager.GameState.IN_BOSS_OFFICE)
        {
            _defaultCam.Priority = 0;
            _bossCam.Priority = 100;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateManager.Instance.OnGameStatusUpdated += HandleGameStatusUpdated;
    }
}
