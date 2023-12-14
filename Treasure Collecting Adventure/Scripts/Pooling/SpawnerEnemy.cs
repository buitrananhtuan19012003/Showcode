using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : SaiMonoBehaviour
{
    [SerializeField] private PoolingObject poolingObject;
    [SerializeField] private List<GameObject> listCurEnemy;
    [SerializeField] private int maxEnemy = 5;
    [SerializeField] private float delaySpawn = 30f;
    [SerializeField] private float timer;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.poolingObject == null)
            this.poolingObject = GetComponent<PoolingObject>();
    }

    private void Start()
    {
        this.timer = this.delaySpawn - 5;
        this.CheckList();
    }

    private void FixedUpdate()
    {
        if (this.listCurEnemy.Count < this.maxEnemy)
        {
            this.timer += Time.fixedDeltaTime;
            if (this.timer > this.delaySpawn)
            {
                int need = this.maxEnemy - this.listCurEnemy.Count;
                for (int i = 0; i < need; i++)
                {
                    Debug.Log("Spawn");
                    Vector3 randomOffsetPos = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                    this.listCurEnemy.Add(this.poolingObject.GetObject(transform.position + randomOffsetPos, transform.rotation));
                }
                this.timer = 0;
            }
        }
    }

    private void CheckList()
    {
        foreach (GameObject obj in this.listCurEnemy)
        {
            EnemyController enemyCtrl = obj.GetComponent<EnemyController>();
            if (enemyCtrl == null || enemyCtrl.EnemyHealth.IsDeath())
            {
                this.listCurEnemy.Remove(obj);
            }
        }

        Invoke("CheckList", 10);
    }
}
