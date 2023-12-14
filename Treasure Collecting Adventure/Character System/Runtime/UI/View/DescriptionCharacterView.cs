using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class DescriptionCharacterView : CharacterView
    {
        /// <summary>
        /// The text reference to display character description.
        /// </summary>
        [Tooltip("The text reference to display character description")]
        [SerializeField]
        protected Text m_Description;

        public override void Repaint(Player player)
        {
            if (this.m_Description != null)
            {
                this.m_Description.text = (player != null ? player.Description : string.Empty);
            }
        }
    }
}