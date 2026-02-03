using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动节点
/// </summary>
public class MoveToTargetNode : BTNode
{
    private string _targetKey;
    private float _stoppingDistanceSqr;
    private Vector3 _targetPos;
    private Vector3 _currentPos;
    private EnemyController _enemyController;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="targetKey">黑板Key</param>
    /// <param name="stoppingDistance">停止距离</param>
    public MoveToTargetNode(string targetKey, float stoppingDistance)
    {
        _targetKey = targetKey;
        _stoppingDistanceSqr = stoppingDistance * stoppingDistance;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);
        if (_enemyController == null)
        {
            currentStatus = NodeStatus.FAILURE;
            return currentStatus;
        }

        _currentPos = _enemyController.transform.position;
        _targetPos = blackboard.GetValue<Vector3>(_targetKey);
        if((_currentPos - _targetPos).sqrMagnitude > 3 * _stoppingDistanceSqr)
        {
            _enemyController.MoveToTarget(_targetPos, EnemyAnimationConfig.FatFatDragonSettings.FatFatDragonRunSpeedRatio);
            currentStatus = NodeStatus.RUNNING;
            return currentStatus;
        }
        else if ((_currentPos - _targetPos).sqrMagnitude > _stoppingDistanceSqr)
        {
            _enemyController.MoveToTarget(_targetPos, EnemyAnimationConfig.FatFatDragonSettings.FatFatDragonWalkSpeedRatio);
            currentStatus = NodeStatus.RUNNING;
            return currentStatus;
        }
        else
        {
            _enemyController.StopMove();
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }
    }
}
