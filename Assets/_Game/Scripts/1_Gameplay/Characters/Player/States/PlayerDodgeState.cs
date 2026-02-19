using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : StateBase
{
    bool isFinish;
    int timerId;
    bool isAttack;
    public override void OnEnter()
    {
        isFinish = false;
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Dodge, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        timerId = TimerMgr.Instance.CreateTimer(true, 500, () =>
        {
            isFinish = true;
        }, 20, () =>
        {});
    }
    public override void OnUpdate()
    {
        if (GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            isAttack = true;
        }
        if(isAttack)
        {
            if (isFinish)
            {
                stateMachine.ChangeState<PlayerDodgeAttackState>();
                isAttack = false;
            }
            return;

        }
        else if (isFinish)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        owner.RudeMove(-owner.transform.forward*0.2f);
        
       
    }
    public override void OnExit()
    {
        TimerMgr.Instance.StopTimer(timerId);
    }


}
