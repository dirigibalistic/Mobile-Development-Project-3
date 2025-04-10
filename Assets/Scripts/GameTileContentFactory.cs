using UnityEngine;

[CreateAssetMenu(fileName = "GameTileContentFactory", menuName = "Scriptable Objects/GameTileContentFactory")]

public class GameTileContentFactory : GameObjectFactory
{
    //need to go look at some other tutorials and figure out what exactly is going on here

    [SerializeField] private GameTileContent destinationPrefab;
    [SerializeField] private GameTileContent emptyPrefab;
    [SerializeField] private GameTileContent wallPrefab;
    [SerializeField] private GameTileContent spawnPointPrefab;

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
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
            case GameTileContentType.SpawnPoint: return Get(spawnPointPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
}