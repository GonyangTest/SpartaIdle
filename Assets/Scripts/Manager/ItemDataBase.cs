using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<GenericItemDataSO> _itemDatabase;
    private Dictionary<int, GenericItemDataSO> _itemDictionary = new Dictionary<int, GenericItemDataSO>();

    protected override void Awake()
    {
        base.Awake();
        LoadItemDatabase();
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        _itemDictionary.Clear();
        foreach (var item in _itemDatabase)
        {
            if (item != null && !_itemDictionary.ContainsKey(item.ItemID))
            {
                _itemDictionary.Add(item.ItemID, item);
            }
        }

    }

    public GenericItemDataSO GetItemByID(int itemID)
    {
        _itemDictionary.TryGetValue(itemID, out GenericItemDataSO item);
        if (item == null)
        {
            Debug.LogWarning($"아이템을 찾을 수 없습니다: {itemID}");
        }
        return item;
    }

    public void AddItemToDatabase(GenericItemDataSO item)
    {
        if (item != null && !_itemDictionary.ContainsKey(item.ItemID))
        {
            _itemDatabase.Add(item);
            _itemDictionary.Add(item.ItemID, item);
        }
    }

    public void LoadItemDatabase()
    {
        _itemDatabase = new List<GenericItemDataSO>(Resources.LoadAll<GenericItemDataSO>(ResourcePaths.Item.ITEM_DATABASE));
    }

    public ItemInstance CreateItem(int itemID)
    {
        GenericItemDataSO itemDataSO = GetItemByID(itemID);
        ItemInstance itemInstance = null;

        switch(itemDataSO.Type)
        {
            case ItemType.Weapon:
                itemInstance = new WeaponInstance(itemDataSO);
                break;
            case ItemType.Armor:
                itemInstance = new ArmorInstance(itemDataSO);
                break;
            case ItemType.Consumable:
                itemInstance = new ItemInstance(itemDataSO, 1);
                break;
            case ItemType.Resources:
                itemInstance = new ItemInstance(itemDataSO, 1);
                break;
        }

        return itemInstance;
    }

    public GameObject SpawnItem(Vector3 position, GenericItemDataSO itemDataSO)
    {
        if(itemDataSO.Prefab == null)
        {
            return null;
        }

        GameObject droppedItem = Instantiate(itemDataSO.Prefab, position, Quaternion.identity);
        return droppedItem;
    }

    public GameObject SpawnItem(Vector3 position, Quaternion quaternion, GenericItemDataSO itemDataSO)
    {
        if(itemDataSO.Prefab == null)
        {
            return null;
        }

        GameObject droppedItem = Instantiate(itemDataSO.Prefab, position, quaternion);
        return droppedItem;
    }
}