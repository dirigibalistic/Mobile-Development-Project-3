using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _mainMenu;
    private Button _startGameButton;
    private Button _exitButton;
    private Label _highestLevelText;
    private int _highestLevel;

    [SerializeField] private string _startLevelName;

    private void Awake()
    {
        SaveManager.Instance.Load();
        _highestLevel = SaveManager.Instance.ActiveSaveData.HighestLevel;

        _mainMenu = GetComponent<UIDocument>();
        _startGameButton = _mainMenu.rootVisualElement.Q("StartGameButton") as Button;
        _exitButton = _mainMenu.rootVisualElement.Q("ExitGameButton") as Button;
        _highestLevelText = _mainMenu.rootVisualElement.Q("HighestLevelText") as Label;

        _startGameButton.RegisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.RegisterCallback<ClickEvent>(OnExitGameClick);

        _highestLevelText.text = "Highest Level: " + _highestLevel.ToString();
    }

    private void OnDisable()
    {
        _startGameButton.UnregisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitGameClick);
    }

    private void OnStartGameClick(ClickEvent evt)
    {
        SceneManager.LoadScene(_startLevelName);
    }

    private void OnExitGameClick(ClickEvent evt)
    {
        Application.Quit();
    }
}
