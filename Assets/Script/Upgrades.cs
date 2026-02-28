using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour
{
    private StatsManager _statsmanager;

    [SerializeField] private Button _upgradeBugResolveBtn;
    [SerializeField] private TextMeshProUGUI _upgradeBugResolveText;
    
    [SerializeField] private Button _upgradeScreenSizeBtn;
    [SerializeField] private TextMeshProUGUI _upgradeScreenSizeText;
    
    [SerializeField] private Button _upgradeFoodQualityBtn;
    [SerializeField] private TextMeshProUGUI _upgradeFoodQualityText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _statsmanager = StatsManager.Instance;
        
        _upgradeBugResolveBtn.onClick.AddListener(UpgradeBugResolve);
        _upgradeBugResolveBtn.interactable = false;

        _upgradeScreenSizeBtn.onClick.AddListener(UpgradeScreenSize);
        _upgradeScreenSizeBtn.interactable = false;

        _upgradeFoodQualityBtn.onClick.AddListener(UpgradeFoodQuality);
        _upgradeFoodQualityBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifier si le joueur a assez d'argent pour améliorer bugPerClickLvl
        if (_statsmanager.BugsPerClickLvl < _statsmanager.PRICE_UPGRADE_BUG_RESOLVE.Length)
        {
            int nextUpgradePrice = _statsmanager.PRICE_UPGRADE_BUG_RESOLVE[_statsmanager.BugsPerClickLvl];
            _upgradeBugResolveBtn.interactable = _statsmanager.Money >= nextUpgradePrice;
            _upgradeBugResolveText.text = $"Upgrade Bug Resolve \n{nextUpgradePrice}$";
        }
        else
        {
            // Le niveau maximum est atteint
            _upgradeBugResolveBtn.interactable = false;
            _upgradeBugResolveText.text = "Bug Resolve MAX";
        }

        // Vérifier si le joueur a assez d'argent pour améliorer la taille d'écran
        if (_statsmanager.ScreenLvl < _statsmanager.PRICE_UPGRADE_SCREEN_HEIGHT.Length)
        {
            int nextUpgradePrice = _statsmanager.PRICE_UPGRADE_SCREEN_HEIGHT[_statsmanager.ScreenLvl];
            _upgradeScreenSizeBtn.interactable = _statsmanager.Money >= nextUpgradePrice;
            _upgradeScreenSizeText.text = $"Upgrade Screen Size \n{nextUpgradePrice}$";
        }
        else
        {
            // Le niveau maximum est atteint
            _upgradeScreenSizeBtn.interactable = false;
            _upgradeScreenSizeText.text = "Screen Size MAX";
        }

        // Vérifier si le joueur a assez d'argent pour améliorer la qualité de nourriture
        if (_statsmanager.FoodLvl < _statsmanager.PRICE_PER_FOOD_LVL.Length)
        {
            int nextUpgradePrice = _statsmanager.PRICE_PER_FOOD_LVL[_statsmanager.FoodLvl];
            _upgradeFoodQualityBtn.interactable = _statsmanager.Money >= nextUpgradePrice;
            _upgradeFoodQualityText.text = $"Upgrade Food Quality \n{nextUpgradePrice}$";
        }
        else
        {
            // Le niveau maximum est atteint
            _upgradeFoodQualityBtn.interactable = false;
            _upgradeFoodQualityText.text = "Food Quality MAX";
        }
    }

    private void UpgradeBugResolve()
    {
        _statsmanager.UpgradeBugPerClickLvl();
    }

    private void UpgradeScreenSize()
    {
        _statsmanager.UpgradeScreenLvl();
    }

    private void UpgradeFoodQuality()
    {
        _statsmanager.UpgradeFoodLvl();
    }
}
