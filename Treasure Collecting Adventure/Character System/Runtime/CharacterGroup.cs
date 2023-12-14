using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class CharacterGroup : ScriptableObject, INameable
    {
        [SerializeField]
        private string m_GroupName = "New Group";
        public string Name
        {
            get { return this.m_GroupName; }
            set { this.m_GroupName = value; }
        }

        [SerializeField]
        private Player[] m_Players = new Player[0];
        public Player[] Players
        {
            get
            {
                return this.m_Players;
            }
        }

        [SerializeField]
        protected int[] m_Amounts = new int[0];
        public int[] Amounts
        {
            get { return this.m_Amounts; }
        }

        [SerializeField]
        protected List<CharacterModifierList> m_Modifiers = new List<CharacterModifierList>();

        public List<CharacterModifierList> Modifiers
        {
            get { return this.m_Modifiers; }
        }
    }
}