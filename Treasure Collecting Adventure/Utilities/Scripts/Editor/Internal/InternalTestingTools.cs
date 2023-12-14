using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LupinrangerPatranger
{
    public class InternalTestingTools
    {
        [MenuItem("Tools/Treasure Collecting Adventure/Internal/Delete PlayerPrefs")]
        public static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        [MenuItem("Tools/Treasure Collecting Adventure/Internal/Delete EditorPrefs")]
        public static void DeleteEditorPrefs()
        {
            EditorPrefs.DeleteAll();
        }

        [MenuItem("Tools/Treasure Collecting Adventure/Internal/Recompile Scripts")]
        public static void RecompileScripts()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}