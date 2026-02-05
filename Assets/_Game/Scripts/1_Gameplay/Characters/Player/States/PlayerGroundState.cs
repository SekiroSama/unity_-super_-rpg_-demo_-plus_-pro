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
        if (Mathf.Abs(owner.verSpeed) > 0.1f&&!owner.isGrounded)
        {
            stateMachine.ChangeState<PlayerAirState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }
        if (GameManager.Instance.inputManager.CurrentInput.isJump)
        {
            float jumpTimer = 0f;
            TimerMgr.Instance.CreateTimer(true, 500, () =>
            {
                Physics.gravity = new Vector3(0, owner.gravity, 0);
            }, 20, () =>
            {
                jumpTimer += 0.04f;
                Physics.gravity = Vector3.Lerp(new Vector3(0,owner.gravity,0),new Vector3(0,owner.jumpForce,0),jumpTimer );
            });
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
