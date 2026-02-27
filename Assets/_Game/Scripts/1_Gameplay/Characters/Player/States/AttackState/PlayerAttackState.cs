using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : StateBase
{
    int[] comboHashIDs = new int[3] { AnimationConfig_UnityChan.StateHashes.Attack01, AnimationConfig_UnityChan.StateHashes.Attack02, AnimationConfig_UnityChan.StateHashes.Attack03 };
    int comboIndex = 0;
    bool _hasAtkInput;
    float _waitTimer;

    public override void OnEnter()
    {
        comboIndex = 0;
        owner.PlayAnimation(comboHashIDs[comboIndex]);
        _hasAtkInput = false;

        owner.UseRootMotion = true;
        _waitTimer = 0f;
    }

    public override void OnUpdate()
    {
        if (GameManager.Instance.inputManager.CurrentInput.IsAttack)
        {
            _hasAtkInput = true;
        }
        //闪避
        if (GameManager.Instance.inputManager.CurrentInput.isDodge)
        {
            stateMachine.ChangeState<PlayerDodgeState>();
            return;
        }
        if (owner.IsAnimationFinished(comboHashIDs[comboIndex], comboIndex == 2 ? 0.5f : 0.75f))
        {
            if (_hasAtkInput && comboIndex < 2)
            {
                comboIndex++;
                owner.PlayAnimation(comboHashIDs[comboIndex]);
                _hasAtkInput = false;
                _waitTimer = 0f;
            }
            else
            {
                if (_hasAtkInput && comboIndex == 2)
                {
                    comboIndex = 0;
                    owner.PlayAnimation(comboHashIDs[comboIndex]);
                    _hasAtkInput = false;
                    _waitTimer = 0f;
                }

                _waitTimer += Time.deltaTime;
                if(_waitTimer > 0.3f)
                {
                    stateMachine.ChangeState<PlayerIdleState>();
                    _waitTimer = 0f;
                    return;
                }
            }
        }
        owner.FaceInput(GameManager.Instance.inputManager.CurrentInput.MoveVector);
    }

    public override void OnExit()
    {
        owner.UseRootMotion = false;
    }
}
