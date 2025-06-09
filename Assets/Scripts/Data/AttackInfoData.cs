using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfoData
{
    [field:SerializeField] public string AttackName {get; private set;}
    [field:SerializeField] public int ComboStateIndex {get; private set;}
    [field:SerializeField][field:Range(0f, 1f)] public float ComboTransitionTime {get; private set;}
    [field:SerializeField] public float DamageMultiplier {get; private set;}
}
