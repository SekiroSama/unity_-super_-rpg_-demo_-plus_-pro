using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投射物攻击
/// 通常返回ing 攻击完成返回成功
/// </summary>
public class ProjectileAttackNode : BTNode
{
    private EnemyController _enemyController;
    private bool _isProjectileAttackTriggered = false;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);

        if (!_isProjectileAttackTriggered)
        {
            _enemyController.ProjectileAttack();
            _isProjectileAttackTriggered = true;
        }

        if (!_enemyController.isAttacking)
        {
            _isProjectileAttackTriggered = false;
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        currentStatus = NodeStatus.RUNNING;
        return currentStatus;
    }
}
