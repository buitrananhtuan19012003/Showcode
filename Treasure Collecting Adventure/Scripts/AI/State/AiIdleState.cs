using UnityEngine;

namespace LupinrangerPatranger
{
    public class AiIdleState : AiState
    {
        private Vector3 playerDirection;
        private float maxSightDistance;

        public AiStateID GetID()
        {
            return AiStateID.Idle;
        }

        public void Enter(AiAgent agent)
        {
            if (DataManager.HasInstance)
            {
                maxSightDistance = DataManager.Instance.GlobalConfig.maxSight;
            }
        }

        public void Exit(AiAgent agent)
        {

        }

        public void Update(AiAgent agent)
        {
            playerDirection = agent.playerTransform.position - agent.transform.position;

            if (playerDirection.magnitude > maxSightDistance)
            {
                return;
            }

            Vector3 agentDirection = agent.transform.forward;

            playerDirection.Normalize();

            float dotProduct = Vector3.Dot(playerDirection, agentDirection);

            if (dotProduct >= 0)
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
            }
        }
    }
}