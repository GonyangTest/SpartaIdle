using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyGroundState : EnemyBaseState
{
    public EnemyGroundState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.enemy.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.enemy.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        IsEnemyInAttackRange();
        if(stateMachine.IsAttacking)
        {
            stateMachine.ChangeState(stateMachine.ComboAttackState);
            return;
        }


        IsEnemyInChasingRange();
        if(stateMachine.IsChasing)
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
