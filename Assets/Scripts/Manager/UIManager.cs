using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform _fixedRoot;
    [SerializeField] private Transform _windowRoot;
    [SerializeField] private Transform _popupRoot;

    private readonly Dictionary<UIType, BaseWindow> _activeWindows = new();
    private readonly Stack<BaseUI> _popupUIStack = new();
    private readonly List<BaseFixed> _fixedUIList = new();

    private UIPool _pool;

    protected override void Awake()
    {
        base.Awake();
        _pool = new UIPool(transform, GameConstants.UI.UI_MANAGER_POOL_SIZE);
    }

    // 카테고리에 따라 자동으로 적절한 UI 열기
    public BaseUI OpenUI(UIType type, OpenParam param = null)
    {
        var category = UIPath.GetCategory(type);
        
        switch (category)
        {
            case UICategory.Fixed:
                return OpenFixedUI(type, param);
            case UICategory.Window:
                return OpenWindow(type, param);
            case UICategory.Popup:
                return OpenPopup(type, param);
            default:
                throw new System.ArgumentException($"Unknown UI category: {category}");
        }
    }

    public void CloseUI(UIType type)
    {
        var category = UIPath.GetCategory(type);

        switch (category)
        {
            case UICategory.Fixed:
                CloseFixedUI(type);
                break;
            case UICategory.Window:
                CloseWindow(type);
                break;
            case UICategory.Popup:
                ClosePopup();
                break;
            default:
                throw new System.ArgumentException($"Unknown UI category: {category}");
        }
    }

    public BaseFixed OpenFixedUI(UIType type, OpenParam param = null)
    {
        // 카테고리 검증
        if (UIPath.GetCategory(type) != UICategory.Fixed)
        {
            Debug.LogWarning($"[OpenFixedUI] UI Type({type}) is not Fixed UI, category: {UIPath.GetCategory(type)}");
        }

        var baseUI = _pool.GetUI(type, _fixedRoot);
        var ui = baseUI as BaseFixed;
        
        if (ui == null)
        {
            Debug.LogError($"[OpenFixedUI] UI({type}) is not BaseFixed type! Actual type: {baseUI?.GetType().Name}");
            return null;
        }
        
        ui.OnOpen(param);
        _fixedUIList.Add(ui);
        ui.gameObject.SetActive(true);
        return ui;
    }

    public void CloseFixedUI(UIType type)
    {
        var ui = _fixedUIList.FirstOrDefault(x => x.UIType == type);
        if (ui != null)
        {
            ui.OnClose();
            _pool.ReturnUI(type, ui);
            _fixedUIList.Remove(ui);
        }
    }

    public void OpenAllFixedUI()
    {
        foreach (var fixedUI in _fixedUIList)
        {
            fixedUI.OnOpen();
        }
    }

    public void CloseAllFixedUI()
    {
        var fixedUIListCopy = _fixedUIList.ToList();
        foreach (var fixedUI in fixedUIListCopy)
        {
            fixedUI.OnClose();
            _pool.ReturnUI(fixedUI.UIType, fixedUI);
            _fixedUIList.Remove(fixedUI);
        }
    }

    public BaseWindow OpenWindow(UIType type, OpenParam param = null)
    {
        // 카테고리 검증
        if (UIPath.GetCategory(type) != UICategory.Window)
        {
            Debug.LogWarning($"[OpenWindow] UI Type({type}) is not Window UI, category: {UIPath.GetCategory(type)}");
        }

        // 이미 열려있는 창이면 그냥 반환
        if (_activeWindows.ContainsKey(type))
        {
            Debug.LogWarning($"[OpenWindow] UI Type({type}) is already opened");
            return _activeWindows[type];
        }

        var ui = _pool.GetUI(type, _windowRoot);
        var window = ui as BaseWindow;
        
        if (window == null)
        {
            Debug.LogError($"[OpenWindow] UI({type}) is not BaseWindow type! Actual type: {ui?.GetType().Name}");
            return null;
        }
        
        window.OnOpen(param);
        window.gameObject.SetActive(true);
        _activeWindows[type] = window;
        return window;
    }

    public void CloseWindow(UIType type)
    {
        if (_activeWindows.TryGetValue(type, out var window))
        {
            window.OnClose();
            _pool.ReturnUI(type, window);
            _activeWindows.Remove(type);
        }
    }

    public void CloseAllWindows()
    {
        var windowKeys = _activeWindows.Keys.ToList();
        foreach (var windowKey in windowKeys)
        {
            CloseWindow(windowKey);
        }
    }

    public BasePopup OpenPopup(UIType type, OpenParam param = null)
    {
        // 카테고리 검증
        if (UIPath.GetCategory(type) != UICategory.Popup)
        {
            Debug.LogWarning($"[OpenPopup] UI Type({type}) is not Popup UI, category: {UIPath.GetCategory(type)}");
        }

        var baseUI = _pool.GetUI(type, _popupRoot);
        var popup = baseUI as BasePopup;
        
        if (popup == null)
        {
            Debug.LogError($"[OpenPopup] UI({type}) is not BasePopup type! Actual type: {baseUI?.GetType().Name}");
            return null;
        }
        
        popup.OnOpen(param);
        _popupUIStack.Push(popup);
        popup.gameObject.SetActive(true);
        return popup;
    }

    public void ClosePopup()
    {
        if (_popupUIStack.TryPop(out var popup))
        {
            popup.OnClose();
            _pool.ReturnUI(popup.UIType, popup);
        }
    }

    // 현재 활성 창 (최상위)
    public BaseWindow CurrentWindow => _activeWindows.Count > 0 ? _activeWindows.Values.LastOrDefault() : null;

    // 현재 최상위 팝업
    public BasePopup CurrentPopup => _popupUIStack.Count > 0 ? _popupUIStack.Peek() as BasePopup : null;

    public T GetWindow<T>() where T : BaseWindow
    {
        foreach (var window in _activeWindows.Values)
        {
            if (window is T tWindow)
                return tWindow;
        }
        return null;
    }

    public T GetFixedUI<T>() where T : BaseFixed
    {
        foreach (var ui in _fixedUIList)
        {
            if (ui is T tUI)
                return tUI;
        }
        return null;
    }

    public bool IsUIActive(UIType type)
    {
        var category = UIPath.GetCategory(type);
        switch (category)
        {
            case UICategory.Fixed:
                return _fixedUIList.Any(ui => ui.UIType == type);
            case UICategory.Window:
                return _activeWindows.ContainsKey(type);
            case UICategory.Popup:
                return _popupUIStack.Any(popup => popup.UIType == type);
            default:
                return false;
        }
    }
}
