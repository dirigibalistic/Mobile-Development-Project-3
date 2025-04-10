using UnityEngine;
using UnityEngine.UIElements;

public class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)] private float _targetingRange = 1.5f;
    [SerializeField] private Transform _turret, _laserBeam;
    [SerializeField, Range(1f, 100f)] private float _damagePerSecond = 10f;

    private TargetPoint _target;
    private Vector3 _laserBeamScale;

    static Collider[] _targetsBuffer = new Collider[100]; 

    const int enemyLayerMask = 1 << 9;

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        base.GameUpdate();
        if (TrackTarget() || AcquireTarget())
        {
            Shoot();
        }
        else _laserBeam.localScale = Vector3.zero;

        //search for target
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += .01f;
        Gizmos.DrawWireSphere(position, _targetingRange);

        if(_target != null)
        {
            Gizmos.DrawLine(position, _target.Position);
        }
    }

    private bool AcquireTarget()
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 3f;

        int hits = Physics.OverlapCapsuleNonAlloc(a, b, _targetingRange, _targetsBuffer, enemyLayerMask);
        if(hits > 0 )
        {
            _target = _targetsBuffer[Random.Range(0, hits)].GetComponent<TargetPoint>();
            Debug.Assert(_target != null, "Targeted non-enemy", _targetsBuffer[0]);
            return true;
        }
        _target = null;
        return false;
    }

    private bool TrackTarget()
    {
        if(_target == null)
        {
            return false;
        }

        Vector3 a = transform.localPosition;
        Vector3 b = _target.Position;
        float x = a.x - b.x;
        float z  = a.z - b.z;
        float r = _targetingRange + 0.125f;

        if (x * x + z * z > r * r)
        {
            _target = null;
            return false;
        }
        return true;
    }

    private void Shoot()
    {
        Vector3 point = _target.Position;
        _turret.LookAt(point);
        _laserBeam.localRotation = _turret.localRotation;

        float d = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = d;
        _laserBeam.localScale = _laserBeamScale;
        _laserBeam.localPosition = _turret.localPosition + 0.5f * d * _laserBeam.forward;

        _target.Enemy.ApplyDamage(_damagePerSecond * Time.deltaTime);
    }
}
