using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger
{
    [CreateAssetMenu(fileName = "ActionTemplate", menuName = "Treasure Collecting Adventure/Triggers/Action Template")]
    [System.Serializable]
    public class ActionTemplate : ScriptableObject
    {
        [SerializeReference]
        public List<Action> actions= new List<Action>();
    }
}