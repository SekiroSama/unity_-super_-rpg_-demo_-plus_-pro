using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : StateBase
{

    public override void OnEnter()
    {
     
    }
    public override void OnUpdate()
    {
        if (owner.isGrounded)
        {
            stateMachine.ChangeState<PlayerGroundState>();
            return;
        }
        if(owner.verSpeed>0)
        {
            stateMachine.ChangeState<PlayerJumpUpState>();
            return;
        }
        else if(owner.verSpeed<0)
        {
            stateMachine.ChangeState<PlayerJumpDownState>();
            return;
        }


    }
    public override void OnExit()
    {

    }
}
