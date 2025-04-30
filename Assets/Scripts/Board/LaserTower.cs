using UnityEngine;
using UnityEngine.UIElements;

public class LaserTower : Tower
{
    [SerializeField, Range(1f, 100f)] private float _damagePerSecond = 10f;
    [SerializeField] private Transform _turret, _laserBeam;
    private AudioSource _audioSource;

    private TargetPoint _target;
    private Vector3 _laserBeamScale;

    static Collider[] _targetsBuffer = new Collider[100]; 

    const int enemyLayerMask = 1 << 9;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        base.GameUpdate();
        if (TrackTarget(ref _target) || AcquireTarget(out _target))
        {
            if(!_audioSource.isPlaying) _audioSource.Play();
            Shoot();
        }
        else
        {
            _audioSource.Stop();
            _laserBeam.localScale = Vector3.zero;
        }

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
