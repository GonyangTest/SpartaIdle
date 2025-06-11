using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RewardItem : BaseWindow
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;

    public void SetItem(ItemReward itemReward)
    {
        _itemIcon.sprite = itemReward.ItemIcon;
        _itemName.text = itemReward.ItemName;
    }
}