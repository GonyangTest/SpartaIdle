using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageDataSO _stageDataSO;
    [SerializeField] private List<SpawnAreaRect> _spawnAreaRects = new List<SpawnAreaRect>();
    public int CurrentStage { get; private set; }

    private StageParam _stageClearParam;
    private float _startTime;
    private bool _progressing = false;
    private AIPlayer _player;

    public event Action<int, int> OnEnemyCountChanged;
    public event Action OnStageDataChanged;
    
    // 적 수 트래킹 관련
    private List<AIEnemy> _activeEnemies = new List<AIEnemy>();
    private int _totalEnemyCount = 0;

    public void SpawnEnemy(int stageNumber)
    {
        if(_player == null) _player = PlayerManager.Instance.Player;

        if(_stageDataSO == null) LoadStageData();

        StageData stageData = _stageDataSO.Stages.Find(stage => stage.StageNumber == stageNumber);
        if(stageData == null) return;

        // 적 총 수 저장
        _totalEnemyCount = stageData.Enemies.Count;
        _activeEnemies.Clear();

        foreach(var enemy in stageData.Enemies)
        {
            Vector3 spawnPosition = _spawnAreaRects[Random.Range(0, _spawnAreaRects.Count)].GetRandomSpawnPosition();
            AIEnemy enemyInstance = EnemyDatabase.Instance.SpawnEnemy(spawnPosition, enemy);
            enemyInstance.SetTarget(_player.transform);
            
            // 활성 적 리스트에 추가
            _activeEnemies.Add(enemyInstance);
        }
    }

    private float _checkInterval = 0.5f; // 0.5초마다 체크
    private float _lastCheckTime = 0f;

    private void Update()
    {
        // 스테이지 진행 중일 때만 적 수 체크 (성능 최적화)
        if (_progressing && Time.time - _lastCheckTime >= _checkInterval)
        {
            CheckForStageClear();
            _lastCheckTime = Time.time;
        }
    }

    private void LoadStageData()
    {
        _stageDataSO = Resources.Load<StageDataSO>("Stage/StageData");
    }

    public void StartStage(int stageNumber)
    {
        CurrentStage = stageNumber;
        _stageClearParam = new StageParam(CurrentStage);
        _startTime = Time.time;

        _stageClearParam.ExpGained = 0;
        _stageClearParam.GoldGained = 0;
        _stageClearParam.ItemRewards = new List<ItemReward>();

        _progressing = true;

        SpawnEnemy(stageNumber);

        OnStageDataChanged?.Invoke();
    }

    public void AddExp(int exp)
    {
        _stageClearParam.ExpGained += exp;
    }

    public void AddGold(int gold)
    {
        _stageClearParam.GoldGained += gold;
    }

    public void AddItemReward(ItemInstance item)
    {
        GenericItemDataSO itemData = ItemDatabase.Instance.GetItemByID(item.ItemID);
        _stageClearParam.ItemRewards.Add(new ItemReward(itemData.ItemID, itemData.ItemName, itemData.Icon));
    }

    public void EndStage(bool isClear)
    {
        if(!_progressing) return;

        _stageClearParam.ElapsedTime = TimeFormatter.ToHourMinuteSecondFormat(Time.time - _startTime);

        if(isClear)
        {
            UIManager.Instance.OpenUI(UIType.StageClear, _stageClearParam);
        }
        else
        {
            UIManager.Instance.OpenUI(UIType.StageFail, _stageClearParam);
        }

        _progressing = false;
    }

    public void NextStage()
    {
        CurrentStage++;
        StartStage(CurrentStage);
    }

    private void CheckForStageClear()
    {
        // null인 적들을 리스트에서 제거 (죽은 적들)
        _activeEnemies.RemoveAll(enemy => enemy == null);
        
        // 모든 적이 죽었으면 스테이지 클리어
        if (_activeEnemies.Count == 0 && _totalEnemyCount > 0)
        {
            EndStage(true);
        }
    }

    public void OnEnemyDeath(AIEnemy enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }

        OnEnemyCountChanged?.Invoke(_activeEnemies.Count, _totalEnemyCount);
        _stageClearParam.RemainEnemyCount = _activeEnemies.Count;
    }

    public int GetActiveEnemyCount()
    {
        _activeEnemies.RemoveAll(enemy => enemy == null);
        return _activeEnemies.Count;
    }

    public int GetTotalEnemyCount()
    {
        return _totalEnemyCount;
    }
}

