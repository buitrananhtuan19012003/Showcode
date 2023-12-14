using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class IconCharacterView : CharacterView
    {
        /// <summary>
        /// The Image to display player icon.
        /// </summary>
        [Tooltip("Image reference to display the icon.")]
        [SerializeField]
        protected Image m_Ícon;

        public override void Repaint(Player player)
        {
            if (this.m_Ícon != null)
            {
                if (player != null)
                {
                    //Updates the icon and enables it.
                    this.m_Ícon.overrideSprite = player.Icon;
                    this.m_Ícon.enabled = true;
                }
                else
                {
                    //If there is no item in this slot, disable icon
                    this.m_Ícon.enabled = false;
                }
            }
        }
    }
}