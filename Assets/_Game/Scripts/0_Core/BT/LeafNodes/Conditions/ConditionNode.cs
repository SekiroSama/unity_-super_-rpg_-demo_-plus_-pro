using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 条件节点
/// 比较成功返回SUCCESS 失败返回FAILURE
/// </summary>
public class ConditionNode: BTNode
{
    private Func<bool> _outAction;
    private UnityAction _successcCallback;
    private UnityAction _failureCallback;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="outAction"></param>
    /// <param name="callback"></param>
    public ConditionNode(Func<bool> outAction, UnityAction successcCallback = null, UnityAction failureCallback = null)
    {
        _outAction = outAction;
        _successcCallback = successcCallback;
        _failureCallback = failureCallback;
    }
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        if (_outAction())
        {
            _successcCallback?.Invoke();
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }
        else
        {
            _failureCallback?.Invoke();
            currentStatus = NodeStatus.FAILURE;
            return currentStatus;
        }
    }
}
