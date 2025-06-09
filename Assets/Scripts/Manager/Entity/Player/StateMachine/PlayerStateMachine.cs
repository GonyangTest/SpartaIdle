using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public AIPlayer player {get;}
    public Vector2 MovementInput {get; set;}
    public float MovementSpeed {get; private set;}
    public float RotateDamping {get; private set;}
    public float MovementSpeedModifier {get; set;} = 1f;

    public bool IsAttacking {get; set;}
    public int ComboIndex {get; set;}

    public Transform MainCameraTransform {get; private set;}



    public PlayerGroundState GroundState {get; private set;}
    public PlayerIdleState IdleState {get; private set;}
    public PlayerRunState RunState {get; private set;}
    public PlayerChasingState ChasingState {get; private set;}

    public PlayerAttackState AttackState {get; private set;}
    public PlayerComboAttackState ComboAttackState {get; private set;}
    public PlayerDeathState DeathState {get; private set;}

    public PlayerStateMachine(AIPlayer player)
    {
        this.player = player;

        MainCameraTransform = Camera.main.transform;

        GroundState = new PlayerGroundState(this);
        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        ChasingState = new PlayerChasingState(this);


        AttackState = new PlayerAttackState(this);
        ComboAttackState = new PlayerComboAttackState(this);
        DeathState = new PlayerDeathState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotateDamping = player.Data.GroundData.BaseRotationDamping;
    }
}
