using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.InventorySystem
{
    [System.Serializable]
    public abstract class ItemModifier : ScriptableObject, IModifier<Item>
    {
        public abstract void Modify(Item item);
     
    }
}