using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUI : BaseWindow
{
    private ShopManager _shopManager;
    [SerializeField] private GameObject _shopItemSlotPrefab;

    private void Awake()
    {
        UIType = UIType.Shop;
    }

    private void Start()
    {
        _shopManager = ShopManager.Instance;

        InitShopItemSlot();
    }

    public void InitShopItemSlot()
    {
        GridLayoutGroup gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        List<GenericItemDataSO> shopItems = _shopManager.GetShopItems();
        foreach (var item in shopItems)
        {
            GameObject shopItemSlot = Instantiate(_shopItemSlotPrefab, gridLayoutGroup.transform);
            shopItemSlot.GetComponent<ItemSlotUI>().SetItem(item);
        }
    }

    public override void OnOpen(OpenParam param = null)
    {
        base.OnOpen(param);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}