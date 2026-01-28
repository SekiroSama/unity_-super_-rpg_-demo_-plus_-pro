using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimationConfig;

public class PlayerIdleState : PlayerGroundState
{
    private float _restTimer = 0f;
    public override void OnEnter()
    {
        _restTimer = 0f;

        if (stateMachine.PreviousState is PlayerAttackState)
        {
            owner.PlayAnimation(AnimationConfig.StateHashes.Idle, AnimationConfig.TransitionSettings.AttackOverTransitionDuration);
        }
        else
        {
            owner.PlayAnimation(AnimationConfig.StateHashes.Idle, AnimationConfig.TransitionSettings.NormalTransitionDuration);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _restTimer += Time.deltaTime;
        if (_restTimer > 10f)
        {
            owner.PlayAnimation(AnimationConfig.StateHashes.Rest, AnimationConfig.TransitionSettings.NormalTransitionDuration);
            _restTimer = -10f;
            return;
        }
    }
}
