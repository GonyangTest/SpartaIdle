using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public AIPlayer Player;
    [SerializeField] private Transform _initPlayerTransform;

    public event Action<int, int, int> OnPlayerInfoDataChanged;

    public int TotalAttack {get; private set;}
    public int TotalDefense {get; private set;}
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int MaxExp { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Level = GameConstants.Player.INITIAL_LEVEL;
        Exp = GameConstants.Player.INITIAL_EXP;
        MaxExp = GameConstants.Player.INITIAL_MAX_EXP;
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
            Exp = GameConstants.Player.INITIAL_EXP;
        }
        
        StageManager.Instance.AddExp(amount);
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
        if(damage < GameConstants.Player.MIN_DAMAGE)
            damage = GameConstants.Player.MIN_DAMAGE;

        Player.Health.TakeDamage(damage);
    }

    public void OnTotalStatChanged(BuffBonus buffBonus)
    {
        TotalAttack = Player.Data.StatData.BaseAttack + buffBonus.AttackBonus;
        TotalDefense = Player.Data.StatData.BaseDefense + buffBonus.DefenseBonus;
    }

    public void ResetPlayerTransform()
    {
        Player.NavMeshAgent.Warp(_initPlayerTransform.position);
        Player.NavMeshAgent.SetDestination(Player.Target.position);
        Player.Health.Heal(Player.Health.MaxHealth);
        // Player.transform.position = _initPlayerTransform.position;
    }
}
