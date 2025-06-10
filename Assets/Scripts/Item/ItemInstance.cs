using System;

[Serializable]
public class ItemInstance
{
    private int _itemID;
    private int _quantity;

    public int Quantity => _quantity;
    public int ItemID => _itemID;

    public ItemInstance(int id, int quantity)
    {
        _itemID = id;
        _quantity = quantity;
    }

    public ItemInstance(GenericItemDataSO itemDataSO, int quantity)
    {
        _itemID = itemDataSO.ItemID;
        _quantity = quantity;
    }

    public bool IsStackable()
    {
        GenericItemDataSO itemData = ItemDatabase.Instance.GetItemByID(_itemID);
        return itemData.IsStackable && _quantity < itemData.MaxStack;
    }

    public void AddQuantity(int amount)
    {
        _quantity += amount;
    }

    public void SubtractQuantity(int amount)
    {
        _quantity = Math.Max(0, _quantity - amount);
    }

    public bool IsEmpty()
    {
        return _quantity <= 0;
    }
}