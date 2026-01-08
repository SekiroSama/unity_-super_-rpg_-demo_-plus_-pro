using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public StateBase CurrentState;

    void Initialize(StateBase startState)
    {
        startState.OnEnter();
        CurrentState = startState;
    }

    void Update()
    {
        CurrentState.OnUpdate();
    }

    void ChangeState(StateBase newState, bool changeToSelf = false)
    {
        if(newState == null || (!changeToSelf && newState == CurrentState))
        {
            return;
        }

        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}
