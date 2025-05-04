using UnityEngine;

public abstract class WarEntity : GameBehavior
{
    private WarFactory _originFactory;

    public WarFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory");
            _originFactory = value;
        }
    }

    public void Recycle()
    {
        _originFactory.Reclaim(this);
    }
}
