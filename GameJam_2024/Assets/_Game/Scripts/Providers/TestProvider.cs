using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestProviderType
{
    Type1, Type2
}
public class TestProvider : AssetBaseProvider
{
    [Header("Assets")]
    public GameObject TestAsset1;
    public GameObject TestAsset2;

    private static TestProvider _instance;
    private static TestProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<TestProvider>("TestAssetProvider");
            }

            return _instance;
        }
    }

    protected override AssetBaseProvider GetInstance()
    {
        return Instance;
    }
    public static GameObject GetAsset(TestProviderType asset)
    {
        var go = Instance.GetObjectFromPool(GameObjectForType(asset));
        go.transform.rotation = Quaternion.identity;

        return go;
    }

    public static void Prewarm()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = Resources.Load<TestProvider>(Strings.AssetProvidersPath + "TestAssetProvider");

        _instance.poolObject = new GameObject
        {
            name = "Pool"
        };

        //prefill and init pool with one value for each WeaponType enum value
        foreach (TestProviderType asset in Enum.GetValues(typeof(TestProviderType)))
        {
            Instance.InstatiatePool(GameObjectForType(asset), PoolSizeForType(asset));
        }
        DontDestroyOnLoad(_instance.poolObject);
    }

    public PoolableObject RegisterObjectAsPoolable(GameObject gameObject, TestProviderType type)
    {
        var poolable = gameObject.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            poolable = gameObject.AddComponent<PoolableObject>();
        }

        poolable.SetPool(GameObjectForType(type), Instance);

        if (GetInstance().pool[gameObject] == null)
        {
            Instance.InstatiatePool(gameObject, PoolSizeForType(type));
        }


        return poolable;
    }

    private static GameObject GameObjectForType(TestProviderType type)
    {
        switch (type)
        {
            case TestProviderType.Type1:
                return Instance.TestAsset1;
            case TestProviderType.Type2:
                return Instance.TestAsset1;
            default:
                return null;
        }
    }

    private static int PoolSizeForType(TestProviderType type)
    {
        switch (type)
        {
            case TestProviderType.Type1:
                return 10;
            case TestProviderType.Type2:
                return 20;
            default:
                return 0;
        }
    }

    public override void ReturnAllToPool()
    {
        foreach (TestProviderType asset in Enum.GetValues(typeof(TestProviderType)))
        {
            ReturnAllItemsToPool(GameObjectForType(asset));
        }
    }
}
