using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : StateBase
{
    public override void OnEnter()
    {
        owner.UpdateAnimation(0f);
        owner.PlayAnimation(AnimHash.Locomotion, 0.1f);
    }

    public override void OnUpdate()
    {
        if (GameManager.Instance.gameInputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (GameManager.Instance.gameInputManager.CurrentInput.MoveVector.sqrMagnitude > 0.01)
        {
            stateMachine.ChangeState<PlayerRunState>();
            return;
        }

        owner.UpdateAnimation(0f);
    }
}
