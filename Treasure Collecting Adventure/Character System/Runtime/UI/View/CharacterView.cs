using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public abstract class CharacterView : MonoBehaviour
    {
        protected virtual void Start() { }
        public abstract void Repaint(Player player);
        public virtual bool RequiresConstantRepaint() { return false; }
    }
}