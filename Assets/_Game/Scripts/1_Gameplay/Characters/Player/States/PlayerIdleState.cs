using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : StateBase
{
    public override void OnUpdate()
    {
        if(GameInputManager.Instance.CurrentInput.MoveVector.sqrMagnitude > 0.01)
        {
            stateMachine.ChangeState<PlayerRunState>();
            return;
        }

        owner.UpdateAnimation(0f);
    }
}
