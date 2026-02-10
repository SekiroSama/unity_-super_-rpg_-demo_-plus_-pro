using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    float runTimer = 0;
    int timerId;
    
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.HorLocomotion, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        PlayerGroundState.runValue = 2f;
        runTimer = 0;
        timerId = TimerMgr.Instance.CreateTimer(false, 8000, () => { }, 20, () =>
        {
            runTimer += .2f;
        });
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (runTimer < owner.fadeTime)
        {
            owner.moveSpeed = Mathf.Lerp(owner.walkSpeed, owner.runSpeed,runTimer / owner.fadeTime);
        }
        if (!GameManager.Instance.inputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }

        owner.UpdateHorLocomotion(Mathf.Lerp(inputValue,PlayerGroundState.runValue,runTimer/owner.fadeTime));
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit() 
    {
        TimerMgr.Instance.StopTimer(timerId);
        //GameManager.Instance.runTimer = 0;
        PlayerGroundState.runValue = 2f;
        owner.moveSpeed = owner.walkSpeed;
    }
}
