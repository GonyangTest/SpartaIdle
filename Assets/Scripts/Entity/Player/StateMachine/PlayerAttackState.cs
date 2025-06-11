using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.player.AnimationData.BaseAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.player.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.player.AnimationData.BaseAttackParameterHash);
    }
}
