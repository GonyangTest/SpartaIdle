using System.Collections.Generic;
using UnityEngine;

public abstract class OpenParam { }

public class StageParam : OpenParam
{
    [Header("스테이지 기본 정보")]
    public int StageNumber;
    public string ElapsedTime;
    
    [Header("클리어 정보")]
    public int ExpGained;
    public int GoldGained;
    public List<ItemReward> ItemRewards = new List<ItemReward>();

    [Header("실패 정보")]
    public int RemainEnemyCount;
    
    public StageParam(int stageNumber)
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