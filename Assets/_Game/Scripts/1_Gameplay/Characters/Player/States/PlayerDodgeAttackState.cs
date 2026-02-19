using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeAttackState : StateBase
{
    bool isFinish;
    int timerId;
    public override void OnEnter()
    {
        isFinish = false;
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.AttackDodge, AnimationConfig_UnityChan.TransitionSettings.AttackTransitionDuration);
        timerId = TimerMgr.Instance.CreateTimer(true, 500, () =>
        {
            isFinish = true;
        }, 20, () =>
        { });
    }
    public override void OnUpdate()
    {
        owner.RudeMove(owner.transform.forward*0.25f);
        if (isFinish)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void OnExit()
    {
       
    }

}
