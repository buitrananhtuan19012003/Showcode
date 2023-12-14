using UnityEngine;

public class DroneState_Attack : IDroneState
{
    private Drone_AiCtrl droneAiCtrl;

    public DroneState_Attack(Drone_AiCtrl controller)
    {
        this.droneAiCtrl = controller;
    }

    public DroneStateId GetId()
    {
        return DroneStateId.Attack;
    }

    public void Enter()
    {
        DroneCtrl droneCtrl = this.droneAiCtrl.DroneCtrl;
        droneCtrl.Agent.stoppingDistance = droneCtrl.MaxDistanceFromEnemy;
    }

    public void Exit()
    {
        ParticleSystem attackFx = this.droneAiCtrl.DroneCtrl.LaserFx;
        if (attackFx.isPlaying)
        {
            attackFx.Stop();
        }
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
        this.ChaseEnemy();
        this.droneAiCtrl.DroneCtrl.ShotLaser();
    }

    private void ChaseEnemy()
    {
        DroneCtrl droneCtrl = this.droneAiCtrl.DroneCtrl;
        if (droneCtrl.TargetFollow != null)
        {
            IInfoScanner closestTarget = this.droneAiCtrl.DroneCtrl.TargetInfoScanner;
            if (closestTarget == null) return;

            droneCtrl.Agent.SetDestination(closestTarget.GetCenterPoint().transform.position);

            Quaternion targetRotation = Quaternion.LookRotation(closestTarget.GetCenterPoint().position - droneCtrl.transform.position);
            droneCtrl.transform.rotation = Quaternion.Slerp(droneCtrl.transform.rotation, targetRotation, Time.fixedDeltaTime * droneCtrl.RotationSpeed);
        }
    }

    //private void FollowPlayer()
    //{
    //    DroneCtrl droneCtrl = this.droneAiCtrl.DroneCtrl;
    //    if (droneCtrl.TargetFollow != null)
    //    {
    //        droneCtrl.Agent.SetDestination(droneCtrl.TargetFollow.transform.position);

    //        if (droneCtrl.Agent.remainingDistance > 5)
    //        {
    //            droneCtrl.Agent.speed = 7;
    //        }
    //        else
    //        {
    //            droneCtrl.Agent.speed = 3;
    //        }
    //    }
    //}
    //private void LookAtEnemy()
    //{
    //    DroneCtrl droneCtrl = this.droneAiCtrl.DroneCtrl;
    //    if (droneCtrl.TargetFollow != null)
    //    {
    //        Transform closestEnemy = droneCtrl.DetectEnemy.FindClosest();
    //        if (closestEnemy == null) return;

    //        Quaternion targetRotation = Quaternion.LookRotation(closestEnemy.position - droneCtrl.transform.position);
    //        droneCtrl.transform.rotation = Quaternion.Slerp(droneCtrl.transform.rotation, targetRotation, Time.fixedDeltaTime * droneCtrl.RotationSpeed);
    //    }
    //}
}
