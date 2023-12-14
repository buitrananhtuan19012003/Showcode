using UnityEngine;

public class EnemyState_Idle : IEnemyState
{
    private Enemy_AIController enemyAiCtrl;

    public EnemyState_Idle(Enemy_AIController controller)
    {
        this.enemyAiCtrl = controller;
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.Idle;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        DebuffsType curDebuff = this.enemyAiCtrl.EnemyCtrl.EnemyDebuffs.CurDebuff;
        if (curDebuff == DebuffsType.None)
        {
            DetectTarget detectTarget = this.enemyAiCtrl.EnemyCtrl.DetectTarget;
            if (detectTarget.IsDetectTarget() && this.enemyAiCtrl.EnemyCtrl.CurInfoScanTarget != null)
            {
                Vector3 targetDirection = this.enemyAiCtrl.EnemyCtrl.CurInfoScanTarget.GetCenterPoint().position - this.enemyAiCtrl.EnemyCtrl.transform.position;
                targetDirection.Normalize();
                Vector3 transformDirection = this.enemyAiCtrl.EnemyCtrl.transform.forward;

                float dotProduct = Vector3.Dot(targetDirection, transformDirection);
                if (dotProduct >= 0)
                {
                    this.enemyAiCtrl.EnemySM.ChangeState(EnemyStateId.Chase);
                }
            }
        }

        else if (curDebuff == DebuffsType.Electrocuted)
        {

        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
