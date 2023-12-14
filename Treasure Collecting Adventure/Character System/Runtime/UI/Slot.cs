using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;

namespace LupinrangerPatranger.CharacterSystem
{
    public class Slot : CallbackHandler
    {
        /* /// <summary>
        /// The text to display item name.
        /// </summary>
        [SerializeField]
        protected Text m_ItemName;
        /// <summary>
        /// Should the name be colored?
        /// </summary>
        [SerializeField]
        protected bool m_UseRarityColor=false;

        /// <summary>
        /// The Image to display item icon.
        /// </summary>
        [SerializeField]
        protected Image m_Ícon;
        /// <summary>
		/// The text to display item stack.
		/// </summary>
		[SerializeField]
        protected Text m_Stack;*/

        //Actions to run when the trigger is used.
        [HideInInspector]
        public List<Restriction> restrictions = new List<Restriction>();

        private Player m_Character;
        /// <summary>
        /// The player this slot is holding
        /// </summary>
        public Player ObservedCharacter
        {
            get { return this.m_Character; }
            set
            {
                this.m_Character = value;
                Repaint();
            }
        }

        /// <summary>
        /// Checks if the slot is empty ObservedPlayer == null
        /// </summary>
        public bool IsEmpty
        {
            get { return ObservedCharacter == null; }
        }

        private CharacterContainer m_Container;

        public CharacterContainer Container
        {
            get { return this.m_Container; }
            set { this.m_Container = value; }
        }

        private int m_Index = -1;
        /// <summary>
        /// Index of item container
        /// </summary>
        public int Index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>();
                callbacks.Add("OnAddCharacter");
                callbacks.Add("OnRemoveCharacter");
                callbacks.Add("OnUseCharacter");
                return callbacks.ToArray();
            }
        }

        protected CharacterView[] m_Views;

        protected virtual void Start()
        {
            Container.OnAddCharacter += (Player player, Slot slot) =>
            {
                if (slot == this)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    eventData.slot = slot;
                    Execute("OnAddCharacter", eventData);
                }
            };

            Container.OnRemoveCharacter += (Player player, int amount, Slot slot) =>
            {
                if (slot == this)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    eventData.slot = slot;
                    Execute("OnRemovePlayer", eventData);
                }
            };

            Container.OnUseCharacter += (Player player, Slot slot) =>
            {
                if (slot == this)
                {
                    CharacterEventData eventData = new CharacterEventData(player);
                    eventData.slot = slot;
                    Execute("OnUseCharacter", eventData);
                }
            };
        }

        /// <summary>
        /// Repaint slot visuals with item information
        /// </summary>
        public virtual void Repaint()
        {
            if (this.m_Views == null)
                this.m_Views = GetComponentsInChildren<CharacterView>().Where(x => x.GetComponentsInParent<Slot>(true).FirstOrDefault() == this).ToArray();

            for (int i = 0; i < this.m_Views.Length; i++)
            {
                this.m_Views[i].Repaint(ObservedCharacter);
            }

            /* if (this.m_ItemName != null){
                //Updates the text with item name and rarity color. If this slot is empty, sets the text to empty.
                this.m_ItemName.text = (!IsEmpty ? (this.m_UseRarityColor?UnityTools.ColorString(ObservedItem.DisplayName, ObservedItem.Rarity.Color):ObservedItem.DisplayName) : string.Empty);
            }

            if (this.m_Ícon != null){
                if (!IsEmpty){
                    //Updates the icon and enables it.
                    this.m_Ícon.overrideSprite = ObservedItem.Icon;
                    this.m_Ícon.enabled = true;
                }else {
                    //If there is no item in this slot, disable icon
                    this.m_Ícon.enabled = false;
                }
            }

            if (this.m_Stack != null) {
                if (!IsEmpty && ObservedItem.MaxStack > 1 ){
                    //Updates the stack and enables it.
                    this.m_Stack.text = ObservedItem.Stack.ToString();
                    this.m_Stack.enabled = true;
                }else{
                    //If there is no item in this slot, disable stack field
                    this.m_Stack.enabled = false;
                }
            }*/
        }

        protected virtual void Update()
        {
            if (this.m_Views == null)
                this.m_Views = GetComponentsInChildren<CharacterView>();

            for (int i = 0; i < this.m_Views.Length; i++)
            {
                if (this.m_Views[i].RequiresConstantRepaint())
                    this.m_Views[i].Repaint(ObservedCharacter);
            }
        }

        //Use the player
        public virtual void Use()
        {
            Container.NotifyTryUseCharacter(ObservedCharacter, this);
            //Check if the character can be used.
            if (CanUse())
            {
                //Check if there is an override character behavior on trigger.
                if ((Trigger.currentUsedTrigger as Trigger) != null && (Trigger.currentUsedTrigger as Trigger).OverrideUse(this, ObservedCharacter))
                {
                    return;
                }
                if (Container.UseReferences)
                {
                    ObservedCharacter.Slot.Use();
                    return;
                }
                //Try to move character
                if (!MoveCharacter())
                {
                    Debug.Log("use");
                    ObservedCharacter.Use();
                    Container.NotifyUseCharacter(ObservedCharacter, this);
                }
            }
        }

        //Checks if we can use the character in this slot
        public virtual bool CanUse()
        {
            return true;
        }

        /// <summary>
        /// Try to move player by move conditions set in inspector
        /// </summary>
        /// <returns>True if player was moved.</returns>
        public virtual bool MoveCharacter()
        {
            if (Container.MoveUsedCharacter)
            {
                for (int i = 0; i < Container.moveCharacterConditions.Count; i++)
                {
                    CharacterContainer.MoveCharacterCondition condition = Container.moveCharacterConditions[i];
                    CharacterContainer moveToContainer = WidgetUtility.Find<CharacterContainer>(condition.window);
                    if (moveToContainer == null || (condition.requiresVisibility && !moveToContainer.IsVisible))
                    {
                        continue;
                    }
                    if (moveToContainer.IsLocked)
                    {
                        CharacterManager.Notifications.inUse.Show();
                        continue;
                    }

                    if (moveToContainer.CanAddCharacter(ObservedCharacter) && moveToContainer.StackOrAdd(ObservedCharacter))
                    {
                        if (!moveToContainer.UseReferences || !Container.CanReferenceCharacters)
                        {
                            // Debug.Log("Move Character from "+Container.Name+" to "+moveToContainer.Name);

                            if (!moveToContainer.CanReferenceCharacters)
                            {
                                CharacterContainer.RemoveCharacterReferences(ObservedCharacter);
                            }
                            Container.RemoveCharacter(Index);
                        }
                        return true;
                    }
                    for (int j = 0; j < moveToContainer.Slots.Count; j++)
                    {
                        if (moveToContainer.CanSwapCharacters(moveToContainer.Slots[j], this) && moveToContainer.SwapCharacters(moveToContainer.Slots[j], this))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Can the player be added to this slot. This does not check if the slot is empty.
        /// </summary>
        /// <param name="player">The player to test adding.</param>
        /// <returns>Returns true if the player can be added.</returns>
        public virtual bool CanAddCharacter(Player player)
        {
            if (player == null) { return true; }
            for (int i = 0; i < restrictions.Count; i++)
            {
                if (!restrictions[i].CanAddCharacter(player))
                {
                    return false;
                }
            }
            return true;
        }
    }
}