using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)] protected private float _targetingRange = 1.5f;

    protected private bool AcquireTarget(out TargetPoint target)
    {
        if(TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            target = TargetPoint.RandomBuffered;
            return true;
        }
        target = null;
        return false;
    }

    protected private bool TrackTarget(ref TargetPoint target)
    {
        if(target == null)
        {
            return false;
        }

        Vector3 a = transform.localPosition;
        Vector3 b = target.Position;
        float x = a.x - b.x;
        float z  = a.z - b.z;
        float r = _targetingRange + 0.125f;

        if (x * x + z * z > r * r)
        {
            target = null;
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += .01f;
        Gizmos.DrawWireSphere(position, _targetingRange);
    }
}
