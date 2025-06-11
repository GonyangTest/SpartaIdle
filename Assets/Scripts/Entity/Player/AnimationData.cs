using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string runParameterName = "Run";

    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string comboAttackParameterName = "ComboAttack";
    [SerializeField] private string baseAttackParameterName = "BaseAttack";
    [SerializeField] private string hitParameterName = "Hit";

    [SerializeField] private string deadParameterName = "Dead";

    public int GroundParameterHash {get; private set;}
    public int IdleParameterHash {get; private set;}
    public int WalkParameterHash {get; private set;}
    public int RunParameterHash {get; private set;}

    public int AttackParameterHash {get; private set;}
    public int ComboAttackParameterHash {get; private set;}
    public int BaseAttackParameterHash {get; private set;}
    public int HitParameterHash {get; private set;}

    public int DeadParameterHash {get; private set;}

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);
        BaseAttackParameterHash = Animator.StringToHash(baseAttackParameterName);
        HitParameterHash = Animator.StringToHash(hitParameterName);
        DeadParameterHash = Animator.StringToHash(deadParameterName);
    }
}
