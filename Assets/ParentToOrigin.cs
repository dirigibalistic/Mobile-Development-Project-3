using UnityEngine;

public class ParentToOrigin : MonoBehaviour
{
    private void Awake()
    {
        this.transform.parent = GameObject.Find("WorldOrigin").transform;
        transform.position = Vector3.zero;
    }
}
