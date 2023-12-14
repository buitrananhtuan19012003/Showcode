using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LupinrangerPatranger.UIWidgets;

namespace LupinrangerPatranger.CharacterSystem
{
    /// <summary>
    /// Helper enum definition for multiple selection of PointerEventData.InputButton
    /// </summary>
    [System.Flags]
    public enum InputButton
    {
        Left = 1,
        Right = 2,
        Middle = 4
    }
    public class CharacterContainer : UIWidget, IDropHandler
    {
        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnAddCharacter");
                callbacks.Add("OnFailedToAddCharacter");
                callbacks.Add("OnRemoveCharacter");
                callbacks.Add("OnRemoveCharacterReference");
                callbacks.Add("OnFailedToRemoveCharacter");
                callbacks.Add("OnTryUseCharacter");
                callbacks.Add("OnUseCharacter");
                callbacks.Add("OnDropCharacter");
                return callbacks.ToArray();
            }
        }
        #region Delegates
        public delegate void AddCharacterDelegate(Player player, Slot slot);
        /// <summary>
        /// Called when an character is added to the container.
        /// </summary>
        public event AddCharacterDelegate OnAddCharacter;

        public delegate void FailedToAddCharacterDelegate(Player player);
        /// <summary>
        /// Called when an character could not be added to the container.
        /// </summary>
        public event FailedToAddCharacterDelegate OnFailedToAddCharacter;

        public delegate void RemoveCharacterDelegate(Player player, int amount, Slot slot);
        /// <summary>
        /// Called when an player was removed from the container
        /// </summary>
        public event RemoveCharacterDelegate OnRemoveCharacter;

        public delegate void FailedToRemoveCharacterDelegate(Player player, int amount);
        /// <summary>
        /// Called when an player could not be removed
        /// </summary>
        public event FailedToRemoveCharacterDelegate OnFailedToRemoveCharacter;

        public delegate void UseCharacterDelegate(Player player, Slot slot);
        /// <summary>
        /// Called when the user trys to use player. This is called before OnUseCharacter and any checks including CanUseCharacter
        /// </summary>
        public event UseCharacterDelegate OnTryUseCharacter;
        /// <summary>
        /// Called when an player was used from this container.
        /// </summary>
        public event UseCharacterDelegate OnUseCharacter;

        public delegate void DropCharacterDelegate(Player player, GameObject droppedInstance);
        /// <summary>
        /// Called when an player was dropped from this container to world
        /// </summary>
        public event DropCharacterDelegate OnDropCharacter;

        #endregion

        /// <summary>
        /// The button to use item in slot
        /// </summary>
        [Header("Behaviour")]
        [Tooltip("The button to use player in slot.")]
        [EnumFlags]
        public InputButton useButton = InputButton.Right;
        /// <summary>
        /// Sets the container as dynamic. Slots are instantiated at runtime.
        /// </summary>
        [Tooltip("Sets the container as dynamic. Slots are instantiated at runtime.")]
        [SerializeField]
        protected bool m_DynamicContainer = false;
        /// <summary>
        /// The parent transform of slots. 
        /// </summary>
        [Tooltip("The parent transform of slots.")]
        [SerializeField]
        protected Transform m_SlotParent;
        /// <summary>
        /// The slot prefab. This game object should contain the Slot component or a child class of Slot. 
        /// </summary>
        [Tooltip("The slot prefab. This game object should contain the Slot component or a child class of Slot.")]
        [SerializeField]
        protected GameObject m_SlotPrefab;

        [Tooltip("If true this container will be used as reference. Referenced containers don't hold the characters itself, they are only referencing an item.")]
        [SerializeField]
        protected bool m_UseReferences = false;
        /// <summary>
        /// If true this container will be used as reference.
        /// </summary>
        public bool UseReferences
        {
            get { return this.m_UseReferences; }
            protected set { this.m_UseReferences = value; }
        }

        [Tooltip("Can the characters be dragged into this container.")]
        [SerializeField]
        protected bool m_CanDragIn = false;
        /// <summary>
        /// Can the characters be dragged into this container
        /// </summary>
        public bool CanDragIn
        {
            get { return this.m_CanDragIn; }
            protected set { this.m_CanDragIn = value; }
        }

        [Tooltip("Can the characters be dragged out from this container.")]
        [SerializeField]
        protected bool m_CanDragOut = false;
        /// <summary>
        /// Can the characters be dragged out from this container.
        /// </summary>
        public bool CanDragOut
        {
            get { return this.m_CanDragOut; }
            protected set { this.m_CanDragOut = value; }
        }

        [Tooltip("Can the players be dropped from this container to ground.")]
        [SerializeField]
        protected bool m_CanDropCharacters = false;

        public bool CanDropCharacters
        {
            get { return this.m_CanDropCharacters; }
            protected set { this.m_CanDropCharacters = value; }
        }

        [Tooltip("Can the characters be referenced from this container.")]
        [SerializeField]
        protected bool m_CanReferenceCharacters = false;
        /// <summary>
        /// Can the items be referenced from this container
        /// </summary>
        public bool CanReferenceCharacters
        {
            get { return this.m_CanReferenceCharacters; }
            protected set { this.m_CanReferenceCharacters = value; }
        }

        [Tooltip("Can the characters be sold from this container.")]
        [SerializeField]
        protected bool m_CanSellCharacters = false;
        /// <summary>
        /// Can the items be sold from this container
        /// </summary>
        public bool CanSellCharacters
        {
            get { return this.m_CanSellCharacters; }
            protected set { this.m_CanSellCharacters = value; }
        }

        [Tooltip("Can characters be used from this container.")]
        [SerializeField]
        protected bool m_CanUseCharacters = false;
        /// <summary>
        /// Can characters be used from this container
        /// </summary>
        public bool CanUseCharacters
        {
            get { return this.m_CanUseCharacters; }
            protected set { this.m_CanUseCharacters = value; }
        }

        [Tooltip("Use context menu for character interaction.")]
        [SerializeField]
        protected bool m_UseContextMenu = false;
        /// <summary>
        /// Use context menu for item interaction
        /// </summary>
        public bool UseContextMenu
        {
            get { return this.m_UseContextMenu; }
            protected set { this.m_UseContextMenu = value; }
        }

        // [Compound("m_UseContextMenu")]
        [SerializeField]
        [EnumFlags]
        protected InputButton m_ContextMenuButton = InputButton.Right;
        public InputButton ContextMenuButton
        {
            get { return this.m_ContextMenuButton; }
            set { this.m_ContextMenuButton = value; }
        }

        [SerializeField]
        protected List<string> m_ContextMenuFunctions = new List<string>();

        public List<string> ContextMenuFunctions
        {
            get { return this.m_ContextMenuFunctions; }
        }

        [Tooltip("Show player tooltips?")]
        [SerializeField]
        protected bool m_ShowTooltips = false;
        /// <summary>
        /// Show character tooltips?
        /// </summary>
        public bool ShowTooltips
        {
            get { return this.m_ShowTooltips; }
            protected set { this.m_ShowTooltips = value; }
        }

        [Tooltip("If true move used character. Move Conditions needs to be defined!")]
        [SerializeField]
        protected bool m_MoveUsedCharacter = false;

        public bool MoveUsedCharacter
        {
            get { return this.m_MoveUsedCharacter; }
            protected set { this.m_MoveUsedCharacter = value; }
        }

        /// <summary>
        /// Conditions for auto moving items when used
        /// </summary>
        public List<MoveCharacterCondition> moveCharacterConditions = new List<MoveCharacterCondition>();

        public List<Restriction> restrictions = new List<Restriction>();

        protected List<Slot> m_Slots = new List<Slot>();
        /// <summary>
        /// Collection of slots this container is holding
        /// </summary>
        public ReadOnlyCollection<Slot> Slots
        {
            get { return this.m_Slots.AsReadOnly(); }
        }

        protected CharacterCollection m_Collection;
        /// <summary>
        /// Set the collection for this container.
        /// </summary>
        public CharacterCollection Collection
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                RemoveCharacters(true);
                value.Initialize();
                this.m_Collection = value;

                CurrencySlot[] currencySlots = GetSlots<CurrencySlot>();

                for (int i = 0; i < currencySlots.Length; i++)
                {
                    Currency defaultCurrency = currencySlots[i].GetDefaultCurrency();
                    Currency currency = m_Collection.Where(x => typeof(Currency).IsAssignableFrom(x.GetType()) && x.Id == defaultCurrency.Id).FirstOrDefault() as Currency;
                    if (currency == null)
                    {
                        ReplaceCharacter(currencySlots[i].Index, defaultCurrency);
                    }
                    else
                    {
                        currencySlots[i].ObservedCharacter = currency;
                        currency.Slots.Add(currencySlots[i]);
                    }
                }

                for (int i = 0; i < this.m_Collection.Count; i++)
                {
                    Player player = this.m_Collection[i];
                    if (player is Currency)
                        continue;

                    player.Slots.RemoveAll(x => x == null);
                    if (player.Slots.Count > 0)
                    {
                        for (int j = 0; j < player.Slots.Count; j++)
                        {
                            player.Slots[j].ObservedCharacter = player;
                        }
                        continue;
                    }
                    if (this.m_DynamicContainer)
                    {
                        Slot slot = CreateSlot();
                        slot.ObservedCharacter = player;
                        player.Slots.Add(slot);
                    }
                    else
                    {
                        Slot slot;
                        if (CanAddCharacter(player, out slot))
                        {
                            ReplaceCharacter(slot.Index, player);
                        }
                    }

                }
            }
        }

        protected override void OnAwake()
        {
            if (this.m_SlotPrefab != null && this.m_SlotPrefab.scene.name != default)
            {
                this.m_SlotPrefab.SetActive(false);
            }
            RefreshSlots();
            RegisterCallbacks();
        }

        public override void Show()
        {
            base.Show();
            Trigger trigger = GetComponent<Trigger>();
            if (trigger != null)
            {
                Trigger.currentUsedTrigger = trigger;
                Trigger.currentUsedWindow = this;
            }

        }

        /// <summary>
        /// Stacks the character from s2 to s1. If stacking is not possible, swap the characters.
        /// </summary>
        /// <param name="s1">Slot 1</param>
        /// <param name="s2">Slot 2</param>
        /// <returns>True if stacking or swaping possible.</returns>
        public bool StackOrSwap(Slot s1, Slot s2)
        {
            if (s1 is CharacterSlot s1Slot && s1Slot.IsCooldown)
                return false;
            if (s2 is CharacterSlot s2Slot && s2Slot.IsCooldown)
                return false;

            if (s1 == s2)
            {
                return false;
            }
            if (s2.Container.UseReferences && !s1.Container.UseReferences)
            {
                return false;
            }

            if (s1.Container.UseReferences && !s2.Container.CanReferenceCharacters)
            {
                return false;
            }

            if (StackOrAdd(s1, s2.ObservedCharacter))
            {
                if (!s2.Container.UseReferences && !s1.Container.UseReferences)
                {
                    s2.Container.RemoveCharacter(s2.Index);
                }

                return true;
            }

            return SwapCharacters(s1, s2);
        }
        //public bool StackOrSwap(Slot s1, Slot s2, Slot s3, Slot s4)
        //{
        //    if (s1 is CharacterSlot s1Slot && s1Slot.IsCooldown)
        //        return false;
        //    if (s2 is CharacterSlot s2Slot && s2Slot.IsCooldown)
        //        return false;
        //    if (s3 is CharacterSlot s3Slot && s3Slot.IsCooldown)
        //        return false;
        //    if (s4 is CharacterSlot s4Slot && s4Slot.IsCooldown)
        //        return false;

        //    if (s1 == s2 == s3 == s4)
        //    {
        //        return false;
        //    }
        //    if (s2.Container.UseReferences && s3.Container.UseReferences && s4.Container.UseReferences && !s1.Container.UseReferences)
        //    {
        //        return false;
        //    }

        //    if (s1.Container.UseReferences && !s2.Container.CanReferenceCharacters && !s3.Container.CanReferenceCharacters && !s4.Container.CanReferenceCharacters)
        //    {
        //        return false;
        //    }

        //    if (StackOrAdd(s1, s2.ObservedCharacter))
        //    {
        //        if (!s2.Container.UseReferences && !s3.Container.UseReferences && !s4.Container.UseReferences && !s1.Container.UseReferences)
        //        {
        //            s2.Container.RemoveCharacter(s2.Index);
        //            //s3.Container.RemoveCharacter(s3.Index);
        //            //s4.Container.RemoveCharacter(s4.Index);
        //        }
        //        return true;
        //    }
        //    /*if (StackOrAdd(s1, s2, s3, s4.ObservedCharacter))
        //    {
        //        if (!s2.Container.UseReferences && !s3.Container.UseReferences && !s4.Container.UseReferences && !s1.Container.UseReferences)
        //        {
        //            s2.Container.RemoveCharacter(s2.Index);
        //            s3.Container.RemoveCharacter(s3.Index);
        //            s4.Container.RemoveCharacter(s4.Index);
        //        }
        //        return true;
        //    }*/

        //    return SwapCharacters(s1, s2, s3, s4);
        //}

        /// <summary>
        /// Swaps characters in slots. From s2 to s1
        /// </summary>
        /// <param name="s1">First slot</param>
        /// <param name="s2">Second slot</param>
        /// <returns>True if swapped.</returns>
        public bool SwapCharacters(Slot s1, Slot s2)
        {
            if (s1 is CharacterSlot s1Slot && s1Slot.IsCooldown)
                return false;
            if (s2 is CharacterSlot s2Slot && s2Slot.IsCooldown)
                return false;
            if (s2.Container.UseReferences && !s1.Container.UseReferences)
            {
                return false;
            }

            //Slots the character is currently inside.
            List<Slot> requiredSlotsObserved = s2.Container.GetRequiredSlots(s1.ObservedCharacter, s2);
            if (requiredSlotsObserved.Count == 0)
            {
                requiredSlotsObserved.Add(s2);
            }

            Player[] charactersInRequiredSlotsObserved = requiredSlotsObserved.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
            Slot[] willBeFreeSlotsObserved = s1.Container.GetSlots(s1.ObservedCharacter);
            Dictionary<Slot, Player> moveLocationsObserved = new Dictionary<Slot, Player>();

            List<Slot> requiredSlots = s1.Container.GetRequiredSlots(s2.ObservedCharacter, s1);
            if (requiredSlots.Count == 0)
            {
                requiredSlots.Add(s1);
            }
            Player[] charactersInRequiredSlots = requiredSlots.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
            Slot[] willBeFreeSlots = s2.Container.GetSlots(s2.ObservedCharacter);

            Dictionary<Slot, Player> moveLocations = new Dictionary<Slot, Player>();

            if (CanMoveCharacters(charactersInRequiredSlotsObserved, willBeFreeSlotsObserved, s1, s1.Container, ref moveLocationsObserved) && CanMoveCharacters(charactersInRequiredSlots, willBeFreeSlots, s2, s2.Container, ref moveLocations))
            {
                //   Debug.Log(s1.Container.Name+":"+s1.ObservedCharacter+" "+s2.Container.Name+": "+s2.ObservedCharacter);
                if (!s1.Container.UseReferences || !s2.Container.CanReferenceCharacters)
                {
                    for (int i = 0; i < charactersInRequiredSlots.Length; i++)
                    {
                        s1.Container.RemoveCharacter(charactersInRequiredSlots[i]);
                    }
                }
                if (!s1.Container.UseReferences)
                {
                    s2.Container.RemoveCharacter(s2.Index);
                }

                if (!s1.Container.UseReferences || !s2.Container.CanReferenceCharacters)
                {
                    for (int i = 0; i < charactersInRequiredSlotsObserved.Length; i++)
                    {
                        s1.Container.RemoveCharacter(charactersInRequiredSlotsObserved[i]);
                    }
                }
                if (!s1.Container.UseReferences)
                {
                    foreach (KeyValuePair<Slot, Player> kvp in moveLocations)
                    {
                        s2.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
                    }
                }
                if (!s2.Container.UseReferences)
                {
                    foreach (KeyValuePair<Slot, Player> kvp in moveLocationsObserved)
                    {
                        s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
                    }
                }


                if (s1.Container == s2.Container)
                {
                    foreach (KeyValuePair<Slot, Player> kvp in moveLocations)
                    {
                        s2.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
                    }
                    foreach (KeyValuePair<Slot, Player> kvp in moveLocationsObserved)
                    {
                        s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
                    }
                }
                return true;
            }
            return false;
        }
        //public bool SwapCharacters(Slot s1, Slot s2, Slot s3, Slot s4)
        //{
        //    if (s1 is CharacterSlot s1Slot && s1Slot.IsCooldown)
        //        return false;
        //    if (s2 is CharacterSlot s2Slot && s2Slot.IsCooldown)
        //        return false;
        //    if (s2.Container.UseReferences && !s1.Container.UseReferences)
        //    {
        //        return false;
        //    }

        //    if (s3 is CharacterSlot s3Slot && s3Slot.IsCooldown)
        //        return false;
        //    if (s3.Container.UseReferences && !s1.Container.UseReferences)
        //    {
        //        return false;
        //    }

        //    if (s4 is CharacterSlot s4Slot && s4Slot.IsCooldown)
        //        return false;
        //    if (s4.Container.UseReferences && !s1.Container.UseReferences)
        //    {
        //        return false;
        //    }

        //    //Slots the character is currently inside.
        //    List<Slot> requiredSlotsObserved = s2.Container.GetRequiredSlots(s1.ObservedCharacter, s2);
        //    if (requiredSlotsObserved.Count == 0)
        //    {
        //        requiredSlotsObserved.Add(s2);
        //        requiredSlotsObserved.Add(s3);
        //    }
        //    List<Slot> requiredSlotsObserved3 = s3.Container.GetRequiredSlots(s1.ObservedCharacter, s3);
        //    if (requiredSlotsObserved.Count == 0)
        //    {
        //        requiredSlotsObserved.Add(s3);
        //    }
        //    List<Slot> requiredSlotsObserved4 = s4.Container.GetRequiredSlots(s1.ObservedCharacter, s4);
        //    if (requiredSlotsObserved.Count == 0)
        //    {
        //        requiredSlotsObserved.Add(s4);
        //    }

        //    Characters[] charactersInRequiredSlotsObserved = requiredSlotsObserved.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
        //    Characters[] charactersInRequiredSlotsObserved3 = requiredSlotsObserved3.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
        //    Characters[] charactersInRequiredSlotsObserved4 = requiredSlotsObserved4.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
        //    Slot[] willBeFreeSlotsObserved = s1.Container.GetSlots(s1.ObservedCharacter);
        //    Dictionary<Slot, Characters> moveLocationsObserved = new Dictionary<Slot, Characters>();

        //    List<Slot> requiredSlots = s1.Container.GetRequiredSlots(s2.ObservedCharacter, s1);
        //    if (requiredSlots.Count == 0)
        //    {
        //        requiredSlots.Add(s1);
        //    }
        //    Characters[] charactersInRequiredSlots = requiredSlots.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
        //    Slot[] willBeFreeSlots = s2.Container.GetSlots(s2.ObservedCharacter);

        //    Dictionary<Slot, Characters> moveLocations = new Dictionary<Slot, Characters>();

        //    if (CanMoveCharacters(charactersInRequiredSlotsObserved, willBeFreeSlotsObserved, s1, s1.Container, ref moveLocationsObserved) && CanMoveCharacters(charactersInRequiredSlots, willBeFreeSlots, s2, s2.Container, ref moveLocations))
        //    {
        //        //   Debug.Log(s1.Container.Name+":"+s1.ObservedCharacter+" "+s2.Container.Name+": "+s2.ObservedCharacter);
        //        if (!s1.Container.UseReferences || !s2.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlots.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlots[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            s2.Container.RemoveCharacter(s2.Index);
        //        }

        //        if (!s1.Container.UseReferences || !s3.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlots.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlots[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            s3.Container.RemoveCharacter(s3.Index);
        //        }
        //        if (!s1.Container.UseReferences || !s4.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlots.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlots[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            s4.Container.RemoveCharacter(s4.Index);
        //        }

        //        if (!s1.Container.UseReferences || !s2.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlotsObserved.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlotsObserved[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocations)
        //            {
        //                s2.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }
        //        if (!s2.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocationsObserved)
        //            {
        //                s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }

        //        if (!s1.Container.UseReferences || !s3.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlotsObserved.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlotsObserved[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocations)
        //            {
        //                s3.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }
        //        if (!s3.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocationsObserved)
        //            {
        //                s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }

        //        if (!s1.Container.UseReferences || !s4.Container.CanReferenceCharacters)
        //        {
        //            for (int i = 0; i < charactersInRequiredSlotsObserved.Length; i++)
        //            {
        //                s1.Container.RemoveCharacter(charactersInRequiredSlotsObserved[i]);
        //            }
        //        }
        //        if (!s1.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocations)
        //            {
        //                s4.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }
        //        if (!s4.Container.UseReferences)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocationsObserved)
        //            {
        //                s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }

        //        if (s1.Container == s2.Container)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocations)
        //            {
        //                s2.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocationsObserved)
        //            {
        //                s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }
        //        if (s1.Container == s3.Container)
        //        {
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocations)
        //            {
        //                s3.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //            foreach (KeyValuePair<Slot, Characters> kvp in moveLocationsObserved)
        //            {
        //                s1.Container.ReplaceCharacter(kvp.Key.Index, kvp.Value);
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Checks if the characters in slots can be swapped.
        /// </summary>
        /// <param name="s1">Slot 1</param>
        /// <param name="s2">Slot 2</param>
        /// <returns>True if characters can be swapped.</returns>
        public bool CanSwapCharacters(Slot s1, Slot s2)
        {
            if (s1 is CharacterSlot s1Slot && s1Slot.IsCooldown)
                return false;
            if (s2 is CharacterSlot s2Slot && s2Slot.IsCooldown)
                return false;

            List<Slot> requiredSlotsObserved = s2.Container.GetRequiredSlots(s1.ObservedCharacter, s2);
            if (requiredSlotsObserved.Count == 0)
            {
                requiredSlotsObserved.Add(s2);
            }
            Player[] charactersInRequiredSlotsObserved = requiredSlotsObserved.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
            Slot[] willBeFreeSlotsObserved = s1.Container.GetSlots(s1.ObservedCharacter);
            Dictionary<Slot, Player> moveLocationsObserved = new Dictionary<Slot, Player>();

            List<Slot> requiredSlots = s1.Container.GetRequiredSlots(s2.ObservedCharacter, s1);
            if (requiredSlots.Count == 0)
            {
                requiredSlots.Add(s1);
            }
            Player[] charactersInRequiredSlots = requiredSlots.Where(x => x.ObservedCharacter != null).Select(y => y.ObservedCharacter).Distinct().ToArray();
            Slot[] willBeFreeSlots = s2.Container.GetSlots(s2.ObservedCharacter);

            Dictionary<Slot, Player> moveLocations = new Dictionary<Slot, Player>();

            return CanMoveCharacters(charactersInRequiredSlotsObserved, willBeFreeSlotsObserved, s1, s1.Container, ref moveLocationsObserved) && CanMoveCharacters(charactersInRequiredSlots, willBeFreeSlots, s2, s2.Container, ref moveLocations);
        }

        /// <summary>
        /// Try to stack the item to any item in container. If it fails add the item.
        /// </summary>
        /// <param name="player">Item to stack/add</param>
        /// <returns>True if item was stacked or added</returns>
        public virtual bool StackOrAdd(Player player)
        {
            if (!StackCharacter(player) && !AddCharacter(player))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to stack the character to the slots character. If Slot is empty add item to the slot.
        /// </summary>
        /// <param name="slot">Slot where to stack or add the character to.</param>
        /// <param name="player">Character to stack</param>
        /// <returns>Returns true if character was stacked or added to slot</returns>
        public virtual bool StackOrAdd(Slot slot, Player player)
        {
            //Check if the character can be stacked to slot;
            if (CanStack(slot, player) && !UseReferences)
            {
                //Stack the character to slot and return true
                slot.ObservedCharacter.Stack += player.Stack;
                RemoveCharacterCompletely(player);
                //OnAddCharacter(character, slot);
                NotifyAddCharacter(player, slot);
                return true;
            }
            //Slot is empty and the character can be added to slot.
            else if (CanAddCharacter(slot.Index, player))
            {
                if (!slot.Container.CanReferenceCharacters)
                {
                    RemoveCharacterReferences(player);
                }
                if (!slot.Container.UseReferences && player.Slot != null)
                {
                    player.Container.RemoveCharacter(player.Slot.Index);
                }

                slot.Container.ReplaceCharacter(slot.Index, player);
                return true;
            }
            //Return false if the item can't be stacked to the slot
            return false;
        }
        /// <summary>
        /// Adds a new character to a free or dynamicly created slot in this container.
        /// </summary>
        /// <param name="player">Character to add</param>
        /// <returns>True if character was added.</returns>
        public virtual bool AddCharacter(Player player)
        {
            Slot slot = null;
            if (CanAddCharacter(player, out slot, true))
            {
                ReplaceCharacter(slot.Index, player);
                return true;
            }
            OnFailedToAddCharacter(player);
            return false;
        }

        public bool StackCharacter(Player player)
        {
            //Check if item or collection is null
            if (player == null) //|| this.m_Collection == null)
            {
                return false;
            }
            //Get all players in collection with same id as the item to stack
            Player[] players = this.m_Collection.Where(x => x != null && x.Id == player.Id && x.Rarity == player.Rarity).ToArray();

            int stack = player.Stack;

            //Loop through the players
            for (int i = 0; i < players.Length; i++)
            {
                Player current = players[i];

                //Check if max stack is reached
                if ((current.Stack + player.Stack) <= current.MaxStack)
                {
                    current.Stack += player.Stack;
                    TryConvertCurrency(current as Currency);
                    //OnAddCharacter(player, current.Slot);
                    NotifyAddCharacter(player, current.Slot);
                    return true;
                }

                if (current.Stack < current.MaxStack)
                {
                    int restAmount = stack - (current.MaxStack - current.Stack);
                    current.Stack = current.MaxStack;
                    stack = restAmount;
                    if (stack <= 0)
                    {
                        return true;
                    }
                }
            }
            player.Stack = stack;

            return false;
        }

        /// <summary>
        /// Condition method if swapping is possible
        /// </summary>
        private bool CanMoveCharacters(Player[] players, Slot[] slotsWillBeFree, Slot preferredSlot, CharacterContainer container, ref Dictionary<Slot, Player> moveLocations)
        {
            List<Slot> reservedSlots = new List<Slot>();
            List<Player> checkedCharacters = new List<Player>(players);
            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player current = checkedCharacters[i];
                List<Slot> requiredSlots = container.GetRequiredSlots(current, preferredSlot);
                for (int j = 0; j < requiredSlots.Count; j++)
                {
                    // Debug.Log("CanMove : "+(requiredSlots[j].IsEmpty || slotsWillBeFree.Contains(requiredSlots[j]))+" "+ requiredSlots[j].CanAddItem(current) +" "+ !reservedSlots.Contains(requiredSlots[j]));

                    if ((requiredSlots[j].IsEmpty || slotsWillBeFree.Contains(requiredSlots[j])) && requiredSlots[j].CanAddCharacter(current) && !reservedSlots.Contains(requiredSlots[j]))
                    {
                        //Debug.Log("CanMove : "+container.Name+" "+requiredSlots[j].Index);

                        reservedSlots.Add(requiredSlots[j]);
                        checkedCharacters.RemoveAt(i);
                        moveLocations.Add(requiredSlots[j], current);
                        break;
                    }
                }
            }

            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player current = checkedCharacters[i];
                if (preferredSlot.CanAddCharacter(current) && !reservedSlots.Contains(preferredSlot))
                {
                    reservedSlots.Add(preferredSlot);
                    checkedCharacters.RemoveAt(i);
                    moveLocations.Add(preferredSlot, current);
                    break;
                }
            }

            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player current = checkedCharacters[i];
                for (int j = 0; j < container.Slots.Count; j++)
                {
                    if ((container.Slots[j].IsEmpty || slotsWillBeFree.Contains(container.Slots[j])) && container.Slots[j].CanAddCharacter(current) && !reservedSlots.Contains(container.Slots[j]))
                    {
                        reservedSlots.Add(container.Slots[j]);
                        checkedCharacters.RemoveAt(i);
                        moveLocations.Add(container.Slots[j], current);
                        break;
                    }
                }
            }
            return checkedCharacters.Count == 0;
        }

        /// <summary>
        /// Checks if the character can be added at index. Free slot is required
        /// </summary>
        /// <param name="index">Slot index.</param>
        /// <param name="player">Character to add.</param>
        /// <returns></returns>
        public virtual bool CanAddCharacter(int index, Player player)
        {
            if (player == null) { return true; }
            List<Slot> requiredSlots = GetRequiredSlots(player);
            Slot slot = this.m_Slots[index];
            if (requiredSlots.Count > 0)
            {
                if (!requiredSlots.Contains(slot)) { return false; }

                for (int i = 0; i < requiredSlots.Count; i++)
                {
                    if (!(requiredSlots[i].IsEmpty && requiredSlots[i].CanAddCharacter(player)))
                    {
                        return false;
                    }
                }
                return true;
            }
            return slot.IsEmpty && slot.CanAddCharacter(player);
        }

        /// <summary>
        /// Checks if the character can be added to this container. Free slot is required.
        /// </summary>
        /// <param name="player">Character to check.</param>
        /// <returns>Returns true if the character can be added.</returns>
        public virtual bool CanAddCharacter(Player player)
        {
            Slot slot = null;
            return CanAddCharacter(player, out slot);
        }

        /// <summary>
        /// Checks if the character can be added to this container. Free slot is required.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <param name="slot">Required or next free slot</param>
        /// <param name="createSlot">Should a slot be created if the container is dynamic.</param>
        /// <returns>Returns true if the character can be added.</returns>
        public virtual bool CanAddCharacter(Player player, out Slot slot, bool createSlot = false)
        {
            slot = null;
            if (player == null) { return true; }
            /* for (int i = 0; i < restrictions.Count; i++)
             {
                 if (!restrictions[i].CanAddCharacter(player))
                 {
                     NotificationOptions message = restrictions[i].GetNotification();
                     if (message != null)
                         message.Show();
                     return false;
                 }
             }*/

            List<Slot> requiredSlots = GetRequiredSlots(player);
            if (requiredSlots.Count > 0)
            {
                for (int i = 0; i < requiredSlots.Count; i++)
                {
                    if (!(requiredSlots[i].IsEmpty && requiredSlots[i].CanAddCharacter(player)))
                    {
                        return false;
                    }
                }
                slot = requiredSlots[0];
                return true;
            }
            else
            {
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    if (this.m_Slots[i].IsEmpty && this.m_Slots[i].CanAddCharacter(player))
                    {
                        slot = this.m_Slots[i];
                        return true;
                    }
                }
            }

            if (this.m_DynamicContainer)
            {
                if (createSlot)
                {
                    slot = CreateSlot();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Replaces the characters at index and returns an array of characters that were replaced.
        /// </summary>
        /// <param name="index">Index of slot to repalce.</param>
        /// <param name="player">Player to replace with.</param>
        /// <returns></returns>
        public virtual Player[] ReplaceCharacter(int index, Player player)
        {
            List<Player> list = new List<Player>();
            if (index < this.m_Slots.Count)
            {
                Slot slot = this.m_Slots[index];
                List<Slot> slotsForCharacter = GetRequiredSlots(player, slot);
                if (slotsForCharacter.Count == 0 && slot.CanAddCharacter(player))
                    slotsForCharacter.Add(slot);

                // Debug.Log("ReplaceCharacter Index:" + index + " SlotsForCharacter:" + slotsForCharacter.Count);

                if (player != null)
                {
                    for (int i = 0; i < slotsForCharacter.Count; i++)
                    {
                        Player current = slotsForCharacter[i].ObservedCharacter;
                        if (current != null && !list.Contains(current))
                        {
                            list.Add(current);
                            RemoveCharacter(slotsForCharacter[i].Index);
                        }
                        slotsForCharacter[i].ObservedCharacter = player;
                        if (UseReferences && !player.ReferencedSlots.Contains(slotsForCharacter[i]))
                            player.ReferencedSlots.Add(slotsForCharacter[i]);
                    }

                    if (!UseReferences)
                    {
                        //if (!this.m_Collection.Contains(player))
                        //    this.m_Collection.Add(player);

                        player.Slots.Clear();
                        player.Slots.AddRange(slotsForCharacter);
                    }
                    else
                    {
                        player.ReferencedSlots = player.ReferencedSlots.Except(slotsForCharacter).ToList();
                        player.ReferencedSlots.AddRange(slotsForCharacter);
                    }
                    //OnAddCharacter(player, slot);
                    NotifyAddCharacter(player, slot);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Removes the character at index from this conrainer
        /// </summary>
        /// <param name="index">The slot index where to remove the character.</param>
        /// <returns>Returns true if the character was removed.</returns>
        public virtual bool RemoveCharacter(int index)
        {

            if (index < this.m_Slots.Count)
            {
                Slot slot = this.m_Slots[index];
                Player player = slot.ObservedCharacter;
                if (player is null) return false;

                if (UseReferences)
                {
                    slot.ObservedCharacter = null;
                    return player.ReferencedSlots.Remove(slot);
                }
                return RemoveCharacter(player);
            }
            return false;
        }

        /// <summary>
        /// Removes the amount/stack of characters
        /// </summary>
        /// <param name="character">Character to remove</param>
        /// <param name="amount">Amount/Stack to remove</param>
        /// <returns>Returns true if the stack of characters is removed.</returns>
        public virtual bool RemoveCharacter(Player player, int amount)
        {
            if (player == null) { return false; }
            if (typeof(Currency).IsAssignableFrom(player.GetType()))
            {
                Currency payCurrency = GetCharacters(player.Id).FirstOrDefault() as Currency;

                ConvertToSmallestCurrency();
                Currency current = null;
                Currency[] currencies = GetCharacters<Currency>();
                for (int i = 0; i < currencies.Length; i++)
                {
                    if (currencies[i].Stack > 0)
                    {
                        current = currencies[i];
                    }
                }

                bool result = TryRemove(current, payCurrency, amount);
                TryConvertCurrency(current);

                CurrencySlot slot = GetSlots<CurrencySlot>().Where(x => x.ObservedCharacter.Id == payCurrency.Id).FirstOrDefault();
                if (result)
                {
                    // OnRemoveCharacter(payCurrency, amount, slot);
                    NotifyRemoveCharacter(payCurrency, amount, slot);
                }
                else
                {
                    OnFailedToRemoveCharacter(payCurrency, amount);
                }

                return result;
            }

            if (player.Stack == amount && RemoveCharacter(player))
            {
                return true;
            }

            if (!HasCharacter(player, amount))
            {
                OnFailedToRemoveCharacter(player, amount);
                return false;
            }

            Player[] checkedPlayers = GetCharacters(player.Id);
            int currentAmount = amount;
            for (int i = 0; i < checkedPlayers.Length; i++)
            {
                Player checkedCharacter = checkedPlayers[i];
                if (checkedCharacter != null)
                {
                    int mStack = checkedCharacter.Stack;

                    checkedCharacter.Stack -= currentAmount;
                    currentAmount -= mStack;
                    //OnRemoveCharacter(checkedCharacter, mStack, checkedCharacter.Slot);
                    NotifyRemoveCharacter(checkedCharacter, mStack, checkedCharacter.Slot);
                    if (checkedCharacter.Stack <= 0)
                    {
                        RemoveCharacterCompletely(checkedCharacter);
                    }
                    if (currentAmount <= 0)
                    {
                        break;
                    }
                }
            }
            return (currentAmount <= 0);
        }

        /// <summary>
        /// Removes the character from this container. If the container uses reference this will remove all references in this container.
        /// </summary>
        /// <param name="player">The character to remove</param>
        /// <returns>Returns true if character was removed</returns>
        public virtual bool RemoveCharacter(Player player)
        {
            if (player == null) { return false; }

            //if (!UseReferences && this.m_Collection.Contains(player))
            //{
            //    //Remove character from the collection
            //    this.m_Collection.Remove(player);

            //    //Remove character from all slots
            //    for (int i = 0; i < this.m_Slots.Count; i++)
            //    {
            //        if (this.m_Slots[i].ObservedCharacter == player)
            //        {
            //            this.m_Slots[i].ObservedCharacter = null;
            //            // OnRemoveCharacter(character, character.Stack, this.m_Slots[i]);
            //            NotifyRemoveCharacter(player, player.Stack, this.m_Slots[i]);
            //            if (this.m_DynamicContainer)
            //            {
            //                if (!(this.m_Slots[i] is CurrencySlot))
            //                    DestroyImmediate(this.m_Slots[i].gameObject);
            //            }
            //        }
            //    }
            //    RefreshSlots();
            //    return true;
            //}
            else if (UseReferences)
            {
                bool result = false;
                //Loop through all slots in this container and remove the character
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    if (this.m_Slots[i].ObservedCharacter == player)
                    {
                        this.m_Slots[i].ObservedCharacter = null;
                        player.ReferencedSlots.Remove(this.m_Slots[i]);
                        //OnRemoveCharacter(character, character.Stack, this.m_Slots[i]);
                        NotifyRemoveCharacter(player, player.Stack, this.m_Slots[i]);
                        result = true;
                    }
                }
                return result;
            }
            return false;
        }

        /// <summary>
        /// Removes all characters from this container. If the container uses reference this will remove all references in this container.
        /// </summary>
        /// <param name="keepInCollection">If set to true, characters will be not removed from collection.</param>
        public virtual void RemoveCharacters(bool keepInCollection = false)
        {
            //Remove all visuals from this container
            if (this.m_DynamicContainer)
            {
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    if (this.m_Slots[i].ObservedCharacter != null)
                    {
                        Player player = this.m_Slots[i].ObservedCharacter;
                        player.Slots.Remove(this.m_Slots[i]);
                        //OnRemoveCharacter(character, character.Stack, this.m_Slots[i]);
                        NotifyRemoveCharacter(player, player.Stack, this.m_Slots[i]);
                    }
                    if (!(this.m_Slots[i] is CurrencySlot))
                        DestroyImmediate(this.m_Slots[i].gameObject);
                }
                RefreshSlots();
            }
            else
            {
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    if (this.m_Slots[i].ObservedCharacter != null)
                    {
                        Player player = this.m_Slots[i].ObservedCharacter;
                        // character.Slots.Remove(this.m_Slots[i]);
                        //  OnRemoveCharacter(character, character.Stack, this.m_Slots[i]);
                        NotifyRemoveCharacter(player, player.Stack, this.m_Slots[i]);
                    }

                    this.m_Slots[i].ObservedCharacter = null;

                }
            }
            if (!UseReferences && !keepInCollection)
            {
                //Remove characters from the collection
                //this.m_Collection.Clear();
            }
        }

        /// <summary>
        /// Check if the container has the given amount of characters
        /// </summary>
        /// <param name="player">Character to check</param>
        /// <param name="amount">Amount/Stack of characters</param>
        /// <returns></returns>
        public bool HasCharacter(Player player, int amount)
        {
            int existingAmount = 0;
            return HasCharacter(player, amount, out existingAmount);
        }

        /// <summary>
        /// Check if the container has the given amount of characters
        /// </summary>
        /// <param name="character">Character to check</param>
        /// <param name="amount">Amount/Stack of characters</param>
        /// <returns></returns>
        public bool HasCharacter(Player player, int amount, out int existingAmount)
        {
            int stack = existingAmount = 0;
            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                Slot slot = this.m_Slots[i];
                if (!slot.IsEmpty && slot.ObservedCharacter.Id == player.Id)
                {

                    stack += slot.ObservedCharacter.Stack;
                }
            }
            existingAmount = stack;
            return stack >= amount;
        }

        /// <summary>
        /// Check if the container has an character with category
        /// </summary>
        public bool HasCategoryCharacter(Category category)
        {
            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                Slot slot = this.m_Slots[i];

                if (!slot.IsEmpty && category.IsAssignable(slot.ObservedCharacter.Category))//slot.ObservedCharacter.Category.Name == category.Name)
                {

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get an array of player with given id in this container.
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>Array of player with given id</returns>
        public Player[] GetCharacters(string idOrName)
        {
            List<Player> players = new List<Player>();
            if (!this.m_UseReferences)
            {
                //players.AddRange(this.m_Collection.Where(x => x.Id == idOrName));
            }
            else
            {
                players.AddRange(this.m_Slots.Where(x => !x.IsEmpty && x.ObservedCharacter.Id == idOrName).Select(y => y.ObservedCharacter));
            }

            if (players.Count == 0)
            {
                if (!this.m_UseReferences)
                {
                    //players.AddRange(this.m_Collection.Where(x => x.Name == idOrName));
                }
                else
                {
                    players.AddRange(this.m_Slots.Where(x => !x.IsEmpty && x.ObservedCharacter.Name == idOrName).Select(y => y.ObservedCharacter));
                }

            }

            return players.ToArray();
        }

        /// <summary>
        /// Returns an array of players of type this container is holding. 
        /// </summary>
        public T[] GetCharacters<T>(bool inherit = false) where T : Player
        {
            return GetCharacters(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        /// Returns an array of players of type this container is holding. 
        /// </summary>
        public Player[] GetCharacters(Type type, bool inherit = false)
        {
            List<Player> players = new List<Player>();
            if (!this.m_UseReferences)
            {
                //players.AddRange(this.m_Collection.Where(x => (!inherit && x.GetType() == type) || (inherit && type.IsAssignableFrom(x.GetType()))));
            }
            else
            {
                players.AddRange(this.m_Slots.Where(x => !x.IsEmpty && ((!inherit && x.ObservedCharacter.GetType() == type) || (inherit && type.IsAssignableFrom(x.ObservedCharacter.GetType())))).Select(y => y.ObservedCharacter));
            }
            return players.ToArray();
        }

        /// Returns required slots for this character in this container. Empty slots are prefered.
        /// </summary>
        /// <param name="character">Character to get the required slots for.</param>
        /// <param name="preferedSlot"></param>
        /// <returns></returns>
        public virtual List<Slot> GetRequiredSlots(Player player, Slot preferedSlot = null)
        {
            List<Slot> slots = new List<Slot>();
            if (player == null) return slots;

            //if (!(player is EquipmentItem equipmentItem)) { return slots; }

            //List<EquipmentRegion> requiredRegions = new List<EquipmentRegion>(equipmentItem.Region);

            //if (preferedSlot != null && preferedSlot.IsEmpty)
            //{
            //    Restrictions.EquipmentRegion[] restrictions = preferedSlot.GetComponents<Restrictions.EquipmentRegion>();
            //    for (int i = requiredRegions.Count - 1; i >= 0; i--)
            //    {
            //        if (restrictions.Select(x => x.region).Contains(requiredRegions[i]))
            //        {
            //            slots.Add(preferedSlot);
            //            requiredRegions.RemoveAt(i);
            //            break;
            //        }
            //    }
            //}

            //for (int i = requiredRegions.Count - 1; i >= 0; i--)
            //{
            //    for (int j = 0; j < this.m_Slots.Count; j++)
            //    {
            //        Restrictions.EquipmentRegion[] restrictions = this.m_Slots[j].GetComponents<Restrictions.EquipmentRegion>();
            //        if (this.m_Slots[j].IsEmpty && restrictions.Select(x => x.region).Contains(requiredRegions[i]))
            //        {
            //            slots.Add(this.m_Slots[j]);
            //            requiredRegions.RemoveAt(i);
            //            break;
            //        }
            //    }
            //}

            //No empty slots availible, second loop
            //if (requiredRegions.Count > 0)
            //{
            //    if (preferedSlot != null)
            //    {
            //        Restrictions.EquipmentRegion[] restrictions = preferedSlot.GetComponents<Restrictions.EquipmentRegion>();
            //        for (int i = requiredRegions.Count - 1; i >= 0; i--)
            //        {
            //            if (restrictions.Select(x => x.region).Contains(requiredRegions[i]))
            //            {
            //                slots.Add(preferedSlot);
            //                requiredRegions.RemoveAt(i);
            //                break;
            //            }
            //        }
            //    }

            //    for (int i = requiredRegions.Count - 1; i >= 0; i--)
            //    {
            //        for (int j = 0; j < this.m_Slots.Count; j++)
            //        {
            //            Restrictions.EquipmentRegion[] restrictions = this.m_Slots[j].GetComponents<Restrictions.EquipmentRegion>();
            //            if (restrictions.Select(x => x.region).Contains(requiredRegions[i]))
            //            {
            //                slots.Add(this.m_Slots[j]);
            //                requiredRegions.RemoveAt(i);
            //                break;
            //            }
            //        }
            //    }
            //}
            return slots;
        }

        /// <summary>
        /// Gets the slots in this container where the character is currently inside.
        /// </summary>
        /// <param name="player">The character in slots</param>
        /// <returns>Array of slots in this container, the character is currently located.</returns>
        public Slot[] GetSlots(Player player)
        {
            List<Slot> list = new List<Slot>();
            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                if (this.m_Slots[i].ObservedCharacter == player)
                {
                    list.Add(this.m_Slots[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Refreshs the slot list and reorganize indices. This method is slow!
        /// </summary>
        public void RefreshSlots()
        {
            if (this.m_DynamicContainer && this.m_SlotParent != null)
            {
                //removed to check only parent children because of currency slot in Loot Window
                // this.m_Slots = this.m_SlotParent.GetComponentsInChildren<Slot>(true).Where(x=>x.GetComponentsInParent<CharacterContainer>(true).FirstOrDefault() == this).ToList();
                this.m_Slots = GetComponentsInChildren<Slot>(true).Where(x => x.GetComponentsInParent<CharacterContainer>(true).FirstOrDefault() == this).ToList();
                this.m_Slots.Remove(this.m_SlotPrefab.GetComponent<Slot>());
            }
            else
            {
                this.m_Slots = GetComponentsInChildren<Slot>(true).Where(x => x.GetComponentsInParent<CharacterContainer>(true).FirstOrDefault() == this).ToList();
            }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                Slot slot = this.m_Slots[i];
                slot.Index = i;
                slot.Container = this;

                //slot.restrictions.AddRange(restrictions); 
                //15.01.2021 Loop through all restrictions and check it is already added.
                for (int j = 0; j < restrictions.Count; j++)
                {
                    if (!slot.restrictions.Contains(restrictions[j]))
                    {
                        slot.restrictions.Add(restrictions[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an array of slots of type this container is providing.
        /// </summary>
        public T[] GetSlots<T>() where T : Slot
        {
            return GetSlots(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Returns an array of slots of type this container is providing.
        /// </summary>
        public Slot[] GetSlots(Type type)
        {
            return this.m_Slots.Where(x => x.GetType() == type).ToArray(); ;
        }

        /// <summary>
        /// Creates a new slot
        /// </summary>
        protected virtual Slot CreateSlot()
        {
            if (this.m_SlotPrefab != null && this.m_SlotParent != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotParent, false);
                Slot slot = go.GetComponent<Slot>();
                this.m_Slots.Add(slot);
                slot.Index = Slots.Count - 1;
                slot.Container = this;
                slot.restrictions.AddRange(restrictions);
                return slot;
            }
            Debug.LogWarning("Please ensure that the slot prefab and slot parent is set in the inspector.");
            return null;
        }

        /// <summary>
        /// Destroy the slot and reorganize indices.
        /// </summary>
        /// <param name="slotID">Slot I.</param>
        protected virtual void DestroySlot(int index)
        {
            if (index < this.m_Slots.Count)
            {
                this.m_Slots[index].StopAllCoroutines();
                DestroyImmediate(this.m_Slots[index].gameObject);
                RefreshSlots();
            }
        }

        /// <summary>
        /// Checks if the character can be stacked to the character in slot.
        /// </summary>
        /// <param name="slot">Slot where to stack the character to.</param>
        /// <param name="player">Character to stack</param>
        /// <returns>Returns true if character can be stacked with the character in slot</returns>
        public bool CanStack(Slot slot, Player player)
        {
            Player slotCharacter = slot.ObservedCharacter;
            return (slotCharacter != null &&
                    player != null &&
                    slotCharacter.Id == player.Id && slotCharacter.Rarity == player.Rarity &&
                    (slotCharacter.Stack + player.Stack) <= slotCharacter.MaxStack);
        }

        /// <summary>
        /// Checks if the character can be stacked to any character in this collection.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool CanStack(Player player)
        {
            //Check if character or collection is null
            //if (player == null || this.m_Collection == null)
            //{
            //    return false;
            //}
            //Get all characters in collection with same id as the character to stack
            //Player[] characters = this.m_Collection.Where(x => x != null && x.Id == player.Id && x.Rarity == player.Rarity).ToArray();
            //Loop through the characters
            //for (int i = 0; i < characters.Length; i++)
            //{
            //    Player current = characters[i];
            //    //Check if max stack is reached
            //    if ((current.Stack + player.Stack) <= current.MaxStack)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        public void ShowByCategory(Dropdown dropdown)
        {
            int current = dropdown.value;
            string category = dropdown.options[current].text;
            if (current != 0)
            {
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    if (this.m_Slots[i].ObservedCharacter != null)
                    {
                        this.m_Slots[i].gameObject.SetActive(this.m_Slots[i].ObservedCharacter.Category.Name == category);
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.m_Slots.Count; i++)
                {
                    this.m_Slots[i].gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Try to convert currency based on currency conversation set in the editor
        /// </summary>
        private void TryConvertCurrency(Currency currency)
        {
            if (currency == null) { return; }
            Currency[] currencies = GetCharacters<Currency>();
            for (int j = 0; j < currency.currencyConversions.Length; j++)
            {
                CurrencyConversion conversion = currency.currencyConversions[j];
                float amount = (currency.Stack * conversion.factor);
                if (amount >= 1f && amount < currency.Stack)
                {
                    float rest = amount % 1f;
                    Currency converted = currencies.Where(x => x.Name == conversion.currency.Name).FirstOrDefault();
                    converted.Stack += Mathf.RoundToInt(amount - rest);
                    currency.Stack = Mathf.RoundToInt(rest / conversion.factor);
                    TryConvertCurrency(converted);
                    break;
                }
            }
        }

        /// <summary>
        /// Try to remove pay currency from current currency
        /// </summary>
        /// <param name="current"></param>
        /// <param name="payCurrency"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private bool TryRemove(Currency current, Currency payCurrency, int price)
        {
            if (current == null)
            {
                return false;
            }
            if (current.Id == payCurrency.Id)
            {
                if (current.Stack - price < 0)
                {
                    return false;
                }
                current.Stack -= price;
                return true;
            }
            else
            {
                for (int j = 0; j < current.currencyConversions.Length; j++)
                {
                    CurrencyConversion conversion = current.currencyConversions[j];
                    if (conversion.factor < 1f)
                    {
                        float amount = (current.Stack * conversion.factor);
                        float rest = amount % 1f;
                        Currency converted = GetCharacters(conversion.currency.Id).FirstOrDefault() as Currency;
                        converted.Stack += Mathf.RoundToInt(amount - rest);
                        current.Stack = Mathf.RoundToInt(rest / conversion.factor);
                        return TryRemove(converted, payCurrency, price);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Convert all currencies to smallest currency value -> Sliver and Gold to Copper
        /// </summary>
        private void ConvertToSmallestCurrency()
        {
            Currency[] currencies = GetCharacters<Currency>();

            for (int i = 0; i < currencies.Length; i++)
            {
                Currency current = currencies[i];

                for (int j = 0; j < current.currencyConversions.Length; j++)
                {
                    CurrencyConversion conversion = current.currencyConversions[j];
                    if (conversion.factor > 1f && current.Stack > 0)
                    {
                        float amount = (current.Stack * conversion.factor);
                        GetCharacters(conversion.currency.Id).FirstOrDefault().Stack += Mathf.RoundToInt(amount);
                        current.Stack = 0;
                        ConvertToSmallestCurrency();
                    }
                }
            }
        }

        /// <summary>
        /// Register callbacks for inspector
        /// </summary>
        protected virtual void RegisterCallbacks()
        {
            OnAddCharacter += (Player player, Slot slot) =>
            {
                if (CharacterManager.IsLoaded)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    eventData.slot = slot;
                    Execute("OnAddCharacter", eventData);
                }
            };
            OnFailedToAddCharacter += (Player player) =>
            {
                if (CharacterManager.IsLoaded)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    Execute("OnFailedToAddCharacter", eventData);
                }
            };
            OnRemoveCharacter += (Player player, int amount, Slot slot) =>
            {
                if (CharacterManager.IsLoaded)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    eventData.slot = slot;
                    Execute("OnRemoveCharacter", eventData);
                }
            };
            OnFailedToRemoveCharacter += (Player player, int amount) =>
            {
                if (CharacterManager.IsLoaded)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    Execute("OnFailedToRemoveCharacter", eventData);
                }
            };
            OnTryUseCharacter += (Player player, Slot slot) =>
            {
                CharacterEventData eventData = new CharacterEventData(player);
                eventData.slot = slot;
                Execute("OnTryUseCharacter", eventData);
            };
            OnUseCharacter += (Player player, Slot slot) =>
            {
                CharacterEventData eventData = new CharacterEventData(player);
                eventData.slot = slot;
                Execute("OnUseCharacter", eventData);
            };

            OnDropCharacter += (Player player, GameObject droppedInstance) =>
            {
                CharacterEventData eventData = new CharacterEventData(player);
                eventData.gameObject = droppedInstance;
                Execute("OnDropCharacter", eventData);
            };
        }

        public static void RemoveCharacters(string windowName, bool keepInCollection = false)
        {
            CharacterContainer container = WidgetUtility.Find<CharacterContainer>(windowName);
            if (container != null)
                container.RemoveCharacters(keepInCollection);
        }

        /// <summary>
        /// Removes the amount/stack of characters in all containers with windowName
        /// </summary>
        /// <param name="windowName">Name of character containers.</param>
        /// <param name="player">Character to remove.</param>
        /// <param name="amount">Amount/Stack to remove.</param>
        /// <returns>Returns true if the stack of characters is removed.</returns>
        public static bool RemoveCharacter(string windowName, Player player, int amount)
        {
            if (!HasCharacter(windowName, player, amount))
            {
                return false;
            }

            int restAmount = amount;
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];
                int currentAmount = 0;
                current.HasCharacter(player, restAmount, out currentAmount);
                int removeAmount = Mathf.Clamp(currentAmount, 0, restAmount);
                current.RemoveCharacter(player, removeAmount);
                restAmount -= removeAmount;
                if (restAmount <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the player from collection it belongs to and all referenced slots
        /// </summary>
        /// <param name="player"></param>
        public static void RemoveCharacterCompletely(Player player)
        {
            if (player == null)
            {
                Debug.LogWarning("Can't remove character that is null.");
                return;
            }
            RemoveCharacterReferences(player);
            if (player.Container != null)
            {
                player.Container.RemoveCharacter(player);
            }
        }

        /// <summary>
        /// Removes the character in all references.
        /// </summary>
        /// <param name="player">Character to remove the references from</param>
        public static void RemoveCharacterReferences(Player player)
        {
            if (player == null) { return; }
            for (int i = 0; i < player.ReferencedSlots.Count; i++)
            {
                //character.ReferencedSlots[i].Container.OnRemoveCharacter(character, character.Stack, character.ReferencedSlots[i]);
                player.ReferencedSlots[i].Container.NotifyRemoveCharacter(player, player.Stack, player.ReferencedSlots[i]);
                player.ReferencedSlots[i].ObservedCharacter = null;
            }
            player.ReferencedSlots.Clear();
        }

        /// <summary>
        /// Checks in all containers named by windowName if the amount of characters exists.
        /// </summary>
        /// <param name="windowName">The name of character containers</param>
        /// <param name="player">The character to check.</param>
        /// <param name="amount">Required amount/stack.</param>
        /// <returns>True if the containers have the amount of characters.</returns>
        public static bool HasCharacter(string windowName, Player player, int amount)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            int existingAmount = 0;
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];
                int currentAmount = 0;
                if (current.HasCharacter(player, amount, out currentAmount))
                {
                    existingAmount += currentAmount;
                }
            }
            return existingAmount >= amount;
        }

        /// <summary>
        /// Checks in all containers named by windowName if an character with category exists.
        /// </summary>
        public static bool HasCategoryCharacter(string windowName, Category category)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];
                if (current.HasCategoryCharacter(category))
                {
                    return true;
                }

            }
            return false;
        }

        //Get the player in any container
        public static Player GetCharacter(string nameOrId)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>();
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];

                Player player = current.GetCharacters(nameOrId).FirstOrDefault();
                if (player != null)
                    return player;
            }
            return null;
        }

        /// <summary>
        /// Get the first character in container by name.
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Player GetCharacter(string windowName, string nameOrId)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];

                Player player = current.GetCharacters(nameOrId).FirstOrDefault();
                if (player != null)
                    return player;
            }
            return null;
        }

        /// <summary>
        /// Get the character amount
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetCharacterAmount(string windowName, string nameOrId)
        {
            int currentAmount = 0;
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];


                Player[] characters = current.GetCharacters(nameOrId);
                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i] != null)
                        currentAmount += characters[i].Stack;
                }
            }
            return currentAmount;
        }

        /// <summary>
        /// Adds characters to character container. This will look up for all containers ordered by priority.
        /// </summary>
        /// <param name="windowName">Name of the character container.</param>
        /// <param name="players">Characters to add.</param>
        /// <param name="allowStacking">Should the characters be stacked, if possible?</param>
        /// <returns>True if characters were stacked or added.</returns>
        public static bool AddCharacters(string windowName, Player[] players, bool allowStacking = true)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            List<Player> checkedCharacters = new List<Player>(players);

            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player character = checkedCharacters[i];
                if (windows.Length > 0)
                {
                    for (int j = 0; j < windows.Length; j++)
                    {
                        CharacterContainer current = windows[j];

                        if ((allowStacking && current.StackOrAdd(checkedCharacters[i])) || (!allowStacking && current.AddCharacter(checkedCharacters[i])))
                        {
                            checkedCharacters.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            return checkedCharacters.Count == 0;
        }

        /// <summary>
        /// Adds an character to character container. This will look up for all containers ordered by priority.
        /// </summary>
        /// <param name="windowName">Name of the character container.</param>
        /// <param name="player">Character to add.</param>
        /// <param name="allowStacking">Should the character be stacked, if possible?</param>
        /// <returns>True if character was stacked or added.</returns>
        public static bool AddCharacter(string windowName, Player player, bool allowStacking = true)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];

                if ((allowStacking && current.StackOrAdd(player)) || (!allowStacking && current.AddCharacter(player)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the characters can be added to container.
        /// </summary>
        /// <param name="windowName">Name of character container.</param>
        /// <param name="player">Character to check</param>
        /// <param name="createSlot">Should a slot be created if the container is dynamic.</param>
        /// <returns>True if character can be added.</returns>
        public static bool CanAddCharacters(string windowName, Player[] players, bool createSlot = false)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            List<Player> checkedCharacters = new List<Player>(players);

            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player player = checkedCharacters[i];
                if (windows.Length > 0)
                {
                    for (int j = 0; j < windows.Length; j++)
                    {
                        CharacterContainer current = windows[j];
                        Slot slot;
                        if (current.CanAddCharacter(player, out slot, createSlot))
                        {
                            checkedCharacters.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            return checkedCharacters.Count == 0;
        }

        /// <summary>
        /// Checks if the items can be stacks.
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="players"></param>
        /// <param name="createSlot"></param>
        /// <returns></returns>
        public static bool CanStackCharacters(string windowName, Player[] players)
        {
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            List<Player> checkedCharacters = new List<Player>(players);

            for (int i = checkedCharacters.Count - 1; i >= 0; i--)
            {
                Player player = checkedCharacters[i];
                if (windows.Length > 0)
                {
                    for (int j = 0; j < windows.Length; j++)
                    {
                        CharacterContainer current = windows[j];
                        if (current.CanStack(player))
                        {
                            checkedCharacters.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            return checkedCharacters.Count == 0;
        }

        /// <summary>
        /// Checks if the character can be added to container.
        /// </summary>
        /// <param name="windowName">Name of character container.</param>
        /// <param name="player">Character to check</param>
        /// <param name="createSlot">Should a slot be created if the container is dynamic.</param>
        /// <returns>True if character can be added.</returns>
        public static bool CanAddCharacter(string windowName, Player player, bool createSlot = false)
        {
            Slot slot;
            return CanAddCharacter(windowName, player, out slot, createSlot);
        }

        /// <summary>
        /// Checks if the Character can be added to container.
        /// </summary>
        /// <param name="windowName">Name of character container.</param>
        /// <param name="player">Character to check</param>
        /// <param name="slot">Required or next free slot where to add.</param>
        /// <param name="createSlot">Should a slot be created if the container is dynamic.</param>
        /// <returns>True if character can be added.</returns>
        public static bool CanAddCharacter(string windowName, Player player, out Slot slot, bool createSlot = false)
        {
            slot = null;
            CharacterContainer[] windows = WidgetUtility.FindAll<CharacterContainer>(windowName);
            for (int j = 0; j < windows.Length; j++)
            {
                CharacterContainer current = windows[j];
                if (current.CanAddCharacter(player, out slot, createSlot))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Cooldown(Player player, float globalCooldown)
        {
            CharacterContainer[] containers = WidgetUtility.FindAll<CharacterContainer>();
            for (int i = 0; i < containers.Length; i++)
            {
                CharacterContainer mContainer = containers[i];
                mContainer.CooldownSlots(player, globalCooldown);

            }
        }

        private void CooldownSlots(Player player, float globalCooldown)
        {
            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                CharacterSlot slot = this.m_Slots[i] as CharacterSlot;
                if (slot != null && !slot.IsEmpty)
                {
                    if (player is UsableCharacter && slot.ObservedCharacter == player)
                    {
                        slot.Cooldown((player as UsableCharacter).Cooldown);
                    }
                    else if (slot.ObservedCharacter.Category == player.Category)
                    {
                        slot.Cooldown(player.Category.Cooldown);
                    }
                    else
                    {
                        slot.Cooldown(globalCooldown);
                    }
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (CanDragIn && CharacterSlot.dragObject != null)
            {
                for (int j = 0; j < this.m_Slots.Count; j++)
                {
                    if (SwapCharacters(this.m_Slots[j], CharacterSlot.dragObject.slot))
                    {
                        CharacterSlot.dragObject.slot.Repaint();
                        CharacterSlot.dragObject = null;
                        return;
                    }
                }
            }
        }

        public void NotifyDropCharacter(Player player, GameObject instance)
        {
            OnDropCharacter(player, instance);
        }

        public void NotifyUseCharacter(Player player, Slot slot)
        {
            OnUseCharacter(player, slot);
        }

        public void NotifyTryUseCharacter(Player player, Slot slot)
        {
            OnTryUseCharacter(player, slot);
        }

        public void NotifyAddCharacter(Player player, Slot slot)
        {
            if (CharacterManager.IsLoaded)
            {
                OnAddCharacter(player, slot);
            }
        }

        public void NotifyRemoveCharacter(Player player, int amount, Slot slot)
        {
            if (CharacterManager.IsLoaded)
                OnRemoveCharacter(player, amount, slot);
        }

        public void MoveTo(string windowName)
        {
            Player[] players = GetCharacters<Player>(true);
            CharacterContainer container = WidgetUtility.Find<CharacterContainer>(windowName);
            for (int i = 0; i < players.Length; i++)
            {
                if (container.StackOrAdd(players[i]))
                {

                    RemoveCharacter(players[i]);
                }
            }
        }

        [System.Serializable]
        public class MoveCharacterCondition
        {
            public string window;
            public bool requiresVisibility = true;
        }
    }
}
