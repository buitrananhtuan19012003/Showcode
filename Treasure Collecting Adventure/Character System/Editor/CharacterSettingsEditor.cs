using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LupinrangerPatranger.CharacterSystem.Configuration
{
    [System.Serializable]
    public class CharacterSettingsEditor : ScriptableObjectCollectionEditor<Settings>
    {

        public override string ToolbarName
        {
            get { return "Settings"; }
        }

        protected override bool CanAdd => false;

        protected override bool CanRemove => false;

        protected override bool CanDuplicate => false;

        public CharacterSettingsEditor(UnityEngine.Object target, List<Settings> players) : base(target, players)
        {
            this.target = target;
            this.players = players;


            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Settings).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                //if (Players.Where(x => x.GetType() == type).FirstOrDefault() == null)
                //{
                //    CreatePlayer(type);
                //}
            }
        }

        protected override bool MatchesSearch(Settings player, string search)
        {
            return (player.Name.ToLower().Contains(search.ToLower()) || search.ToLower() == player.GetType().Name.ToLower());
        }

        protected override string ButtonLabel(int index, Settings player)
        {
            return "  " + GetSidebarLabel(player);
        }
    }
}