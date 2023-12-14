using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public bool Velocity;
    public bool DesiredVelocity;
    public bool Path;

    private void OnDrawGizmos()
    {
        if (this.navMeshAgent == null) return;

        if (this.Velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.navMeshAgent.transform.position, this.navMeshAgent.transform.position + this.navMeshAgent.velocity);
        }        
        
        if (this.DesiredVelocity)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.navMeshAgent.transform.position, this.navMeshAgent.transform.position + this.navMeshAgent.desiredVelocity);
        }

        if (this.Path)
        {
            Gizmos.color = Color.black;
            NavMeshPath path = this.navMeshAgent.path;
            Vector3 prevCorner = this.navMeshAgent.transform.position;
            foreach (Vector3 corner in path.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
