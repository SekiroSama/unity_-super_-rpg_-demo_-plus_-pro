using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isSleepTriggered = false;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);
        if (!_enemyController.isDowned)
        {
            _isSleepTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }
        else
        {
            if (!_isSleepTriggered)
            {
                _enemyController.Sleep();
                _isSleepTriggered = true;
            }

            currentStatus = NodeStatus.RUNNING;
            return currentStatus;
        }
    }
}
