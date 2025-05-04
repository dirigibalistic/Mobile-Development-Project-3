using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private GameTile _tilePrefab;

    private bool _showPaths = false;
    public bool ShowPaths
    {
        get => _showPaths;
        set
        {
            _showPaths = value;
            if (_showPaths)
                foreach (GameTile tile in _tiles)
                    tile.ShowPath();

            else
                foreach (GameTile tile in _tiles)
                    tile.HidePath();
        }
    }

    private bool _showGrid = false;
    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            _showGrid = value;
            foreach (GameTile tile in _tiles)
            {
                tile.ToggleShowGrid(_showGrid);
            }
        }
    }

    private Vector2Int _size;

    private GameTile[] _tiles;
    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();
    private GameTileContentFactory _contentFactory;

    private List<GameTile> _spawnPoints = new List<GameTile>();
    public int SpawnPointsCount => _spawnPoints.Count;

    private List<GameTileContent> _updatingContent = new List<GameTileContent>();

    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory, int spawnPointNumber)
    {
        this._size = size;
        this._contentFactory = contentFactory;

        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        #region reset
        if (_tiles != null)
        {
            foreach (GameTile tile in _tiles)
            {
                Destroy(tile.Content.gameObject);
                Destroy(tile.gameObject);
            }
        }
        _spawnPoints = new List<GameTile>();
        _updatingContent = new List<GameTileContent>();
        #endregion
        //deletes old tiles, etc. to reinitialize the board for a new wave
        //this is messy and probably the wrong way to do this but it does work

        _tiles = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }

                //mark alternating tiles
                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0) tile.IsAlternative = !tile.IsAlternative;

                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }

        int destinationIndex = Random.Range(0, _tiles.Length);
        int[] spawnPointIndices = new int[spawnPointNumber];

        ToggleDestination(_tiles[destinationIndex]);

        for (int i = 0; i < spawnPointNumber; i++)
        {
            int value = Random.Range(0, _tiles.Length);
            while (spawnPointIndices.Contains(value) || value == destinationIndex || //don't put spawn point on destination or other spawn points
                value == destinationIndex + size.x || value == destinationIndex - size.x || //don't put spawn point directly adjacent to destination
                value == destinationIndex + 1 || value == destinationIndex - 1)
            {
                value = Random.Range(0, _tiles.Length);
            }
            spawnPointIndices[i] = value;
            ToggleSpawnPoint(_tiles[value]);
        }
    }

    private bool FindPaths()
    {
        foreach (GameTile tile in _tiles)
        {
            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchFrontier.Enqueue(tile);
            }
            else tile.ClearPath();
        }
        if (_searchFrontier.Count == 0) return false; //return false if board invalid (no destinations)

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                //change search order on alternating tiles, causes "diagonal" movement - looks nicer
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (GameTile tile in _tiles) if (!tile.HasPath) return false;

        if (_showPaths)
        {
            foreach (GameTile tile in _tiles)
            {
                tile.ShowPath();
            }
        }
        return true;
    }

    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
                return _tiles[x + y * _size.x];
        }
        return null;
    }

    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if (_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }

    /*public void ToggleTower(GameTile tile, TowerType towerType)
    {
        if (tile.Content.Type == GameTileContentType.Tower)
        {
            _updatingContent.Remove(tile.Content);
            if(((Tower)tile.Content).TowerType == towerType)
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
            else
            {
                tile.Content = _contentFactory.Get(towerType);
                _updatingContent.Add(tile.Content);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(towerType);
            if (FindPaths())
            {
                _updatingContent.Add(tile.Content);
            }
            else
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(towerType);
            _updatingContent.Add(tile.Content);
        }
    }*/

    public void ToggleShowPaths()
    {
        ShowPaths = !ShowPaths;
    }

    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _updatingContent.Count; i++)
        {
            _updatingContent[i].GameUpdate();
        }
    }

    public bool ChangeTileContent(GameTile tile, GameTileContentType contentType)
    {
        if (tile.Content.Type == contentType)
        {
            Debug.Log("Building already present or space is already empty");
            return false;
        }

        if (tile.Content.Type == GameTileContentType.Destination || tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            Debug.Log("Cannot remove enemy spawn or destination");
            return false;
        }

        if (tile.Content.Type == GameTileContentType.LaserTower || tile.Content.Type == GameTileContentType.MortarTower)
        {
            _updatingContent.Remove(tile.Content);
        }

        switch (contentType)
        {
            case GameTileContentType.Empty:
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
                return true;

            case GameTileContentType.Wall:
                tile.Content = _contentFactory.Get(GameTileContentType.Wall);
                if (FindPaths()) return true;
                else
                {
                    Debug.Log("Cannot block all enemy paths");
                    tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                    FindPaths();
                    return false;
                }

            case GameTileContentType.LaserTower:
                tile.Content = _contentFactory.Get(GameTileContentType.LaserTower);
                if (FindPaths())
                {
                    _updatingContent.Add(tile.Content);
                    return true;
                }
                else
                {
                    Debug.Log("Cannot block all enemy paths");
                    tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                    FindPaths();
                    return false;
                }

            case GameTileContentType.MortarTower:
                tile.Content = _contentFactory.Get(GameTileContentType.MortarTower);
                if (FindPaths())
                {
                    _updatingContent.Add(tile.Content);
                    return true;
                }
                else
                {
                    Debug.Log("Cannot block all enemy paths");
                    tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                    FindPaths();
                    return false;
                }

            case GameTileContentType.Destination:
                Debug.Log("Cannot place destination");
                return false;

            case GameTileContentType.SpawnPoint:
                Debug.Log("Cannot place enemy spawn point");
                return false;

            default: return false;
        }
    }
}
