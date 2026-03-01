using System;
using Unity.VisualScripting;
using UnityEngine;

public class RescaleComputer : MonoBehaviour
{
    private Vector2 defaultSize;
    private Vector2 realDefaultSize;
    private Vector3 defaultScale;
    private Vector3 realDefaultScale;
    [SerializeField] Transform ModeleEcran;

    public void getBackToRealOriginalSize()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = realDefaultSize;
        ModeleEcran.localScale = realDefaultScale;
    }

    public void getBackToOriginalSize()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = defaultSize;
        ModeleEcran.localScale = defaultScale;
    }

    public void RescaleWidthAndHeight(float multiplier)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta *= multiplier;
        ModeleEcran.localScale *= multiplier;
    }

    public void RescaleHeightMultiplier(float multiplier)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new(defaultSize.x, gameObject.GetComponent<RectTransform>().sizeDelta.y * multiplier);
        Vector2 vector2 = gameObject.GetComponent<RectTransform>().anchoredPosition;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new(vector2.x, vector2.y*multiplier); 
        ModeleEcran.localScale = new(ModeleEcran.localScale.x, multiplier * ModeleEcran.localScale.y , ModeleEcran.localScale.z);
        //Definitif
        defaultScale = ModeleEcran.localScale;
        defaultSize = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void HandleBugsPerClickUpdated(int newLvl, int oldLvl)
    {
        RescaleHeightMultiplier(1f + 0.1f * (float)newLvl);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateManager.Instance.OnGameReset += getBackToRealOriginalSize;
        defaultScale = ModeleEcran.localScale;
        realDefaultSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        realDefaultScale = ModeleEcran.localScale;
        defaultSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        StatsManager.Instance.OnScreenLevelUpdated += HandleBugsPerClickUpdated;
    }

    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.W))
    //    //{
    //    //    RescaleHeightMultiplier(1.2f);
    //    //}
    //}
}
