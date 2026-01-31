using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 条件节点
/// </summary>
/// <typeparam name="T">要比较的类型</typeparam>
public class ConditionNode<T> : BTNode where T : IComparable
{
    private string _blackBoardKey;
    private T _targetValue;
    private CompareType _compareType;
    public ConditionNode(string blackBoardKey, T targetValue, CompareType compareType)
    {
        _blackBoardKey = blackBoardKey;
        _targetValue = targetValue;
        _compareType = compareType;
    }
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        switch (_compareType)
        {
            case CompareType.Greater:
                if(Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) > 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    return NodeStatus.SUCCESS;
                }
                break;
            case CompareType.Less:
                if (Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) < 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    return NodeStatus.SUCCESS;
                }
                break;
            case CompareType.Equal:
                if (Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) == 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    return NodeStatus.SUCCESS;
                }
                break;
        }

        currentStatus = NodeStatus.FAILURE;
        return currentStatus;
    }
}

public enum CompareType
{
    Greater,
    Less,
    Equal
}
