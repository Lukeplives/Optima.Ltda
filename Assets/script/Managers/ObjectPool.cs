using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;
    public enum PoolTag
    {
        BalaPadrao,
        BalaExplosiva,
        BalaSniper,
        InimigoTerrestre,
        InimigoVoador,
        InimigoGrande,
        InimigoKamikaze,
        ItemFerro,
        ItemFerroDROP,
        ItemComb,
        ItemCombDROP,
        TorretaMetralhadora,
        TorretaSniper,
        TorretaExplosiva,
        TorretaLancaChama,
        ProjetilTerrestre,
        ProjetilVoador,
        ProjetilGrande,
        KamikazeExplosao,
        ExplosaoEffect,
        TiroEffect,
        AmmoUI
    }
    void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    // Spawn básico
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool com a tag {tag} não existe");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        if (obj.activeSelf)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                GameObject newObj = Instantiate(pool.prefab);
                newObj.SetActive(false);
                poolDictionary[tag].Enqueue(newObj);
                obj = newObj;
            }
            else
            {
                Debug.LogWarning($"Pool {tag} não encontrado na lista!");
            }
        }

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
    
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag.ToString())) return null;

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);
        return obj;
        
    }


    //Spawn por Tag
    
    public GameObject SpawnFromPool(PoolTag tag, Vector3 position, Quaternion rotation)
    {
        return SpawnFromPool(tag.ToString(), position, rotation);
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
    }
}
