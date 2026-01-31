using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    Dictionary<string, object> data = new Dictionary<string, object>();

    public void SetValue<T>(string key, T value)
    {
        data[key] = value;
    }

    public T GetValue<T>(string key)
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
