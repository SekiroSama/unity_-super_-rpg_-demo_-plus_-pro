using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 泛用行为节点
/// 通常返回ing 出口委托为true时返回成功
/// </summary>
public class GenericActionNode : BTNode
{
    private bool _is_enterActionTriggered = false;
    private UnityAction _enterAction;
    private Func<bool> _outAction;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="enterAction">进入节点调用的委托</param>
    /// <param name="outAction">出口委托，为true时返回成功</param>
    public GenericActionNode(UnityAction enterAction, Func<bool> outAction)
    {
        _enterAction = enterAction;
        _outAction = outAction;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        if (!_is_enterActionTriggered)
        {
            _enterAction?.Invoke();
            _is_enterActionTriggered = true;
        }

        if (_outAction.Invoke())
        {
            _is_enterActionTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
