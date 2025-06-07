using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Resources
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Generic Item")]
public class GenericItemDataSO : ScriptableObject
{
    public int ItemID;
    public string ItemName;
    public ItemType Type;
    public ItemRarity Rarity;
    public Sprite icon;
    public GameObject Prefab;
    public bool IsStackable;
    public int MaxStack;
    public int Price;
    public string Description;

    public int GetSellPrice()
    {
        return (int)(Price * 0.5f);
    }
}
