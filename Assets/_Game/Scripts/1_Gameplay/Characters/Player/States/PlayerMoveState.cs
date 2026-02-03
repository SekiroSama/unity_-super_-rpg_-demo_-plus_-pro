using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Locomotion, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //角色混合树动画参数更新
        owner.UpdateLocomotion(inputValue);
          
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
}
