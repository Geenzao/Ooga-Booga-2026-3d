using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    private static TutoManager _instance;

    [SerializeField] private GameObject _tuto1;
    [SerializeField] private Button _quitTuto1;
    [SerializeField] private GameObject _tuto2;
    [SerializeField] private Button _quitTuto2;
    [SerializeField] private GameObject _tuto3;
    [SerializeField] private Button _quitTuto3;
    
    public static TutoManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        
        _quitTuto1.onClick.AddListener(HideTuto1);
        _quitTuto2.onClick.AddListener(HideTuto2);
        _quitTuto3.onClick.AddListener(HideTuto3);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideAllTutos();
        ShowTuto1();
    }

    private void HideAllTutos()
    {
        if (_tuto1 != null) _tuto1.SetActive(false);
        if (_tuto2 != null) _tuto2.SetActive(false);
        if (_tuto3 != null) _tuto3.SetActive(false);
    }

    public void ShowTuto1()
    {
        if (_tuto1 != null)
        {
            _tuto1.SetActive(true);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.TUTO;
        }
    }

    public void ShowTuto2()
    {
        if (_tuto2 != null)
        {
            _tuto2.SetActive(true);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.TUTO;
        }
    }

    public void ShowTuto3()
    {
        if (_tuto3 != null)
        {
            _tuto3.SetActive(true);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.TUTO;
        }
    }

    public void HideTuto1()
    {
        if (_tuto1 != null)
        {
            _tuto1.SetActive(false);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_OFFICE;
        }
    }

    public void HideTuto2()
    {
        if (_tuto2 != null)
        {
            _tuto2.SetActive(false);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_OFFICE;
        }
    }

    public void HideTuto3()
    {
        if (_tuto3 != null)
        {
            _tuto3.SetActive(false);
            GameStateManager.Instance.GameStatus = GameStateManager.GameState.IN_BOSS_OFFICE;
        }
    }
}
