using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Character")]
    [ComponentMenu("Character System/Show Window")]
    public class ShowWindow : Action, ITriggerUnUsedHandler
    {
        [Tooltip("The name of the window to show.")]
        [SerializeField]
        private string m_WindowName = "Loot";
        [SerializeField]
        private bool m_DestroyWhenEmpty = true;

        private CharacterContainer m_CharacterContainer;
        private ActionStatus m_WindowStatus = ActionStatus.Inactive;

        public override void OnSequenceStart()
        {
            this.m_WindowStatus = ActionStatus.Inactive;
            this.m_CharacterContainer = WidgetUtility.Find<CharacterContainer>(this.m_WindowName);
            if (this.m_CharacterContainer != null)
            {
                this.m_CharacterContainer.RegisterListener("OnClose", (CallbackEventData eventData) => { this.m_WindowStatus = ActionStatus.Success; });
            }
        }

        public void OnTriggerUnUsed(GameObject player)
        {
            if (m_CharacterContainer != null)
            {
                this.m_CharacterContainer.Close();
                Trigger.currentUsedWindow = null;
            }
        }

        public override ActionStatus OnUpdate()
        {
            if (this.m_CharacterContainer == null)
            {
                Debug.LogWarning("Missing window " + this.m_WindowName + " in scene!");
                return ActionStatus.Failure;
            }

            if (this.m_WindowStatus == ActionStatus.Inactive)
            {
                Trigger.currentUsedWindow = this.m_CharacterContainer;
                if (this.m_CharacterContainer == null)
                {
                    this.m_CharacterContainer.Show();
                }
                else
                {
                    //this.m_CharacterContainer.Collection = this.m_CharacterCollection;
                    this.m_CharacterContainer.Show();

                }
                this.m_WindowStatus = ActionStatus.Running;
            }
            return this.m_WindowStatus;
        }
    }
}