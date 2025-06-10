using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public AIPlayer Player;

    public event Action<int, int, int> OnPlayerInfoDataChanged;

    public int TotalAttack {get; private set;}
    public int TotalDefense {get; private set;}
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
    
    protected void OnEnable()
    {
        PlayerBuffManager.Instance.OnStatsChanged += OnTotalStatChanged;
    }

    protected void OnDisable()
    {
        PlayerBuffManager.Instance.OnStatsChanged -= OnTotalStatChanged;
    }

    public void AddExp(int amount)
    {
        Exp += amount;
        if (Exp >= MaxExp)
        {
            AddLevel();
            Exp = 0;
        }
        
        OnPlayerInfoDataChanged?.Invoke(MaxExp, Exp, Level);
    }

    public void AddLevel()
    {
        Level++;
    }

    public void Heal(int amount)
    {
        Player.Health.Heal(amount);
    }

    public void TakeDamage(int amount)
    {
        int damage = amount - TotalDefense;
        if(damage < 0)
            damage = 0;

        Player.Health.TakeDamage(damage);
    }

    public void OnTotalStatChanged(BuffBonus buffBonus)
    {
        TotalAttack = Player.Data.StatData.BaseAttack + buffBonus.AttackBonus;
        TotalDefense = Player.Data.StatData.BaseDefense + buffBonus.DefenseBonus;
    }
}
