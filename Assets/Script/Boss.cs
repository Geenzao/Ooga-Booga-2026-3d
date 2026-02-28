using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (GameStateManager.Instance.GameStatus == GameStateManager.GameState.IN_BOSS_OFFICE)
                GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_OFFICE;
            else
                GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_BOSS_OFFICE;
        }
    }
}
