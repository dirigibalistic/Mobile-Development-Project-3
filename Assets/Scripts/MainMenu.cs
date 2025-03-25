using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _mainMenu;
    private Button _startGameButton;
    private Button _exitButton;

    [SerializeField] private string _startLevelName;

    private void Awake()
    {
        _mainMenu = GetComponent<UIDocument>();

        _startGameButton = _mainMenu.rootVisualElement.Q("StartGameButton") as Button;
        _exitButton = _mainMenu.rootVisualElement.Q("ExitGameButton") as Button;

        _startGameButton.RegisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.RegisterCallback<ClickEvent>(OnExitGameClick);
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
