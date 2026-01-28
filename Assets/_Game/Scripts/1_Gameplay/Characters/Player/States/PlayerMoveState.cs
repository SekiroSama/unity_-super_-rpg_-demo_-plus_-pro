using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{

    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig.StateHashes.Locomotion, AnimationConfig.TransitionSettings.NormalTransitionDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        owner.Move(GameManager.Instance.InputManager.CurrentInput.MoveVector);
    }
}
