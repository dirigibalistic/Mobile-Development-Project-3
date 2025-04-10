using UnityEngine;

[CreateAssetMenu(fileName = "GameTileContentFactory", menuName = "Scriptable Objects/GameTileContentFactory")]

public class GameTileContentFactory : GameObjectFactory
{
    //need to go look at some other tutorials and figure out what exactly is going on here

    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPointPrefab;
    [SerializeField] private Tower _towerPrefab;

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed");
        Destroy(content.gameObject);
    }

    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination: return Get(_destinationPrefab);
            case GameTileContentType.Empty: return Get(_emptyPrefab);
            case GameTileContentType.Wall: return Get(_wallPrefab);
            case GameTileContentType.SpawnPoint: return Get(_spawnPointPrefab);
            case GameTileContentType.Tower: return Get(_towerPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
}