using UnityEngine;

public abstract class EnemyAbstract : SaiMonoBehaviour
{
    [SerializeField] protected EnemyController enemyCtrl;

    public EnemyController EnemyCtrl { get => this.enemyCtrl; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.enemyCtrl == null)
            this.enemyCtrl = transform.parent.GetComponent<EnemyController>();
    }
}
