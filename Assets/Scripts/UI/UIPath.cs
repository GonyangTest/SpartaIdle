using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class UIPathData
{
    [SerializeField] private UICategory _category;
    [SerializeField] private UIType _uiType;
    [SerializeField] private string _path;
    [SerializeField] private bool _isUse = true;
    
    public UIPathData(UIType type, string resourcePath, UICategory uiCategory)
    {
        _uiType = type;
        _path = resourcePath;
        _category = uiCategory;
    }

    public UICategory Category => _category;
    public UIType UiType => _uiType;
    public string Path => _path;
    public bool IsUsed => _isUse;
}

public enum UICategory
{
    Fixed,    // 고정 UI (HUD)
    Window,   // 창형 UI
    Popup     // 팝업 UI
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