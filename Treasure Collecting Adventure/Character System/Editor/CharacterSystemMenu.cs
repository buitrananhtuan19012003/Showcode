using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LupinrangerPatranger.CharacterSystem
{
	public static class CharacterSystemMenu
    {
		private const string componentToolbarMenu = "Tools/Treasure Collecting Adventure/Character System/Components/";

		[MenuItem ("Tools/Treasure Collecting Adventure/Character System/Editor", false, 0)]
		private static void OpenItemEditor ()
		{
            CharacterSystemEditor.ShowWindow ();
		}

		[MenuItem("Tools/Treasure Collecting Adventure/Character System/Merge Database", false, 1)]
		private static void OpenMergeDatabaseEditor()
		{
			MergeDatabaseEditor.ShowWindow();
		}

		[MenuItem("Tools/Treasure Collecting Adventure/Character System/Item Reference Updater", false, 2)]
		private static void OpenItemReferenceEditor()
		{
            CharacterReferenceEditor.ShowWindow();
		}

		[MenuItem ("Tools/Treasure Collecting Adventure/Character System/Create Character Manager", false, 3)]
		private static void CreateCharacterManager()
		{
			GameObject go = new GameObject ("Character Manager");
			go.AddComponent<CharacterManager> ();
			Selection.activeGameObject = go;
		}

		[MenuItem ("Tools/Treasure Collecting Adventure/Character System/Create Character Manager", true)]
		static bool ValidateCreateInventoryManager()
		{
			return GameObject.FindObjectOfType<CharacterManager> () == null;
		}
	}
}