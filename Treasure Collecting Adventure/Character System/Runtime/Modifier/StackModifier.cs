using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [CreateAssetMenu(fileName = "SimpleStackModifier", menuName = "Treasure Collecting Adventure/Character System/Modifiers/Stack")]
    [System.Serializable]
    public class StackModifier : CharacterModifier
    {
        [SerializeField]
        protected int m_Min = 1;
        [SerializeField]
        protected int m_Max = 2;

        public override void Modify(Player player)
        {
            int stack = Random.Range(this.m_Min, this.m_Max);
            player.Stack = stack;
        }
    }
}