using UnityEngine;

public class Explosion : WarEntity
{
    [SerializeField, Range(0f, 1f)] private float _duration = 0.5f;
    [SerializeField] private AnimationCurve _opacityCurve;
    [SerializeField] private AnimationCurve _scaleCurve;

    static int colorPropertyID = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock propertyBlock;

    private float _scale;
    private MeshRenderer _meshRenderer;

    private float _age;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        Debug.Assert(_meshRenderer != null, "Explosion missing renderer");
    }

    public void Initialize(Vector3 position, float blastRadius, float damage)
    {
        TargetPoint.FillBuffer(position, blastRadius);
        for (int i = 0; i < TargetPoint.BufferedCount; i++)
        {
            TargetPoint.GetBuffered(i).Enemy.ApplyDamage(damage);
        }

        transform.localPosition = position;
        _scale = 2f * blastRadius;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        if (_age > _duration)
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        if(propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        float t = _age / _duration;
        Color c = Color.clear;
        c.a = _opacityCurve.Evaluate(t);
        propertyBlock.SetColor(colorPropertyID, c);
        _meshRenderer.SetPropertyBlock(propertyBlock);
        transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));

        return true;
    }
}
