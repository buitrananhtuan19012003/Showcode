using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : SaiMonoBehaviour
{
    [SerializeField] private float delayTime = 0.05f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> inflectionPoints = new List<Transform>();

    private Vector3[] points;
    private readonly int pointMiddleLeft = 1;
    private readonly int pointCenter = 2;
    private readonly int pointMiddleRight = 3;
    private readonly int pointEnd = 4;
    private readonly float randomPosOffset = 0.3f;
    private readonly float randomWidthOffsetMin = 1f;
    private readonly float randomWidthOffsetMax = 2f;

    public List<Transform> InflectionPoints { get => this.inflectionPoints; set => this.inflectionPoints = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.lineRenderer == null)
            this.lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDisable()
    {
        StopCoroutine(this.AnimateElectricBeam());
    }

    public IEnumerator AnimateElectricBeam()
    {
        if (this.inflectionPoints == null || this.inflectionPoints.Count == 0)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(this.delayTime);

            int totalPoint = 1 + (this.inflectionPoints.Count - 1) * 4;
            this.points = new Vector3[totalPoint];
            this.lineRenderer.positionCount = totalPoint;

            this.GenerateArrayPoint();
            this.lineRenderer.SetPositions(this.points);
            this.lineRenderer.startWidth = this.GetRandomWidthOffset();
            this.lineRenderer.endWidth = this.GetRandomWidthOffset();
        }
    }

    private void GenerateArrayPoint()
    {
        for (int i = 0; i < this.inflectionPoints.Count; i++)
        {
            this.points[i * 4] = this.inflectionPoints[i].position;

            if (i != 0)
                this.CalculateMiddlePoints(i * 4);
        }

    }

    private void CalculateMiddlePoints(int endIndex)
    {
        int startIndex = endIndex - this.pointEnd;
        Vector3 center = this.GetMiddleWithRandom(this.points[startIndex], this.points[endIndex]);

        this.points[startIndex + this.pointCenter] = center;
        this.points[startIndex + this.pointMiddleLeft] = this.GetMiddleWithRandom(this.points[startIndex], center);
        this.points[startIndex + this.pointMiddleRight] = this.GetMiddleWithRandom(center, this.points[endIndex]);
    }

    private float GetRandomWidthOffset()
    {
        return Random.Range(this.randomWidthOffsetMin, this.randomWidthOffsetMax);
    }

    private Vector3 GetMiddleWithRandom(Vector3 point1, Vector3 point2)
    {
        Vector3 middlePoint = Vector3.Lerp(point1, point2, 0.5f);
        Vector3 randomOffset = new Vector3(
            Random.Range(-this.randomPosOffset, this.randomPosOffset),
            Random.Range(-this.randomPosOffset, this.randomPosOffset),
            Random.Range(-this.randomPosOffset, this.randomPosOffset));

        return middlePoint + randomOffset;
    }

    public void SetListPoint(List<Transform> listTransform)
    {
        this.inflectionPoints.Clear();
        this.inflectionPoints = listTransform;
    }

    public void SetListPoint(List<EnemyController> listEnemyCtrl)
    {
        this.inflectionPoints.Clear();
        for(int i = 0; i < listEnemyCtrl.Count; i++)
        {
            this.inflectionPoints.Add(listEnemyCtrl[i].CenterPoint);
        }
    }
}
