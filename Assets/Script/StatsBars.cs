using UnityEngine;
using UnityEngine.UI;

public class StatsBars : MonoBehaviour
{
    [SerializeField] private Image _foodbar;
    [SerializeField] private Image _xpbar;

    private StatsManager _statsmanager;

    private void HandleFoodUpdated(int newVal, int oldVal)
    {
        //Debug.Log("okok");
        updateBar(_foodbar, (float)newVal / 100f);
    }

    private void HandleBugsUpdated(int newVal, int oldVal)
    {
        updateBar(_xpbar, (float)newVal / (float)_statsmanager.CurrentMaxBugs);
    }

    private void Start()
    {
        _statsmanager = StatsManager.Instance;
        _statsmanager.OnFoodUpdated += HandleFoodUpdated;
        _statsmanager.OnBugMeterUpdated += HandleBugsUpdated;
    }

    private void updateBar(Image to_update, float amount)
    {
        to_update.fillAmount = amount;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
