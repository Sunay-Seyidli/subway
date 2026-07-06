using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;
    private Dictionary<string, GameObject> _prefabDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        _prefabDictionary = new Dictionary<string, GameObject>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            _prefabDictionary[pool.tag] = pool.prefab;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = CreateNewObject(pool.tag, pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary[pool.tag] = objectPool;
        }
    }

    private GameObject CreateNewObject(string tag, GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = $"{tag}_Pooled";
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn;
        Queue<GameObject> pool = _poolDictionary[tag];

        if (pool.Count > 0 && !pool.Peek().activeInHierarchy)
        {
            objectToSpawn = pool.Dequeue();
        }
        else
        {
            objectToSpawn = CreateNewObject(tag, _prefabDictionary[tag]);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        pool.Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        obj.transform.SetParent(null);
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        obj.transform.SetParent(null);
    }
}
