using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<PoolingKey, Pool<IPoolable>> _poolDictionary
       = new Dictionary<PoolingKey, Pool<IPoolable>>();

    public PoolListSO poolListSO;

    private void Awake()
    {
        foreach (PoolingItemSO item in poolListSO.GetList())
        {
            CreatePool(item);
        }
    }

    private void CreatePool(PoolingItemSO item)
    {
        var key = item.PoolObj.PoolEnum;
        var pool = new Pool<IPoolable>(item.PoolObj, new PoolingKey(key), transform, item.poolCount);
        
        _poolDictionary.Add(new PoolingKey(key), pool);
        if (key is EnemyPoolingType) return;
        StartCoroutine(WarmUpEffect(pool));
    }

    private IEnumerator WarmUpEffect(Pool<IPoolable> pool)
    {
        var obj = pool.Pop();
        var go = obj.GameObject;

        go.transform.SetParent(transform);
        go.transform.position = new Vector3(9999, 9999, 9999); // 화면 바깥
        go.SetActive(true);
        yield return null; // 한 프레임 기다리기
        Push(obj, true); // 다시 푸시
    }


    public IPoolable Pop(Enum type)
    {
        PoolingKey key = new PoolingKey(type);

        if (!_poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"Prefab does not exist on pool : {key}");
            return null;
        }

        var item = _poolDictionary[key].Pop();
        item.OnPop();
        return item;
    }

    public void Push(IPoolable obj, bool resetParent = false)
    {
        if (resetParent)
            obj.GameObject.transform.SetParent(transform);
        obj.OnPush();
        _poolDictionary[new PoolingKey(obj.PoolEnum)].Push(obj);
    }

}

public static class PoolingUtility
{
    public static IPoolable Pop(this GameObject gameObject, Enum type)
    {
        if (PoolManager.Instance == null)
        {
            Debug.LogError("PoolManager�� �����ϴ�.");
            return null;
        }
        else
        {
            IPoolable poolable = PoolManager.Instance.Pop(type);
            GameObject obj = poolable.GameObject;
            obj.transform.parent = null;
            obj.transform.position = Vector3.zero;
            return poolable;
        }
    }
    public static IPoolable Pop(this GameObject gameObject, Enum type, Transform parent)
    {
        if (PoolManager.Instance == null)
        {
            Debug.LogError("PoolManager�� �����ϴ�.");
            return null;
        }
        else
        {
            IPoolable poolable = PoolManager.Instance.Pop(type);
            GameObject obj = poolable.GameObject;
            obj.transform.parent = parent;
            obj.transform.position = Vector3.zero;
            return poolable;
        }
    }
    public static IPoolable Pop(this GameObject gameObject, Enum type, Vector3 position, Quaternion rotation)
    {
        if (PoolManager.Instance == null)
        {
            Debug.LogError("PoolManager�� �����ϴ�.");
            return null;
        }
        else
        {
            IPoolable poolable = PoolManager.Instance.Pop(type);
            GameObject obj = poolable.GameObject;
            obj.transform.parent = null;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return poolable;
        }
    }
    public static void Push(this IPoolable poolable)
    {
        PoolManager poolManager = PoolManager.Instance;
        if (poolManager == null)
        {
            Debug.LogError("PoolManager�� �����ϴ�.");
        }
        else
        {
            GameObject obj = poolable.GameObject;
            obj.transform.parent = poolManager.transform;
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            poolManager.Push(poolable);
        }
    }
}


