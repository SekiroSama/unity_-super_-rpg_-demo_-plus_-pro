using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected PlayerController playerController;
    protected StateMachine stateMachine;
    public StateBase(PlayerController playerController, StateMachine stateMachine)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
    }

    public virtual void OnEnter()
    {

    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
}
