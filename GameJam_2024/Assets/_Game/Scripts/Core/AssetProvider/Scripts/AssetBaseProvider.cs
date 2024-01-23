﻿using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AssetBaseProvider : MonoBehaviour, IPool
{
    protected GameObject poolObject;

    public readonly Dictionary<GameObject, Stack<GameObject>> pool = new Dictionary<GameObject, Stack<GameObject>>();

    public readonly List<GameObject> ActivePool = new List<GameObject>();
    protected abstract AssetBaseProvider GetInstance();

    protected GameObject GetObjectFromPool(GameObject requiredObject, int count = 0)
    {
        if (GetInstance().pool.ContainsKey(requiredObject))
        {
            if (GetInstance().pool[requiredObject].Count > 0)
            {
                var obj = GetInstance().pool[requiredObject].Pop();
                
                if (obj == null || obj.activeInHierarchy)
                {
                    return count < 5 ? GetObjectFromPool(requiredObject, count++) : CreatePoolableObject(requiredObject);
                }
                obj.SetActive(true);
                obj.transform.SetParent(null);
                ActivePool.Add(obj);
                return obj;
            }
            else
            {
                return CreatePoolableObject(requiredObject);
            }
        }
        else
        {
            return CreatePoolableObject(requiredObject);
        }
    }

    protected GameObject CreatePoolableObject(GameObject poolableObject)
    {
        var newObject = Instantiate(poolableObject);

        var poolable = newObject.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            poolable = newObject.AddComponent<PoolableObject>();
        }

        poolable.SetPool(poolableObject, GetInstance());
        newObject.SetActive(true);

        return newObject;
    }

    protected void InstatiatePool(GameObject gameObject, int poolSize)
    {
        GetInstance().pool[gameObject] = new Stack<GameObject>();
        FillPool(gameObject, poolSize);
    }

    protected void FillPool(GameObject templateObject, int numberOfInstances)
    {
        var pool = GetInstance().pool[templateObject];
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject newObject = CreatePoolableObject(templateObject);
            newObject.transform.SetParent(GetInstance().poolObject.transform);
            newObject.SetActive(false);
            pool.Push(newObject);
        }
    }

    public void ReturnToPool(GameObject objectToReturn, GameObject key)
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(poolObject.transform);
        pool[key].Append(objectToReturn);
        ActivePool.Remove(objectToReturn);
    }

    public virtual void ReturnAllToPool()
    {

    }


    protected virtual void ReturnAllItemsToPool(GameObject poolObject)
    {
        List<GameObject> active = new List<GameObject>();
        active.AddRange(ActivePool);
        var count = ActivePool.Count;
        for (int i = 0; i < count; i++)
        {
            var item = active[i];

            var poolable = item.GetComponent<PoolableObject>();
            poolable.ReturnToPool();
        }
        ActivePool.Clear();
    }
}

