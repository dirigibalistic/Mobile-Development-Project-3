using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDController : MonoBehaviour
{
    private GameController _gameController;

    private UIDocument _document;
    private VisualElement _winMenu;
    private VisualElement _loseMenu;
    private VisualElement _gameHUD;

    private Button _winExitButton;
    private Button _loseExitButton;

    private void Awake()
    {
        _gameController = GetComponentInParent<GameController>();

        _document = GetComponent<UIDocument>();
        _winMenu = _document.rootVisualElement.Q("WinMenu");
        _loseMenu = _document.rootVisualElement.Q("LoseMenu");
        _gameHUD = _document.rootVisualElement.Q("GameHUD");

        _winExitButton = _winMenu.Q("WinExitButton") as Button;
        _loseExitButton = _loseMenu.Q("LoseExitButton") as Button;
    }

    private void Start()
    {
        HideAllMenus();
    }

    private void OnEnable()
    {
        _winExitButton.RegisterCallback<ClickEvent>(ExitToMenu);
        _loseExitButton.RegisterCallback<ClickEvent>(ExitToMenu);
    }

    private void ExitToMenu(ClickEvent evt)
    {
        _gameController.SceneManager.ExitToMenu();
    }

    public void ShowWinMenu()
    {
        HideAllMenus();
        _winMenu.style.display = DisplayStyle.Flex;
    }

    public void ShowLoseMenu()
    {
        HideAllMenus();
        _loseMenu.style.display = DisplayStyle.Flex;
    }

    public void ShowGameHUD()
    {
        HideAllMenus();
        _gameHUD.style.display = DisplayStyle.Flex;
    }

    private void HideAllMenus()
    {
        _winMenu.style.display = DisplayStyle.None;
        _loseMenu.style.display = DisplayStyle.None;
        _gameHUD.style.display = DisplayStyle.None;
    }
}