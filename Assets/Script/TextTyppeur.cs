using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TextTyppeur : MonoBehaviour
{
    [SerializeField] private List<SerializableTuple<int,string>> BullshitSentance;
    public List<SerializableTuple<int, string>> Last200Bullshit = new();
    [SerializeField] private TMP_Text consoleTypeur;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
