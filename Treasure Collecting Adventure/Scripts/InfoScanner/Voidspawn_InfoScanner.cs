using UnityEngine;

public class Voidspawn_InfoScanner : SaiMonoBehaviour, IInfoScanner
{
    [SerializeField] private EnemyController enemyCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.enemyCtrl == null)
            this.enemyCtrl = GetComponent<EnemyController>();
    }

    public FactionType GetFactionType()
    {
        return FactionType.Voidspawn;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Transform GetCenterPoint()
    {
        return this.enemyCtrl.CenterPoint;
    }

    public string GetTargetName()
    {
        return gameObject.name;
    }

    public int GetTargetLevel()
    {
        return 1;
    }

    public IHealth GetHealth()
    {
        return this.enemyCtrl.EnemyHealth;
    }

    public bool CanScan()
    {
        return !this.enemyCtrl.EnemyHealth.IsDeath();
    }
}
