using UnityEngine;
using UnityEngine.AI;

namespace LupinrangerPatranger
{
    public class AiChasePlayerState : AiState
    {
        private float timer = 0f;
        private float maxDistance;
        private float maxTime;

        public AiStateID GetID()
        {
            return AiStateID.ChasePlayer;
        }

        public void Enter(AiAgent agent)
        {
            if (DataManager.HasInstance)
            {
                maxDistance = DataManager.Instance.GlobalConfig.maxDistance;
                maxTime = DataManager.Instance.GlobalConfig.maxTime;
            }
        }

        public void Update(AiAgent agent)
        {
            if (!agent.navMeshAgent.enabled)
            {
                return;
            }

            timer -= Time.deltaTime;

            if (!agent.navMeshAgent.hasPath)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }

            if (timer < 0f)
            {
                Vector3 direction = agent.playerTransform.position - agent.navMeshAgent.destination;
                direction.y = 0;
                if (direction.sqrMagnitude > maxDistance * maxDistance)
                {
                    if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                    {
                        agent.navMeshAgent.destination = agent.playerTransform.position;
                    }
                }
                timer = maxTime;
            }
        }

        public void Exit(AiAgent agent)
        {

        }
    }
}