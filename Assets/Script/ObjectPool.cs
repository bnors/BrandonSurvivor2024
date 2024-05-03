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
    [SerializeField] GameObject objectToPool;
    [SerializeField] int poolCount = 100;

    List<GameObject> poolObject = new();

    private static ObjectPool instance;

    public static ObjectPool getInstance() => instance;
    int poolIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = Instantiate(objectToPool, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            poolObject.Add(obj);
        }
    }

    public GameObject getPooledObject()
    {
        poolIndex %= poolCount;
        GameObject p = poolObject[poolIndex++];
        p.GetComponent<IPoolable>().Reset();
        return p;
    }
}
