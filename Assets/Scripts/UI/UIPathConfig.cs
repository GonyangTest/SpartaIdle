using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UIPathConfig", menuName = "UI/UI Path Config")]
public class UIPathConfig : ScriptableObject
{
    [SerializeField] private List<UIPathData> _uiPaths = new List<UIPathData>();
    private Dictionary<UIType, UIPathData> _pathCache;
    
    void OnEnable()
    {
        RefreshCache();
    }

    public void RefreshCache()
    {
        _pathCache = new Dictionary<UIType, UIPathData>();
        foreach (var pathData in _uiPaths)
        {
            if (pathData.IsUsed && !_pathCache.ContainsKey(pathData.UiType))
            {
                _pathCache[pathData.UiType] = pathData;
            }
        }
    }
    
    public string GetPath(UIType type)
    {
        if (_pathCache?.TryGetValue(type, out var pathData) == true)
        {
            return pathData.Path;
        }
        
        Debug.LogWarning($"UI Path not found for type: {type}");
        return string.Empty;
    }
    
    public UICategory GetCategory(UIType type)
    {
        if (_pathCache?.TryGetValue(type, out var pathData) == true)
        {
            return pathData.Category;
        }
        
        Debug.LogWarning($"UI Category not found for type: {type}");
        return UICategory.Window; // 기본값
    }
    
    public bool HasPath(UIType type)
    {
        return _pathCache?.ContainsKey(type) == true;
    }
    
    public List<UIType> GetUITypesByCategory(UICategory category)
    {
        var result = new List<UIType>();
        if (_pathCache == null) return result;
        
        foreach (var kvp in _pathCache)
        {
            if (kvp.Value.Category == category)
            {
                result.Add(kvp.Key);
            }
        }
        return result;
    }
}
