using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger
{
    public class AiAttackPlayerState : AiState
    {
        public AiStateID GetID()
        {
            return AiStateID.Attack;
        }

        public void Enter(AiAgent agent)
        {
            agent.weapons.ActivateWeapon();
            agent.weapons.SetTarget(agent.playerTransform);
            agent.navMeshAgent.stoppingDistance = 5f;
            agent.weapons.SetFiring(true);
        }

        public void Update(AiAgent agent)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        public void Exit(AiAgent agent)
        {
            agent.navMeshAgent.stoppingDistance = 0f;
        }
    }
}