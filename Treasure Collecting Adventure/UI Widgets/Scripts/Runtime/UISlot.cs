﻿using UnityEngine;

namespace LupinrangerPatranger.UIWidgets
{
    public class UISlot<T> : MonoBehaviour where T : class
    {
        private UIContainer<T> m_Container;
        /// <summary>
        /// The item container that holds this slot
        /// </summary>
        public UIContainer<T> Container
        {
            get { return this.m_Container; }
            set { this.m_Container = value; }
        }

        private int m_Index = -1;
        /// <summary>
        /// Index of item container
        /// </summary>
        public int Index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        private T m_Item;
        private T m_Character;
        /// <summary>
        /// The item this slot is holding
        /// </summary>
        public virtual T ObservedItem
        {
            get
            {
                return this.m_Item;
            }
            set
            {
                this.m_Item = value;
                Repaint();
            }
        }
        public virtual T ObservedCharacter
        {
            get
            {
                return this.m_Character;
            }
            set
            {
                this.m_Character = value;
                Repaint();
            }
        }

        /// <summary>
        /// Checks if the slot is empty ObservedItem == null
        /// </summary>
        public bool IsEmpty
        {
            get { return ObservedItem == null; }
        }

        /// <summary>
        /// Repaint slot visuals with item information
        /// </summary>
        public virtual void Repaint()
        {
        }

        /// <summary>
        /// Can the item be added to this slot. This does not check if the slot is empty.
        /// </summary>
        /// <param name="item">The item to test adding.</param>
        /// <returns>Returns true if the item can be added.</returns>
        public virtual bool CanAddItem(T item)
        {
            return true;
        }
        public virtual bool CanAddCharacter(T character)
        {
            return true;
        }
    }
}