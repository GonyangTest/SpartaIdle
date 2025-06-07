using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class UIPathData
{
    [SerializeField] private UICategory _category;
    [SerializeField] private UIType _uiType;
    [SerializeField] private string _path;
    [SerializeField] private bool _isEnabled = true;
    
    public UIPathData(UIType type, string resourcePath, UICategory uiCategory)
    {
        _uiType = type;
        _path = resourcePath;
        _category = uiCategory;
    }

    public UICategory Category => _category;
    public UIType UiType => _uiType;
    public string Path => _path;
    public bool IsEnabled => _isEnabled;
}

public enum UICategory
{
    Fixed,    // 고정 UI (HUD)
    Window,   // 창형 UI
    Popup     // 팝업 UI
}

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
            if (pathData.IsEnabled && !_pathCache.ContainsKey(pathData.UiType))
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

public static class UIPath
{
    private static UIPathConfig config;
    
    static UIPath()
    {
        LoadConfig();
    }
    
    private static void LoadConfig()
    {
        config = Resources.Load<UIPathConfig>("UI/UIPathConfig");
        if (config == null)
        {
            Debug.LogError("UIPathConfig is not found. Please create UIPathConfig.asset in ScriptableObject/UI folder.");
        }
    }
    
    public static string GetPath(UIType type)
    {
        if (config == null) LoadConfig();
        return config?.GetPath(type) ?? string.Empty;
    }
    
    public static UICategory GetCategory(UIType type)
    {
        if (config == null) LoadConfig();
        return config?.GetCategory(type) ?? UICategory.Window;
    }
    
    public static bool HasPath(UIType type)
    {
        if (config == null) LoadConfig();
        return config?.HasPath(type) ?? false;
    }
    
    public static List<UIType> GetUITypesByCategory(UICategory category)
    {
        if (config == null) LoadConfig();
        return config?.GetUITypesByCategory(category) ?? new List<UIType>();
    }
}