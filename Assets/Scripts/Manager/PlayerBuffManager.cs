using System;
using System.Collections.Generic;
using UnityEngine;


// 나중에 확장성 고려 해야할듯...
[Serializable]
public class Buff
{
    public ConsumableEffect Effect;
    public float RemainingTime;
    public DateTime StartTime;
    public Action OnUpdatedBuff;

    
    public Buff(ConsumableEffect effect)
    {
        Effect = effect;
        RemainingTime = effect.Duration;
        StartTime = DateTime.Now;
    }
    
    public bool IsExpired => RemainingTime <= 0f;
}

public class BuffBonus
{
    public int AttackBonus = 0;
    public int DefenseBonus = 0;
}

public class PlayerBuffManager : Singleton<PlayerBuffManager>
{
    [Header("활성 버프 리스트")]
    public List<Buff> activeBuffs = new List<Buff>();
    
    [Header("스탯 보너스")]
    private BuffBonus _buffBonus = new BuffBonus();
    
    private float _updateTime = 0f;
    
    // 이벤트
    public event Action<Buff> OnBuffRemoved;
    public event Action<BuffBonus> OnStatsChanged;
    public event Action<List<Buff>> OnBuffDisplayUpdated;

    private PlayerManager _playerManager;

    private void Update()
    {
        // 초마다 업데이트
        _updateTime += Time.deltaTime;
        if (_updateTime >= 1f)
        {
            UpdateBuffs();
            _updateTime = 0f;
        }
    }

    public void ApplyConsumableEffects(List<ConsumableEffect> effects)
    {
        foreach (var effect in effects)
        {
            ApplyEffect(effect);
        }
    }

    public void ApplyEffect(ConsumableEffect effect)
    {
        switch (effect.DurationType)
        {
            case EffectDurationType.Instant:
                ApplyInstant(effect);
                break;
                
            case EffectDurationType.Timed:
                ApplyTimedBuff(effect);
                break;
        }
    }

    // 즉시 적용 효과, 마나 회복 등 확장성 고려 필요
    private void ApplyInstant(ConsumableEffect effect)
    {
        var playerManager = PlayerManager.Instance;
        if (playerManager?.Player != null)
        {
            float healAmount = effect.IsPercentage 
                ? playerManager.Player.Health.MaxHealth * (effect.Value / 100f)
                : effect.Value;
                
            playerManager.Heal((int)healAmount);
        }
    }

    private void ApplyTimedBuff(ConsumableEffect effect)
    {
        // 기존 같은 타입의 버프가 있으면 제거
        RemoveBuffByType(effect.EffectType);
        
        // 새 버프 추가
        var newBuff = new Buff(effect);
        activeBuffs.Add(newBuff);
        
        UpdateStatsFromBuffs();

        OnBuffDisplayUpdated?.Invoke(activeBuffs);
    }

    private void UpdateBuffs()
    {
        bool needsUpdate = false;
        
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            activeBuffs[i].RemainingTime -= 1;
            
            if (activeBuffs[i].IsExpired)
            {
                var expiredBuff = activeBuffs[i];
                activeBuffs.RemoveAt(i);
                OnBuffRemoved?.Invoke(expiredBuff);
                needsUpdate = true;
            }
        }
        if (needsUpdate)
        {
            UpdateStatsFromBuffs();
        }
        
        OnBuffDisplayUpdated?.Invoke(activeBuffs);
    }

    private void UpdateStatsFromBuffs()
    {
        if(_playerManager == null)
        {
            _playerManager = PlayerManager.Instance;
        }

        // 모든 보너스 초기화
        _buffBonus.AttackBonus = 0;
        _buffBonus.DefenseBonus = 0;
        
        // 활성 버프들로부터 보너스 계산 (합산 계산), 나중에 확장성 고려 필요
        foreach (var buff in activeBuffs)
        {
            var effect = buff.Effect;
            float value = effect.Value;
            
            switch (effect.EffectType)
            {
                case EffectType.AttackBoost:
                    _buffBonus.AttackBonus += effect.IsPercentage ? (int)(_playerManager.Player.Data.StatData.BaseAttack * (value / 100f)) : (int)value;
                    break;
                case EffectType.DefenseBoost:
                    _buffBonus.DefenseBonus += effect.IsPercentage ? (int)(_playerManager.Player.Data.StatData.BaseDefense * (value / 100f)) : (int)value;
                    break;
            }
        }
        
        OnStatsChanged?.Invoke(_buffBonus);
    }

    public void RemoveBuffByType(EffectType effectType)
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            if (activeBuffs[i].Effect.EffectType == effectType)
            {
                activeBuffs.RemoveAt(i);
            }
        }

        OnBuffDisplayUpdated?.Invoke(activeBuffs);
    }

    // 버프 랜덤 제거
    public void RandomRemoveBuff()
    {
        if (activeBuffs.Count > 0)
        {
            var randomBuff = activeBuffs[UnityEngine.Random.Range(0, activeBuffs.Count)];
            activeBuffs.Remove(randomBuff);

            OnBuffDisplayUpdated?.Invoke(activeBuffs);
        }
    }


    public void ClearAllBuffs()
    {
        activeBuffs.Clear();
        UpdateStatsFromBuffs();

        OnBuffDisplayUpdated?.Invoke(activeBuffs);
    }
} 