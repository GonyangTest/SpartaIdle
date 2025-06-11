using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnPlayerDataChanged;

    public PlayerManager PlayerManager;
    public CurrencyManager CurrencyManager;
    public StageManager StageManager;

    
    
    UIManager _uiManager;

    protected override void Awake()
    {
        base.Awake();

        PlayerManager = PlayerManager.Instance;
        CurrencyManager = CurrencyManager.Instance;
        StageManager = StageManager.Instance;
    }

    private void Start()
    {
        _uiManager = UIManager.Instance;
        GameStart();
    }

    public void RestartStage()
    {
        ResetPlayerTransform();
        _uiManager.CloseAllWindows();
        StageManager.StartStage(StageManager.CurrentStage);
    }

    public void NextStage()
    {
        ResetPlayerTransform();
        _uiManager.CloseAllWindows();
        StageManager.StartStage(StageManager.CurrentStage + 1);
    }

    void GameStart()
    {
        _uiManager.OpenUI(UIType.Main);

        StageManager.StartStage(1);
    }

    void GameOver()
    {
        StageManager.EndStage(false);
    }

    [ContextMenu("ExpTest")]
    void ExpTest()
    {
        PlayerManager.AddExp(10);
        OnPlayerDataChanged?.Invoke();
    }

    [ContextMenu("StageStartTest")]
    void StageStartTest()
    {
        StageManager.StartStage(0);
    }

    [ContextMenu("StageFailTest")]
    void StageFailTest()
    {
        StageManager.EndStage(false);
    }

    [ContextMenu("StageClearTest")]
    void StageClearTest()
    {
        StageManager.EndStage(true);
    }

    public void ResetPlayerTransform()
    {
        PlayerManager.ResetPlayerTransform();
    }
}
