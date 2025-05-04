using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Enemy : GameBehavior
{
    [SerializeField] private Transform _model;
    [SerializeField] private RectTransform _healthBarFill;

    [SerializeField] private int _moneyValue;
    [SerializeField] private int _damage;

    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private ParticleSystem _hitPlayerParticles;
    [SerializeField] private AudioClip _deathSound;

    private GameBoardController _boardController;
    private EnemyFactory _originFactory;
    public EnemyFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory");
            _originFactory = value;
        }
    }

    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress, _progressFactor;

    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;

    float Health { get; set; } = 100;
    private float _startingHealth;

    public void SpawnOn(GameTile tile, GameBoardController boardController)
    {
        _boardController = boardController;
        Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go", this);
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;

        _progress = 0f;
        _startingHealth = Health;
        PrepareIntro();
    }

    public override bool GameUpdate() //moves enemy, returns true if alive
    {
        if (Health <= 0f)
        {
            DeathEffects();
            _boardController.EnemyKilled(_moneyValue); //killed by player, so gain money
            OriginFactory.Reclaim(this);
            return false;
        }

        _progress += Time.deltaTime * _progressFactor;
        while(_progress >= 1f)
        {
            if(_tileTo == null)
            {
                AudioSource.PlayClipAtPoint(_deathSound, transform.position);
                Instantiate(_hitPlayerParticles, transform.position, Quaternion.identity);
                _boardController.EnemyReachedDestination(_damage); //reached destination, damage player
                OriginFactory.Reclaim(this);
                return false;
            }

            _progress = (_progress - 1f) / _progressFactor;
            PrepareNextState();
            _progress *= _progressFactor;
        }
        if(_directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        return true;
    }

    private void PrepareNextState()
    {
        _tileFrom = _tileTo;
        _tileTo = _tileTo.NextTileOnPath;
        _positionFrom = _positionTo;
        if(_tileTo == null)
        {
            PrepareOutro();
            return;
        }
        _positionTo = _tileFrom.ExitPoint;
        _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
        _direction = _tileFrom.PathDirection;
        _directionAngleFrom = _directionAngleTo;

        switch (_directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    private void PrepareForward()
    {
        transform.localRotation = _direction.GetRotation();
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = Vector3.zero;
        _progressFactor = 1f;
    }

    void PrepareTurnRight()
    {
        _directionAngleTo = _directionAngleFrom + 90f;
        _model.localPosition = new Vector3(-0.5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = 1f / (Mathf.PI * 0.25f);
    }

    void PrepareTurnLeft()
    {
        _directionAngleTo = _directionAngleFrom - 90f;
        _model.localPosition = new Vector3(0.5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = 1f / (Mathf.PI * 0.25f);
    }

    void PrepareTurnAround()
    {
        _directionAngleTo = _directionAngleFrom + 180f;
        _model.localPosition = Vector3.zero;
        _progressFactor = 2f;
    }

    private void PrepareIntro()
    {
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileFrom.ExitPoint;
        _direction = _tileFrom.PathDirection;
        _directionChange = DirectionChange.None;
        _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
        transform.localRotation = _direction.GetRotation();
        _progressFactor = 2f;
    }

    private void PrepareOutro()
    {
        _positionTo = _tileFrom.transform.localPosition;
        _directionChange = DirectionChange.None;
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = Vector3.zero;
        transform.localRotation = _direction.GetRotation();
        _progressFactor = 2f;
    }

    public void ApplyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied");
        Health -= damage;
        _healthBarFill.localScale = new Vector3(math.remap(0, _startingHealth, 0, 1, Health), 1f, 1f);
    }

    private void DeathEffects()
    {
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_deathSound, transform.position);
    }
}
