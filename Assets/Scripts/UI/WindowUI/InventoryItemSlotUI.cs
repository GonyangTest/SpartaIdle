using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemSlotUI : BaseUI
{
    public ItemInstance _item;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _quantityText;
    private ItemDatabase _itemDatabase;
    private InventoryManager _inventoryManager;
    public int Index;

    public void Clear()
    {
        _item = null;
        _itemName.text = "";
        _itemIcon.gameObject.SetActive(false);
        _quantityText.text = "";
    }

    private void Awake()
    {
        if (_itemDatabase == null)
            _itemDatabase = ItemDatabase.Instance;
        if (_inventoryManager == null)
            _inventoryManager = InventoryManager.Instance;
    }

    public void SetItem(ItemInstance item)
    {
        _item = item;
        
        // _itemDatabase가 초기화되지 않은 경우 즉시 초기화
        if (_itemDatabase == null)
        {
            _itemDatabase = ItemDatabase.Instance;
        }
        
        // 아이템 데이터가 유효한지 확인
        GenericItemDataSO itemData = _itemDatabase.GetItemByID(_item.ItemID);
        if (itemData != null)
        {
            _itemName.text = itemData.ItemName;
            _itemIcon.sprite = itemData.Icon;
            _itemIcon.gameObject.SetActive(true);
            _quantityText.text = _item.Quantity > 1 ? _item.Quantity.ToString() : "";
        }
    }
    
    public void OnClick()
    {
        _inventoryManager.SetSelectedItem(_item);
        _inventoryManager.UseSelectedItem();
    }
}