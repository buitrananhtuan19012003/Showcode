using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.Graphs
{
    [CreateAssetMenu(fileName = "Formula", menuName = "Treasure Collecting Adventure/Graphs/Formula")]
    [System.Serializable]
    public class Formula : ScriptableObject, IGraphProvider
    {
        [SerializeField]
        protected FormulaGraph m_Graph;

        public Graph GetGraph()
        {
            return this.m_Graph;
        }

        public static implicit operator float(Formula formula)
        {
            FormulaOutput output = formula.GetGraph().nodes.Find(x => x.GetType() == typeof(FormulaOutput)) as FormulaOutput;
            return output.GetInputValue<float>("result", output.result);
        }
    }
}