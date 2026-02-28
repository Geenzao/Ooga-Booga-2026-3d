using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private static StatsManager _instance;
    
    public static StatsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<StatsManager>();
                
                if (_instance == null)
                {
                    GameObject go = new GameObject("StatsManager");
                    _instance = go.AddComponent<StatsManager>();
                }
            }
            return _instance;
        }
    }

/*-------------------------------------*
*               VARIABLES              * 
*--------------------------------------*/
    private float _timer = 0f;


    private int _foodMeter = 100; // Jauge de bouffe qui baisse dans le temps et augmente quand on mange
    public readonly int FOOD_USED_PER_SEC = 1; // On perd 1 de bouffe par seconde
    private int _foodLvl = 0; // Le niveau de bouffe qu'on a
    public readonly int[] FOOD_PER_BITE = new int[] { 5, 10, 15 }; // la bouffe remise dans la barre a chaque croc
    public readonly int[] PRICE_PER_FOOD_LVL = new int[] { 2, 3 }; // Le prix de la bouffe a chaque lvl d'amélioration
    public event Action<int, int> OnFoodUpdated;


    private int _xp = 0; // Jauge d'XP, les lettres rapporte 1 et une erreur retire 5
    public readonly int XP_PER_GOOD_LETTER = 1; // l'xp que rapporte une lettre bonne
    public readonly int XP_PER_BAD_LETTER = -5; // l'xp que retire une mauvaise lettre
    public readonly int[] XP_THRESHOLDS = new int[] { 200, 650, 1500 }; // Les seuils atteindre pour changer de grade : stagiaire, employé, manager
    public event Action<int, int> OnXPUpdated;


    private int _bugMeter; // Jauge de bug, elle augmente a chaque seconde 
    public readonly int[] BUG_PER_SEC_PER_XP_LVL = new int[] { 2, 5, 10 }; //Nombre de bugs par secondes
    private int _bugsPerClickLvl = 0; // le niveau d'amélioration du nombre de bugs par click
    public readonly int[] BUG_RESOLVE_PER_LVL = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // les bug résolu par lvl a chaque click sur l'ecran
    public readonly int[] PRICE_UPGRADE_BUG_RESOLVE= new int[] { 20, 50, 100, 170, 250, 340, 500, 700, 1000 }; // prix de chaque amélioration de l'éfficacité des clicks
    public event Action<int, int> OnBugMeterUpdated;


    private int _screenLvl = 0; // La taille de l'écran qui augmente le nombre maximum de bug possibles
    public readonly int[] MAX_BUG_PER_SCREEN_HEIGHT = new int[] { 100, 120, 140, 160, 180, 200 }; // Nombre max de bug par niveau d'amélioration
    public readonly int[] PRICE_UPGRADE_SCREEN_HEIGHT = new int[] { 10, 20, 40, 80, 160 }; // Nombre max de bug par niveau d'amélioration


    private int _money = 50000; // L'argent qu'on a actuellement
    public readonly int[] MONEY_PER_SEC_PER_LVL = new int[] { 1, 5, 10 }; // Argent que l'on gagne toute les secondes
    public event Action<int, int> OnMoneyUpdated;


  /*-------------------------------------*
  *           GETTER SETTER              * 
  *--------------------------------------*/
  public int FoodMeter
    {
        get { return _foodMeter; }
    }

    public int FoodLvl
    {
        get { return _foodLvl; }
    }

    public int XP
    {
        get { return _xp;}
    }

    public int XPLvl
    {
        get {
            int currentLvl = 0;
            foreach(int lvl in XP_THRESHOLDS)
            {
                if(_xp > lvl)
                    currentLvl ++;
            }
            return currentLvl;
        }
    }

    public int BugMeter
    {
        get { return _bugMeter;}
    }

    public int CurrentMaxBugs
    {
        get { return MAX_BUG_PER_SCREEN_HEIGHT[_screenLvl]; }
    }

    public int CurrentBugsPerSec
    {
        get { return BUG_PER_SEC_PER_XP_LVL[XPLvl]; }
    }

    public int BugsPerClickLvl
    {
        get { return _bugsPerClickLvl;}
    }

    public int ScreenLvl
    {
        get { return _screenLvl;}
    }

    public int Money
    {
        get { return _money;}
    }

