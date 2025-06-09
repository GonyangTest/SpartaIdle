using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<GenericItemDataSO> InventoryItems = new List<GenericItemDataSO>();

    public void AddItem(GenericItemDataSO item)
    {
        InventoryItems.Add(item);
    }

    public void RemoveItem(GenericItemDataSO item) { InventoryItems.Remove(item); }

    public void ClearInventory() { InventoryItems.Clear(); }

    public List<GenericItemDataSO> GetInventoryItems() { return InventoryItems; }

    public bool HasItem(GenericItemDataSO item) { return InventoryItems.Contains(item); }
}
