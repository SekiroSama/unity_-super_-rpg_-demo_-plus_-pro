using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : StateBase
{
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Defense, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        owner.isDefense = true;
    }
    public override void OnUpdate()
    {
        if(!GameManager.Instance.inputManager.CurrentInput.isDefense)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        if(GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerDefenseAttackState>();
            return;
        }
    }
    public override void OnExit()
    {
        owner.isDefense =false;
    }

  
}
