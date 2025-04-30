using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputBroadcaster _input;
    [SerializeField] private GameHUDController _HUDController;
    [SerializeField] private GameSceneManager _sceneManager;
    [SerializeField] private GameBoardController _boardController;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private AudioPlayer _audioPlayer;

    public InputBroadcaster Input => _input;
    public GameHUDController HUDController => _HUDController;
    public GameSceneManager SceneManager => _sceneManager;
    public GameBoardController BoardController => _boardController;
    public PlayerData PlayerData => _playerData;
    public AudioPlayer AudioPlayer => _audioPlayer;
}