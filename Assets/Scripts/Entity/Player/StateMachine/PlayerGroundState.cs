using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.AnimationData.GroundParameterHash);
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


        GetEnemyInChasingRange();
        if(_target != null)
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
