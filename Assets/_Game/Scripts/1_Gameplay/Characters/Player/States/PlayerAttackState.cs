using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : StateBase
{
    public override void OnEnter()
    {
        owner.UpdateAnimation(0f);
        owner.PlayAnimation(AnimHash.Attack01);
    }

    public override void OnUpdate()
    {
        if(owner.IsAttckFinished())
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }
}
