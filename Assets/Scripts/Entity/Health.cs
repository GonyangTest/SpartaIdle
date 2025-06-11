using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;

    private float invulnerabilityDuration = GameConstants.Health.INVULNERABILITY_DURATION;
    
    // 이벤트들
    public event Action<int, int> OnHealthChanged; // (현재HP, 최대HP)
    public event Action<int> OnDamageReceived; // (받은 데미지)
    public event Action OnDeath;
    
    private Animator _animator;
    
    // 무적 시간 관리
    private float invulnerabilityTimer;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;
    public bool IsInvulnerable => invulnerabilityTimer > 0f;
    public float HealthPercentage => (float)currentHealth / maxHealth;
    
    public void Initialize(EntitySO entitySO)
    {
        maxHealth = entitySO.HealthData.MaxHealth;
        currentHealth = maxHealth;
    }
    
    
    private void Update()
    {
        // 무적 시간 처리
        if (IsInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                invulnerabilityTimer = 0f;
                Debug.Log($"{gameObject.name} 무적 시간 종료");
            }
        }
    }
    
    public bool TakeDamage(int damage)
    {
        // 무적 상태이거나 이미 죽었으면 데미지 무시
        if (IsInvulnerable || !IsAlive)
        {
            return false;
        }
        
        // 데미지 적용
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        Debug.Log($"{gameObject.name}이(가) {damage} 데미지를 받았습니다. (남은 HP: {currentHealth}/{maxHealth})");
        
        // 이벤트 발생
        OnDamageReceived?.Invoke(damage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // 무적 시간 적용
        SetInvulnerable(invulnerabilityDuration);
        
        // 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
        
        return true;
    }
    
    public void Heal(int amount)
    {
        if (!IsAlive) return;
        
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        Debug.Log($"{gameObject.name}이(가) {amount} 회복했습니다. (현재 HP: {currentHealth}/{maxHealth})");
    }
    
    public void SetInvulnerable(float duration)
    {
        invulnerabilityTimer = duration;
    }
    
    private void Die()
    {
        // 사망 이벤트 발생
        OnDeath?.Invoke();
    }
} 