using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    private UIDocument _mainMenu;
    private Button _startGameButton;
    private Button _exitButton;
    private Button _resetScoreButton;
    private Label _highestLevelText;
    private int _highestLevel;
    private AudioSource _audioSource;

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

        _highestLevelText.text = "Highest Level: " + _highestLevel.ToString();

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        _startGameButton.UnregisterCallback<ClickEvent>(OnStartGameClick);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitGameClick);
        _resetScoreButton.UnregisterCallback<ClickEvent>(OnResetScoreClick);
    }

    private void OnResetScoreClick(ClickEvent evt)
    {
        _audioSource.Play();
        SaveManager.Instance.ActiveSaveData.HighestLevel = 0;
        _highestLevelText.text = "Highest Level: 0";
        SaveManager.Instance.Save();
    }

    private void OnStartGameClick(ClickEvent evt)
    {
        _audioSource.Play();
        SceneManager.LoadScene(_startLevelName);
    }

    private void OnExitGameClick(ClickEvent evt)
    { 
        _audioSource.Play();
        Application.Quit();
    }
}
