using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UIPool
{
    private readonly Dictionary<UIType, ObjectPool<BaseUI>> _pools = new();
    private readonly Transform _parent;
    private readonly int _poolSize;

    public UIPool(Transform parent = null, int poolSize = GameConstants.UI.DEFAULT_POOL_SIZE)
    {
        _parent = parent;
        _poolSize = poolSize;
    }

    public BaseUI GetUI(UIType type, Transform parent)
    {
        // 해당 타입의 풀이 없으면 생성
        if (!_pools.ContainsKey(type))
        {
            CreatePoolForType(type);
        }

        var pool = _pools[type];
        var ui = pool.Get();
        
        // 부모 설정
        if (parent != null)
        {
            ui.transform.SetParent(parent, false);
        }
        
        return ui;
    }

    public void ReturnUI(UIType type, BaseUI ui)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            pool.Return(ui);
        }
        else
        {
            Debug.LogWarning($"[UIPool] No pool found for UIType: {type}");
            Object.Destroy(ui.gameObject);
        }
    }

    private void CreatePoolForType(UIType type)
    {
        string path = UIPath.GetPath(type);
        GameObject prefab = Resources.Load<GameObject>(path);
        
        if (prefab == null)
        {
            Debug.LogError($"[UIPool] Failed to load prefab for UIType: {type} at path: {path}");
            return;
        }

        BaseUI baseUI = prefab.GetComponent<BaseUI>();
        if (baseUI == null)
        {
            Debug.LogError($"[UIPool] Prefab at path: {path} doesn't have BaseUI component");
            return;
        }

        _pools[type] = new ObjectPool<BaseUI>(baseUI, _parent, _poolSize);
    }

    public void ClearPool(UIType type)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            pool.Clear();
            _pools.Remove(type);
        }
    }

    public void ClearAllPools()
    {
        foreach (var pool in _pools.Values)
        {
            pool.Clear();
        }
        _pools.Clear();
    }
}