using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Item/Armor Item")]
public class ArmorItemDataSO : GenericItemDataSO
{
    public int Defense;
    public int Health;
    public int Mana;

    public override void Use()
    {
        Debug.Log("ArmorItemDataSO Use");
    }
}
