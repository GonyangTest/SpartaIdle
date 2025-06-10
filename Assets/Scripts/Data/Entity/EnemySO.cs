using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDropItemData
{
    [field:SerializeField] public GenericItemDataSO Item {get; private set;}
    [field:SerializeField] public int DropRate {get; private set;}
}


[System.Serializable]
public class EnemyRewardData
{
    [field:SerializeField] public int Gold {get; private set;} = 0;
    [field:SerializeField] public int Exp {get; private set;} = 0;
    [field:SerializeField] public List<EnemyDropItemData> DropItemDatas {get; private set;}
}

[System.Serializable]
public class EnemyGroundData
{
    [field:SerializeField][field:Range(0f, 25f)] public float BaseSpeed {get; private set;} = 5f;
    [field:SerializeField][field:Range(0f, 25f)] public float BaseRotationDamping {get; private set;} = 1f;


    [field:Header("RunData")]
    [field:SerializeField][field:Range(0f, 25f)] public float RunSpeedModifier {get; private set;} = 1f;

}

[System.Serializable]
public class EnemyAttackData
{
    [field:SerializeField] public List<AttackInfoData> AttackInfoDatas {get; private set;}
    public int GetAttackInfoCount() {return AttackInfoDatas.Count;}
    public AttackInfoData GetAttackInfoData(int index) {return AttackInfoDatas[index];}
}

[System.Serializable]
public class EnemyHitData
{
    [field:SerializeField] public float HitStunDuration {get; private set;} = 0.5f;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy")]
public class EnemySO : EntitySO
{
    [field:SerializeField] public float PlayerChasingRange {get; private set;} = 10f;
    [field:SerializeField] public float AttackRange {get; private set;} = 2f;
    [field:SerializeField] public int Damage {get; private set;}


    [field:SerializeField] public EnemyGroundData GroundData {get; private set;}
    [field:SerializeField] public EnemyAttackData AttackData {get; private set;}
    [field:SerializeField] public EnemyHitData HitData {get; private set;}
    [field:SerializeField] public EnemyRewardData RewardData {get; private set;}
}
