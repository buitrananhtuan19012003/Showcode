using UnityEngine;

namespace LupinrangerPatranger
{
    public class AiFindWeaponState : AiState
    {
        public AiStateID GetID()
        {
            return AiStateID.FindWeapon;
        }

        public void Enter(AiAgent agent)
        {
            //WeaponPickup pickup = FindClosetWeapon(agent);
            //agent.navMeshAgent.destination = pickup.transform.position;
            agent.navMeshAgent.speed = DataManager.Instance.GlobalConfig.pickupWeaponSpeed;
        }

        public void Update(AiAgent agent)
        {
            if (agent.weapons.HasWeapon())
            {
                agent.stateMachine.ChangeState(AiStateID.Attack);
            }
        }

        public void Exit(AiAgent agent)
        {

        }

        //private WeaponPickup FindClosetWeapon(AiAgent agent)
        //{
        //    WeaponPickup[] weapons = Object.FindObjectsOfType<WeaponPickup>();
        //    WeaponPickup closetWeapon = null;
        //    float closetDistance = float.MaxValue;
        //    foreach (var weapon in weapons)
        //    {
        //        float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
        //        if (distanceToWeapon < closetDistance)
        //        {
        //            closetDistance = distanceToWeapon;
        //            closetWeapon = weapon;
        //        }
        //    }
        //    return closetWeapon;
        //}
    }
}