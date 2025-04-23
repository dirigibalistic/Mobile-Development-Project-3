using UnityEngine;

[CreateAssetMenu(fileName = "Bindings", menuName = "Scriptable Objects/Bindings")]
public class Bindings : ScriptableObject
{
    private int _money, _round;

    public int Money => _money;
    public int Round => _round;
}
