/**
 * Created by Mike Marek (April 2018)
 * ObjectPool.cs
 * Creates an initial pool of managed GameObjects that can be accessed when a
 * new instance of one is needed. This avoids having to call Instantiate() many
 * times, which can be expensive.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public          GameObject          pooledObject    = null;
    public          uint                initialPoolSize = 100;
    public          bool                shouldExpand    = true;

    private         List<GameObject>    pool;

    private static  ObjectPool          _instance;


    /**
     * Obtain singleton reference and populate the initial object pool.
     *
     * @param   null
     * @return  null
    **/
    void Awake()
    {
        pool = new List<GameObject>();
        _instance = this;

        if (pooledObject == null)
        {
            Debug.LogError("a pooled object prefab must be specified");
            return;
        }

        for (uint i = 0; i < initialPoolSize; i++)
            AddObjectToPool();
    }


    /**
     * Return an unused GameObject from the object pool.
     *
     * @param   null
     * @return  GameObject  object for use - null if pool is full
    **/
    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        if (shouldExpand)
            return AddObjectToPool();

        return null;
    }


    /**
     * Add a new object to the object pool.
     *
     * @param   null
     * @return  GameObject  newly added object
    **/
    private GameObject AddObjectToPool()
    {
        GameObject go = Instantiate(pooledObject);
        go.transform.SetParent(transform);
        pool.Add(go);
        return go;
    }


    // Getter method for class singleton instance.
    public static ObjectPool Instance { get { return _instance; } }
}
