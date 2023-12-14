using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TextCore.Text;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class Currency : Player
    {
        public override int MaxStack
        {
            get { return int.MaxValue; }
        }

        public CurrencyConversion[] currencyConversions;
    }
}