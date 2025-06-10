using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnPlayerDataChanged;
    public event Action OnGoldDataChanged;
    public event Action OnStageDataChanged;

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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
    }

    void GameStart()
    {
        _uiManager.OpenUI(UIType.Main);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }

    private void StageClear(int stage)
    {
        // StageManager.NextStage();
        // _uiManager.OpenUI(UIType.StageClear, new OpenParam(stage));
    }

    [ContextMenu("Test")]
    void Test()
    {
        PlayerManager.AddExp(100);
        CurrencyManager.AddGold(100);
        StageManager.StartStage();
        
        OnPlayerDataChanged?.Invoke();
        OnGoldDataChanged?.Invoke();
        OnStageDataChanged?.Invoke();
    }

    [ContextMenu("ExpTest")]
    void ExpTest()
    {
        PlayerManager.AddExp(10);
        OnPlayerDataChanged?.Invoke();
    }
}
