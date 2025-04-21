using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsPoolObject : MonoBehaviour
{
public List<GameObject> pool = new List<GameObject>();
public GameObject pooledObject;
private int initialPoolSize = 10;

void Start()
{
    
    for (int i = 0; i < initialPoolSize; i++)
    {
        GameObject obj = Instantiate(pooledObject);
        obj.SetActive(false);
        pool.Add(obj);
    }
}

public GameObject GetPooledObject()
{
    foreach (GameObject obj in pool)
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
            return obj;
        }
    }
    GameObject newObj = Instantiate(pooledObject);
    pool.Add(newObj);
    return newObj;
}

public void ReturnToPool(GameObject obj)
{
    obj.SetActive(false);
}


}
