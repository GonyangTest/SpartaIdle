using UnityEngine;

public class EnemyHitState : EnemyGroundState
{
    private float hitStunTimer;
    
    public EnemyHitState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 움직임 멈춤
        stateMachine.MovementSpeedModifier = 0f;
        
        base.Enter();
        
        // 피격 스턴 시간 초기화
        hitStunTimer = stateMachine.enemy.Data.HitData.HitStunDuration;
        
        // 피격 애니메이션 시작
        StartAnimation(stateMachine.enemy.AnimationData.HitParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        // 피격 애니메이션 종료
        StopAnimation(stateMachine.enemy.AnimationData.HitParameterHash);
    }

    public override void Update()
    {
        base.Update();
        
        // 스턴 시간 감소
        hitStunTimer -= Time.deltaTime;
        
        // 스턴 시간 종료 시 Idle 상태로 복귀
        if (hitStunTimer <= 0f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
} 