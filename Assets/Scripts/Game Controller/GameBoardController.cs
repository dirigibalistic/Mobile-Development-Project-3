using UnityEngine;
using UnityEngine.UIElements;

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

    private TowerType _selectedTowerType;

    static GameBoardController instance;

    private void Awake()
    {
        _board.Initialize(_boardSize, _tileContentFactory);
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
        if(tile != null) _board.ToggleWall(tile);
    }

    public void BoardTick()
    {
        _spawnProgress += _spawnSpeed * Time.deltaTime;
        while(_spawnProgress >= 1f)
        {
            _spawnProgress--;
            SpawnEnemy();
        }
        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetSpawnPoint(Random.Range(0,_board.SpawnPointsCount));
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        _enemies.Add(enemy);
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
}