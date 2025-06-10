using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<ItemInstance> InventoryItems = new List<ItemInstance>();

    public ItemInstance _selectedItem;

    public void AddItem(ItemInstance item)
    {
        GenericItemDataSO itemData = ItemDatabase.Instance.GetItemByID(item.ItemID);

        if(itemData.IsStackable)
        {
            ItemInstance stackableItem = GetItemSlot(item);
            if(stackableItem != null)
            {
                stackableItem.AddQuantity(item.Quantity);
                return;
            }
        }

        InventoryItems.Add( item);
    }

    public void RemoveItem(ItemInstance item) { InventoryItems.Remove(item); }

    public void ClearInventory() { InventoryItems.Clear(); }

    public List<ItemInstance> GetInventoryItems() { return InventoryItems; }

    public bool HasItem(ItemInstance item) { return InventoryItems.Contains(item); }

    ItemInstance GetItemSlot(ItemInstance item)
    {
        GenericItemDataSO itemData = ItemDatabase.Instance.GetItemByID(item.ItemID);

        for(int i = 0; i < InventoryItems.Count; i++)
        {
            if(InventoryItems[i].ItemID == item.ItemID && InventoryItems[i].Quantity < itemData.MaxStack)
            {
                return InventoryItems[i];
            }
        }
        return null;
    }

    public void SetSelectedItem(ItemInstance item)
    {
        _selectedItem = item;
    }

    public void UseSelectedItem()
    {
        if(_selectedItem == null)
        {
            RemoveItem(_selectedItem);
            return;
        }
    }
}
