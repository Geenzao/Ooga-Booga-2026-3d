using UnityEngine;

public class RedCursor : MonoBehaviour
{
    private StatsManager statsManager;
    [SerializeField] private RectTransform blueScreen;
    private Vector2 defaultSize;

    private void HandleBugsUpdated(int newVal, int oldVal)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new(defaultSize.x, (float)blueScreen.sizeDelta.y * ((float)newVal / (float)statsManager.CurrentMaxBugs)); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsManager = StatsManager.Instance;
        StatsManager.Instance.OnBugMeterUpdated += HandleBugsUpdated;
        defaultSize = gameObject.GetComponent<RectTransform>().sizeDelta;
    }


    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(blueScreen.sizeDelta);
    //}
}
