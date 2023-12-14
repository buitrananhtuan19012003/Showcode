using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger
{
    [System.Serializable]
    public class Actions 
    {
        [SerializeReference]
        public List<Action> actions = new List<Action>();
    }
}