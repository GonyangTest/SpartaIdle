using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public AIEnemy enemy {get;}
    public Vector2 MovementInput {get; set;}
    public float MovementSpeed {get; private set;}
    public float RotateDamping {get; private set;}
    public float MovementSpeedModifier {get; set;} = 1f;
    
    public bool IsChasing {get; set;}
    public bool IsAttacking {get; set;}
    public int ComboIndex {get; set;}

    public Transform MainCameraTransform {get; private set;}

    public EnemyGroundState GroundState {get; private set;}
    public EnemyIdleState IdleState {get; private set;}
    public EnemyWalkState WalkState {get; private set;}
    public EnemyRunState RunState {get; private set;}
    public EnemyChasingState ChasingState {get; private set;}

    public EnemyDeadState DeadState {get; private set;}

    public EnemyAttackState AttackState {get; private set;}
    public EnemyComboAttackState ComboAttackState {get; private set;}

    public EnemyStateMachine(AIEnemy enemy)
    {
        this.enemy = enemy;

        MainCameraTransform = Camera.main.transform;

        GroundState = new EnemyGroundState(this);
        IdleState = new EnemyIdleState(this);
        WalkState = new EnemyWalkState(this);
        RunState = new EnemyRunState(this);
        ChasingState = new EnemyChasingState(this);

        DeadState = new EnemyDeadState(this);


        AttackState = new EnemyAttackState(this);
        ComboAttackState = new EnemyComboAttackState(this);

        MovementSpeed = enemy.Data.GroundData.BaseSpeed;
    }
}
