using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// debugNode
/// </summary>
public class DebugNode : BTNode
{
    string _message;
    NodeStatus _result;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="message">日志</param>
    /// <param name="result">返回结果</param>
    public DebugNode(string message, NodeStatus result)
    {
        _message = message;
        _result = result;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        Debug.Log(_message);
        currentStatus = _result;
        return currentStatus;
    }
}
