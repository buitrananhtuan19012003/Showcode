using UnityEngine;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterContainerPopulator : MonoBehaviour
    {
        [SerializeField]
        protected List<Entry> m_Entries = new List<Entry>();

        protected virtual void Start()
        {
            if (!CharacterManager.HasSavedData())
            {
                for (int i = 0; i < this.m_Entries.Count; i++)
                {
                    CharacterContainer container = WidgetUtility.Find<CharacterContainer>(this.m_Entries[i].name);
                    if (container != null)
                    {
                        Player[] groupPlayers = CharacterManager.CreateInstances(this.m_Entries[i].group);
                        for (int j = 0; j < groupPlayers.Length; j++)
                        {
                            container.StackOrAdd(groupPlayers[j]);
                        }
                    }
                }
            }
        }

        [System.Serializable]
        public class Entry
        {
            public string name = "Character";
            public CharacterGroup group;
        }
    }
}