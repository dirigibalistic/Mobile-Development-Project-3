using System;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private Vector2Int _boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard _board;

    [Header("Tile Content")]
    [SerializeField] private GameTileContentFactory _tileContentFactory;

    [Header("Enemies")]
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField, Range(0.1f, 10f)] private float _spawnSpeed = 1.0f;

    [Header("Projectiles")]
    [SerializeField] private WarFactory _warFactory;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private float _spawnProgress;
    private int _totalEnemiesToSpawn;
    private int _spawnsRemaining;
    private int _enemiesKilled;

    public event Action OnRoundWon;

    private GameTileContentType _selectedContentType = GameTileContentType.Empty;

    static GameBoardController instance;
    private GameController _gameController;

    private void Awake()
    {
        _gameController = GetComponentInParent<GameController>();
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnValidate()
    {
        if (_boardSize.x < 2) _boardSize.x = 2;
        if (_boardSize.y < 2) _boardSize.y = 2;
    }

    public void HandleTouch(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        GameTile tile = _board.GetTile(ray);
        int cost;

        switch (_selectedContentType)
        {
            case GameTileContentType.Empty:
                cost = 0;
                break;
            case GameTileContentType.Wall:
                cost = 25;
                break;
            case GameTileContentType.LaserTower:
                cost = 50;
                break;
            case GameTileContentType.MortarTower:
                cost = 100;
                break;
            default:
                Debug.Log("Trying to place unsupported type");
                return;
        }

        if (tile != null)
        {
            if (_gameController.PlayerData.SpendMoney(cost))
            {
                _gameController.PlayerData.GainMoney(tile.Content.Price);
                if(!_board.ChangeTileContent(tile, _selectedContentType)) _gameController.PlayerData.GainMoney(cost);
            }
            else
            {
                Debug.Log("Not enough money");
            }

            _gameController.AudioPlayer.PlayButtonSound();
        }
    }

    public void BoardTick()
    {
        _spawnProgress += _spawnSpeed * Time.deltaTime;
        while(_spawnProgress >= 1f && _spawnsRemaining > 0)
        {
            _spawnProgress--;
            SpawnEnemy();
        }
        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();

        if(_enemiesKilled >= _totalEnemiesToSpawn)
        {
            OnRoundWon?.Invoke();
        }
    }

    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetSpawnPoint(UnityEngine.Random.Range(0,_board.SpawnPointsCount));
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint, this);
        _enemies.Add(enemy);
        _spawnsRemaining--;
    }

    public static Shell SpawnShell()
    {
        Shell shell = instance._warFactory.Shell;
        instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance._warFactory.Explosion;
        instance._nonEnemies.Add(explosion);
        return explosion;
    }
    public void SetSelectedContent(GameTileContentType contentType)
    {
        _selectedContentType = contentType;
    }

    public void ToggleGrid()
    {
        _board.ShowGrid = !_board.ShowGrid;
    }

    public void ToggleArrows()
    {
        _board.ShowPaths = !_board.ShowPaths;
    }

    public void InitializeBoard(Vector2Int size, int spawnPointNumber, int totalEnemyNumber)
    {
        _totalEnemiesToSpawn = totalEnemyNumber;
        _spawnsRemaining = _totalEnemiesToSpawn;
        _enemiesKilled = 0;

        _board.Initialize(size, _tileContentFactory, spawnPointNumber);
    }

    public void EnemyKilled(int money)
    {
        _gameController.PlayerData.GainMoney(money);
        _enemiesKilled++;
        //update hud?
    }

    internal void EnemyReachedDestination(int damage)
    {
        _gameController.PlayerData.TakeDamage(damage);
        _enemiesKilled++;
    }
}