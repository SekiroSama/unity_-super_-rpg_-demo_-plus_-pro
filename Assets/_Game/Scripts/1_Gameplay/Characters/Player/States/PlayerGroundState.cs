using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : StateBase
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
        if (GameManager.Instance.InputManager.CurrentInput.MoveVector.sqrMagnitude > 0.01&&!GameManager.Instance.InputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }
        if (GameManager.Instance.InputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerRunState>();
            return;
        }

    }
    public override void OnExit()
    {

    }
}
