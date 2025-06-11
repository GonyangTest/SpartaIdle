using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.AnimationData.IdleParameterHash);

        // RunState에서는 기본 목적지로 이동
        if (stateMachine.player.Target != null)
        {
            if(stateMachine.player.SetDestination(stateMachine.player.Target.position))
            {
                stateMachine.ChangeState(stateMachine.RunState);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();


    }
}
