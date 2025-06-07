using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : BaseFixed
{
    private void Awake()
    {
        UIType = UIType.Main;
    }
    
    [Header("Player Info")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _expBar;

    [Header("Currency Info")]
    [SerializeField] private TextMeshProUGUI _goldText;
    
    [Header("Stage Info")]
    [SerializeField] private TextMeshProUGUI _stageText;

    private GameManager _gameManager;
    private StageManager _stageManager;
    private CurrencyManager _currencyManager;
    private PlayerManager _playerManager;

    public override void OnOpen(OpenParam param = null)
    {
        base.OnOpen(param);

        _gameManager = GameManager.Instance;

        _stageManager = _gameManager.StageManager;
        _currencyManager = _gameManager.CurrencyManager;
        _playerManager = _gameManager.PlayerManager;
        
        // 게임 데이터 변화 이벤트 구독
        if(_gameManager != null)
        {
            _gameManager.OnPlayerDataChanged += UpdatePlayerInfo;
            _gameManager.OnGoldDataChanged += UpdateGold;
            _gameManager.OnStageDataChanged += UpdateStageInfo;
        }

        RefreshAllUI();
    }

    public override void OnClose()
    {
        base.OnClose();

        _gameManager = null;
        _stageManager = null;
        _currencyManager = null;
        _playerManager = null;

        // 이벤트 구독 해제
        if (_gameManager != null)
        {
            _gameManager.OnPlayerDataChanged -= UpdatePlayerInfo;
            _gameManager.OnGoldDataChanged -= UpdateGold;
            _gameManager.OnStageDataChanged -= UpdateStageInfo;
        }
    }

    public void RefreshAllUI()
    {
        UpdatePlayerInfo();
        UpdateGold();
        UpdateStageInfo();
    }

    private void UpdatePlayerInfo()
    {
        if(_playerManager == null)
        {
            _playerManager = _gameManager.PlayerManager;
        }

        if (_levelText != null)
            _levelText.text = $"Lv {_playerManager.Level}";
            
        if (_expBar != null)
        {
            _expBar.fillAmount = (float)_playerManager.Exp / _playerManager.MaxExp;
        }
    }

    private void UpdateStageInfo()
    {   
        if(_stageManager == null)
        {
            _stageManager = _gameManager.StageManager;
        }

        if (_stageText != null)
            _stageText.text = $"Stage: {_stageManager.CurrentStage}";
    }

    public void UpdateGold()
    {
        if(_currencyManager == null)
        {
            _currencyManager = _gameManager.CurrencyManager;
        }

        if (_goldText != null)
            _goldText.text = _currencyManager.Gold.ToString("N0");
    }
}