using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public event Action<int> OnGoldDataChanged;

    public int Gold { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Gold = 1000;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        StageManager.Instance.AddGold(amount);
        OnGoldDataChanged?.Invoke(Gold);
    }

    public void SubtractGold(int amount)
    {
        Gold -= amount;
        OnGoldDataChanged?.Invoke(Gold);
    }
}
