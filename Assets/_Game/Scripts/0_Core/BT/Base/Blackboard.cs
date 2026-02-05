using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    Dictionary<Enemy_AIBlackBoard_Config, object> data = new Dictionary<Enemy_AIBlackBoard_Config, object>();

    public void SetValue<T>(Enemy_AIBlackBoard_Config key, T value)
    {
        data[key] = value;
    }

    public T GetValue<T>(Enemy_AIBlackBoard_Config key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        else
        {
            return default;
            throw new KeyNotFoundException($"Key '{key}' not found in Blackboard.");
        }
    }
}
