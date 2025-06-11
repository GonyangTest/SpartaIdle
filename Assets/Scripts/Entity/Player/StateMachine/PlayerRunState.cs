using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;

        base.Enter();

        // RunState에서는 기본 목적지로 이동
        if (stateMachine.player.Target != null)
        {
            stateMachine.player.SetDestination(stateMachine.player.Target.position);
            Debug.Log($"[PlayerRunState] 기본 목적지 설정: {stateMachine.player.Target.name}");
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


    }
}
