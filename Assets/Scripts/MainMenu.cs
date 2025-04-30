using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEditor.UIElements;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private UIDocument _mainMenu;
    private Button _startGameButton;
    private Button _exitButton;
    private Button _resetScoreButton;
    private Label _highestLevelText;
    private int _highestLevel;

    private VisualElement _titleLabel, _subtitleLabel, _menuContainer;
    private UnityEngine.UI.Image _fadeImage;
    private float _fadeTime = 0.5f;

    [SerializeField] private AudioClip _buttonSound;

    [SerializeField] private string _startLevelName;

    private void Awake()
    {
        SaveManager.Instance.Load();
        _highestLevel = SaveManager.Instance.ActiveSaveData.HighestLevel;

        _mainMenu = GetComponent<UIDocument>();
        _startGameButton = _mainMenu.rootVisualElement.Q("StartGameButton") as Button;
        _exitButton = _mainMenu.rootVisualElement.Q("ExitGameButton") as Button;
        _resetScoreButton = _mainMenu.rootVisualElement.Q("ResetScoreButton") as Button;
        _highestLevelText = _mainMenu.rootVisualElement.Q("HighestLevelText") as Label;

        _startGameButton.RegisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.RegisterCallback<ClickEvent>(OnExitGameClick);
        _resetScoreButton.RegisterCallback<ClickEvent>(OnResetScoreClick);

        _titleLabel = _mainMenu.rootVisualElement.Q("TitleLabel");
        _subtitleLabel = _mainMenu.rootVisualElement.Q("SubtitleLabel");
        _menuContainer = _mainMenu.rootVisualElement.Q("MenuContainer");

        _highestLevelText.text = "Highest Level: " + _highestLevel.ToString();

        _fadeImage = GetComponentInChildren<UnityEngine.UI.Image>();
    }

    private void Start()
    {
        Invoke("ShowMainMenu", 0.1f);
        //just calling it was making the menu transitions not show for some reason
    }

    

    private void OnDisable()
    {
        _startGameButton.UnregisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitGameClick);
        _resetScoreButton.UnregisterCallback<ClickEvent>(OnResetScoreClick);
    }

    private void OnResetScoreClick(ClickEvent evt)
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        SaveManager.Instance.ActiveSaveData.HighestLevel = 0;
        _highestLevelText.text = "Highest Level: 0";
        SaveManager.Instance.Save();
    }

    private void OnStartGameClick(ClickEvent evt)
    {
        _fadeImage.DOFade(1, 0.5f);
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        StartCoroutine(StartGame());
    }

    private void OnExitGameClick(ClickEvent evt)
    {
        _fadeImage.DOFade(1, 0.5f);
        AudioHelper.PlayClip2D(_buttonSound, 0.5f);
        StartCoroutine(ExitGame());
    }

    private void ShowMainMenu()
    {
        _fadeImage.DOFade(0, 0.5f);
        _titleLabel.SetEnabled(true); 
        _subtitleLabel.SetEnabled(true);
        _menuContainer.SetEnabled(true);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_fadeTime);
        SceneManager.LoadScene(_startLevelName);
    }

    private IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(_fadeTime);
        Application.Quit();
    }
}
