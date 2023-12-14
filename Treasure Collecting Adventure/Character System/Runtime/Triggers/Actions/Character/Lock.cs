using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Character")]
    [ComponentMenu("Character System/Lock")]
    public class Lock : Action
    {
        [Tooltip("The name of the window to lock.")]
        [SerializeField]
        private string m_WindowName = "Loot";
        [SerializeField]
        private bool m_State = true;
        private CharacterContainer m_CharacterContainer;

        public override void OnStart()
        {
            this.m_CharacterContainer = WidgetUtility.Find<CharacterContainer>(this.m_WindowName);
        }

        public override ActionStatus OnUpdate()
        {
            if (this.m_CharacterContainer == null)
            {
                Debug.LogWarning("Missing window " + this.m_WindowName + " in scene!");
                return ActionStatus.Failure;
            }

            this.m_CharacterContainer.Lock(this.m_State);
           
            return ActionStatus.Success;
        }
    }
}
