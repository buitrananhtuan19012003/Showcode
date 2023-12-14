using UnityEditor;
using UnityEngine;

public class SaiMonoBehaviour : MonoBehaviour
{
    [ContextMenu("Load Component")]
    public void ContextMenuLoadComponent()
    {
        this.LoadComponent();
    }

    protected virtual void Awake()
    {
        this.LoadComponent();
    }

    protected virtual void Reset()
    {
        //For overrite
    }

    protected virtual void LoadComponent()
    {
        //For overrite
    }

}

//[CustomEditor(typeof(SaiMonoBehaviour), true)]
//public class SaiCustomInscpector : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();
//        SaiMonoBehaviour saiMono = (SaiMonoBehaviour)target;
//        if (GUILayout.Button("Load Component"))
//        {
//            saiMono.ContextMenuLoadComponent();
//        }

//        this.AddMoreButton();
//        DrawDefaultInspector();
//    }

//    public virtual void AddMoreButton()
//    {
//        //for ovrride
//    }
//}
