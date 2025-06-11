using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private List<GenericItemDataSO> _shopItems;

    public List<GenericItemDataSO> ShopItems => _shopItems;

    public List<GenericItemDataSO> GetShopItems()
    {
        return _shopItems;
    }

    public void BuyItem(GenericItemDataSO item)
    {
        if(CurrencyManager.Instance.Gold >= item.Price)
        {
            CurrencyManager.Instance.SubtractGold(item.Price);

            ItemInstance newItem = ItemDatabase.Instance.CreateItem(item.ItemID);
            InventoryManager.Instance.AddItem(newItem);

            Debug.Log($"BuyItem: {item.ItemName}");
        }

        else
        {
            Debug.Log($"Not enough gold to buy {item.ItemName}");
        }
    }
}
