using System;
using Unity.VisualScripting;
using UnityEngine;

public class StyleOverForComputer : MonoBehaviour{
    [SerializeField] private float multTaille = 1.02f;
    [SerializeField] private RescaleComputer rs;
    [SerializeField] private TextTyppeur tt;
    private bool isMouseAbove = false;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (Physics.Raycast(ray, out hit))
        {
            //Est dessus
            if (hit.collider.gameObject == gameObject)
            {
                if (!isMouseAbove)
                {
                    isMouseAbove = true;
                    gameObject.GetComponent<Outline>().enabled = true;
                    rs.RescaleWidthAndHeight(multTaille);
                    //Debug.Log("Mouse Enter");
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (gameObject.name == "ClickableMonitor")
                    {
                        StatsManager.Instance.ResolveBug();
                        tt.AddASentance();
                    }
                }
            }
            else
            {
                if (isMouseAbove)
                {
                    isMouseAbove = false;
                    rs.getBackToOriginalSize();
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
                rs.getBackToOriginalSize();
                gameObject.GetComponent<Outline>().enabled = false;
                //Debug.Log("Mouse Exit");
            }
        }
    }
}
