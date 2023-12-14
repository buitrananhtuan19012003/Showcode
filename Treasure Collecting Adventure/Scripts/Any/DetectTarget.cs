using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected List<Transform> visibleTargets = new List<Transform>();
    [SerializeField] protected float detectionRange = 15f;

    protected readonly float delayTime = 1f;

    public float DetectionRange { get => this.detectionRange; set => this.detectionRange = value; }

    protected virtual void OnEnable()
    {
        StartCoroutine(this.FindTargetWithDelay(this.delayTime));
    }

    protected virtual void OnDisable()
    {
        this.visibleTargets.Clear();
    }

    protected virtual IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            this.FindVisibleTarget();
            yield return new WaitForSeconds(delay);
        }
    }
    protected virtual void FindVisibleTarget()
    {
        this.visibleTargets.Clear();
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, this.detectionRange, this.targetLayer);

        for (int i = 0; i < hitCollider.Length; i++)
        {
            Transform targetTransform = hitCollider[i].transform;
            if (targetTransform == null) continue;

            Vector3 directionToEnemy = targetTransform.position - transform.position;
            if (!Physics.Raycast(transform.position, directionToEnemy, this.detectionRange, this.obstacleLayer))
            {
                this.visibleTargets.Add(targetTransform);
            }
        }
    }

    public virtual bool IsDetectTarget()
    {
        return this.visibleTargets.Count > 0;
    }

    public virtual Transform FindClosest()
    {
        if (this.visibleTargets.Count == 0) return null;

        Transform closestTarget = null;
        for (int i = 0; i < this.visibleTargets.Count; i++)
        {
            if (closestTarget == null)
            {
                closestTarget = this.visibleTargets[i];
            }
            else
            {
                if (Vector3.Distance(transform.position, closestTarget.position) >
                    Vector3.Distance(transform.position, this.visibleTargets[i].position))
                {
                    closestTarget = this.visibleTargets[i];
                }
            }
        }

        return closestTarget;
    }

    public virtual IInfoScanner FindClosest(FactionType factionType)
    {
        if (this.visibleTargets.Count == 0) return null;

        IInfoScanner closestEnemy = null;
        for (int i = 0; i < this.visibleTargets.Count; i++)
        {
            IInfoScanner targetScan = this.visibleTargets[i].GetComponent<IInfoScanner>();
            if (targetScan == null || targetScan.GetFactionType() != factionType) continue;

            Vector3 directionToEnemy = targetScan.GetCenterPoint().position - transform.position;
            if (Physics.Raycast(transform.position, directionToEnemy, this.detectionRange, this.obstacleLayer)) continue;

            if (closestEnemy == null)
            {
                closestEnemy = targetScan;
            }
            else
            {
                if (Vector3.Distance(transform.position, closestEnemy.GetCenterPoint().position) >
                    Vector3.Distance(transform.position, this.visibleTargets[i].transform.position))
                {
                    closestEnemy = targetScan;
                }
            }
        }

        return closestEnemy;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = this.IsDetectTarget() ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, this.detectionRange);
    }
}
