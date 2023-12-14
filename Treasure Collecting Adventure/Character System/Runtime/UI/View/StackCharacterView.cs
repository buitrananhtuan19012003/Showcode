using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class StackCharacterView : CharacterView
    {
        /// <summary>
        ///The text to display item stack.
        /// </summary>
        [Tooltip("The text reference to display item stack.")]
        [SerializeField]
        protected Text m_Stack;

        protected override void Start()
        {
            if (this.m_Stack != null)
                this.m_Stack.raycastTarget = false;
        }

        public override void Repaint(Player player)
        {
            if (this.m_Stack != null)
            {
                if (player != null && player.MaxStack > 1)
                {
                    //Updates the stack and enables it.
                    this.m_Stack.text = player.Stack.ToString();
                    this.m_Stack.enabled = true;
                }
                else
                {
                    //If there is no item in this slot, disable stack field
                    this.m_Stack.enabled = false;
                }
            }
        }
    }
}