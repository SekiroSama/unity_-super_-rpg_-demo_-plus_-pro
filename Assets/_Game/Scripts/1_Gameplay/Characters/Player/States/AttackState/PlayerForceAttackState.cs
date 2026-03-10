using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceAttackState : StateBase
{
    bool isFinish;
    int timerId;
    //计时0.8s用于动画过渡的时间
    public override void OnEnter()
    {
        isFinish = false;
        
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.ForceAttack, AnimationConfig_UnityChan.TransitionSettings.AttackTransitionDuration);
        //owner.IgnoreCollsion();
        timerId = TimerMgr.Instance.CreateTimer(true, 1000, () =>
        {
            isFinish = true;
        }, 20, () =>
        { 

        });
    }
    public override void OnUpdate()
    {
        if(owner.isRush)
            owner.RudeMove(owner.transform.forward*0.25f);
        if (owner.weaponController.isHit)
        {
            StyleRankManager.Instance.AddScore(30);
            owner.weaponController.isHit = false;
        }
            
        if (isFinish)
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public override void OnExit()
    {
       owner.isHurt = false;
       //owner.ResetCollsion();
    }

}
