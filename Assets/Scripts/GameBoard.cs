using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private GameTile _tilePrefab;
    [SerializeField] private Texture2D _gridTexture;

    private bool _showPaths = true;
    public bool ShowPaths
    {
        get => _showPaths;
        set
        {
            _showPaths = value;
            if (_showPaths)
                foreach(GameTile tile in _tiles)
                    tile.ShowPath();
            
            else
                foreach (GameTile tile in _tiles)
                    tile.HidePath();
        }
    }

    private bool _showGrid = true;
    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            _showGrid = value;
            foreach(GameTile tile in _tiles)
            {
                tile.ToggleShowGrid(_showGrid);
            }
        }
    }

    private Vector2Int _size;

    private GameTile[] _tiles;
    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();
    private GameTileContentFactory _contentFactory;

    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        this._size = size;
        this._contentFactory = contentFactory;

        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        _tiles = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if(x > 0)
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

        ToggleDestination(_tiles[_tiles.Length / 2]);
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
            if(tile != null)
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);
            if(x >= 0 && x < _size.x && y >= 0 && y < _size.y)
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

    public void ToggleShowPaths()
    {
        ShowPaths = !ShowPaths;
    }
}
