using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private GameObject _bossPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private GameObject _firedPanel;

    [SerializeField] private List<TextMeshProUGUI> _scoreTexts;

    [SerializeField] private CanvasGroup _blackPanel;
    [SerializeField] private float fadeDuration = 0.5f;

    private async Task FadeAsync(float start, float end)
    {
        float time = 0f;
        _blackPanel.alpha = start;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            _blackPanel.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            await Task.Yield(); // attend la frame suivante
        }

        _blackPanel.alpha = end;
    }

    private async Task FadeToBlack()
    {
        await FadeAsync(0f, 1f);
    }

    private async Task FadeToNormal()
    {
        await FadeAsync(1f, 0f);
    }

    private async Task SwitchUIAsync(List<GameObject> desac, List<GameObject> activate)
    {
        //await FadeToBlack();
        foreach (GameObject go in desac)
            go.SetActive(false);
        foreach (GameObject go in activate)
            go.SetActive(true);
            
        //await FadeToNormal();
    }

    private void SwitchUI(List<GameObject> desac, List<GameObject> activate)
    {
        foreach (GameObject go in desac)
            go.SetActive(false);
        foreach (GameObject go in activate)
            go.SetActive(true);
    }

    private void setScoreText()
    {
        foreach (TextMeshProUGUI scoreText in _scoreTexts)
        {
            scoreText.text =
                "Score : " + StatsManager.Instance.XP;
        }
    }

    private async void HandleGameStatusUpdated(GameStateManager.GameState newState, GameStateManager.GameState oldState)
    {
        switch (newState)
        {
            case GameStateManager.GameState.TUTO:
                // Ne rien faire, on garde l'UI actuelle et on affiche juste le tuto par-dessus
                break;
            case GameStateManager.GameState.IN_OFFICE:
                SwitchUI(new List<GameObject> { _bossPanel, _firedPanel, _deathPanel }, new List<GameObject> { _shopPanel, _statsPanel });
                break;
            case GameStateManager.GameState.IN_BOSS_OFFICE:
                SwitchUI(new List<GameObject> { _shopPanel, _firedPanel, _deathPanel }, new List<GameObject> { _bossPanel, _statsPanel });
                inputField.Select();
                inputField.ActivateInputField();
                break;
            case GameStateManager.GameState.DEAD:
                setScoreText();
                SwitchUI(new List<GameObject> { _bossPanel, _shopPanel, _statsPanel, _firedPanel }, new List<GameObject> { _deathPanel });
                break;
            case GameStateManager.GameState.FIRED:
                setScoreText();
                SwitchUI(new List<GameObject> { _bossPanel, _shopPanel, _statsPanel, _deathPanel }, new List<GameObject> { _firedPanel });
                break;
        }
    }

    //Would be moved to another script if it wasnt a jam
    public void OnRetryClick()
    {
        GameStateManager.Instance.ResetGame();
    }

    //Would be moved to another script if it wasnt a jam
    public void OnQuitClick()
    {
        Application.Quit();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameStateManager = GameStateManager.Instance;
        _gameStateManager.OnGameStatusUpdated += HandleGameStatusUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
