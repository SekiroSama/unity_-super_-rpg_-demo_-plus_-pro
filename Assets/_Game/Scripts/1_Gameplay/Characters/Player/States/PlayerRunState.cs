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
        if (GameManager.Instance.gameInputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (GameManager.Instance.gameInputManager.CurrentInput.MoveVector.sqrMagnitude <= 0.01)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }

        owner.Move(GameManager.Instance.gameInputManager.CurrentInput.MoveVector);
        owner.UpdateAnimation(GameManager.Instance.gameInputManager.CurrentInput.MoveVector.magnitude);
    }
}
