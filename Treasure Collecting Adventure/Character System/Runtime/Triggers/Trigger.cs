using UnityEngine;
using UnityEngine.EventSystems;


namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [DisallowMultipleComponent]
    public class Trigger : BehaviorTrigger
    {
        public enum FailureCause
        {
            Unknown,
            FurtherAction, // Requires a user action(Select an item for crafting)
            NotEnoughCurrency, // Player does not have enough money 
            Remove, // No longer exists (Item was removed)
            ContainerFull, // Given container is full
            InUse, // Something is already in use (Player is already crafting something-> no start possible)
            Requirement // Missing requirements for this action( Missing ingredient to craft)
        }

        public override PlayerInfo PlayerInfo => CharacterManager.current.PlayerInfo;

        public static CharacterContainer currentUsedWindow;
        protected delegate void ItemEventFunction<T>(T handler, Player player, GameObject players);
        protected delegate void FailureItemEventFunction<T>(T handler, Player player, GameObject players, FailureCause failureCause);

        //Deprecate use SendMessage with Use
        //used for UI Button reference
        public void StartUse()
        {
            Use();
        }


        public void StartUse(CharacterContainer window)
        {
            if (window.IsVisible)
            {
                Trigger.currentUsedWindow = window;
                Use();
            }
        }

        public void StopUse()
        {
            InUse = false;
        }

        public virtual bool OverrideUse(Slot slot, Player player)
        {
            return false;
        }

        protected override void DisplayInUse()
        {
            CharacterManager.Notifications.inUse.Show();
        }

        protected override void DisplayOutOfRange()
        {
            CharacterManager.Notifications.toFarAway.Show();
        }

        protected void ExecuteEvent<T>(ItemEventFunction<T> func, Player player, bool includeDisabled = false) where T : ITriggerEventHandler
        {
            for (int i = 0; i < this.m_TriggerEvents.Length; i++)
            {
                ITriggerEventHandler handler = this.m_TriggerEvents[i];
                if (ShouldSendEvent<T>(handler, includeDisabled))
                {
                    func.Invoke((T)handler, player, PlayerInfo.gameObject);
                }
            }

            string eventID = string.Empty;
            if (this.m_CallbackHandlers.TryGetValue(typeof(T), out eventID))
            {
                CallbackEventData triggerEventData = new CallbackEventData();
                triggerEventData.AddData("Trigger", this);
                triggerEventData.AddData("Player", PlayerInfo.gameObject);
                triggerEventData.AddData("EventData", new PointerEventData(EventSystem.current));
                triggerEventData.AddData("Player", player);
                base.Execute(eventID, triggerEventData);
            }
        }

        protected void ExecuteEvent<T>(FailureItemEventFunction<T> func, Player player, FailureCause failureCause, bool includeDisabled = false) where T : ITriggerEventHandler
        {
            for (int i = 0; i < this.m_TriggerEvents.Length; i++)
            {
                ITriggerEventHandler handler = this.m_TriggerEvents[i];
                if (ShouldSendEvent<T>(handler, includeDisabled))
                {
                    func.Invoke((T)handler, player, CharacterManager.current.PlayerInfo.gameObject, failureCause);
                }
            }

            string eventID = string.Empty;
            if (this.m_CallbackHandlers.TryGetValue(typeof(T), out eventID))
            {
                CallbackEventData triggerEventData = new CallbackEventData();
                triggerEventData.AddData("Trigger", this);
                triggerEventData.AddData("Player", PlayerInfo.gameObject);
                triggerEventData.AddData("EventData", new PointerEventData(EventSystem.current));
                triggerEventData.AddData("Player", player);
                triggerEventData.AddData("FailureCause", failureCause);
                base.Execute(eventID, triggerEventData);
            }
        }
    }
}