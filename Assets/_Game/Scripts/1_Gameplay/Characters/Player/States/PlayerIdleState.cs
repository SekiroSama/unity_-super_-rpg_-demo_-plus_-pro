using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimationConfig;

public class PlayerIdleState : StateBase
{
    public override void OnEnter()
    {
        owner.UpdateLocomotion(0f);

        if(stateMachine.PreviousState is PlayerAttackState)
        {
            owner.PlayAnimation(AnimationConfig.StateHashes.Idle, AnimationConfig.TransitionSettings.AttackOverTransitionDuration);
        }
        else
        {
            owner.PlayAnimation(AnimationConfig.StateHashes.Idle, AnimationConfig.TransitionSettings.NormalTransitionDuration);
        }
    }

    public override void OnUpdate()
    {
        if (GameManager.Instance.InputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (GameManager.Instance.InputManager.CurrentInput.MoveVector.sqrMagnitude > 0.01)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }

        owner.UpdateLocomotion(0f);
    }
}
