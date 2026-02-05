using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 破韧节点
/// 通常返回ing
/// </summary>
public class DownedNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isDownedTriggered = false;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);
        if (!_enemyController.isDowned)
        {
            _isDownedTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }
        else
        {
            if (!_isDownedTriggered)
            {
                _enemyController.Downed();
                _isDownedTriggered = true;
            }

            currentStatus = NodeStatus.RUNNING;
            return currentStatus;
        }
    }
}
