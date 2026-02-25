using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpDownState : PlayerAirState
{
    public override void OnEnter()
    {
   
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.VerLocomotion, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        owner.Move(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }
    public override void OnExit()
    {

    }


}
