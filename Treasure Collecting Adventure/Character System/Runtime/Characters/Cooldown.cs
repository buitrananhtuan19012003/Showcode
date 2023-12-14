using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem.CharacterActions
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Player")]
    [ComponentMenu("Character System/Cooldown")]
    [System.Serializable]
    public class Cooldown : CharacterAction
    {
        [SerializeField]
        private float m_GlobalCooldown = 0.5f;


        public override ActionStatus OnUpdate()
        {
            CharacterContainer.Cooldown(player, this.m_GlobalCooldown);
            return ActionStatus.Success;
        }

    }
}
