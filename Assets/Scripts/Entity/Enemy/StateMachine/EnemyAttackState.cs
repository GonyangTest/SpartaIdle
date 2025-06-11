using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.enemy.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.enemy.AnimationData.BaseAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.enemy.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.enemy.AnimationData.BaseAttackParameterHash);
    }
}
