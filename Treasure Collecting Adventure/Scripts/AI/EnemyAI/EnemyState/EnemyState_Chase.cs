using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : MonoBehaviour, IEnemyState
{
    private Enemy_AIController enemyAiCtrl;
    private float attackCD = 1.5f;
    //private float attackRange = 2;
    private float timer = 0;
    private float timePassed = 0;

    //private bool useMovementPrediction = false;
    //private float movementPredictionThreshold = 0;
    //private float movementPredictionTime = 1;

    public EnemyState_Chase(Enemy_AIController controller)
    {
        this.enemyAiCtrl = controller;
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.Chase;
    }

    public void Enter()
    {
        this.timePassed = this.attackCD;
        this.timer = 0;
    }

    public void Update()
    {
        if (!this.enemyAiCtrl.EnemyCtrl.NavMeshAgent.enabled) return;
        if (this.enemyAiCtrl.EnemyCtrl.CurInfoScanTarget == null) return;

        Vector3 followPos = this.enemyAiCtrl.EnemyCtrl.FollowPos;
        this.FollowTarget(followPos);
        this.AttackTarget(followPos);
        //if (this.enemyAiCtrl.EnemyCtrl.Target != null)
        //{
        //    Vector3 followPos = this.enemyAiCtrl.EnemyCtrl.FollowPos;
        //    Debug.Log("Start Chase");
        //    this.FollowTarget(followPos);
        //    this.AttackTarget(followPos);
        //}
        //else
        //{
        //    this.enemyAiCtrl.EnemyCtrl.Animator.Rebind();
        //    this.enemyAiCtrl.EnemySM.ChangeState(EnemyStateId.Idle);
        //}

    }

    public void FixedUpdate()
    {
        if (!this.enemyAiCtrl.EnemyCtrl.NavMeshAgent.enabled) return;

    }

    public void Exit()
    {

    }

    private void FollowTarget(Vector3 followPos)
    {
        Transform target = this.enemyAiCtrl.EnemyCtrl.CurInfoScanTarget.GetCenterPoint();
        this.enemyAiCtrl.EnemyCtrl.transform.LookAt(new Vector3(target.position.x, this.enemyAiCtrl.EnemyCtrl.transform.position.y, target.position.z));
        //this.enemyAiCtrl.EnemyCtrl.NavMeshAgent.SetDestination(followPos);

        this.timer += Time.deltaTime;
        if (this.timer > this.enemyAiCtrl.AiConfig.MaxTime)
        {
            Vector3 direction = target.position - this.enemyAiCtrl.EnemyCtrl.transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > this.enemyAiCtrl.AiConfig.MaxDistance * this.enemyAiCtrl.AiConfig.MaxDistance)
            {
                this.enemyAiCtrl.EnemyCtrl.NavMeshAgent.SetDestination(followPos);
                //if (!this.useMovementPrediction)
                //    navMeshAgent.SetDestination(target.position);
                //else
                //    navMeshAgent.SetDestination(target.position + PlayerCtrl.Instance.PlayerLocomotion.AverageVelocity * this.movementPredictionTime);
                this.timer = 0;
            }
        }
    }

    private void AttackTarget(Vector3 followPos)
    {
        this.timePassed += Time.deltaTime;
        if (this.enemyAiCtrl.EnemyCtrl.CurInfoScanTarget == null) return;
        if (this.timePassed > this.attackCD)
        {
            if (Vector3.Distance(followPos, this.enemyAiCtrl.EnemyCtrl.transform.position)
                <= this.enemyAiCtrl.EnemyCtrl.EnemyData.AttackRange)
            {
                this.enemyAiCtrl.EnemyCtrl.Animator.SetInteger("RandomAttack", Random.Range(0, 3));
                this.enemyAiCtrl.EnemyCtrl.Animator.SetTrigger("Attack");
                this.timePassed = 0;
            }
        }
    }
}
