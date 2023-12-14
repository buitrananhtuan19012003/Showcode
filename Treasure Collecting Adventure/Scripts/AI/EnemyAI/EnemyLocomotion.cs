using UnityEngine;

public class EnemyLocomotion : EnemyAbstract
{
    private float moveState = 0;

    private void Update()
    {
        if (this.enemyCtrl.NavMeshAgent.velocity.magnitude > 0)
        {
            this.moveState += Time.deltaTime * 1000f;
            this.moveState = Mathf.Clamp(this.moveState, 0, 1);
        }
        else
        {
            this.moveState -= Time.deltaTime * 1000f;
            this.moveState = Mathf.Clamp(this.moveState, 0, 1);
        }
        this.enemyCtrl.Animator.SetFloat("MoveState", this.moveState);
    }
}
