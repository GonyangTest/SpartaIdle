using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public AIPlayer Player;
    

    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int MaxExp { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Level = 1;
        Exp = 0;
        MaxExp = 100;
    }

    public void AddExp(int amount)
    {
        Exp += amount;
        if (Exp >= MaxExp)
        {
            AddLevel();
            Exp = 0;
        }
    }

    public void AddLevel()
    {
        Level++;
    }
}
