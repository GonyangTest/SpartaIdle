using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 모든 움직임 정지
        stateMachine.MovementSpeedModifier = 0f;
        
        base.Enter();
        
        // NavMeshAgent 정지
        if (stateMachine.player.NavMeshAgent != null)
        {
            stateMachine.player.NavMeshAgent.isStopped = true;
        }
        
        // 사망 애니메이션 시작
        StartAnimation(stateMachine.player.AnimationData.DeadParameterHash);
        
        // Collider 비활성화 (선택적)
        var colliders = stateMachine.player.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            if (!(collider is CharacterController)) // CharacterController는 유지
            {
                collider.enabled = false;
            }
        }
        
        Debug.Log("[PlayerDeathState] Player 사망 상태 진입");
    }

    public override void Exit()
    {
        base.Exit();
        
        // NavMeshAgent 재시작
        if (stateMachine.player.NavMeshAgent != null)
        {
            stateMachine.player.NavMeshAgent.isStopped = false;
        }
        
        // 사망 애니메이션 종료
        StopAnimation(stateMachine.player.AnimationData.DeadParameterHash);
        
        // Collider 재활성화
        var colliders = stateMachine.player.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        
        Debug.Log("[PlayerDeathState] Player 사망 상태 종료");
    }

    public override void Update()
    {
        // 사망 상태에서는 아무것도 하지 않음
        // 부활 로직은 외부에서 처리
    }
} 