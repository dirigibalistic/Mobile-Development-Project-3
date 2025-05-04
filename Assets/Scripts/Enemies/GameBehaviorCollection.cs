using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameBehaviorCollection
{
    private List<GameBehavior> _behaviors = new List<GameBehavior>();

    public void Add(GameBehavior behavior)
    {
        _behaviors.Add(behavior);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _behaviors.Count; i++)
        {
            if (!_behaviors[i].GameUpdate()) //if enemy is dead, remove from list
            {
                int lastIndex = _behaviors.Count - 1;
                _behaviors[i] = _behaviors[lastIndex];
                _behaviors.RemoveAt(lastIndex);
                i--;
            }
        }
    }

    public void Reset()
    {
        foreach (GameBehavior behavior in _behaviors)
        {
            Object.Destroy(behavior?.gameObject);
        }

        _behaviors.Clear();
    }
}