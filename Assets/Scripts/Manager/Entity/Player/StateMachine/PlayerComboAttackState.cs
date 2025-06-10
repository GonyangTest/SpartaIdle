using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedCombo;

    AttackInfoData attackInfoData;


    
    public PlayerComboAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false;

        int comboIndex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.player.Data.AttackData.GetAttackInfoData(comboIndex);
        stateMachine.player.Animator.SetInteger("Combo", comboIndex);

        // 임시
        Collider[] colliders = Physics.OverlapSphere(stateMachine.player.transform.position, 10f, LayerMask.GetMask("Enemy"));
        foreach(Collider collider in colliders)
        {
            AIEnemy enemy = collider.GetComponent<AIEnemy>();
            if(enemy != null)
            {
                int damage = (int)(PlayerManager.Instance.TotalAttack * attackInfoData.DamageMultiplier);
                enemy.TakeDamage(damage);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.AnimationData.ComboAttackParameterHash);

        if(!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.player.Animator, "Attack");

        Debug.Log($"[{GetType().Name}] NormalizedTime: {normalizedTime:F4}");
        if(normalizedTime < 1f) // 공격 애니메이션 재생 중
        {
            if(normalizedTime >= attackInfoData.ComboTransitionTime)
            {
                Debug.Log($"[{GetType().Name}] ComboTransitionTime: {normalizedTime:F4}");
                TryComboAttack();
                if(alreadyAppliedCombo)
                {
                    Debug.Log($"[{GetType().Name}] AlreadyAppliedCombo: {alreadyAppliedCombo}");
                }
            }

        }
        else
        {
            if(alreadyAppliedCombo)
            {
               stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
               stateMachine.ChangeState(stateMachine.ComboAttackState);
               Debug.Log($"[{GetType().Name}] ComboIndex: {stateMachine.ComboIndex}");
            }
            else
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
            }
        }
    }

    private void TryComboAttack()
    {
        if(alreadyAppliedCombo) return;

        if(attackInfoData.ComboStateIndex == -1) return;

        if(!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }
}
