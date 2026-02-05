using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static BTNode;

/// <summary>
/// 攻击
/// 通常返回ing 攻击完成返回成功
/// </summary>
public class AtkNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isMeleeHitTriggered = false;
    private UnityAction _atkAction;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="atkAction">攻击函数委托</param>
    public AtkNode(UnityAction atkAction)
    {
        _atkAction = atkAction;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);

        if (!_isMeleeHitTriggered)
        {
            _atkAction?.Invoke();
            _isMeleeHitTriggered = true;
        }

        if (!_enemyController.isAttacking)
        {
            _isMeleeHitTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
