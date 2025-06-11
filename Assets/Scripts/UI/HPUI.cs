using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : BaseFixed
{
    private void Awake()
    {
        UIType = UIType.Main;
    }
    
    [Header("Player Info")]
    [SerializeField] private Image _hpBar;

    private PlayerManager _playerManager;

    public void OnEnable()
    {
        _playerManager = PlayerManager.Instance;
        
        _playerManager.Player.Health.OnHealthChanged += UpdateHP;
    }

    public void OnDestroy()
    {
        _playerManager.Player.Health.OnHealthChanged -= UpdateHP;
    }

    private void UpdateHP(int currentHP, int maxHP)
    {
        if(_hpBar != null)
        {
            _hpBar.fillAmount = (float)currentHP / maxHP;
        }
    }
}