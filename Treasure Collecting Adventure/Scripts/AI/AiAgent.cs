using UnityEngine;
using UnityEngine.AI;

namespace LupinrangerPatranger
{
    public class AiAgent : MonoBehaviour
    {
        public AiStateID initState;
        [HideInInspector]
        public AiStateMachine stateMachine;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public Ragdoll ragdoll;
        [HideInInspector]
        public UIHealthBar healthBar;
        [HideInInspector]
        public AiHealth health;
        [HideInInspector]
        public Transform playerTransform;
        [HideInInspector]
        public AiWeapon weapons;
        [HideInInspector]
        public Animator animator;

        void Start()
        {
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<AiHealth>();
            ragdoll = GetComponent<Ragdoll>();
            healthBar = GetComponentInChildren<UIHealthBar>();
            weapons = GetComponent<AiWeapon>();
            stateMachine = new AiStateMachine(this);
            stateMachine.RegisterState(new AiChasePlayerState());
            stateMachine.RegisterState(new AiDeathState());
            stateMachine.RegisterState(new AiIdleState());
            stateMachine.RegisterState(new AiFindWeaponState());
            stateMachine.RegisterState(new AiAttackPlayerState());
            stateMachine.ChangeState(initState);
        }

        void Update()
        {
            stateMachine.Update();
        }

        public void DisableAll()
        {
            animator.enabled = false;
            navMeshAgent.enabled = false;
            health.enabled = false;
            ragdoll.enabled = false;
            healthBar.enabled = false;
            weapons.enabled = false;
        }
    }
}