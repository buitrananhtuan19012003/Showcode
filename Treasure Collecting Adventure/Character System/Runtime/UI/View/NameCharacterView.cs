using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class NameCharacterView : CharacterView
    {
        /// <summary>
        /// The text reference to display item name.
        /// </summary>
        [Tooltip("Text reference to display character name.")]
        [InspectorLabel("Name")]
        [SerializeField]
        protected Text m_CharacterName;
        /// <summary>
        /// Should the character name be colored in rarity color?
        /// </summary>
        [Tooltip("Should the name use rarity color?")]
        [SerializeField]
        protected bool m_UseRarityColor = true;

        public override void Repaint(Player player)
        {
            if (this.m_CharacterName != null)
            {
                //Updates the text with item name and rarity color. If this slot is empty, sets the text to empty.
                this.m_CharacterName.text = (player != null ? (this.m_UseRarityColor ? UnityTools.ColorString(player.DisplayName, player.Rarity.Color) : player.DisplayName) : string.Empty);
            }
        }
    }
}