using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LupinrangerPatranger.StatSystem
{
    public static class StatSystemMenu
    {
        [MenuItem("Tools/Treasure Collecting Adventure/Stat System/Editor", false, 0)]
        private static void OpenItemEditor()
        {
            StatSystemEditor.ShowWindow();
        }

		[MenuItem("Tools/Treasure Collecting Adventure/Stat System/Create Stats Manager", false, 1)]
		private static void CreateStatManager()
		{
			GameObject go = new GameObject("Stats Manager");
			go.AddComponent<StatsManager>();
			Selection.activeGameObject = go;
		}

		[MenuItem("Tools/Treasure Collecting Adventure/Stat System/Create Stats Manager", true)]
		private static bool ValidateCreateStatusSystem()
		{
			return GameObject.FindObjectOfType<StatsManager>() == null;
		}
	}
}