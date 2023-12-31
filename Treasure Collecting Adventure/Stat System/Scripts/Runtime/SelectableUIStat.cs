﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.StatSystem
{
    public class SelectableUIStat : UIStat
    {
        protected override StatsHandler GetStatsHandler()
        {
            if (SelectableObject.current != null)
            {
                return SelectableObject.current.GetComponent<StatsHandler>();

            }
            return null;
        }
    }
}