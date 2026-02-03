using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState: PlayerGroundState
{
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig.StateHashes.Locomotion, AnimationConfig.TransitionSettings.NormalTransitionDuration);
        //GameManager.Instance.runTimer = 0;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //if (GameManager.Instance.runTimer<owner.fadeTime)
        //{
        //   owner.moveSpeed = Mathf.Lerp(owner.walkSpeed, owner.runSpeed, GameManager.Instance.runTimer/owner.fadeTime);
        //}
        if (!GameManager.Instance.inputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }

        //owner.UpdateLocomotion(Mathf.Lerp(inputValue,1.5f,GameManager.Instance.runTimer/owner.fadeTime));
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit() 
    {
        //GameManager.Instance.runTimer = 0;
        runValue = 1.5f;
        owner.moveSpeed = owner.walkSpeed;
    }
}
