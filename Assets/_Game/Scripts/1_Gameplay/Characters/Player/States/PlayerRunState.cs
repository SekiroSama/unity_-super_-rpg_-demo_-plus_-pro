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
        if (GameManager.Instance.InputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (GameManager.Instance.InputManager.CurrentInput.MoveVector.sqrMagnitude <= 0.01)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }

        owner.Move(GameManager.Instance.InputManager.CurrentInput.MoveVector);
        owner.UpdateLocomotion(GameManager.Instance.InputManager.CurrentInput.MoveVector.magnitude);
    }
}
