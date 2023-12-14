using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger
{
    public class AiHealth : Health
    {
        private float blinkDuration;
        private UIHealthBar healthBar;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Ragdoll ragdoll;

        private float timeDestroyAI;

        private AiAgent aiAgent;

        protected override void OnStart()
        {
            aiAgent = GetComponent<AiAgent>();
            if (DataManager.HasInstance)
            {
                maxHealth = DataManager.Instance.GlobalConfig.aiMaxHealth;
                blinkDuration = DataManager.Instance.GlobalConfig.blinkDuration;
                timeDestroyAI = DataManager.Instance.GlobalConfig.timeDestroyAI;
            }
            currentHealth = maxHealth;
            ragdoll = GetComponent<Ragdoll>();
            healthBar = GetComponentInChildren<UIHealthBar>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        protected override void OnDamage(Vector3 direction, Rigidbody rigidbody)
        {
            if (healthBar != null)
            {
                healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
            }
            StartCoroutine(EnemyFlash());
        }

        protected override void OnDeath(Vector3 direction, Rigidbody rigidbody)
        {
            AiDeathState deathState = aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState;
            deathState.direction = direction;
            deathState.rigidbody = rigidbody;
            aiAgent.stateMachine.ChangeState(AiStateID.Death);
        }

        private IEnumerator EnemyFlash()
        {
            skinnedMeshRenderer.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(blinkDuration);
            skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
            StopCoroutine(nameof(EnemyFlash));
        }

        public void DestroyWhenDeath()
        {
            Destroy(this.gameObject, timeDestroyAI);
        }
    }
}