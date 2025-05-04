using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDController : MonoBehaviour
{
    private GameController _gameController;

    private UIDocument _document;
    private VisualElement _winMenu, _loseMenu, _gameHUD, _buildMenu, _coinIcon;
    private int _currentCoinFrame;
    [SerializeField] private List<Texture2D> _coinFrames = new List<Texture2D>();
    [SerializeField] private Texture2D _pauseIcon;
    [SerializeField] private Texture2D _playIcon;

    private Button _winExitButton, _loseExitButton;
    private Button _buildButton, _destroyButton, _wallButton, _laserButton, _mortarButton, _arrowsButton, _gridButton, _pauseButton, _startButton, _winNextRoundButton, _loseRestartButton;
    private Label _moneyLabel, _roundLabel;
    private ProgressBar _healthBar;
    private bool _gamePaused = false;

    public event Action OnStartRoundPressed;

    [SerializeField] private UnityEngine.UI.Image _fadeImage;
    [SerializeField] private UnityEngine.UI.Image _vignetteImage;
    private float _fadeTime = 0.5f;

    private void Awake()
    {
        _gameController = GetComponentInParent<GameController>();

        _document = GetComponent<UIDocument>();
        _winMenu = _document.rootVisualElement.Q("WinMenu");
        _loseMenu = _document.rootVisualElement.Q("LoseMenu");
        _gameHUD = _document.rootVisualElement.Q("GameHUD");
        _buildMenu = _gameHUD.Q("BuildMenu");
        _coinIcon = _gameHUD.Q("CoinIcon");

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
        _roundLabel = _gameHUD.Q("RoundLabel") as Label;
        _healthBar = _gameHUD.Q("HealthBar") as ProgressBar;

        _fadeImage = GetComponentInChildren<UnityEngine.UI.Image>();

        //i'm going insane. there must be an easier way get all of these at once.
    }

    private void Start()
    {
        HideAllMenus();
        _fadeImage.DOFade(0, _fadeTime);
        _coinIcon.schedule.Execute(UpdateCoin).Every(84);
    }

    private void Update()
    {
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
        _gameController.AudioPlayer.PlayButtonSound();
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
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Empty);
    }

    private void SelectWall(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.SetSelectedContent(GameTileContentType.Wall);
    }

    private void SelectLaser(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.SetSelectedContent(GameTileContentType.LaserTower);
    }

    private void SelectMortar(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.SetSelectedContent(GameTileContentType.MortarTower);
    }
    private void ToggleGrid(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.ToggleGrid();
    }

    private void ToggleArrows(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.BoardController.ToggleArrows();
    }

    private void TogglePause(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gamePaused = !_gamePaused;
        if (_gamePaused)
        {
            AudioListener.pause = true;
            _pauseButton.style.backgroundImage = _playIcon;
            _vignetteImage.DOFade(1, _fadeTime).SetUpdate(true);
            StartCoroutine(FadeTimeScale(0, _fadeTime));
        }
        else 
        {
            AudioListener.pause = false;
            _pauseButton.style.backgroundImage = _pauseIcon;
            _vignetteImage.DOFade(0, _fadeTime).SetUpdate(true);
            StartCoroutine(FadeTimeScale(1, _fadeTime));
        }
    }
    private IEnumerator FadeTimeScale(float fadeTo, float duration)
    {
        float value = Time.timeScale;

        if (fadeTo < Time.timeScale)
        {
            while (value > fadeTo)
            {
                value -= Time.unscaledDeltaTime / duration;
                if (value < fadeTo) value = fadeTo;
                Time.timeScale = value;
                yield return null;
            }
        }
        else if (fadeTo > Time.timeScale)
        {
            while (value < fadeTo)
            {
                value += Time.unscaledDeltaTime / duration;
                if (value > fadeTo) value = fadeTo;
                Time.timeScale = value;
                yield return null;
            }
        }        
    }

    private void StartRound(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayStartRoundSound();
        OnStartRoundPressed?.Invoke();
    }

    internal void UpdateHealthDisplay(float percent) //visual studio keeps changing my functions from public to internal. I have no idea if that's good or bad
    {
        _healthBar.value = percent;
    }

    internal void UpdateMoneyText(int money)
    {
        _moneyLabel.text = money.ToString();
    }

    public void UpdateRoundText(int round)
    {
        _roundLabel.text = "ROUND: " + round.ToString();
    }

    private void NextRound(ClickEvent evt)
    {
        _gameController.AudioPlayer.PlayButtonSound();
        _gameController.PlayerData.NextRound();
    }

    private void ExitToMenu(ClickEvent evt)
    {
        _fadeImage.DOFade(1, _fadeTime);
        _gameController.AudioPlayer.PlayButtonSound();
        StartCoroutine(ExitToMenu());
    }
    private IEnumerator ExitToMenu()
    {
        yield return new WaitForSeconds(_fadeTime);
        _gameController.SceneManager.LoadMenu();
    }

    private void RestartRound(ClickEvent evt)
    {
        _fadeImage.DOFade(1, _fadeTime);
        _gameController.AudioPlayer.PlayButtonSound();
        StartCoroutine(RestartRound());
    }
    private IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(_fadeTime);
        _gameController.SceneManager.LoadMainLevel();
    }

    private void UpdateCoin()
    {
        if (_coinFrames.Count == 0) return;

        _currentCoinFrame = (_currentCoinFrame + 1) % _coinFrames.Count;
        var frame = _coinFrames[_currentCoinFrame];
        _coinIcon.style.backgroundImage = frame;
    }
}