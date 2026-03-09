using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : StateBase
{
    public override void OnEnter()
    {
        owner.PlayAnimation(AnimationConfig_UnityChan.StateHashes.Hurt, AnimationConfig_UnityChan.TransitionSettings.NormalTransitionDuration);
    }
    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }

   
}
