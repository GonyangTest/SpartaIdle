using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Item/Weapon Item")]
public class WeaponItemDataSO : GenericItemDataSO
{
    public int Damage;
    public int CriticalChance;
    public int CriticalDamage;
    public int AttackSpeed;
    public int AttackRange;
}
