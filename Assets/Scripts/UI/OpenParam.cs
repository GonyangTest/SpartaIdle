using System.Collections.Generic;
using UnityEngine;

public abstract class OpenParam { }

public class StageClearParam : OpenParam
{
    [Header("스테이지 기본 정보")]
    public int StageNumber;
    public string ClearTime;
    
    [Header("보상 정보")]
    public int ExpGained;
    public int GoldGained;
    public List<ItemReward> ItemRewards = new List<ItemReward>();
    
    public StageClearParam(int stageNumber)
    {
        StageNumber = stageNumber;
    }
}

[System.Serializable]
public class ItemReward
{
    public int ItemID;
    public string ItemName;
    public Sprite ItemIcon;
    
    public ItemReward(int itemId, string itemName, Sprite itemIcon)
    {
        ItemID = itemId;
        ItemName = itemName;
        ItemIcon = itemIcon;
    }
}

public class GenericParam : OpenParam
{
    private Dictionary<string, object> _parameters = new Dictionary<string, object>();
    
    public void Set<T>(string key, T value)
    {
        _parameters[key] = value;
    }
    
    public T Get<T>(string key, T defaultValue = default(T))
    {
        if (_parameters.TryGetValue(key, out var value) && value is T)
        {
            return (T)value;
        }
        return defaultValue;
    }
    
    public bool TryGet<T>(string key, out T value)
    {
        value = default(T);
        if (_parameters.TryGetValue(key, out var obj) && obj is T)
        {
            value = (T)obj;
            return true;
        }
        return false;
    }
}

/*
사용 예시:

// 스테이지 클리어 파라미터 생성
var clearParam = new StageClearParam(1)
{
    StageName = "던전 1층",
    ClearTime = 154.5f,
    Grade = ClearGrade.A,
    ExpGained = 1250,
    GoldGained = 500,
    IsLevelUp = true,
    PreviousLevel = 5,
    NewLevel = 6,
    IsNewRecord = true,
    PreviousBestTime = 180.2f
};

// 성과 통계 설정
clearParam.ClearStats.MonstersDefeated = 25;
clearParam.ClearStats.DamageTaken = 120;
clearParam.ClearStats.CriticalHits = 8;
clearParam.ClearStats.PotionsUsed = 3;

// 아이템 보상 추가
clearParam.ItemRewards.Add(new ItemReward(10001, "체력 물약", potionIcon, 2, ItemRarity.Common));
clearParam.ItemRewards.Add(new ItemReward(20001, "철 검", swordIcon, 1, ItemRarity.Rare));

// UI 열기
_uiManager.OpenUI(UIType.StageClear, clearParam);

// 범용 파라미터 사용 예시
var genericParam = new GenericParam();
genericParam.Set("stageNumber", 1);
genericParam.Set("playerName", "Player1");
genericParam.Set("clearTime", 123.45f);
_uiManager.OpenUI(UIType.SomeUI, genericParam);
*/