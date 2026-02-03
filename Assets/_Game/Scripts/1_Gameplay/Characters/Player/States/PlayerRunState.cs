using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    float runTimer = 0;
    int timerId;
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig.StateHashes.Locomotion, AnimationConfig.TransitionSettings.NormalTransitionDuration);
        runTimer = 0;
        timerId = TimerMgr.Instance.CreateTimer(false, 8000, () => { }, 200, () =>
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

        owner.UpdateLocomotion(Mathf.Lerp(inputValue,1.5f,runTimer/owner.fadeTime));
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit() 
    {
        //GameManager.Instance.runTimer = 0;
        runValue = 1.5f;
        owner.moveSpeed = owner.walkSpeed;
    }
}
