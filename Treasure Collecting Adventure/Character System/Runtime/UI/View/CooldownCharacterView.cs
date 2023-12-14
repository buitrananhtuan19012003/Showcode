using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CooldownCharacterView : CharacterView
    {
        /// <summary>
        /// The image to display cooldown.
        /// </summary>
        [Tooltip("The image to display cooldown.")]
        [SerializeField]
        protected Image m_CooldownOverlay;
        /// <summary>
        /// The text to display cooldown.
        /// </summary>
        [Tooltip("The text to display cooldown.")]
        [SerializeField]
        protected Text m_Cooldown;

        protected override void Start()
        {
            if (this.m_CooldownOverlay != null)
                this.m_CooldownOverlay.raycastTarget = false;
        }

        public override void Repaint(Player player)
        {
            if (player != null && player.IsInCooldown)
            {
                if (this.m_Cooldown != null)
                    this.m_Cooldown.text = (player.CooldownDuration - (Time.time - player.CooldownTime)).ToString("f1");

                if (this.m_CooldownOverlay != null)
                    this.m_CooldownOverlay.fillAmount = Mathf.Clamp01(1f - ((Time.time - player.CooldownTime) / player.CooldownDuration));
            }else {
                if (this.m_Cooldown != null)
                    this.m_Cooldown.text = string.Empty;
                if(this.m_CooldownOverlay != null)
                    this.m_CooldownOverlay.fillAmount = 0f;
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}