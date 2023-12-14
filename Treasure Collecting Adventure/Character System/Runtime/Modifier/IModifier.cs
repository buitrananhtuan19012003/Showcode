using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public interface IModifier<T>
    {
        void Modify(T player);
    }
}
