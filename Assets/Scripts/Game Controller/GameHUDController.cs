using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDController : MonoBehaviour
{
    private GameController _gameController;

    private UIDocument _document;
    private VisualElement _winMenu, _loseMenu, _gameHUD, _buildMenu;

    private Button _winExitButton, _loseExitButton;
    private Button _buildButton, _destroyButton, _wallButton, _laserButton, _mortarButton, _arrowsButton, _gridButton, _pauseButton, _startButton;

    private void Awake()
    {
        _gameController = GetComponentInParent<GameController>();

        _document = GetComponent<UIDocument>();
        _winMenu = _document.rootVisualElement.Q("WinMenu");
        _loseMenu = _document.rootVisualElement.Q("LoseMenu");
        _gameHUD = _document.rootVisualElement.Q("GameHUD");
        _buildMenu = _gameHUD.Q("BuildMenu");

        _winExitButton = _winMenu.Q("WinExitButton") as Button;
        _loseExitButton = _loseMenu.Q("LoseExitButton") as Button;

        _buildButton = _gameHUD.Q("BuildButton") as Button;
        _destroyButton = _gameHUD.Q("DestroyButton") as Button;
        _wallButton = _gameHUD.Q("WallButton") as Button;
        _laserButton = _gameHUD.Q("LaserButton") as Button;
        _mortarButton = _gameHUD.Q("MortarButton") as Button;
        _arrowsButton = _gameHUD.Q("ArrowsButton") as Button;
        _gridButton = _gameHUD.Q("GridButton") as Button;
        _pauseButton = _gameHUD.Q("PauseButton") as Button;
        _startButton = _gameHUD.Q("StartButton") as Button;

        //i'm going insane. there must be an easier way to do this.
    }

    private void Start()
    {
        HideAllMenus();
    }

    private void OnEnable()
    {
        _winExitButton.RegisterCallback<ClickEvent>(ExitToMenu);
        _loseExitButton.RegisterCallback<ClickEvent>(ExitToMenu);

        _buildButton.RegisterCallback<ClickEvent>(ToggleBuildMenu);
        _destroyButton.RegisterCallback<ClickEvent>(SelectEmpty);
        _wallButton.RegisterCallback<ClickEvent>(SelectWall);
        _laserButton.RegisterCallback<ClickEvent>(SelectLaser);
        _mortarButton.RegisterCallback<ClickEvent>(SelectMortar);

        _gridButton.RegisterCallback<ClickEvent>(ToggleGrid);
        _arrowsButton.RegisterCallback<ClickEvent>(ToggleArrows);

        _pauseButton.RegisterCallback<ClickEvent>(TogglePause);
        _startButton.RegisterCallback<ClickEvent>(StartRound);
    }

    private void OnDisable()
    {
        _winExitButton.UnregisterCallback<ClickEvent>(ExitToMenu);
        _loseExitButton.UnregisterCallback<ClickEvent>(ExitToMenu);

        _buildButton.UnregisterCallback<ClickEvent>(ToggleBuildMenu);
        _destroyButton.UnregisterCallback<ClickEvent>(SelectEmpty);
        _wallButton.UnregisterCallback<ClickEvent>(SelectWall);
        _laserButton.UnregisterCallback<ClickEvent>(SelectLaser);
        _mortarButton.UnregisterCallback<ClickEvent>(SelectMortar);

        _gridButton.UnregisterCallback<ClickEvent>(ToggleGrid);
        _arrowsButton.UnregisterCallback<ClickEvent>(ToggleArrows);

        _pauseButton.UnregisterCallback<ClickEvent>(TogglePause);
        _startButton.UnregisterCallback<ClickEvent>(StartRound);
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

    public void ToggleBuildMenu(ClickEvent evt)
    {
        if (_buildMenu.style.display == DisplayStyle.None) _buildMenu.style.display = DisplayStyle.Flex;
        else _buildMenu.style.display = DisplayStyle.None;
    }

    private void HideAllMenus()
    {
        _winMenu.style.display = DisplayStyle.None;
        _loseMenu.style.display = DisplayStyle.None;
        _gameHUD.style.display = DisplayStyle.None;
    }

    private void SelectEmpty(ClickEvent evt)
    {
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Empty);
    }

    private void SelectWall(ClickEvent evt)
    {
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Wall);
    }

    private void SelectLaser(ClickEvent evt)
    {
        _gameController.BoardController.SetSelectedContent(TowerType.Laser);
    }

    private void SelectMortar(ClickEvent evt)
    {
        _gameController.BoardController.SetSelectedContent(TowerType.Mortar);
    }
    private void ToggleGrid(ClickEvent evt)
    {
        _gameController.BoardController.ToggleGrid();
    }

    private void ToggleArrows(ClickEvent evt)
    {
        _gameController.BoardController.ToggleArrows();
    }

    private void TogglePause(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    private void StartRound(ClickEvent evt)
    {
        throw new NotImplementedException();
    }
}