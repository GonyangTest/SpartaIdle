using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<ItemInstance> InventoryItems = new List<ItemInstance>();

    public ItemInstance _selectedItem;

    public Action OnInventoryChanged;

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
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemInstance item) { 
        if(item.Quantity <= 1)
        {
            InventoryItems.Remove(item);
        }
        else
        {
            item.SubtractQuantity(1);
        }

        OnInventoryChanged?.Invoke();
    }

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
        if(_selectedItem != null)
        {
            _selectedItem.Use();
            RemoveItem(_selectedItem);
            return;
        }
    }

    public void AddItemReward(ItemInstance item)
    {
        AddItem(item);
        StageManager.Instance.AddItemReward(item);
    }
}
