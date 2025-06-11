using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected readonly EnemyGroundData groundData;
    protected Transform _target;
    // Update 간 시간 체크를 위한 변수들 추가
    private float lastUpdateTime;
    private int updateFrameCount = 0;
    
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.enemy.Data.GroundData;
        lastUpdateTime = Time.time;
    }

    public virtual void Enter()
    {
        _target = stateMachine.enemy.Target;
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
        stateMachine.enemy.Animator.SetBool(AnimationHash, true);
    }

    protected void StopAnimation(int AnimationHash)
    {
        stateMachine.enemy.Animator.SetBool(AnimationHash, false);
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

    protected void IsEnemyInChasingRange()
    {
        if(_target != null)
        {
            if(Vector3.Distance(stateMachine.enemy.transform.position, _target.transform.position) <= 5.0f)
            {
                stateMachine.IsChasing = true;
            }
            else
            {
                stateMachine.IsChasing = false;
            }
        }
        else
        {
            stateMachine.IsChasing = false;
        }
    }
    protected void IsEnemyInAttackRange()
    {
        if(_target != null)
        {
            if(Vector3.Distance(stateMachine.enemy.transform.position, _target.transform.position) <= 1.0f)
            {
                stateMachine.IsAttacking = true;
            }
            else
            {
                stateMachine.IsAttacking = false;
            }
        }
        else
        {
            stateMachine.IsAttacking = false;
        }
    }
}
