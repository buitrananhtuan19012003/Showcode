using LupinrangerPatranger.StatSystem;
using UnityEngine;

namespace LupinrangerPatranger.Graphs
{
    [System.Serializable]
    public abstract class StatNode : FlowNode
    {
        [Input(true, false)]
        public string stat = string.Empty;
        [Output]
        public Stat statValue;

        public StatNode() { }
    }
}