using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterModifierHandler : MonoBehaviour
    {
        public List<CharacterModifier> modifiers = new List<CharacterModifier>();

        public void ApplyModifiers(Player player)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].Modify(player);
            }
        }

        public void ApplyModifiers(Player[] players)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                for (int j = 0; j < players.Length; j++)
                {
                    modifiers[i].Modify(players[j]);
                }
            }
        }
    }
}