﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LupinrangerPatranger.Graphs
{
    [CustomPropertyDrawer(typeof(FlowGraph),true)]
    public class FlowGraphPropertyDrawer : GraphPropertyDrawer<FlowGraphView>{}
}