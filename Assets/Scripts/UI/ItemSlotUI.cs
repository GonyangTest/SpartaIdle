using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : BaseUI
{
    public GenericItemDataSO _item;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;

    public void SetItem(GenericItemDataSO item)
    {
        _item = item;
        _itemName.text = item.ItemName;
        _itemPrice.text = item.Price.ToString();
        _itemIcon.sprite = item.Icon;
    }

    public void OnClick()
    {
        ShopManager.Instance.BuyItem(_item);
    }
}