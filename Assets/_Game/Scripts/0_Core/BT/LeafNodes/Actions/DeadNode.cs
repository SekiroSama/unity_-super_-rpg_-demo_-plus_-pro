using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 死亡节点
/// 通常返回ing
/// </summary>
public class DeadNode : BTNode
{
    private bool _isDeadTriggered = false;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        if(_isDeadTriggered) return currentStatus;
        blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController).Die();
        _isDeadTriggered = true;
        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
