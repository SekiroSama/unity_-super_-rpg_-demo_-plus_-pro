using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : StateBase
{

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        if(GameInputManager.Instance.CurrentInput.MoveVector.sqrMagnitude <= 0.01)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }

        owner.Move(GameInputManager.Instance.CurrentInput.MoveVector);
        owner.UpdateAnimation(GameInputManager.Instance.CurrentInput.MoveVector.magnitude);
    }
}
