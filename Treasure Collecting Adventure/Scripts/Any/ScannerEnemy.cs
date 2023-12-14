using System.Collections.Generic;
using UnityEngine;

public class ScannerEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private List<EnemyController> enemies = new List<EnemyController>();

    private int maxScanTimes = 3;
    private float scanRange = 5f;

    public int MaxSearchTimes { get => this.maxScanTimes; set => this.maxScanTimes = value; }
    public float SearchRadius { get => this.scanRange; set => this.scanRange = value; }
    public List<EnemyController> Enemies { get => this.enemies; }

    public void Scan(EnemyController firstEnemy, int maxScanTimes, float scanRange)
    {
        this.maxScanTimes = maxScanTimes;
        this.scanRange = scanRange;
        this.enemies.Clear();
        this.enemies.Add(firstEnemy);
        this.SearchForClosestEnemy(firstEnemy.CenterPoint, this.maxScanTimes - 1);
    }

    private void SearchForClosestEnemy(Transform origin, int searchTimeLeft)
    {
        if (searchTimeLeft <= 0) return;

        Collider[] hitCollider = Physics.OverlapSphere(origin.position, this.scanRange, this.enemyLayer);
        EnemyController closestEnemy = null;

        for (int i = 0; i < hitCollider.Length; i++)
        {
            EnemyController enemyCtrl = hitCollider[i].transform.GetComponent<EnemyController>();
            if (enemyCtrl == null || this.enemies.Contains(enemyCtrl)) continue;

            Vector3 directionToEnemy = enemyCtrl.CenterPoint.position - origin.position;
            if (Physics.Raycast(origin.position, directionToEnemy, this.scanRange, this.obstacleLayer)) continue;

            if (closestEnemy == null)
            {
                closestEnemy = enemyCtrl;
            }
            else
            {
                if (Vector3.Distance(origin.position, closestEnemy.transform.position) >
                    Vector3.Distance(origin.position, enemyCtrl.transform.position))
                {
                    closestEnemy = enemyCtrl;
                }
            }
        }

        if (closestEnemy != null)
        {
            this.enemies.Add(closestEnemy);
            this.SearchForClosestEnemy(closestEnemy.CenterPoint, searchTimeLeft - 1);
        }
    }
}
