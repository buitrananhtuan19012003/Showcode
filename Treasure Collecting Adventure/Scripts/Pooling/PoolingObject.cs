using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public int Amount;
    public GameObject ObjectPrefab;
}

public class PoolingObject : MonoBehaviour
{
    [SerializeField] protected GameObject objectPrefab;
    [SerializeField] protected int amount = 7;
    [SerializeField] protected List<SpawnObject> listObjPrefab = new List<SpawnObject>();
    [SerializeField] protected List<GameObject> pools = new List<GameObject>();
    [SerializeField] protected Transform spawnPool;

    protected virtual void Start()
    {
        //Single Object
        if (this.listObjPrefab == null)
        {
            if (this.pools.Count < this.amount)
            {
                int need = this.amount - this.pools.Count;
                for (int i = 0; i < need; i++)
                {
                    GameObject newObj = Instantiate(this.objectPrefab, transform.position, transform.rotation, this.spawnPool);
                    newObj.SetActive(false);
                    this.pools.Add(newObj);
                }
            }
        }

        //Multi Object
        else
        {
            for (int i = 0; i < this.listObjPrefab.Count; i++)
            {
                for (int j = 0; j < this.listObjPrefab[i].Amount; j++)
                {
                    GameObject newObj = Instantiate(this.listObjPrefab[i].ObjectPrefab, this.spawnPool);
                    newObj.SetActive(false);
                    this.pools.Add(newObj);
                }
            }
        }
    }

    public virtual GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < this.pools.Count; i++)
        {
            if (this.ObjectIsActive(this.pools[i]) == false)
            {
                this.pools[i].transform.position = position;
                this.pools[i].transform.rotation = rotation;
                RefreshObject(this.pools[i]);
                return this.pools[i];
            }
        }

        GameObject newObj = Instantiate(this.objectPrefab, position, rotation, this.spawnPool);
        this.pools.Add(newObj);
        return newObj;
    }

    public virtual bool ObjectIsActive(GameObject obj)
    {
        return obj.activeSelf;
    }

    public virtual void RefreshObject(GameObject obj)
    {
        obj.SetActive(true);
    }
}
