using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatData
{
    [field:SerializeField] public int BaseAttack {get; private set;} = 3;
    [field:SerializeField] public int BaseDefense {get; private set;} = 0;
}
