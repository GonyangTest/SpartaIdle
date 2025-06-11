using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyGroundState
{
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;

        base.Enter();

        // Chase 시작 시 한번 더 Enemy 확인
        IsEnemyInChasingRange();

        if (stateMachine.IsChasing)
        {
            stateMachine.enemy.SetDestination(_target.position);
        }
        else
        {
            Debug.LogWarning("[PlayerChasingState] Enter 시 Target이 null입니다!");
        }

        StartAnimation(stateMachine.enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.enemy.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }
}
