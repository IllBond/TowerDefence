using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionState : State
{
    public ReactionState(EnemyBase character, StateMachine stateMachine) : base(character, stateMachine)
    { }

    public override void Enter()
    {
        character.animator.SetTrigger("reaction");
        character.SetMaterial(character.frozzenMaterial);
    }


    public override void HandleInput()
    { }

    public override void LogicUpdate()
    { }

    public override void PhysicsUpdate()
    { }

    public override void Exit()
    {
        character.SetMaterial(character.baseMaterial);
    }
}
