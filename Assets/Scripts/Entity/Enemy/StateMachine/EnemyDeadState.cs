using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private float deathTimer = 1f; // 3초 후 GameObject 파괴
    private float currentTimer;
    
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // 모든 움직임 정지
        stateMachine.MovementSpeedModifier = 0f;
        
        base.Enter();
        
        // 타이머 초기화
        currentTimer = deathTimer;
        
        // NavMeshAgent 정지
        if (stateMachine.enemy.NavMeshAgent != null)
        {
            stateMachine.enemy.NavMeshAgent.isStopped = true;
        }
        
        // 사망 애니메이션 시작
        StartAnimation(stateMachine.enemy.AnimationData.DeadParameterHash);
        
        // Collider 비활성화
        var colliders = stateMachine.enemy.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            if (!(collider is CharacterController))
            {
                collider.enabled = false;
            }
        }
        
        // HealthBar 숨기기
        if (stateMachine.enemy.HealthBar != null)
        {
            stateMachine.enemy.HealthBar.SetActive(false);
        }
        
        Debug.Log("[EnemyDeathState] Enemy 사망 상태 진입");
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(stateMachine.enemy.AnimationData.DeadParameterHash);
        
        Debug.Log("[EnemyDeathState] Enemy 사망 상태 종료");
    }

    public override void Update()
    {
        base.Update();
        
        // 사망 타이머 감소
        currentTimer -= Time.deltaTime;
        
        // 시간이 다 되면 GameObject 파괴
        if (currentTimer <= 0f)
        {
            // StageManager에 적 사망 알림
            StageManager.Instance.OnEnemyDeath(stateMachine.enemy);
            
            Object.Destroy(stateMachine.enemy.gameObject);
        }
    }
} 