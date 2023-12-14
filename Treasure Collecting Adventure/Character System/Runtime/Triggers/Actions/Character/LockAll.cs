using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Player")]
    [ComponentMenu("Character System/Lock All")]
    public class LockAll : Action
    {
        [SerializeField]
        private bool m_State = true;

        public override ActionStatus OnUpdate()
        {
            CharacterContainer[] containers = GameObject.FindObjectsOfType<CharacterContainer>();
            for (int i = 0; i < containers.Length; i++)
            {
                containers[i].Lock(this.m_State);
            }

            return ActionStatus.Success;
        }
    }
}
