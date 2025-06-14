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
    [SerializeField] private TextMeshProUGUI _enemyCountText;

    [Header("Shop Info")]
    [SerializeField] private Button _shopButton;

    [Header("Inventory Info")]
    [SerializeField] private Button _inventoryButton;   

    private GameManager _gameManager;
    private StageManager _stageManager;
    private CurrencyManager _currencyManager;
    private PlayerManager _playerManager;
    private UIManager _uiManager;

    public override void OnOpen(OpenParam param = null)
    {
        base.OnOpen(param);

        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;

        _stageManager = _gameManager.StageManager;
        _currencyManager = _gameManager.CurrencyManager;
        _playerManager = _gameManager.PlayerManager;
        
        // 게임 데이터 변화 이벤트 구독
        if(_gameManager != null)
        {
            _playerManager.OnPlayerInfoDataChanged += UpdatePlayerInfo;
            _currencyManager.OnGoldDataChanged += UpdateGold;
            _stageManager.OnStageDataChanged += UpdateStageInfo;
            _stageManager.OnEnemyCountChanged += UpdateEnemyCount;
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
            _playerManager.OnPlayerInfoDataChanged -= UpdatePlayerInfo;
            _currencyManager.OnGoldDataChanged -= UpdateGold;
            _stageManager.OnStageDataChanged -= UpdateStageInfo;
            _stageManager.OnEnemyCountChanged -= UpdateEnemyCount;
        }
    }

    public void RefreshAllUI()
    {
        UpdatePlayerInfo(_playerManager.MaxExp, _playerManager.Exp, _playerManager.Level);
        UpdateGold(_currencyManager.Gold);
        UpdateStageInfo();
    }

    private void UpdatePlayerInfo(int maxExp, int exp, int level)
    {
        if (_levelText != null)
            _levelText.text = $"Lv {level}";
            
        if (_expBar != null)
        {
            _expBar.fillAmount = (float)exp / maxExp;
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

        int totalEnemyCount = _stageManager.GetTotalEnemyCount();
            
        UpdateEnemyCount(totalEnemyCount, totalEnemyCount);
    }
    
    private void UpdateEnemyCount(int activeEnemies, int totalEnemies)
    {
        if (_stageManager != null && _enemyCountText != null)
        {
            _enemyCountText.text = $"({activeEnemies}/{totalEnemies})";
        }
    }

    public void UpdateGold(int gold)
    {
        if(_currencyManager == null)
        {
            _currencyManager = _gameManager.CurrencyManager;
        }

        if (_goldText != null)
            _goldText.text = gold.ToString("N0");
    }

    public void OnClickShopButton()
    {
        _uiManager = UIManager.Instance;

        if (_uiManager.IsUIActive(UIType.Shop))
        {
            _uiManager.CloseUI(UIType.Shop);
        }
        else
        {
            _uiManager.OpenUI(UIType.Shop);
        }
    }

    public void OnClickInventoryButton()
    {
        if (_uiManager.IsUIActive(UIType.Inventory))
        {
            _uiManager.CloseUI(UIType.Inventory);
        }
        else
        {
            _uiManager.OpenUI(UIType.Inventory);
        }
    }
}