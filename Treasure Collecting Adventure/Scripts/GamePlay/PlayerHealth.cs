using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LupinrangerPatranger
{
    public class PlayerHealth : Health
    {
        public Volume postProcessing;
        [Range(0, 1)]
        public float maxValue;
        private Ragdoll ragdoll;
        //private ActiveWeapon activeWeapon;
        private CharacterAiming aiming;

        protected override void OnStart()
        {
            if (DataManager.HasInstance)
            {
                maxHealth = DataManager.Instance.GlobalConfig.playerMaxHealth;
            }
            currentHealth = maxHealth;
            ragdoll = GetComponent<Ragdoll>();
            //activeWeapon = GetComponent<ActiveWeapon>();
            aiming = GetComponent<CharacterAiming>();
        }

        protected override void OnDamage(Vector3 direction, Rigidbody rigidbody)
        {
            //if (postProcessing.profile.TryGet(out Vignette vignette))
            //{
            //    float percent = 1.0f - (currentHealth / maxHealth);
            //    vignette.intensity.value = percent * maxValue;
            //}
        }

        protected override void OnHealth(float amount)
        {
            //if (postProcessing.profile.TryGet(out Vignette vignette))
            //{
            //    float percent = 1.0f - (currentHealth / maxHealth);
            //    vignette.intensity.value = percent * maxValue;
            //}
        }

        protected override void OnDeath(Vector3 direction, Rigidbody rigidbody)
        {
            ragdoll.ActiveRagdoll();
            direction.y = 1f;
            ragdoll.ApplyForce(direction, rigidbody);
            //activeWeapon.DropWeapon();
            aiming.enabled = false;
            CameraManager.Instance.EnableKillCam();
        }
    }
}