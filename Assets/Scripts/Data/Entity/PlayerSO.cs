using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerGroundData
{
    [field:SerializeField][field:Range(0f, 25f)] public float BaseSpeed {get; private set;} = 5f;


    [field:Header("RunData")]
    [field:SerializeField][field:Range(0f, 25f)] public float RunSpeedModifier {get; private set;} = 1f;

}


[System.Serializable]
public class PlayerAttackData
{
    [field:SerializeField] public List<AttackInfoData> AttackInfoDatas {get; private set;}
    public int GetAttackInfoCount() {return AttackInfoDatas.Count;}
    public AttackInfoData GetAttackInfoData(int index) {return AttackInfoDatas[index];}
}

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/Player")]
public class PlayerSO : EntitySO
{
    [field:SerializeField] public StatData StatData {get; private set;}
    [field:SerializeField] public PlayerGroundData GroundData {get; private set;}
    [field:SerializeField] public PlayerAttackData AttackData {get; private set;}
}
