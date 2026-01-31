using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等待节点
/// </summary>
public class WaitNode : BTNode
{
    private float _duration;
    private float _elapsedTime;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="duration">等待时间</param>
    public WaitNode(float duration) 
    { 
        _duration = duration;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _duration)
        {
            currentStatus = NodeStatus.SUCCESS;
            _elapsedTime = 0f;
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }
}
