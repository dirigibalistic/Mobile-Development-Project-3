using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        Debug.Assert(Enemy != null, "Enemy prefab missing Enemy component on root", this);
        Debug.Assert(GetComponent<SphereCollider>() != null, "TargetPoint missing sphere collider", this);
        Debug.Assert(gameObject.layer == 9, "TargetPoint on wrong layer", this);
    }
}