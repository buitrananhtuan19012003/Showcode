using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class SkillCharacterView : CharacterView
    {
        [Tooltip("Text reference to display skill value.")]
        [SerializeField]
        protected Text m_Value;

        public override void Repaint(Player player)
        {
            Skill skill = player as Skill;

            if (this.m_Value != null)
            {
                this.m_Value.text = (skill != null ? skill.CurrentValue.ToString("F1") + "%" : string.Empty);
            }
        }
    }
}