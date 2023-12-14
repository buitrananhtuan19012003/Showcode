using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class CharacterGeneratorData
    {
        public Player player;
        public int minStack = 1;
        public int maxStack = 1;
       // public float propertyRandomizer = 0.15f;
        [Range(0f, 1f)]
        public float chance = 1.0f;
        public CharacterModifierList modifiers;
    }
}