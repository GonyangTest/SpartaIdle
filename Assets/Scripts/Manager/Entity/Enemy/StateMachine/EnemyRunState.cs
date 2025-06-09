using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyGroundState
{
    public EnemyRunState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;

        base.Enter();

        // RunState에서는 기본 목적지로 이동
        if (stateMachine.enemy.Target != null)
        {
            stateMachine.enemy.SetDestination(stateMachine.enemy.Target.position);
            Debug.Log($"[PlayerRunState] 기본 목적지 설정: {stateMachine.enemy.Target.name}");
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
