using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDController : MonoBehaviour
{
    private GameController _gameController;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _startRoundSound;

    private UIDocument _document;
    private VisualElement _winMenu, _loseMenu, _gameHUD, _buildMenu;

    private Button _winExitButton, _loseExitButton;
    private Button _buildButton, _destroyButton, _wallButton, _laserButton, _mortarButton, _arrowsButton, _gridButton, _pauseButton, _startButton, _winNextRoundButton, _loseRestartButton;
    private Label _moneyLabel;
    private ProgressBar _healthBar;
    private bool _gamePaused = false;

    public event Action OnStartRoundPressed;

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
        _winNextRoundButton = _winMenu.Q("NextRoundButton") as Button;
        _loseRestartButton = _loseMenu.Q("RestartRoundButton") as Button;

        _buildButton = _gameHUD.Q("BuildButton") as Button;
        _destroyButton = _gameHUD.Q("DestroyButton") as Button;
        _wallButton = _gameHUD.Q("WallButton") as Button;
        _laserButton = _gameHUD.Q("LaserButton") as Button;
        _mortarButton = _gameHUD.Q("MortarButton") as Button;
        _arrowsButton = _gameHUD.Q("ArrowsButton") as Button;
        _gridButton = _gameHUD.Q("GridButton") as Button;
        _pauseButton = _gameHUD.Q("PauseButton") as Button;
        _startButton = _gameHUD.Q("StartButton") as Button;

        _moneyLabel = _gameHUD.Q("MoneyLabel") as Label;
        _healthBar = _gameHUD.Q("HealthBar") as ProgressBar;


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
        _winNextRoundButton.RegisterCallback<ClickEvent>(NextRound);
        _loseRestartButton.RegisterCallback<ClickEvent>(RestartRound);

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
        _winNextRoundButton.UnregisterCallback<ClickEvent>(NextRound);
        _loseRestartButton.UnregisterCallback<ClickEvent>(RestartRound);

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
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
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
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _buildMenu.SetEnabled(!_buildMenu.enabledInHierarchy);
    }

    private void HideAllMenus()
    {
        _winMenu.style.display = DisplayStyle.None;
        _loseMenu.style.display = DisplayStyle.None;
        _gameHUD.style.display = DisplayStyle.None;
    }

    private void SelectEmpty(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Empty);
    }

    private void SelectWall(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Wall);
    }

    private void SelectLaser(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.SetSelectedContent(GameTileContentType.LaserTower);
    }

    private void SelectMortar(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.SetSelectedContent(GameTileContentType.MortarTower);
    }
    private void ToggleGrid(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.ToggleGrid();
    }

    private void ToggleArrows(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.BoardController.ToggleArrows();
    }

    private void TogglePause(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gamePaused = !_gamePaused;
        if (_gamePaused)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1; 
        }
    }

    private void StartRound(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_startRoundSound, 0.5f);
        OnStartRoundPressed?.Invoke();
    }

    internal void UpdateHealthDisplay(float percent)
    {
        _healthBar.value = percent;
    }

    internal void UpdateMoneyText(int money)
    {
        _moneyLabel.text = money.ToString();
    }

    private void NextRound(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.PlayerData.NextRound();
    }

    private void RestartRound(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        _gameController.SceneManager.RestartRound();
    }
}