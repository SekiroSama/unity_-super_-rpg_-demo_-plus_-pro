using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 条件节点
/// </summary>
/// <typeparam name="T">要比较的类型</typeparam>
public class ConditionNode<T> : BTNode where T : IComparable
{
    private string _blackBoardKey;
    private T _targetValue;
    private CompareType _compareType;
    private UnityAction _unityAction;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="blackBoardKey">谁来比较</param>
    /// <param name="targetValue">和谁比较</param>
    /// <param name="compareType">比较条件</param>
    /// <param name="unityAction">成功回调</param>
    public ConditionNode(string blackBoardKey, T targetValue, CompareType compareType, UnityAction unityAction = null)
    {
        _blackBoardKey = blackBoardKey;
        _targetValue = targetValue;
        _compareType = compareType;
        _unityAction = unityAction;
    }
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        switch (_compareType)
        {
            case CompareType.Greater:
                if(Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) > 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    _unityAction?.Invoke();
                    return NodeStatus.SUCCESS;
                }
                break;
            case CompareType.Less:
                if (Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) < 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    _unityAction?.Invoke();
                    return NodeStatus.SUCCESS;
                }
                break;
            case CompareType.Equal:
                if (Comparer<T>.Default.Compare(blackboard.GetValue<T>(_blackBoardKey), _targetValue) == 0)
                {
                    currentStatus = NodeStatus.SUCCESS;
                    _unityAction?.Invoke();
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
