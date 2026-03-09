using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseAttackState : PlayerDefenseState
{
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.DefenseAttack, AnimationConfig_UnityChan.TransitionSettings.AttackTransitionDuration);
    }
    public override void OnUpdate()
    {
       base.OnUpdate();
        if(owner.isHurt)
        {
            stateMachine.ChangeState<PlayerForceAttackState>();
            return;
        }
    }
    public override void OnExit()
    {   
        base.OnExit();
     
    }

    
}
