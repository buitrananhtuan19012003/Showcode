﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LupinrangerPatranger.StatSystem.Configuration
{
    [System.Serializable]
    public class Default : Settings
    {
        public override string Name
        {
            get
            {
                return "Default";
            }
        }
        [Header("Debug")]
        public bool debugMessages = true;
    }
}