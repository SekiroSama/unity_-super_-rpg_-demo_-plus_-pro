using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BTNode;

/// <summary>
/// 龙吼
/// 通常返回ing 龙吼完成过返回成功 
/// </summary>
public class DragonShoutNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isDragonShouTriggered = false;//防止多次触发 _enemyController.DragonShout();
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);
        if (_enemyController.isDragonShouTriggered)
        {
            _isDragonShouTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        if (!_isDragonShouTriggered)
        {
            _enemyController.DragonShout();
            _isDragonShouTriggered = true;
        }
        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
