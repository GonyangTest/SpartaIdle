using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Item/Consumable Item")]
public class ConsumableItemDataSO : GenericItemDataSO
{
    [Header("소모품 효과")]
    public List<ConsumableEffect> effects = new List<ConsumableEffect>();
    
    [Header("사용 조건")]
    public float coolTime = 0f;           // 쿨타임 (초)
    
    public override void Use()
    {
        PlayerBuffManager.Instance.ApplyConsumableEffects(effects);
    }
} 