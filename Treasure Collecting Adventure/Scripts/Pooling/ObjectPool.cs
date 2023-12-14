using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : BaseManager<ObjectPool>
{
    public List<Bullet> pooledObjects;
    public Bullet objectToPool;
    private int amountToPool;

    private void Start()
    {
        pooledObjects = new List<Bullet>();
        amountToPool = DataManager.Instance.GlobalConfig.maxBulletPoolSize;
        Bullet tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool,this.transform,true);
            tmp.Deactive();
            pooledObjects.Add(tmp);
        }
    }

    public Bullet GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].IsActive)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
