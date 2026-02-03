using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : StateBase
{
    public float inputValue;
    public float runValue;

    public override void OnEnter()
    {
    }
    public override void OnUpdate()
    {
        
        inputValue = GameManager.Instance.inputManager.CurrentInput.MoveVector.sqrMagnitude;
        if (GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.isJump)
        {
            stateMachine.ChangeState<PlayerAirState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.MoveVector.sqrMagnitude <= 0.01)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.MoveVector.sqrMagnitude > 0.01&&!GameManager.Instance.inputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.IsRun)
        {
            stateMachine.ChangeState<PlayerRunState>();
            return;
        }
        
    }
    public override void OnExit()
    {

    }
}
