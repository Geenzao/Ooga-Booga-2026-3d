using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;

[DefaultExecutionOrder(-100)]
public class TextTyppeur : MonoBehaviour
{
    [SerializeField] private List<SerializableTuple<int,string>> BullshitSentance;
    public List<SerializableTuple<int, string>> Last200Bullshit = new();
    [SerializeField] private TMP_Text consoleTypeur;

    int _bugs;
    int _max_bugs;

    [SerializeField] private TextMeshProUGUI _bugs_Cpt;
    [SerializeField] private TextMeshProUGUI _rank_Txt;
    [SerializeField] private TextMeshProUGUI _clic_efficiency_Txt;

    public readonly Dictionary<int, string> LEVELS_NAMES = new Dictionary<int, string>
    {
        { 0, "Intern" },
        { 1, "Employee" },
        { 2, "Manager" },
        { 3, "Bullcheater master" },
    };

    public void Reset()
    {
        Last200Bullshit.Clear();
        AddASentance();
    }

    public void AddASentance()
    {
        SerializableTuple<int, string> choosenSentance = BullshitSentance[UnityEngine.Random.Range(0, BullshitSentance.Count)];
        Last200Bullshit.Add(choosenSentance);
        MakeARender();
        
    }
    public void MakeARender()
    {
        //Calcul du nombre de lignes
        if(Last200Bullshit.Count > 200)
        {
            Last200Bullshit.RemoveAt(0);
        }
        float availableHeight = consoleTypeur.rectTransform.rect.height - 40;
        float lineHeight = consoleTypeur.fontSize * consoleTypeur.lineSpacingAdjustment + consoleTypeur.font.fontInfo.LineHeight / consoleTypeur.font.faceInfo.pointSize * consoleTypeur.fontSize;
        float maxLines = Mathf.Floor(availableHeight / consoleTypeur.fontSize) * 0.9f;

        //Ajout de texte dans la console jusqu'à être sur le point de dépasser le nombre max de lignes
        consoleTypeur.text = "";
        int sommeLignes = 0;
        int i = 0;
        while(sommeLignes < maxLines && i < Last200Bullshit.Count)
        {
            if (sommeLignes + Last200Bullshit[Last200Bullshit.Count - i - 1].Item1 > maxLines)
            {
                break;
            }
            else
            {
                i = i + 1;
                sommeLignes += Last200Bullshit[Last200Bullshit.Count - i].Item1;
            }
        }
        for(int j=i; j > 0;j--)
        {
            consoleTypeur.text += Last200Bullshit[Last200Bullshit.Count - j].Item2;
            consoleTypeur.text += "\n";
        }
    }

    private void updateMaxBugsTextColor()
    {
        if (_max_bugs <= 0)
        {
            _bugs_Cpt.color = Color.green;
            return;
        }

        float ratio = Mathf.Clamp01((float)_bugs / _max_bugs);
        _bugs_Cpt.color = Color.Lerp(Color.green, Color.red, ratio);
    }

    private void updateMaxBugsText(int maxBugs)
    {
        _max_bugs = maxBugs;
        _bugs_Cpt.text = $"{_bugs}/{_max_bugs} (+{StatsManager.Instance.BUG_PER_SEC_PER_XP_LVL[StatsManager.Instance.XPLvl]}/s)";
        updateMaxBugsTextColor();
    }

    private void updateBugsText(int bugs)
    {
        _bugs = bugs;
        _bugs_Cpt.text = $"{_bugs}/{_max_bugs} (+{StatsManager.Instance.BUG_PER_SEC_PER_XP_LVL[StatsManager.Instance.XPLvl]}/s)";
        updateMaxBugsTextColor();
    }

    private void HandleScreenLevelUpdated(int newLvl, int oldLvl)
    {
        MakeARender();
        updateMaxBugsText(StatsManager.Instance.MAX_BUG_PER_SCREEN_HEIGHT[newLvl]);
    }

    private void HandleBugMeterUpdated(int newBugs, int oldBugs)
    {
        updateBugsText(newBugs);
    }

    private void HandleXPLevelUpdated(int newLvl, int oldLvl)
    {
        _rank_Txt.text = LEVELS_NAMES[newLvl];
    }

    private void HandleBugPerClicUpdated(int newLvl, int oldLvl)
    {
        _clic_efficiency_Txt.text = $"Clic efficiency: {StatsManager.Instance.BUG_RESOLVE_PER_LVL[StatsManager.Instance.BugsPerClickLvl]}";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StatsManager.Instance.OnBugMeterUpdated += HandleBugMeterUpdated;
        StatsManager.Instance.OnBugsPerClicLevelUpdated += HandleBugPerClicUpdated;
        GameStateManager.Instance.OnGameReset += Reset;
        StatsManager.Instance.OnScreenLevelUpdated += HandleScreenLevelUpdated;
        StatsManager.Instance.OnXPLvlUpdated += HandleXPLevelUpdated;
        AddASentance();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.A))
    //    //{
    //    //    AddASentance();
    //    //}
    //}
}
