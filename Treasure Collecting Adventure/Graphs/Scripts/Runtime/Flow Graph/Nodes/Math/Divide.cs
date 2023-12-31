﻿using UnityEngine;

namespace LupinrangerPatranger.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Add")]
    [NodeStyle("Icons/Divide",false,"Math")]
    public class Divide : FlowNode
    {
        [Input(false,true)]
        public float a;
        [Input(false, true)]
        public float b;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return GetInputValue("a", a) / GetInputValue("b", b);
        }
    }
}