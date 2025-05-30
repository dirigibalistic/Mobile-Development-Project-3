using UnityEngine;

public class Shell : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;
    private float _age, _blastRadius, _damage;
    [SerializeField] private AudioClip _explosionSound;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float blastRadius, float damage)
    {
        this._launchPoint = launchPoint;
        this._targetPoint = targetPoint;
        this._launchVelocity = launchVelocity;
        this._blastRadius = blastRadius;
        this._damage = damage;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        Vector3 p = _launchPoint + _launchVelocity * _age;
        p.y -= 0.5f * 9.81f * _age * _age;

        if (p.y <= 0f)
        {
            GameBoardController.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            OriginFactory.Reclaim(this);
            return false;
        }

        transform.localPosition = p;

        /* Vector3 d = _launchVelocity;
        d.y -= 9.81f * _age;
        transform.localRotation = Quaternion.LookRotation(d); */ //don't need rotation for cannonballs
        return true;
    }
}
