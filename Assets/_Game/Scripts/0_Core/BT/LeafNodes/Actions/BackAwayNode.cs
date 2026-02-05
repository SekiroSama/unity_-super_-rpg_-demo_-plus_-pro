using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对峙后退
/// 通常返回ing 精力充足返回成功
/// </summary>
public class BackAwayNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isBackAwayTriggered = false;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);

        if(_enemyController.Stamina >= 0 || !_enemyController.isBackAwaying)
        {
            _isBackAwayTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        if (!_isBackAwayTriggered)
        {
            _enemyController.BackAway();
            _isBackAwayTriggered = true;
        }
        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
