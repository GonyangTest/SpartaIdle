using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : BaseWindow
{
    [Header("Inventory Item Slot")]
    [SerializeField] private InventoryItemSlotUI[] _inventoryItemSlots;
    [SerializeField] private Transform _slotPanel;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = InventoryManager.Instance;

        UIType = UIType.Inventory;

        _inventoryItemSlots = new InventoryItemSlotUI[_slotPanel.childCount];
        for(int i = 0; i < _slotPanel.childCount; i++)
       {
           _inventoryItemSlots[i] = _slotPanel.GetChild(i).GetComponent<InventoryItemSlotUI>();
           _inventoryItemSlots[i].Index = i;
       }
    }

    private void OnEnable()
    {
        _inventoryManager.OnInventoryChanged += RefreshInventoryItemSlot;
    }

    private void OnDisable()
    {
        _inventoryManager.OnInventoryChanged -= RefreshInventoryItemSlot;
    }

    private void RefreshInventoryItemSlot()
    {
        if(_inventoryManager == null)
            _inventoryManager = InventoryManager.Instance;

        List<ItemInstance> inventoryItems = _inventoryManager.GetInventoryItems();
        for(int i = 0; i < _inventoryItemSlots.Length; i++)
        {
            if(i < inventoryItems.Count)
            {
                _inventoryItemSlots[i].SetItem(inventoryItems[i]);
            }
            else
            {
                _inventoryItemSlots[i].Clear();
            }
        }
    }

    private void Start()
    {
        _inventoryManager = InventoryManager.Instance;

        RefreshInventoryItemSlot();
    }

    public override void OnOpen(OpenParam param = null)
    {
        RefreshInventoryItemSlot();

        base.OnOpen(param);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}