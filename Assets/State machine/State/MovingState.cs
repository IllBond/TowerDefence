using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    public MovingState(EnemyBase character, StateMachine stateMachine) : base(character, stateMachine)
    { }

    public override void Enter()
    {
        character.animator.SetTrigger("walk");
    }

    public override void HandleInput()
    { }

    public override void LogicUpdate()
    { }

    public override void PhysicsUpdate()
    { }

    public override void Exit()
    { }
}
