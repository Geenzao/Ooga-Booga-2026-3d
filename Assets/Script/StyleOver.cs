using System;
using Unity.VisualScripting;
using UnityEngine;

public class StyleOver : MonoBehaviour{
    [SerializeField] private float multTaille = 1.06f;
    private Vector3 initialScale;
    private bool isMouseAbove = false;

    private void Start()
    {
        initialScale = transform.localScale;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (Physics.Raycast(ray, out hit))
        {
            //Est dessus
            if (
                hit.collider.gameObject == gameObject
                &&
                (GameStateManager.Instance.GameStatus == GameStateManager.GameState.IN_OFFICE || gameObject.name == "ClickableBoss")
                )
            {
                if (!isMouseAbove)
                {
                    isMouseAbove = true;
                    gameObject.GetComponent<Outline>().enabled = true;
                    gameObject.transform.localScale = gameObject.transform.localScale * multTaille;
                    //Debug.Log("Mouse Enter");
                }
                if(Input.GetMouseButtonUp(0))
                {
                    if(gameObject.name == "ClickableFood")
                    {
                        StatsManager.Instance.Eat();
                    }
                    else if (gameObject.name == "ClickableBoss")
                    {
                        switch (GameStateManager.Instance.GameStatus)
                        {
                            case GameStateManager.GameState.IN_BOSS_OFFICE:
                                GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_OFFICE;
                                break;
                            case GameStateManager.GameState.IN_OFFICE:
                                GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_BOSS_OFFICE;
                                break;
                        }
                    }
                }
            }
            else
            {
                if (isMouseAbove)
                {
                    isMouseAbove = false;
                    gameObject.transform.localScale = initialScale;
                    gameObject.GetComponent<Outline>().enabled = false;
                    //Debug.Log("Mouse Exit");
                }
            }
        }
        else
        {
            if (isMouseAbove)
            {
                isMouseAbove = false;
                gameObject.transform.localScale = initialScale;
                gameObject.GetComponent<Outline>().enabled = false;
                //Debug.Log("Mouse Exit");
            }
        }
    }
}
