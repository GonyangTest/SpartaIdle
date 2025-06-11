using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StageClearUI : BaseWindow
{
    private StageClearParam _stageClearParam;

    public TextMeshProUGUI _stageNumberText;
    public TextMeshProUGUI _clearTimeText;
    public TextMeshProUGUI _expText;
    public TextMeshProUGUI _goldText;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _nextStageButton;

    [SerializeField] private Transform _rewardItemListPanel;

    [SerializeField] private GameObject _rewardItem;

    [SerializeField] private List<GameObject> _rewardItemList = new List<GameObject>();

    private GameManager _gameManager;

    private void Awake()
    {
        UIType = UIType.StageClear;
    }

    public override void OnOpen(OpenParam param = null)
    {
        base.OnOpen(param);

        _stageClearParam = param as StageClearParam;

        _stageNumberText.text = $"{_stageClearParam.StageNumber} 스테이지 클리어";
        _clearTimeText.text = _stageClearParam.ClearTime;
        _expText.text = _stageClearParam.ExpGained.ToString();
        _goldText.text = _stageClearParam.GoldGained.ToString();

        foreach (var itemReward in _stageClearParam.ItemRewards)
        {
            GameObject itemSlot = Instantiate(_rewardItem, _rewardItemListPanel);
            itemSlot.GetComponent<RewardItem>().SetItem(itemReward);
            _rewardItemList.Add(itemSlot);
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        
        // 모든 자식 오브젝트 삭제
        foreach (var item in _rewardItemList)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        
        // 리스트 클리어
        _rewardItemList.Clear();
    }

    public void OnClickRetryButton()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }

        _gameManager.RestartStage();
    }

    public void OnClickNextStageButton()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }

        _gameManager.NextStage();
    }
}