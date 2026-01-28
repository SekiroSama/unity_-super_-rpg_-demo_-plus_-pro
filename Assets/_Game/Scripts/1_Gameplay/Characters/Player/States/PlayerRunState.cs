using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState: PlayerGroundState
{
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig.StateHashes.Locomotion, AnimationConfig.TransitionSettings.NormalTransitionDuration);
        GameManager.Instance.Timer = 0;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (GameManager.Instance.Timer<owner.fadeTime)
        {
           owner.moveSpeed = Mathf.Lerp(owner.walkSpeed, owner.runSpeed, GameManager.Instance.Timer/owner.fadeTime);
        }
        if (!GameManager.Instance.inputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit() 
    {
        owner.moveSpeed = owner.walkSpeed;
    }
}
