using System.Collections.Generic;
using UnityEngine;

public class LeadTracker : MonoBehaviour
{
    [SerializeField] private List<EnemyController> listEnemy;
    [SerializeField] private float delay = 1;
    [SerializeField] private float radius = 1;

    private readonly float[] noises = new float[4] { 0.2f, 0.33f, 0.5f, 0.83f };

    public void Add(EnemyController enemy)
    {
        if (!this.listEnemy.Contains(enemy))
        {
            //enemy.Target = transform;
            this.listEnemy.Add(enemy);
        }
    }

    public void Remove(EnemyController enemy)
    {
        if (this.listEnemy.Contains(enemy))
        { 
            this.listEnemy.Remove(enemy);
            //enemy.Target = null;
        }
    }

    public void ClearList()
    {
        for (int i = 0; i < listEnemy.Count; ++i)
        {
            //this.listEnemy[i].Target = null;
            this.listEnemy.RemoveAt(i);
            i--;
        }
    }

    private void FixedUpdate()
    {
        if (this.listEnemy.Count > 0)
        {
            Invoke("MakeAgentsNoiseCircleTarget", this.delay);
        } 
        if (this.listEnemy.Count == 0)
        {

        }
    }

    private void MakeAgentsCircleTarget()
    {
        for (int i = 0; i < this.listEnemy.Count; i++)
        {
            Vector3 followPos = new Vector3(
                transform.position.x + this.radius * Mathf.Cos(2 * Mathf.PI * i / this.listEnemy.Count),
                transform.position.y,
                transform.position.z + this.radius * Mathf.Sin(2 * Mathf.PI * i / this.listEnemy.Count));
            this.listEnemy[i].FollowPos = followPos;
        }
    }

    private void MakeAgentsRandomCircleTarget()
    {
        int virtualTotal = (int) (this.listEnemy.Count * 1.5f);
        List<Vector3> listPos = new List<Vector3>();
        for (int i = 0; i < virtualTotal; i++)
        {
            Vector3 followPos = new Vector3(
                transform.position.x + this.radius * Mathf.Cos(2 * Mathf.PI * i / virtualTotal),
                transform.position.y,
                transform.position.z + this.radius * Mathf.Sin(2 * Mathf.PI * i / virtualTotal));
            listPos.Add(followPos);
        }

        foreach (EnemyController e in this.listEnemy)
        {
            int random = Random.Range(0, listPos.Count);
            e.FollowPos = listPos[random];
            listPos.RemoveAt(random);
        }
    }

    private void MakeAgentsNoiseCircleTarget()
    {
        int virtualTotal = this.listEnemy.Count + 4;
        List<int> noiseIndexs = new List<int>();
        for (int i = 0; i < 4; i++)
            noiseIndexs.Add((int)(this.noises[0] * virtualTotal));

        List<Vector3> listPos = new List<Vector3>();
        for (int i = 0; i < virtualTotal; i++)
        {
            if (noiseIndexs.Contains(i)) continue;

            Vector3 followPos = new Vector3(
                transform.position.x + this.radius * Mathf.Cos(2 * Mathf.PI * i / virtualTotal),
                transform.position.y,
                transform.position.z + this.radius * Mathf.Sin(2 * Mathf.PI * i / virtualTotal));
            listPos.Add(followPos);
        }


        for (int i = 0; i < this.listEnemy.Count; i++)
        {
            this.listEnemy[i].FollowPos = listPos[i];
        }
    }
}
