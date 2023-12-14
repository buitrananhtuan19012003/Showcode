using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [CreateAssetMenu(fileName = "SimpleSkillModifier", menuName = "Treasure Collecting Adventure/Character System/Modifiers/Skill")]
    public class SkillModifier : ScriptableObject, IModifier<Skill>
    {
        [SerializeField]
        protected AnimationCurve m_Chance;
        [SerializeField]
        protected AnimationCurve m_Gain;

        public void Modify(Skill player)
        {
            float currentValue = player.CurrentValue;
            float chance = this.m_Chance.Evaluate(currentValue / 100f) * 100f;
            float p = Random.Range(0f, 100f);

            if (chance > p)
            {
                float gainValue = this.m_Gain.Evaluate(currentValue / 100f);
                player.CurrentValue = player.CurrentValue + gainValue;

                CharacterManager.Notifications.skillGain.Show(player.DisplayName, gainValue.ToString("F1"), player.CurrentValue.ToString("F1"));
            }
        }
    }
}