using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameTileContentFactory", menuName = "Scriptable Objects/GameTileContentFactory")]

public class GameTileContentFactory : ScriptableObject
{
    //need to go look at some other tutorials and figure out what exactly is going on here

    [SerializeField] private GameTileContent destinationPrefab;
    [SerializeField] private GameTileContent emptyPrefab;
    [SerializeField] private GameTileContent wallPrefab;

    private Scene _contentScene;

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed");
        Destroy(content.gameObject);
    }

    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    private void MoveToFactoryScene(GameObject o)
    {
        if(!_contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                _contentScene = SceneManager.GetSceneByName(name);
                if (!_contentScene.isLoaded)
                {
                    _contentScene = SceneManager.CreateScene(name);
                }
            }
            else _contentScene = SceneManager.CreateScene(name);
        }
        SceneManager.MoveGameObjectToScene(o, _contentScene);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
}
