using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public abstract class CharacterModifier : ScriptableObject, IModifier<Player>
    {
        public abstract void Modify(Player player);
     
    }
}