/*-------------------------------------*
*              FONCTIONS               * 
*--------------------------------------*/

    // Paye si possible et renvoie un booleen
    public bool Pay(int money)
    {
        int oldMoney = _money;
        if(_money - money >= 0)
        {
            _money -= money;
            OnMoneyUpdated?.Invoke(_money, oldMoney);
            return true;
        }
        return false;
    }
    // Remet de la bouffe dans la barre de bouffe
    public void Eat()
    {
        int oldFood = _foodMeter;
        if (Pay(PRICE_PER_FOOD_LVL[_foodLvl]))
            _foodMeter += FOOD_PER_BITE[_foodLvl];
        OnFoodUpdated?.Invoke(_foodMeter, oldFood);
    }

    public void Starve(int nb)
    {
        int oldFood = _foodMeter;
        if ((_foodMeter -= nb) <= 0) {
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.DEAD;        
        } //TODO die
        _foodMeter -= nb;
        OnFoodUpdated?.Invoke(_foodMeter, oldFood);
    }

    public void ComputeBugs()
    {
        int oldBugs = _bugMeter;
        int newBugs = _bugMeter + CurrentBugsPerSec;
        if (newBugs >= CurrentMaxBugs) { ; } //TODO viré, le mauvais développeur
        _bugMeter = newBugs;
        OnBugMeterUpdated(_bugMeter, oldBugs);
    }

    // Augmente le niveau d'amélioration de la bouffe
    public int UpgradeFoodLvl()
    {
        Debug.Log("Food level avant : " + (_foodLvl + 1) + ", Argent avant : " + Money);
        if(_foodLvl +1 <= PRICE_PER_FOOD_LVL.Length)
            if(Pay(PRICE_PER_FOOD_LVL[_foodLvl]))
                _foodLvl += 1;

        Debug.Log("Food level : " + (_foodLvl + 1) + ", Argent restant : " + Money);
        return _foodLvl;
  }

    // Ajoute de l'XP
    public void BonusXP()
    {
        int oldXP = _xp;
        _xp += XP_PER_GOOD_LETTER;
        OnXPUpdated(_xp, oldXP);
    }

    // Retire de l'XP
    public void MalusXP()
    {
        int currentXPLevel = XPLvl;
        int oldXP = _xp;
        if ((_xp += XP_PER_BAD_LETTER) >= XP_THRESHOLDS[currentXPLevel - 1])
            _xp += XP_PER_BAD_LETTER;
        else
            _xp = XP_THRESHOLDS[currentXPLevel - 1];
        OnXPUpdated(_xp, oldXP);
    }

    // Résoud des bugs en les retirant du _bugMeter
    public int ResolveBug()
    {
        int oldBugsCpt = _bugMeter;
        int toResolve = 0;
        if(_bugMeter - BUG_RESOLVE_PER_LVL[_bugsPerClickLvl] >= 0)
        {
            toResolve = BUG_RESOLVE_PER_LVL[_bugsPerClickLvl];
            _bugMeter -= BUG_RESOLVE_PER_LVL[_bugsPerClickLvl];
        }
        else
        {
            toResolve = _bugMeter;
            _bugMeter = 0;
        }
        OnBugMeterUpdated(_bugMeter, oldBugsCpt);
        return toResolve;
    }

    public void UpgradeBugPerClickLvl()
    {
        Debug.Log("Bug par clic avant : " + (_bugsPerClickLvl + 1) + ", Argent avant : " + Money);
        if(_bugsPerClickLvl +1 <= PRICE_UPGRADE_BUG_RESOLVE.Length)
            if(Pay(PRICE_UPGRADE_BUG_RESOLVE[_bugsPerClickLvl]))
                _bugsPerClickLvl += 1;

        Debug.Log("Bug par clic : " + (_bugsPerClickLvl + 1) + ", Argent restant : " + Money);
    }

    public void UpgradeScreenLvl()
    {
        Debug.Log("Screen level avant : " + (_screenLvl + 1) + ", Argent avant : " + Money);
        if(_screenLvl +1 <= PRICE_UPGRADE_SCREEN_HEIGHT.Length)
            if(Pay(PRICE_UPGRADE_SCREEN_HEIGHT[_screenLvl]))
                _screenLvl += 1;

        Debug.Log("Screen level : " + (_screenLvl + 1) + ", Argent restant : " + Money);
    }

    void Start()
    {
        Init();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void Reset()
    {
        Init();
    }

    private void Init()
    {
        // TODO : initialiser toute les valeur pour le départ et le reset
        OnMoneyUpdated?.Invoke(Money, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Décrémenter la nourriture chaque seconde
        _timer += Time.deltaTime;
        
        // Toute les secondes
        if (_timer >= 1f)
        {
            // TODO : Gérer la bouffe par secondes et la mort de faim. FOOD_USED_PER_SEC
            Starve(FOOD_USED_PER_SEC);

            // TODO : Gérer les bugs par secondes et le fais de se faire virer. Max de bugs : MAX_BUG_PER_SCREEN_HEIGHT
            ComputeBugs();

            // TODO : Gérer l'argent par secondes. Grade d'employé : XPLvl, Argent par grade : MONEY_PER_SEC_PER_LVL


            //TESTS
            BonusXP();

            _timer = 0f;
        }
    }
}