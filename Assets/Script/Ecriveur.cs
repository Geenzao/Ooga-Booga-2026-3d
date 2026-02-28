using NUnit.Framework.Constraints;
using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling.Editor;
using UnityEngine.LightTransport;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class Ecriveur : MonoBehaviour
{
    [SerializeField] private TMP_Text sentencePrevious;
    [SerializeField] private TMP_Text sentenceActual;
    [SerializeField] private TMP_Text SayedToBoss;

    [SerializeField] AudioSource bossHappy;
    [SerializeField] AudioSource bossSurprised;
    [SerializeField] AudioSource bossAngry;

    [SerializeField] Image bossHappyI;
    [SerializeField] Image bossSurprisedI;
    [SerializeField] Image bossAngryI;

    private string textPrevious = "";
    private string textActual = "";

    //Si l'utilisateur à écrit "je su", la variable vaut 5
    private int indexOfActualCharacter = 0;
    private List<int> indexesOfErrorsInActualSentence = new();
    private List<int> indexesOfErrorsInPreviousSentence = new();
    private List<string> listOfSentences = new List<string>(new String[]{"Optimize headcount for maximum shareholder value",
    "Reduce labor costs to improve margins",
    "Lower the budget allocated to employee benefits",
    "Delay salary reviews due to financial constraints",
    "Outsource roles to cut operational expenses",
    "Freeze hiring to control overhead",
    "Implement unpaid overtime during peak periods",
    "Minimize training investment to protect short term profits",
    "Reallocate bonuses toward executive compensation",
    "Streamline teams through strategic downsizing",
    "Deprioritize employee well being initiatives",
    "Cap promotions to maintain structural efficiency",
    "Replace senior staff with lower cost junior resources",
    "Shift from full time contracts to temporary staffing",
    "Limit remote work to increase supervision",
    "Consolidate roles without adjusting compensation",
    "Reduce paid leave allowances",
    "Eliminate non essential perks",
    "Increase performance targets without raising salaries",
    "Postpone infrastructure upgrades to cut spending",
    "Centralize decision making to tighten control",
    "Suppress dissent to maintain alignment",
    "Encourage internal competition over collaboration",
    "Prioritize short term quarterly results over long term stability",
    "Standardize compensation regardless of workload variance",
    "Leverage workforce flexibility to avoid long term commitments",
    "Reassess employee value based solely on output metrics",
    "Implement strict cost control measures across all departments",
    "Shift healthcare costs to employees",
    "Delay reimbursements to preserve cash flow",
    "Mandatory off the clock email responses",
    "Install monitoring software to track productivity",
    "Encourage employees to sacrifice personal time for company goals",
    "Dismiss union organizing attempts as disloyal behavior",
    "Create culture of fear to discourage speaking up",
    "Normalize working through lunch breaks",
    "Use non compete clauses to limit career mobility",
    "Implement stack ranking to force terminations",
    "Threaten automation to suppress wage negotiations",
    "Replace benefits with empty recognition programs",
    "Mandate return to office to force voluntary resignations",
    "Gaslight employees about market rate compensation",
    "Use unlimited PTO to discourage actual vacation time",
    "Schedule important meetings during off hours",
    "Promote hustle culture over work life balance",
    "Treat employees as disposable resources",
    "Exploit passion for the mission to justify low pay",
    "Create arbitrary metrics to deny bonuses",
    "Normalize responding to Slack on weekends",
    "Use corporate jargon to obscure unethical decisions",
    "Mr Zober is ugly" });

    private void DrawRandomSentenceForActualSentence()
    {
        textActual = listOfSentences[UnityEngine.Random.Range(0, listOfSentences.Count)];
    }

    private string GenerateBalisedText(string sentence, List<int> indexesOfErrors, int indexOfLastTypedCharacter = 0)
    {
        if (indexOfLastTypedCharacter > sentence.Length)
        {
            Debug.LogError("La c est chelou mec. Va gronder Hugo");
        }
        string toReturn = "";
        int i = 0;
        while (i < indexOfLastTypedCharacter)
        {
            bool isError = false;
            foreach (int index in indexesOfErrors)
            {
                if (index == i)
                {
                    isError = true;
                    break;
                }
            }
            if (isError)
            {
                toReturn += "<color=red>";
                toReturn += sentence[i];
                toReturn += "</color>";
            }
            else
            {
                toReturn += "<color=green>";
                toReturn += sentence[i];
                toReturn += "</color>";
            }
            i = i + 1;
        }
        toReturn += "<color=black>";
        while (i < sentence.Length)
        {
            toReturn += sentence[i];
            i= i + 1;
        }
        toReturn += "</color>";
        return toReturn;
    }
    public void OnSentenceEnding()
    {
        indexOfActualCharacter = 0;
        textPrevious = textActual;
        indexesOfErrorsInPreviousSentence = indexesOfErrorsInActualSentence;
        indexesOfErrorsInActualSentence = new List<int>();
        //Gérer l'émotion du boss
        if(indexesOfErrorsInPreviousSentence.Count == 0)
        {
            bossAngryI.gameObject.SetActive(false);
            bossSurprisedI.gameObject.SetActive(false);
            bossHappyI.gameObject.SetActive(true);
            bossHappy.Play();

        }
        if (indexesOfErrorsInPreviousSentence.Count == 1)
        {
            bossAngryI.gameObject.SetActive(false);
            bossSurprisedI.gameObject.SetActive(true);
            bossHappyI.gameObject.SetActive(false);
            bossSurprised.Play();
        }
        if (indexesOfErrorsInPreviousSentence.Count > 1)
        {
            bossAngryI.gameObject.SetActive(true);
            bossSurprisedI.gameObject.SetActive(false);
            bossHappyI.gameObject.SetActive(false);
            bossAngry.Play();
        }
        sentencePrevious.text = GenerateBalisedText(textPrevious, indexesOfErrorsInPreviousSentence, textPrevious.Length);
        DrawRandomSentenceForActualSentence();
        sentenceActual.text = GenerateBalisedText(textActual, indexesOfErrorsInActualSentence);
        SayedToBoss.text = "";
    }

    void Start()
    {
        //Placement de premières phrases
        SayedToBoss.text = "";
        textPrevious = "";
        sentencePrevious.text = "";
        DrawRandomSentenceForActualSentence();
        bossAngryI.gameObject.SetActive(false);
        bossSurprisedI.gameObject.SetActive(false);
        bossHappyI.gameObject.SetActive(true);
        sentenceActual.text = GenerateBalisedText(textActual,indexesOfErrorsInActualSentence);
    }

    public void OnNewCharacter(char character)
    {
        SayedToBoss.text += character;
        //Vérification des erreurs
        if (character == textActual[indexOfActualCharacter])
        {
            //TODO:IncreaseXP
        }
        else
        {
            indexesOfErrorsInActualSentence.Add(indexOfActualCharacter);
            //TODO:LowerXP
        }
        indexOfActualCharacter++;
        if (indexOfActualCharacter >= textActual.Length)
        {
            OnSentenceEnding();
            //TODO:Jouer le son du patron content ou pas content
        }
        sentenceActual.text = GenerateBalisedText(textActual, indexesOfErrorsInActualSentence, indexOfActualCharacter);
    }   
}
