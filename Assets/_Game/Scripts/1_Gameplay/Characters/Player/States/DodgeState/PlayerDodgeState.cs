using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : StateBase
{
    bool isFinish;
    int timerId;
    bool isAttack;
    //计时动画过渡时间
    public override void OnEnter()
    {
        isFinish = false;
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Dodge, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        timerId = TimerMgr.Instance.CreateTimer(true, 500, () =>
        {
            isFinish = true;
           
        }, 20, () =>
        {
            owner.ghostMaterial.SetFloat("_StartTime",Time.time);
            owner.CreateGhost();
        });
    }
    public override void OnUpdate()
    {
        if (GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            isAttack = true;
        }
        //如果在躲避过程中按攻击键就可以冲刺攻击
        if(isAttack)
        {
            if (isFinish)
            {
                stateMachine.ChangeState<PlayerForceAttackState>();
                isAttack = false;
            }
            return;
        }
        else if (isFinish)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        owner.RudeMove(-owner.transform.forward*owner.dodgeSpeed);
       
    }
    public override void OnExit()
    {
        TimerMgr.Instance.StopTimer(timerId);
    }


}
