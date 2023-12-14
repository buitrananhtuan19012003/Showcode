using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class CharacterModifierList
    {
        public List<CharacterModifier> modifiers = new List<CharacterModifier>();

        public void Modify(Player player)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].Modify(player);
            }
        }
    }
}