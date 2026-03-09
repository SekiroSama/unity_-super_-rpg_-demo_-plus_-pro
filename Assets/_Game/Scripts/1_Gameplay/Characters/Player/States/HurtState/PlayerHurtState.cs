using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : StateBase
{
    bool isFinish;
    int timerId;
    public override void OnEnter()
    {
        isFinish = false;
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Hurt, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        timerId = TimerMgr.Instance.CreateTimer(false ,800,() =>
        {
            isFinish = true;
        }, 20, () =>
        {

        });
    }
    public override void OnUpdate()
    {
        if(isFinish)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void OnExit()
    {

    }

   
}
