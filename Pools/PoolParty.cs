using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolParty : MonoBehaviour
{
    [SerializeField]
    protected List<Pool> party = new List<Pool>();
    
    private static PoolParty instance;
    #region Properties
    public static PoolParty Instance { get => instance; }
    public List<Pool> Party { get => party; }
    #endregion
    public Pool GetPool(string name)
    {
        foreach(Pool pool in party)
        {
            if(pool.Name == name)
            {
                return pool;
            }
        }
        return null;
    }
    public Pool GetPool(GameObject gameObject)
    {
        foreach (Pool pool in party)
        {
            foreach (GameObject pooledObject in pool.PooledObjects)
            {
                if (gameObject == pooledObject)
                {
                    return pool;
                }
            }
        }
        return null;
    }

    private void Awake()
    {
        #region Singleton
        instance = Singleton.Singletonize(instance, this);
        #endregion
    }

    [Serializable]
    public class Pool
    {
        [SerializeField]
        private string name;
        [Header("--POOL--")]
        [SerializeField]
        private List<GameObject> pooledObjects;
        [Header("--PARENT--")]
        [SerializeField]
        private Transform parent;
        [Header("--PREFAB--")]
        [SerializeField]
        private GameObject prefab;

        #region Properties
        public string Name { get => name; }
        public Transform Parent { get => parent; }
        public List<GameObject> PooledObjects { get => pooledObjects; }
        #endregion

        public GameObject CreatePooledObject()
        {
            GameObject newObject = Instantiate(prefab, parent);
            pooledObjects.Add(newObject);
            return newObject;
        }

        public GameObject GetPooledObject()
        {
            foreach (GameObject pooledObject in pooledObjects)
            {
                if (pooledObject != null && !pooledObject.activeInHierarchy)
                {
                    pooledObject.SetActive(true);
                    return pooledObject;
                }
            }
            GameObject newObject = CreatePooledObject();
            return newObject;
        }
        public void GetBackToPool(GameObject gameObject)
        {
            if (gameObject != null && IsObjectInPool(gameObject))
            {
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"object didn't belong to pool: {name}");
            }
        }
        public bool IsObjectInPool(GameObject gameObject)
        {
            foreach (GameObject pooledObject in pooledObjects)
            {
                if (pooledObject == gameObject)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
