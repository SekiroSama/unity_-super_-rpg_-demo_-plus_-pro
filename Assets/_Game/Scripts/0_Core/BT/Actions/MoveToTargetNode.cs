using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动节点
/// </summary>
public class MoveToTargetNode : BTNode
{
    private string _targetKey;
    private float _stoppingDistance;
    private Transform _targetTransform;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="targetKey">黑板Key</param>
    /// <param name="stoppingDistance">停止距离</param>
    public MoveToTargetNode(string targetKey, float stoppingDistance)
    {
        _targetKey = targetKey;
        _stoppingDistance = stoppingDistance;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _targetTransform = blackboard.GetValue<Transform>(_targetKey);



        return currentStatus;
    }
}
