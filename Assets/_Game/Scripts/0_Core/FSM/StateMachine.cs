using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public StateBase CurrentState;
    public PlayerController owner;
    Dictionary<System.Type, StateBase> States = new Dictionary<System.Type, StateBase>();

    public StateMachine(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Initialize<T>() where T : StateBase, new()
    {
        ChangeState<T>();
    }

    public void Update()
    {
        CurrentState.OnUpdate();
    }

    public void ChangeState<T>() where T: StateBase, new()
    {
        if(typeof(T) == CurrentState?.GetType())
        {
            return;
        }

        if (!States.ContainsKey(typeof(T)))
        {
            StateBase newstate = new T();
            newstate.Initialize(owner, this);
            States.Add(typeof(T), newstate);
        }

        CurrentState?.OnExit();
        CurrentState = States[typeof(T)];
        CurrentState.OnEnter();
    }
}
