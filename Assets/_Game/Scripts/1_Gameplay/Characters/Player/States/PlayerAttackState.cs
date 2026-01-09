using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : StateBase
{
    int[] comboHashIDs = new int[3] { AnimHash.Attack01, AnimHash.Attack02, AnimHash.Attack03 };
    int comboIndex = 0;
    bool _hasAtkInput;

    public override void OnEnter()
    {
        owner.UpdateAnimation(0f);

        comboIndex = 0;
        owner.PlayAnimation(comboHashIDs[comboIndex]);
        _hasAtkInput = false;

        owner.UseRootMotion = true;
    }

    public override void OnUpdate()
    {
        if (GameInputManager.Instance.CurrentInput.IsAttack)
        {
            _hasAtkInput = true;
        }

        if (owner.IsAnimationFinished(comboHashIDs[comboIndex]))
        {
            if (_hasAtkInput && comboIndex < 2)
            {
                comboIndex++;
                owner.PlayAnimation(comboHashIDs[comboIndex]);
                _hasAtkInput = false;
            }
            else
            {
                stateMachine.ChangeState<PlayerIdleState>();
                return;
            }
        }
    }

    public override void OnExit()
    {
        owner.UseRootMotion = false;
    }
}
