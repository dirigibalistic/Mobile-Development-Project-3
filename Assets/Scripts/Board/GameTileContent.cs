using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField] private GameTileContentType _type;
    private GameTileContentFactory _originFactory;
    public GameTileContentType Type => _type;
    public bool BlocksPath => Type == GameTileContentType.Wall || Type == GameTileContentType.LaserTower || Type == GameTileContentType.MortarTower;

    [SerializeField]
    private int _price;
    public int Price => _price;

    public GameTileContentFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(OriginFactory == null, "No origin factory");
            _originFactory = value;
        }
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {

    }
}