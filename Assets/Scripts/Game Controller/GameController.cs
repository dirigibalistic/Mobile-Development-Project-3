using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Unit _playerShipPrefab;
    [SerializeField] private Transform _playerShipSpawnLocation;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private InputBroadcaster _input;
    [SerializeField] private GameHUDController _HUDController;
    [SerializeField] private GameSceneManager _sceneManager;
    [SerializeField] private GameBoardController _boardController;
    [SerializeField] private PlayerData _playerData;

    public Unit PlayerShipPrefab => _playerShipPrefab;
    public Transform PlayerShipSpawnLocation => _playerShipSpawnLocation;
    public UnitSpawner UnitSpawner => _unitSpawner;
    public InputBroadcaster Input => _input;
    public GameHUDController HUDController => _HUDController;
    public GameSceneManager SceneManager => _sceneManager;
    public GameBoardController BoardController => _boardController;
    public PlayerData PlayerData => _playerData;
}