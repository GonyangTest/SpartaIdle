using System;
using UnityEngine;

[Serializable]

public enum EffectDurationType
{
    Instant,
    Timed,
}

public enum EffectType
{
    Heal,        // 즉시 회복
    AttackBoost,        // 공격력 증가
    DefenseBoost,       // 방어력 증가
}

[Serializable]
public class ConsumableEffect
{
    [Header("효과 기본 정보")]
    public EffectType EffectType;
    public EffectDurationType DurationType;
    public float Value;                    // 효과 수치 (회복량, 증가량 등)
    public float Duration;                 // 지속시간 (초) - 즉시 효과면 0
    public bool IsPercentage;             // 퍼센트 기반인지 고정값인지

    [Header("효과 이미지")]
    public string EffectName;             // 효과 이름
    public string Description;            // 효과 설명
    public Sprite EffectIcon;             // 효과 아이콘

    // 기본 생성자
    public ConsumableEffect()
    {
    }

    // 깊은 복사를 위한 복사 생성자
    public ConsumableEffect(ConsumableEffect original)
    {
        if (original != null)
        {
            EffectType = original.EffectType;
            DurationType = original.DurationType;
            Value = original.Value;
            Duration = original.Duration;
            IsPercentage = original.IsPercentage;
            EffectName = original.EffectName;
            Description = original.Description;
            EffectIcon = original.EffectIcon;
        }
    }

    // 깊은 복사 메서드
    public ConsumableEffect Clone()
    {
        return new ConsumableEffect(this);
    }
} 