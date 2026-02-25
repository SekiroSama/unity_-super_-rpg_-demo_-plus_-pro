using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    float moveTimer = 0;
    int timerId;
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.HorLocomotion, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        moveTimer = 0;
        timerId = TimerMgr.Instance.CreateTimer(false, 8000, () => { }, 200, () =>
        {
           moveTimer += .2f;
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (moveTimer < owner.fadeTime)
        {
            owner.moveSpeed = Mathf.Lerp(owner.runSpeed, owner.walkSpeed, moveTimer / owner.fadeTime);
        }
        //角色混合树动画参数更新
        owner.UpdateHorLocomotion(Mathf.Lerp(PlayerGroundState.runValue, PlayerGroundState.inputValue, moveTimer / owner.fadeTime));

        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit()
    {
        TimerMgr.Instance.StopTimer(timerId);
        PlayerGroundState.runValue = 0;
    }
}
