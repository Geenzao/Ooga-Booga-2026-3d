using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsBars : MonoBehaviour
{
    [SerializeField] private Image _foodbar;
    [SerializeField] private Image _bugsbar;
    [SerializeField] private Image _xpbar;
    [SerializeField] private TextMeshProUGUI _moneyAmountText;
    [SerializeField] private TextMeshProUGUI _moneyMultText;

    private StatsManager _statsmanager;

    private void HandleFoodUpdated(int newVal, int oldVal)
    {
        //Debug.Log("okok");
        updateBar(_foodbar, (float)newVal / 100f);
    }

    private void HandleBugsUpdated(int newVal, int oldVal)
    {
        updateBar(_bugsbar, (float)newVal / (float)_statsmanager.CurrentMaxBugs);
    }

    private void HandleXPUpdated(int newVal, int oldVal)
    {
        int currentLevelAllXp;
        int currentLevelXp;
        if (_statsmanager.XPLvl <= 0)
        {
            currentLevelAllXp = _statsmanager.XP_THRESHOLDS[_statsmanager.XPLvl];
            currentLevelXp = _statsmanager.XP;
        }
        else
        {
            currentLevelAllXp = _statsmanager.XP_THRESHOLDS[_statsmanager.XPLvl] - _statsmanager.XP_THRESHOLDS[_statsmanager.XPLvl - 1];
            currentLevelXp = _statsmanager.XP - _statsmanager.XP_THRESHOLDS[_statsmanager.XPLvl - 1];
        }
        float percent = (float)currentLevelXp / (float)currentLevelAllXp;
        updateBar(_xpbar, percent);
    }

    private void HandleMoneyUpdated(int newVal, int oldVal)
    {
        _moneyAmountText.text = newVal + "$";
    }

    private void Start()
    {
        _statsmanager = StatsManager.Instance;
        _statsmanager.OnFoodUpdated += HandleFoodUpdated;
        _statsmanager.OnBugMeterUpdated += HandleBugsUpdated;
        _statsmanager.OnXPUpdated += HandleXPUpdated;
        _statsmanager.OnMoneyUpdated += HandleMoneyUpdated;
        //_statsmanager.OnXpLvlUpdated += HandleXpLvlUpdated;
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
