using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected EnemyBase character;
    protected StateMachine stateMachine;

    protected State(EnemyBase character, StateMachine stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    { }

    public virtual void HandleInput()
    { }

    public virtual void LogicUpdate()
    { }

    public virtual void PhysicsUpdate()
    { }

    public virtual void Exit()
    { }

}