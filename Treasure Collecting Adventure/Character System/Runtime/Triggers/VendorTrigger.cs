using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class VendorTrigger : Trigger, ITriggerUnUsedHandler
    {
        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnSelectSellCharacter");
                callbacks.Add("OnSoldCharacter");
                callbacks.Add("OnFaildToSellCharacter");
                callbacks.Add("OnSelectBuyCharacter");
                callbacks.Add("OnBoughtCharacter");
                callbacks.Add("OnFaildToBuyCharacter");
                return callbacks.ToArray();
            }
        }

        [Header("Vendor")]
        [Range(0f, 10f)]
        [SerializeField]
        protected float m_BuyPriceFactor = 1.0f;
        [Range(0f, 10f)]
        [SerializeField]
        protected float m_SellPriceFactor = 1.0f;
        [SerializeField]
        protected string m_PurchasedStorageWindow = "Player";
        [SerializeField]
        protected string m_PaymentWindow = "Player";
        [SerializeField]
        protected bool m_RemoveCharacterAfterPurchase = false;

        [Header("Buy & Sell Dialog")]
        [SerializeField]
        private string m_BuySellDialogName = "BuySellDialog";
        [SerializeField]
        private bool m_DisplaySpinner = true;

        [Header("Buy")]
        [SerializeField]
        private string m_BuyDialogTitle = "Buy";
        [SerializeField]
        private string m_BuyDialogText = "How many characters do you want to buy?";
        [SerializeField]
        private string m_BuyDialogButton = "Buy";

        [Header("Sell")]
        [SerializeField]
        private string m_SellDialogTitle = "Sell";
        [SerializeField]
        private string m_SellSingleDialogText = "Are you sure you want to sell this item?";
        [SerializeField]
        private string m_SellMultipleDialogText = "How many items do you want to sell?";
        [SerializeField]
        private string m_SellDialogButton = "Sell";

        private DialogBox m_BuySellDialog;
        private Spinner m_AmountSpinner;
        private CharacterContainer m_PriceInfo;

        private CharacterContainer m_PurchasedStorageContainer;
        private CharacterContainer m_PaymentContainer;

        protected static void Execute(ITriggerSelectSellCharacter handler, Player player, GameObject players)
        {
            handler.OnSelectSellCharacter(player, players);
        }

        protected static void Execute(ITriggerSoldCharacter handler, Player player, GameObject players)
        {
            handler.OnSoldCharacter(player, players);
        }

        protected static void Execute(ITriggerFailedToSellCharacter handler, Player player, GameObject players, FailureCause failureCause)
        {
            handler.OnFailedToSellCharacter(player, players, failureCause);
        }

        protected static void Execute(ITriggerSelectBuyCharacter handler, Player player, GameObject players)
        {
            handler.OnSelectBuyCharacter(player, players);
        }

        protected static void Execute(ITriggerBoughtCharacter handler, Player player, GameObject players)
        {
            handler.OnBoughtCharacter(player, players);
        }

        protected static void Execute(ITriggerFailedToBuyCharacter handler, Player player, GameObject players, FailureCause failureCause)
        {
            handler.OnFailedToBuyCharacter(player, players, failureCause);
        }

        protected override void Start()
        {
            base.Start();
            this.m_BuySellDialog = WidgetUtility.Find<DialogBox>(this.m_BuySellDialogName);
            if (this.m_BuySellDialog != null)
            {
                this.m_AmountSpinner = this.m_BuySellDialog.GetComponentInChildren<Spinner>();
                this.m_PriceInfo = this.m_BuySellDialog.GetComponentInChildren<CharacterContainer>();
            }
            this.m_PurchasedStorageContainer = WidgetUtility.Find<CharacterContainer>(this.m_PurchasedStorageWindow);
            this.m_PaymentContainer = WidgetUtility.Find<CharacterContainer>(this.m_PaymentWindow);

            /*  ItemCollection collection = GetComponent<ItemCollection>();
              for (int i = 0; i < collection.Count; i++) {
                  collection[i].BuyPrice = Mathf.RoundToInt(m_BuyPriceFactor*collection[i].BuyPrice);
              }*/
        }

        public override bool OverrideUse(Slot slot, Player player)
        {
            if (slot.Container.CanSellCharacters)
            {
                if (!player.IsSellable)
                {
                    CharacterManager.Notifications.cantSellCharacter.Show(player.DisplayName);
                    return true;
                }
                SellCharacter(player, player.Stack, true);
            }
            else if (Trigger.currentUsedWindow == slot.Container)
            {

                BuyCharacter(player, 1);
            }
            return true;
        }

        public void BuyCharacter(Player player, int amount, bool showDialog = true)
        {
            if (showDialog)
            {
                this.m_AmountSpinner.gameObject.SetActive(this.m_DisplaySpinner);

                this.m_AmountSpinner.onChange.RemoveAllListeners();
                this.m_AmountSpinner.current = 1;
                this.m_AmountSpinner.min = 1;
                ObjectProperty property = player.FindProperty("BuyBack");
                this.m_AmountSpinner.max = (this.m_RemoveCharacterAfterPurchase || property != null && property.boolValue) ? player.Stack : int.MaxValue;
                this.m_AmountSpinner.onChange.AddListener(delegate (float value)
                {
                    Currency price = Instantiate(player.BuyCurrency);
                    price.Stack = Mathf.RoundToInt(this.m_BuyPriceFactor * player.BuyPrice * value);
                    this.m_PriceInfo.RemoveCharacters();
                    this.m_PriceInfo.StackOrAdd(price);
                });
                this.m_AmountSpinner.onChange.Invoke(this.m_AmountSpinner.current);

                ExecuteEvent<ITriggerSelectBuyCharacter>(Execute, player);
                this.m_BuySellDialog.Show(this.m_BuyDialogTitle, this.m_BuyDialogText, player.Icon, delegate (int result)
                {
                    if (result == 0)
                    {
                        BuyCharacter(player, Mathf.RoundToInt(this.m_AmountSpinner.current), false);
                    }
                }, this.m_BuyDialogButton, "Cancel");
            }
            else
            {
                if (this.m_PurchasedStorageContainer == null || this.m_PaymentContainer == null)
                {
                    return;
                }
                Rarity rarity = player.Rarity;
                Player instance = Instantiate(player);

                instance.Rarity = rarity;
                instance.Stack = amount;
                Currency price = Instantiate(instance.BuyCurrency);
                price.Stack = Mathf.RoundToInt(this.m_BuyPriceFactor * instance.BuyPrice * amount);

                if (this.m_PaymentContainer.RemoveCharacter(price, price.Stack))
                {

                    if (amount > instance.MaxStack)
                    {
                        int stack = instance.Stack;
                        Currency singlePrice = Instantiate(instance.BuyCurrency);
                        singlePrice.Stack = Mathf.RoundToInt(instance.BuyPrice * this.m_BuyPriceFactor);
                        // singlePrice.Stack = Mathf.RoundToInt(this.m_BuyPriceFactor * singlePrice.Stack);
                        int purchasedStack = 0;
                        for (int i = 0; i < stack; i++)
                        {
                            Player singleCharacter = Instantiate(instance);
                            singleCharacter.Rarity = instance.Rarity;
                            singleCharacter.Stack = 1;
                            if (!this.m_PurchasedStorageContainer.StackOrAdd(singleCharacter))
                            {
                                this.m_PaymentContainer.StackOrAdd(singlePrice);
                                CharacterManager.Notifications.containerFull.Show(this.m_PurchasedStorageWindow);
                                ExecuteEvent<ITriggerFailedToBuyCharacter>(Execute, instance, FailureCause.ContainerFull);
                                break;
                            }
                            purchasedStack += 1;
                            ExecuteEvent<ITriggerBoughtCharacter>(Execute, singleCharacter);
                        }
                        if (this.m_RemoveCharacterAfterPurchase)
                        {
                            player.Container.RemoveCharacter(player, purchasedStack);
                        }
                        CharacterManager.Notifications.boughtCharacter.Show(purchasedStack.ToString() + "x" + instance.DisplayName, singlePrice.Stack * purchasedStack + " " + price.Name);
                    }
                    else
                    {
                        Player characterInstance = Instantiate(instance);
                        characterInstance.Rarity = instance.Rarity;

                        if (!this.m_PurchasedStorageContainer.StackOrAdd(characterInstance))
                        {
                            this.m_PaymentContainer.StackOrAdd(price);
                            CharacterManager.Notifications.containerFull.Show(this.m_PurchasedStorageWindow);
                            ExecuteEvent<ITriggerFailedToBuyCharacter>(Execute, instance, FailureCause.ContainerFull);
                        }
                        else
                        {
                            ObjectProperty property = player.FindProperty("BuyBack");

                            if (this.m_RemoveCharacterAfterPurchase || property != null && property.boolValue)
                            {
                                player.RemoveProperty("BuyBack");
                                player.Container.RemoveCharacter(player, amount);
                            }
                            CharacterManager.Notifications.boughtCharacter.Show(characterInstance.Name, price.Stack + " " + price.Name);
                            ExecuteEvent<ITriggerBoughtCharacter>(Execute, characterInstance);
                        }
                    }
                }
                else
                {
                    CharacterManager.Notifications.noCurrencyToBuy.Show(player.DisplayName, price.Stack + " " + price.Name);
                    ExecuteEvent<ITriggerFailedToBuyCharacter>(Execute, player, FailureCause.NotEnoughCurrency);
                }
            }
        }

        public void SellCharacter(Player player, int amount, bool showDialog = true)
        {
            if (showDialog)
            {
                this.m_AmountSpinner.gameObject.SetActive(this.m_DisplaySpinner);
                if (player.Stack > 1)
                {
                    this.m_AmountSpinner.onChange.RemoveAllListeners();
                    this.m_AmountSpinner.current = amount;
                    this.m_AmountSpinner.min = 1;
                    this.m_AmountSpinner.max = player.Stack;
                    this.m_AmountSpinner.onChange.AddListener(delegate (float value)
                    {
                        Currency price = Instantiate(player.SellCurrency);
                        price.Stack = Mathf.RoundToInt(this.m_SellPriceFactor * player.SellPrice * value);
                        this.m_PriceInfo.RemoveCharacters();
                        this.m_PriceInfo.StackOrAdd(price);
                    });
                    this.m_AmountSpinner.onChange.Invoke(this.m_AmountSpinner.current);
                }
                else
                {
                    this.m_AmountSpinner.current = 1;
                    this.m_AmountSpinner.gameObject.SetActive(false);
                    Currency price = Instantiate(player.SellCurrency);
                    price.Stack = Mathf.RoundToInt(this.m_SellPriceFactor * player.SellPrice);
                    this.m_PriceInfo.RemoveCharacters();
                    this.m_PriceInfo.StackOrAdd(price);
                }

                ExecuteEvent<ITriggerSelectSellCharacter>(Execute, player);
                this.m_BuySellDialog.Show(this.m_SellDialogTitle, player.Stack > 1 ? this.m_SellMultipleDialogText : this.m_SellSingleDialogText, player.Icon, delegate (int result)
                {
                    if (result == 0)
                    {
                        SellCharacter(player, Mathf.RoundToInt(this.m_AmountSpinner.current), false);
                    }

                }, this.m_SellDialogButton, "Cancel");
            }
            else
            {
                Currency price = Instantiate(player.SellCurrency);
                price.Stack = Mathf.RoundToInt(this.m_SellPriceFactor * player.SellPrice * amount);

                if (player.Container.RemoveCharacter(player, amount))
                {
                    ExecuteEvent<ITriggerSoldCharacter>(Execute, player);
                    this.m_PaymentContainer.StackOrAdd(price);
                    if (player.CanBuyBack)
                    {
                        player.AddProperty("BuyBack", true);
                        Trigger.currentUsedWindow.AddCharacter(player);
                    }
                    CharacterManager.Notifications.soldCharacter.Show((amount > 1 ? amount.ToString() + "x" : "") + player.Name, price.Stack + " " + price.Name);
                }
                else
                {
                    ExecuteEvent<ITriggerFailedToSellCharacter>(Execute, player, FailureCause.Remove);
                }
            }
        }

        public void OnTriggerUnUsed(GameObject player)
        {
            if (Trigger.currentUsedTrigger == this && this.m_BuySellDialog.IsVisible)
            {
                this.m_BuySellDialog.Close();
            }
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            this.m_CallbackHandlers.Add(typeof(ITriggerSelectSellCharacter), "OnSelectSellCharacter");
            this.m_CallbackHandlers.Add(typeof(ITriggerSoldCharacter), "OnSoldCharacter");
            this.m_CallbackHandlers.Add(typeof(ITriggerFailedToSellCharacter), "OnFailedToSellCharacter");
            this.m_CallbackHandlers.Add(typeof(ITriggerSelectBuyCharacter), "OnSelectBuyCharacter");
            this.m_CallbackHandlers.Add(typeof(ITriggerBoughtCharacter), "OnBoughtCharacter");
            this.m_CallbackHandlers.Add(typeof(ITriggerFailedToBuyCharacter), "OnFailedToBuyCharacter");
        }
    }
}