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
    
    void Awake()
    {
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

/*-------------------------------------*
*               VARIABLES              * 
*--------------------------------------*/
    private float _timer = 0f;


    private int _foodMeter = 100; // Jauge de bouffe qui baisse dans le temps et augmente quand on mange
    public readonly int FOOD_USED_PER_SEC = -1; // On perd 1 de bouffe par seconde
    private int _foodLvl = 0; // Le niveau de bouffe qu'on a
    public readonly int[] FOOD_PER_BITE = new int[] { 5, 10, 15 }; // la bouffe remise dans la barre a chaque croc
    public readonly int[] PRICE_PER_FOOD_LVL = new int[] { 2, 3 }; // Le prix de la bouffe a chaque lvl d'amélioration


    private int _xp = 0; // Jauge d'XP, les lettres rapporte 1 et une erreur retire 5
    public readonly int XP_PER_GOOD_LETTER = 1; // l'xp que rapporte une lettre bonne
    public readonly int XP_PER_BAD_LETTER = -5; // l'xp que retire une mauvaise lettre
    public readonly int[] XP_THRESHOLDS = new int[] { 200, 650, 1500 }; // Les seuils atteindre pour changer de grade : stagiaire, employé, manager


    private int _bugMeter; // Jauge de bug, elle augmente a chaque seconde 
    public readonly int[] BUG_PER_SEC_PER_XP_LVL = new int[] { 2, 5, 10 }; //Nombre de bugs par secondes
    private int _bugsPerClickLvl = 0; // le niveau d'amélioration du nombre de bugs par click
    public readonly int[] BUG_RESOLVE_PER_LVL = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // les bug résolu par lvl a chaque click sur l'ecran
    public readonly int[] PRICE_UPGRADE_BUG_RESOLVE= new int[] { 20, 50, 100, 170, 250, 340, 500, 700, 1000 }; // prix de chaque amélioration de l'éfficacité des clicks


    private int _screenLvl = 0; // La taille de l'écran qui augmente le nombre maximum de bug possibles
    public readonly int[] MAX_BUG_PER_SCREEN_HEIGHT = new int[] { 100, 120, 140, 160, 180, 200 }; // Nombre max de bug par niveau d'amélioration
    public readonly int[] PRICE_UPGRADE_SCREEN_HEIGHT = new int[] { 10, 20, 40, 80, 160 }; // Nombre max de bug par niveau d'amélioration


    private int _money = 0; // L'argent qu'on a actuellement
    public readonly int[] MONEY_PER_SEC_PER_LVL = new int[] { 1, 5, 10 }; // Argent que l'on gagne toute les secondes

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

    public int BugsPerClickLvl
    {
        get { return _bugsPerClickLvl;}
    }

    public int ScreenLvl
    {
        get { return _screenLvl;}
    }

/*-------------------------------------*
*              FONCTIONS               * 
*--------------------------------------*/

    // Paye si possible et renvoie un booleen
    public bool Pay(int money)
    {
        if(_money - money >= 0)
        {
            _money -= money;
            return true;
        }
        return false;
    }
    // Remet de la bouffe dans la barre de bouffe
    public void Eat()
    {
        if (Pay(PRICE_PER_FOOD_LVL[_foodLvl]))
            _foodMeter += FOOD_PER_BITE[_foodLvl];
    }

    // Augmente le niveau de la bouffe
    public void UpgradeFoodLvl()
    {
        if(_foodLvl +1 <= PRICE_PER_FOOD_LVL.Length)
            if(Pay(PRICE_PER_FOOD_LVL[_foodLvl]))
                _foodLvl += 1;
    }

    // Ajoute de l'XP
    public void BonusXP()
    {
        _xp += XP_PER_GOOD_LETTER;
    }

    // Retire de l'XP
    public void MalusXP()
    {
        int _xplvl = XPLvl;
        if ((_xp += XP_PER_BAD_LETTER) >= XP_THRESHOLDS[_xplvl - 1])
            _xp += XP_PER_BAD_LETTER;
        else
            _xp = XP_THRESHOLDS[_xplvl - 1];
    }

    // Résoud des bugs en les retirant du _bugMeter
    public int ResolveBug()
    {
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
        return toResolve;
    }

    public void UpgradeBugPerClickLvl()
    {
        if(_bugsPerClickLvl +1 <= PRICE_UPGRADE_BUG_RESOLVE.Length)
            if(Pay(PRICE_UPGRADE_BUG_RESOLVE[_bugsPerClickLvl]))
                _bugsPerClickLvl += 1;
    }

    public void UpgradeScreenLvl()
    {
        if(_screenLvl +1 <= PRICE_UPGRADE_SCREEN_HEIGHT.Length)
            if(Pay(PRICE_UPGRADE_SCREEN_HEIGHT[_screenLvl]))
                _screenLvl += 1;
    }

    void Start()
    {
        Init();
    }

    public void Reset()
    {
        Init();
    }

    private void Init()
    {
        // TODO : initialiser toute les valeur pour le départ et le reset
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

            // TODO : Gérer les bugs par secondes et le fais de se faire virer. Max de bugs : MAX_BUG_PER_SCREEN_HEIGHT

            // TODO : Gérer l'argent par secondes. Grade d'employé : XPLvl, Argent par grade : MONEY_PER_SEC_PER_LVL


            _timer = 0f;
        }
    }
}