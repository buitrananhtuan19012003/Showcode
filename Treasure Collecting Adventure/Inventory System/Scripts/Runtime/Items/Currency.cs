﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LupinrangerPatranger.InventorySystem
{
    [System.Serializable]
    public class Currency : Item
    {
        public override int MaxStack
        {
            get{return int.MaxValue;}
        }

        public CurrencyConversion[] currencyConversions;

       
    }
}