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
            if (hit.collider.gameObject == gameObject)
            {
                if (!isMouseAbove)
                {
                    isMouseAbove = true;
                    gameObject.GetComponent<Outline>().enabled = true;
                    gameObject.transform.localScale = gameObject.transform.localScale * multTaille;
                    //Debug.Log("Mouse Enter");
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
