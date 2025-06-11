using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StageFailUI : BaseWindow
{
    private StageParam _stageParam;

    public TextMeshProUGUI _stageNumberText;
    public TextMeshProUGUI _elapsedTimeText;
    public TextMeshProUGUI _remainEnemyCountText;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _previousStageButton;

    private GameManager _gameManager;

    private void Awake()
    {
        UIType = UIType.StageClear;
    }

    public override void OnOpen(OpenParam param = null)
    {
        base.OnOpen(param);

        _stageParam = param as StageParam;

        _stageNumberText.text = $"{_stageParam.StageNumber} 스테이지 실패";
        _elapsedTimeText.text = _stageParam.ElapsedTime;
        _remainEnemyCountText.text = _stageParam.RemainEnemyCount.ToString();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void OnClickRetryButton()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }

        _gameManager.RestartStage();
    }

    public void OnClickPreviousStageButton()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }

        _gameManager.PreviousStage();
    }
}