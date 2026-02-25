using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimationConfig_UnityChan;

public class PlayerIdleState : PlayerGroundState
{
    private float _restTimer = 0f;
    public override void OnEnter()
    {
        _restTimer = 0f;

        if (stateMachine.PreviousState is PlayerAttackState)
        {
            owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Idle, AnimationConfig_UnityChan.TransitionSettings.AttackOverTransitionDuration);
        }
        else
        {
            owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Idle, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _restTimer += Time.deltaTime;
        if (_restTimer > 10f)
        {
            owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Rest, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
            _restTimer = -10f;
            return;
        }
    }
}
