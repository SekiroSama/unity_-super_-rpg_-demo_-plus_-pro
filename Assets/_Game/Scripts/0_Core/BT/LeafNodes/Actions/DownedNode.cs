using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 破韧节点
/// 通常返回ing
/// </summary>
public class DownedNode : BTNode
{
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController).Downed();
        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
