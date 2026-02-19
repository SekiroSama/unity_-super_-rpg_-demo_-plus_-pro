using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : StateBase
{
    bool isFinish;
    int timerId;
    float timer = 0;   
    public override void OnEnter()
    {
        isFinish = false;
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Dodge, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        timerId = TimerMgr.Instance.CreateTimer(true, 500, () =>
        {
            isFinish = true;
        }, 20, () =>
        {
            
            timer += .02f;
        });
    }
    public override void OnUpdate()
    {
        owner.RudeMove(-owner.transform.forward*0.2f);
        if(GameManager.Instance.inputManager.CurrentInput.IsAttack&&isFinish)
        {
            //stateMachine.ChangeState<>();
        }
        if (isFinish)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }
    public override void OnExit()
    {

    }


}
