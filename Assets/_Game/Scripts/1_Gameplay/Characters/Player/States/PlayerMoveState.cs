using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    
    public override void OnEnter()
    {
        Debug.Log(1);
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.HorLocomotion, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //角色混合树动画参数更新
        owner.UpdateHorLocomotion(inputValue);
          
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
}
