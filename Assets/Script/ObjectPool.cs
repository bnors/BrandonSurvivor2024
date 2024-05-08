using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public interface IPoolable
{
    void Reset();
}

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string poolName;  // Unique identifier for each pool
        public GameObject prefab;  // Prefab to be pooled
        public int poolCount;  // Number of objects in the pool
    }

    [SerializeField] private List<Pool> pools = new List<Pool>();  // Let Unity handle this via Inspector

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private static ObjectPool instance;

    public static ObjectPool Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolCount; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);  // Start with inactive objects
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolName, objectPool);
        }
    }

    public GameObject GetPooledObject(string poolName)
    {
        if (!poolDictionary.ContainsKey(poolName)) return null;

        GameObject pooledObject = null;

        // Attempt to dequeue an object from the pool
        if (poolDictionary[poolName].Count > 0)
        {
            pooledObject = poolDictionary[poolName].Dequeue();
        }

        // Check if the object is null or has been destroyed
        if (pooledObject == null)
        {
            // If an object has been destroyed, re-instantiate a new one from the prefab
            Pool pool = pools.Find(p => p.poolName == poolName);
            if (pool.prefab != null)
            {
                pooledObject = Instantiate(pool.prefab);
            }
            else
            {
                return null;
            }
        }

        // Activate and reset the pooled object
        pooledObject.SetActive(true);
        IPoolable poolable = pooledObject.GetComponent<IPoolable>();
        poolable?.Reset();

        // Re-enqueue the object back into the pool
        poolDictionary[poolName].Enqueue(pooledObject);
        return pooledObject;
    }
}