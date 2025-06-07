using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int Gold { get; private set; }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void SubtractGold(int amount)
    {
        Gold -= amount;
    }
}
