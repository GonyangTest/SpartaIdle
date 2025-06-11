using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _pool;
    private readonly List<T> _activeObjects;
    private readonly int _poolSize;

    public ObjectPool(T prefab, Transform parent = null, int poolSize = GameConstants.UI.DEFAULT_POOL_SIZE)
    {
        _prefab = prefab;
        _parent = parent;
        _poolSize = poolSize;

        _pool = new Queue<T>();
        _activeObjects = new List<T>();

        Initialize();
    }

    protected void Initialize()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewObject();
        }
    }

    protected T CreateNewObject()
    {
        if (_prefab == null)
        {
            Debug.LogError("ObjectPool: Prefab is null!");
            return null;
        }

        T newObject = Object.Instantiate(_prefab, _parent);
        newObject.gameObject.SetActive(false);
        _pool.Enqueue(newObject);

        return newObject;
    }

    public virtual T Get()
    {
        T obj = null;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
            if (obj != null)
            {
                _pool.Dequeue();
            }
        }

        if (obj != null)
        {
            obj.gameObject.SetActive(true);
            _activeObjects.Add(obj);
        }

        return obj;
    }

    public void Return(T obj)
    {
        if (obj == null) return;

        if (_activeObjects.Contains(obj))
        {
            _activeObjects.Remove(obj);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("ObjectPool: this object is not in the pool");
        }
    }

    public void ReturnAll()
    {
        var activeObjectsCopy = new List<T>(_activeObjects);
        foreach (var obj in activeObjectsCopy)
        {
            Return(obj);
        }
    }

    public void Clear()
    {
        // 활성 오브젝트들 파괴
        foreach (var obj in _activeObjects)
        {
            if (obj != null)
            {
                Object.Destroy(obj.gameObject);
            }
        }

        // 풀에 있는 오브젝트들 파괴
        while (_pool.Count > 0)
        {
            var obj = _pool.Dequeue();
            if (obj != null)
            {
                Object.Destroy(obj.gameObject);
            }
        }

        _activeObjects.Clear();
    }
} 