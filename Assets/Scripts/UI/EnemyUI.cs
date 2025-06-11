using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHPUI : BaseFixed
{
    private void Awake()
    {
        UIType = UIType.Main;
    }
    
    [Header("Player Info")]
    [SerializeField] private Image _hpBar;

    private AIEnemy _enemy;

    public void OnEnable()
    {
        _enemy = GetComponentInParent<AIEnemy>();

        _enemy.Health.OnHealthChanged += UpdateHP;
    }


    public void OnDestroy()
    {
        _enemy.Health.OnHealthChanged -= UpdateHP;
    }

    private void UpdateHP(int currentHP, int maxHP)
    {
        if(_hpBar != null)
        {
            _hpBar.fillAmount = (float)currentHP / maxHP;
        }
    }
}