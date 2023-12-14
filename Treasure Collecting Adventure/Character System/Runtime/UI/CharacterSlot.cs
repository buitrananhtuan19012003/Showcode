using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LupinrangerPatranger.UIWidgets;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// Key to use character.
		/// </summary>
        [SerializeField]
        protected KeyCode m_UseKey;

        /*private bool m_IsCooldown;*/
        /// <summary>
        /// Gets a value indicating whether this slot is in cooldown.
        /// </summary>
        /// <value><c>true</c> if this slot is in cooldown; otherwise, <c>false</c>.</value>
        public bool IsCooldown
        {
            get { return !IsEmpty && ObservedCharacter.IsInCooldown; }
        }
        //protected float cooldownDuration;
        //protected float cooldownInitTime;

        private static DragObject m_DragObject;
        public static DragObject dragObject
        {
            get { return m_DragObject; }
            set
            {
                m_DragObject = value;
                //Set the dragging icon
                if (m_DragObject != null && m_DragObject.player != null)
                {
                    UICursor.Set(m_DragObject.player.Icon);
                }
                else
                {
                    //if value is null, remove the dragging icon
                    UICursor.Clear();
                }
            }
        }
        protected Coroutine m_DelayTooltipCoroutine;
        protected ScrollRect m_ParentScrollRect;
        protected bool m_IsMouseKey;

        protected override void Start()
        {
            base.Start();
            this.m_ParentScrollRect = GetComponentInParent<ScrollRect>();
            this.m_IsMouseKey = m_UseKey == KeyCode.Mouse0 || m_UseKey == KeyCode.Mouse1 || m_UseKey == KeyCode.Mouse2;
        }

        /// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(m_UseKey) && !UnityTools.IsPointerOverUI())
            {
                if (!(this.m_IsMouseKey && TriggerRaycaster.IsPointerOverTrigger()))
                    Use();
            }
            /*  if (Container != null && Container.IsVisible)
              {
                  UpdateCooldown();
              }*/
        }

        /*public override void Repaint()
        {
            base.Repaint();

            if (this.m_Description != null)
            {
                this.m_Description.text = (ObservedCharacter != null ? ObservedCharacter.Description : string.Empty);
            }

            if (this.m_Ingredients != null)
            {
                this.m_Ingredients.RemoveCharacters();
                if (!IsEmpty)
                {
                    for (int i = 0; i < ObservedCharacter.ingredients.Count; i++)
                    {
                        Characters ingredient = Instantiate(ObservedCharacter.ingredients[i].character);
                        ingredient.Stack = ObservedCharacter.ingredients[i].amount;
                        this.m_Ingredients.StackOrAdd(ingredient);
                    }
                }
            }

            if (this.m_EnchantingMaterials != null)
            {
                this.m_EnchantingMaterials.RemoveItems();
                if (!IsEmpty && ObservedItem.EnchantingRecipe != null)
                {
                    for (int i = 0; i < ObservedItem.EnchantingRecipe.Ingredients.Count; i++)
                    {
                        Item ingredient = Instantiate(ObservedItem.EnchantingRecipe.Ingredients[i].item);
                        ingredient.Stack = ObservedItem.EnchantingRecipe.Ingredients[i].amount;
                        this.m_EnchantingMaterials.StackOrAdd(ingredient);
                    }
                }
            }

            if (this.m_SlotPrefab != null)
            {
                for (int i = 0; i < this.m_SlotCache.Count; i++)
                {
                    this.m_SlotCache[i].gameObject.SetActive(false);
                }
                if (!IsEmpty)
                {
                    List<KeyValuePair<string, string>> pairs = ObservedCharacter.GetPropertyInfo();

                    if (pairs != null && pairs.Count > 0)
                    {
                        while (pairs.Count > this.m_SlotCache.Count)
                        {
                            CreateSlot();
                        }

                        for (int i = 0; i < pairs.Count; i++)
                        {
                            StringPairSlot slot = this.m_SlotCache[i];
                            slot.gameObject.SetActive(true);
                            slot.Target = pairs[i];
                        }
                        this.m_SlotPrefab.transform.parent.gameObject.SetActive(true);
                    }
                }
            }

            if (this.m_BuyPrice != null)
            {
                this.m_BuyPrice.RemoveCharacters();
                if (!IsEmpty)
                {
                    Currency price = Instantiate(ObservedCharacter.BuyCurrency);
                    price.Stack = Mathf.RoundToInt(ObservedCharacter.BuyPrice);
                    this.m_BuyPrice.StackOrAdd(price);
                }
            }
        }*/

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltip();
        }

        protected IEnumerator DelayTooltip(float delay)
        {
            float time = 0.0f;
            yield return true;
            while (time < delay)
            {
                time += Container.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                yield return true;
            }
            if (CharacterManager.UI.tooltip != null && ObservedCharacter != null)
            {
                CharacterManager.UI.tooltip.Show(UnityTools.ColorString(ObservedCharacter.DisplayName, ObservedCharacter.Rarity.Color), ObservedCharacter.Description, ObservedCharacter.Icon, ObservedCharacter.GetPropertyInfo());
                if (CharacterManager.UI.sellPriceTooltip != null && ObservedCharacter.IsSellable && ObservedCharacter.SellPrice > 0)
                {
                    CharacterManager.UI.sellPriceTooltip.RemoveCharacters();
                    Currency currency = Instantiate(ObservedCharacter.SellCurrency);
                    currency.Stack = ObservedCharacter.SellPrice * ObservedCharacter.Stack;

                    CharacterManager.UI.sellPriceTooltip.StackOrAdd(currency);
                    CharacterManager.UI.sellPriceTooltip.Show();
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseTooltip();
        }

        private void ShowTooltip()
        {
            if (Container.ShowTooltips && this.isActiveAndEnabled && dragObject == null && ObservedCharacter != null)
            {
                if (this.m_DelayTooltipCoroutine != null)
                {
                    StopCoroutine(this.m_DelayTooltipCoroutine);
                }
                this.m_DelayTooltipCoroutine = StartCoroutine(DelayTooltip(0.3f));
            }
        }

        private void CloseTooltip()
        {
            if (Container.ShowTooltips && CharacterManager.UI.tooltip != null)
            {
                CharacterManager.UI.tooltip.Close();
                if (CharacterManager.UI.sellPriceTooltip != null)
                {
                    CharacterManager.UI.sellPriceTooltip.RemoveCharacters();
                    CharacterManager.UI.sellPriceTooltip.Close();
                }
            }
            if (this.m_DelayTooltipCoroutine != null)
            {
                StopCoroutine(this.m_DelayTooltipCoroutine);
            }
        }

        // In order to receive OnPointerUp callbacks, we need implement the IPointerDownHandler interface
        public virtual void OnPointerDown(PointerEventData eventData) { }

        //Detects the release of the mouse button
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (!eventData.dragging)
            {
                Stack stack = CharacterManager.UI.stack;

                bool isUnstacking = stack != null && stack.player != null;

                if (!isUnstacking && CharacterManager.Input.unstackEvent.HasFlag<Configuration.Input.UnstackInput>(Configuration.Input.UnstackInput.OnClick) && Input.GetKey(CharacterManager.Input.unstackKeyCode) && ObservedCharacter.Stack > 1)
                {
                    Unstack();
                    return;
                }

                //Check if we are currently unstacking the character
                if (isUnstacking && Container.StackOrAdd(this, stack.player))
                {
                    stack.player = null;
                    UICursor.Clear();
                }

                if (isUnstacking)
                {
                    return;
                }

                if (ObservedCharacter == null)
                    return;

                if (Container.useButton.HasFlag((InputButton)Mathf.Clamp(((int)eventData.button * 2), 1, int.MaxValue)))
                {
                    Use();
                }

                else if (Container.UseContextMenu && Container.ContextMenuButton.HasFlag((InputButton)Mathf.Clamp(((int)eventData.button * 2), 1, int.MaxValue)))
                {
                    UIWidgets.ContextMenu menu = CharacterManager.UI.contextMenu;
                    if (menu == null) { return; }
                    menu.Clear();

                    if (Trigger.currentUsedTrigger != null && Trigger.currentUsedTrigger is VendorTrigger && Container.CanSellCharacters)
                    {
                        menu.AddMenuCharacter("Sell", Use);
                    }
                    else if (ObservedCharacter is UsableCharacter)
                    {
                        menu.AddMenuCharacter("Use", Use);

                    }
                    if (ObservedCharacter.MaxStack > 1 || ObservedCharacter.MaxStack == 0)
                    {
                        menu.AddMenuCharacter("Unstack", Unstack);
                    }

                    menu.AddMenuCharacter("Drop", DropCharacter);

                    if (ObservedCharacter.EnchantingRecipe != null)
                    {
                        menu.AddMenuCharacter("Enchant", delegate ()
                        {
                            CharacterContainer container = WidgetUtility.Find<CharacterContainer>("Enchanting");
                            container.Show();

                            container.ReplaceCharacter(0, ObservedCharacter);

                        });
                    }

                    if (ObservedCharacter.CanDestroy)
                        menu.AddMenuCharacter("Destroy", DestroyCharacter);

                    for (int i = 0; i < Container.ContextMenuFunctions.Count; i++)
                    {
                        int cnt = i;
                        if (!string.IsNullOrEmpty(Container.ContextMenuFunctions[cnt]))
                        {

                            menu.AddMenuCharacter(Container.ContextMenuFunctions[cnt], () => { Container.gameObject.SendMessage(Container.ContextMenuFunctions[cnt], ObservedCharacter, SendMessageOptions.DontRequireReceiver); });
                        }
                    }

                    menu.Show();
                }
            }
        }

        //Called by a BaseInputModule before a drag is started.
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (Container.IsLocked)
            {
                CharacterManager.Notifications.inUse.Show();
                return;
            }

            //Check if we can start dragging
            if (!IsEmpty && !ObservedCharacter.IsInCooldown && Container.CanDragOut)
            {
                //If key for unstacking characters is pressed and if the stack is greater then 1, show the unstack ui.
                if (CharacterManager.Input.unstackEvent.HasFlag<Configuration.Input.UnstackInput>(Configuration.Input.UnstackInput.OnDrag) && Input.GetKey(CharacterManager.Input.unstackKeyCode) && ObservedCharacter.Stack > 1)
                {
                    Unstack();
                }
                else
                {
                    //Set the dragging slot
                    // draggedSlot = this;
                    //if(base.m_Ícon == null || !base.m_Ícon.raycastTarget || eventData.pointerCurrentRaycast.gameObject == base.m_Ícon.gameObject)
                    if (eventData.pointerCurrentRaycast.gameObject != gameObject)
                        dragObject = new DragObject(this);

                }
            }
            if (this.m_ParentScrollRect != null && dragObject == null)
            {
                this.m_ParentScrollRect.OnBeginDrag(eventData);
            }
        }

        //When draging is occuring this will be called every time the cursor is moved.
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (this.m_ParentScrollRect != null)
            {
                this.m_ParentScrollRect.OnDrag(eventData);
            }
        }

        //Called by a BaseInputModule when a drag is ended.
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            RaycastHit hit;
            if (!UnityTools.IsPointerOverUI() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (Container.CanDropCharacters)
                {
                    DropCharacter();
                }
                else if (Container.UseReferences && Container.CanDragOut)
                {
                    Container.RemoveCharacter(Index);

                }
            }

            dragObject = null;

            if (this.m_ParentScrollRect != null)
            {
                this.m_ParentScrollRect.OnEndDrag(eventData);
            }
            //Repaint the slot
            Repaint();
        }

        //Called by a BaseInputModule on a target that can accept a drop.
        public virtual void OnDrop(PointerEventData data)
        {
            if (dragObject != null && Container.CanDragIn)
            {
                Container.StackOrSwap(this, dragObject.slot);
            }
        }

        //Try to drop the character to ground
        private void DropCharacter()
        {
            if (Container.IsLocked)
            {
                CharacterManager.Notifications.inUse.Show();
                return;
            }

            if (ObservedCharacter.IsInCooldown)
                return;

            //Get the character to drop
            Player player = dragObject != null ? dragObject.player : ObservedCharacter;

            //Check if the character is droppable
            if (player != null && player.IsDroppable)
            {
                //Get character prefab
                GameObject prefab = player.OverridePrefab != null ? player.OverridePrefab : player.Prefab;
                RaycastHit hit;
                Vector3 position = Vector3.zero;
                Vector3 forward = Vector3.zero;
                if (CharacterManager.current.PlayerInfo.transform != null)
                {
                    position = CharacterManager.current.PlayerInfo.transform.position;
                    forward = CharacterManager.current.PlayerInfo.transform.forward;
                }

                //Cast a ray from mouse postion to ground
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && !UnityTools.IsPointerOverUI())
                {
                    //Clamp the drop distance to max drop distance defined in setting.
                    Vector3 worldPos = hit.point;
                    Vector3 diff = worldPos - position;
                    float distance = diff.magnitude;
                    //if player is null this does not work!
                    if (distance > (CharacterManager.DefaultSettings.maxDropDistance - (transform.localScale.x / 2)))
                    {
                        position = position + (diff / distance) * CharacterManager.DefaultSettings.maxDropDistance;
                    }
                    else
                    {
                        position = worldPos;
                    }
                }
                else
                {
                    position = position + forward;
                }

                //Instantiate the prefab at position
                GameObject go = CharacterManager.Instantiate(prefab, position + Vector3.up * 0.3f, Quaternion.identity);
                go.name = go.name.Replace("(Clone)", "");
                //Reset the character collection of the prefab with this character
                CharacterCollection collection = go.GetComponent<CharacterCollection>();
                if (collection != null)
                {
                    collection.Clear();
                    collection.Add(player);
                }
                //PlaceCharacter placeCharacter = go.GetComponentInChildren<PlaceCharacter>(true);
                //if (placeCharacter != null)
                //    placeCharacter.enabled = true;

                CharacterContainer.RemoveCharacterCompletely(player);
                Container.NotifyDropCharacter(player, go);
            }
        }

        //Unstack characters
        private void Unstack()
        {

            if (CharacterManager.UI.stack != null)
            {
                CharacterManager.UI.stack.SetCharacter(ObservedCharacter);
            }
        }

        private void DestroyCharacter()
        {
            Container.RemoveCharacter(Index);
        }

        /// <summary>
        /// Set the slot in cooldown
        /// </summary>
        /// <param name="duration">In seconds</param>
        public void Cooldown(float duration)
        {
            //if (!m_IsCooldown && duration > 0f)
            // {
            ObservedCharacter.SetCooldown(duration);
            //  cooldownDuration = cooldown;
            // cooldownInitTime = Time.time;
            // this.m_IsCooldown = true;
            //}
        }

        /// <summary>
        /// Updates the cooldown image and sets if the slot is in cooldown.
        /// </summary>
       /* private void UpdateCooldown()
        {
            if (this.m_IsCooldown && this.m_CooldownOverlay != null)
            {
                if (Time.time - cooldownInitTime < cooldownDuration)
                {
                    if (this.m_Cooldown != null) {
                        this.m_Cooldown.text = (cooldownDuration - (Time.time - cooldownInitTime)).ToString("f1");
                    }
                    this.m_CooldownOverlay.fillAmount = Mathf.Clamp01(1f - ((Time.time - cooldownInitTime) / cooldownDuration));
                }else{
                    if(this.m_Cooldown != null)
                        this.m_Cooldown.text = string.Empty;

                    this.m_CooldownOverlay.fillAmount = 0f;
                }
            }
            this.m_IsCooldown = (cooldownDuration - (Time.time - cooldownInitTime)) > 0f;
        }*/

        public override void Use()
        {
            if (Container.IsLocked)
            {
                CharacterManager.Notifications.inUse.Show();
                return;
            }

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
                    CloseTooltip();
                    ObservedCharacter.Use();
                    Container.NotifyUseCharacter(ObservedCharacter, this);
                }
                else
                {
                    CloseTooltip();
                    ShowTooltip();
                }

            }
            else if (IsCooldown && !IsEmpty)
            {
                CharacterManager.Notifications.inCooldown.Show(ObservedCharacter.DisplayName, (ObservedCharacter.CooldownDuration - (Time.time - ObservedCharacter.CooldownDuration)).ToString("f2"));
            }
        }

        //Can we use the character
        public override bool CanUse()
        {
            return ObservedCharacter != null && !ObservedCharacter.IsInCooldown;
        }

        /*  protected virtual StringPairSlot CreateSlot()
        {
            if (this.m_SlotPrefab != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab.gameObject);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotPrefab.transform.parent, false);
                StringPairSlot slot = go.GetComponent<StringPairSlot>();
                this.m_SlotCache.Add(slot);

                return slot;
            }
            Debug.LogWarning("[ItemSlot] Please ensure that the slot prefab is set in the inspector.");
            return null;
        }*/

        public class DragObject
        {
            public CharacterContainer container;
            public Slot slot;
            public Player player;

            public DragObject(Slot slot)
            {
                this.slot = slot;
                this.container = slot.Container;
                this.player = slot.ObservedCharacter;
            }
        }
    }
}
