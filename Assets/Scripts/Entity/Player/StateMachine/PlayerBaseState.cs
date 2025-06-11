using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected GameObject _target;
    
    // Update 간 시간 체크를 위한 변수들 추가
    private float lastUpdateTime;
    private int updateFrameCount = 0;
    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.player.Data.GroundData;
        lastUpdateTime = Time.time;
    }

    public virtual void Enter()
    {
        // Enter 시 시간 초기화
        lastUpdateTime = Time.time;
        updateFrameCount = 0;
    }

    public virtual void Exit()
    {
    }

    public virtual void HandleInput()
    {
    }

    public virtual void Update()
    {

    }

    public virtual void PhysicsUpdate()
    {
    }
    
    protected void StartAnimation(int AnimationHash)
    {
        stateMachine.player.Animator.SetBool(AnimationHash, true);
    }

    protected void StopAnimation(int AnimationHash)
    {
        stateMachine.player.Animator.SetBool(AnimationHash, false);
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {

        // Update 간 시간 체크
        float currentTime = Time.time;
        float deltaTime = currentTime - lastUpdateTime;
        updateFrameCount++;
        
        
        lastUpdateTime = currentTime;

        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0);
        // 애니메이션 전환되고 있는 상황 && 다음 애니메이션이 tag
        if(animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsTag(tag))
        {
            return nextStateInfo.normalizedTime;
        }
        // 애니메이션 전환되고 있지 않을 때 && 현재 애니메이션이 tag
        else if(!animator.IsInTransition(0) && currentStateInfo.IsTag(tag))
        {
            return currentStateInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }


    }

    protected void GetEnemyInChasingRange()
    {
        float minDistance = float.MaxValue;
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        GameObject nearestEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(stateMachine.player.transform.position, 10f, enemyLayer);
        foreach(Collider collider in colliders)
        {
            // 제일 가까이 있는 적 
            float distance = Vector3.Distance(stateMachine.player.transform.position, collider.gameObject.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = collider.gameObject;
            }

        }

        if(nearestEnemy != null && nearestEnemy.GetComponent<AIEnemy>().Health.IsAlive)
        {
            _target = nearestEnemy;
            Debug.Log($"[GetEnemyInChasingRange] Enemy 발견: {_target.name}");
            return;
        }

        _target = null;
        stateMachine.IsAttacking = false;
    }

    protected void IsEnemyInAttackRange()
    {
        if(_target != null)
        {
            if(Vector3.Distance(stateMachine.player.transform.position, _target.transform.position) <= GameConstants.Player.ATTACK_RANGE)
            {
                stateMachine.IsAttacking = true;
                return;
            }
        }
        
        stateMachine.IsAttacking = false;
    }
}
