using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChasingState : PlayerGroundState
{
    public PlayerChasingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;

        base.Enter();

        // Chase 시작 시 한번 더 Enemy 확인
        GetEnemyInChasingRange();

        if (_target != null)
        {
            stateMachine.player.SetDestination(_target.transform.position);
            Debug.Log($"[PlayerChasingState] 추적 목적지 설정: {_target.name} 위치: {_target.transform.position}");
        }

        StartAnimation(stateMachine.player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

    if (_target != null)
    {
        // 목적지를 지속적으로 업데이트
        stateMachine.player.SetDestination(_target.transform.position);
    }
    else
    {
        // Enemy를 잃어버리면 Idle 상태로 돌아감
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    }
}
