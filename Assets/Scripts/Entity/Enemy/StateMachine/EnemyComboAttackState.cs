using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComboAttackState : EnemyAttackState
{
    private bool alreadyAppliedCombo;

    AttackInfoData attackInfoData;


    
    public EnemyComboAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.enemy.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false;

        int comboIndex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.enemy.Data.AttackData.GetAttackInfoData(comboIndex);
        stateMachine.enemy.Animator.SetInteger("Combo", comboIndex);

        int damage = (int)(stateMachine.enemy.Data.StatData.BaseAttack * attackInfoData.DamageMultiplier);
        // 임시
        PlayerManager.Instance.Player.TakeDamage(damage);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.enemy.AnimationData.ComboAttackParameterHash);

        if(!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.enemy.Animator, "Attack");

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

            if(attackInfoData.ComboStateIndex == GameConstants.Animation.INVALID_COMBO_INDEX) return;

        if(!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }
}
