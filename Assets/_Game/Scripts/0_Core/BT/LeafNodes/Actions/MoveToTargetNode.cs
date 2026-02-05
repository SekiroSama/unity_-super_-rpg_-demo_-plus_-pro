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
    private float _speedRatio;
    private Vector3 _targetPos;
    private Vector3 _currentPos;
    private EnemyController _enemyController;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="targetKey">黑板Key,用来取目标位置</param>
    /// <param name="stoppingDistance">停止距离</param>
    /// <param name="speedRatio">移动速度</param>
    public MoveToTargetNode(string targetKey, float stoppingDistance, float speedRatio)
    {
        _targetKey = targetKey;
        _stoppingDistanceSqr = stoppingDistance * stoppingDistance;
        _speedRatio = speedRatio;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);
        _currentPos = _enemyController.transform.position;
        _targetPos = blackboard.GetValue<Vector3>(_targetKey);

        if ((_currentPos - _targetPos).sqrMagnitude > _stoppingDistanceSqr)
        {
            _enemyController.MoveToTarget(_targetPos, _speedRatio);
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
