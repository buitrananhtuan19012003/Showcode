using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CraftingTrigger : Trigger
    {
        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnCraftStart");
                callbacks.Add("OnFailedCraftStart");
                callbacks.Add("OnCraftCharacter");
                callbacks.Add("OnFailedToCraftCharacter");
                callbacks.Add("OnCraftStop");
                return callbacks.ToArray();
            }
        }

        [Header("Crafting")]
        [SerializeField]
        protected string m_IngredientsWindow = "Player";
        [SerializeField]
        protected string m_StorageWindow = "Player";
        [SerializeField]
        protected string m_CraftingProgressbar = "CraftingProgress";

        protected static void Execute(ITriggerFailedCraftStart handler, Player player, GameObject players, FailureCause failureCause)
        {
            handler.OnFailedCraftStart(player, players, failureCause);
        }

        protected static void Execute(ITriggerCraftStart handler, Player player, GameObject players)
        {
            handler.OnCraftStart(player, players);
        }

        protected static void Execute(ITriggerCraftCharacter handler, Player player, GameObject players)
        {
            handler.OnCraftCharacter(player, players);
        }

        protected static void Execute(ITriggerFailedToCraftCharacter handler, Player player, GameObject players, FailureCause failureCause)
        {
            handler.OnFailedToCraftCharacter(player, players, failureCause);
        }

        protected static void Execute(ITriggerCraftStop handler, Player player, GameObject players)
        {
            handler.OnCraftStop(player, players);
        }

        protected CharacterContainer m_ResultStorageContainer;
        protected CharacterContainer m_RequiredIngredientsContainer;
        protected bool m_IsCrafting;
        protected float m_ProgressDuration;
        protected float m_ProgressInitTime;
        protected Progressbar m_Progressbar;
        protected Spinner m_AmountSpinner;

        protected override void Start()
        {
            base.Start();
            this.m_ResultStorageContainer = WidgetUtility.Find<CharacterContainer>(this.m_StorageWindow);
            this.m_RequiredIngredientsContainer = WidgetUtility.Find<CharacterContainer>(this.m_IngredientsWindow);
            this.m_Progressbar = WidgetUtility.Find<Progressbar>(this.m_CraftingProgressbar);

            CharacterContainer container = GetComponent<CharacterContainer>();
            if (container != null)
            {
                container.RegisterListener("OnShow", (CallbackEventData ev) => { InUse = true; });
                container.RegisterListener("OnClose", (CallbackEventData ev) => { InUse = false; });
            }
        }

        public override bool OverrideUse(Slot slot, Player player)
        {

            if (Trigger.currentUsedWindow == player.Container && !slot.MoveCharacter())
            {
                this.m_AmountSpinner = Trigger.currentUsedWindow.GetComponentInChildren<Spinner>();
                if (this.m_AmountSpinner != null)
                {
                    this.m_AmountSpinner.min = 1;
                    this.m_AmountSpinner.max = int.MaxValue;
                    StartCrafting(player, (int)this.m_AmountSpinner.current);
                }
                else
                {
                    StartCrafting(player, 1);
                }
            }
            return true;
        }

        protected override void Update()
        {
            base.Update();
            if (this.m_IsCrafting && this.m_Progressbar != null)
            {
                this.m_Progressbar.SetProgress(GetCraftingProgress());
            }
        }

        protected override void OnTriggerInterrupted()
        {
            StopAllCoroutines();
            this.m_IsCrafting = false;
            if (this.m_Progressbar != null)
                this.m_Progressbar.SetProgress(0f);
            GameObject user = CharacterManager.current.PlayerInfo.gameObject;
            if (user != null)
                user.SendMessage("SetControllerActive", true, SendMessageOptions.DontRequireReceiver);

            LoadCachedAnimatorStates();
        }

        private float GetCraftingProgress()
        {
            if (Time.time - m_ProgressInitTime < m_ProgressDuration)
            {
                return Mathf.Clamp01(((Time.time - m_ProgressInitTime) / m_ProgressDuration));
            }
            return 1f;
        }

        public void StartCrafting(Player player, int amount)
        {
            if (player == null)
            {
                CharacterManager.Notifications.selectCharacter.Show();
                ExecuteEvent<ITriggerFailedCraftStart>(Execute, player, FailureCause.FurtherAction);
                return;
            }
            if (this.m_IsCrafting)
            {
                NotifyAlreadyCrafting(player);
                return;
            }

            CraftingRecipe recipe = GetCraftingRecipe(player);
            if (recipe == null || !recipe.CheckConditions()) return;

            if (!HasIngredients(player))
            {
                NotifyMissingIngredients(player);
                return;
            }

            GameObject user = CharacterManager.current.PlayerInfo.gameObject;
            if (user != null)
            {
                user.SendMessage("SetControllerActive", false, SendMessageOptions.DontRequireReceiver);
                Animator animator = CharacterManager.current.PlayerInfo.animator;
                if (animator != null)
                    animator.CrossFadeInFixedTime(Animator.StringToHash(recipe.AnimatorState), 0.2f);

            }
            StartCoroutine(CraftCharacters(player, amount));
            ExecuteEvent<ITriggerCraftStart>(Execute, player);
        }

        private bool HasIngredients(Player player)
        {
            CraftingRecipe recipe = GetCraftingRecipe(player);
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                if (!this.m_RequiredIngredientsContainer.HasCharacter(recipe.Ingredients[i].player, recipe.Ingredients[i].amount))
                {
                    return false;
                }
            }
            return true;
        }

        protected virtual CraftingRecipe GetCraftingRecipe(Player player)
        {
            return player.CraftingRecipe;
        }

        public void StopCrafting(Player player)
        {
            this.m_IsCrafting = false;
            GameObject user = CharacterManager.current.PlayerInfo.gameObject;
            if (user != null)
                user.SendMessage("SetControllerActive", true, SendMessageOptions.DontRequireReceiver);

            LoadCachedAnimatorStates();
            StopCoroutine("CraftCharacters");
            ExecuteEvent<ITriggerCraftStop>(Execute, player);
            if (this.m_Progressbar != null)
                this.m_Progressbar.SetProgress(0f);
        }

        private IEnumerator CraftCharacters(Player player, int amount)
        {
            this.m_IsCrafting = true;
            for (int i = 0; i < amount; i++)
            {
                if (HasIngredients(player))
                {
                    yield return StartCoroutine(CraftCharacter(player));
                }
                else
                {
                    NotifyMissingIngredients(player);
                    break;
                }
            }
            StopCrafting(player);
            this.m_IsCrafting = false;
        }

        protected virtual IEnumerator CraftCharacter(Player player)
        {
            CraftingRecipe recipe = GetCraftingRecipe(player);
            this.m_ProgressDuration = recipe.Duration;
            this.m_ProgressInitTime = Time.time;
            yield return new WaitForSeconds(recipe.Duration);

            //if (recipe.Skill != null)
            //{
            //    Skill skill = ItemContainer.GetItem(recipe.Skill.Id) as Skill;
            //    if (skill != null && !skill.CheckSkill())
            //    {
            //        NotifyFailedToCraft(item, FailureCause.Unknown);
            //        if (recipe.RemoveIngredientsWhenFailed)
            //        {
            //            for (int j = 0; j < recipe.Ingredients.Count; j++)
            //            {
            //                this.m_RequiredIngredientsContainer.RemoveItem(recipe.Ingredients[j].item, recipe.Ingredients[j].amount);
            //            }
            //        }
            //        yield break;
            //    }
            //}

            Player craftedCharacter = Instantiate(player);
            craftedCharacter.Stack = 1;
            recipe.CraftingModifier.Modify(craftedCharacter);

            if (this.m_ResultStorageContainer.StackOrAdd(craftedCharacter))
            {
                for (int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    this.m_RequiredIngredientsContainer.RemoveCharacter(recipe.Ingredients[i].player, recipe.Ingredients[i].amount);
                }
                NotifyCharacterCrafted(craftedCharacter);
            }
            else
            {
                NotifyFailedToCraft(player, FailureCause.ContainerFull);
                StopCrafting(player);
            }
        }


        protected virtual void NotifyAlreadyCrafting(Player player)
        {
            CharacterManager.Notifications.alreadyCrafting.Show();
            ExecuteEvent<ITriggerFailedCraftStart>(Execute, player, FailureCause.InUse);
        }

        protected virtual void NotifyMissingIngredients(Player player)
        {
            CharacterManager.Notifications.missingIngredient.Show();
            ExecuteEvent<ITriggerFailedCraftStart>(Execute, player, FailureCause.Requirement);
        }

        protected virtual void NotifyCharacterCrafted(Player player)
        {
            CharacterManager.Notifications.craftedCharacter.Show(UnityTools.ColorString(player.Name, player.Rarity.Color));
            ExecuteEvent<ITriggerCraftCharacter>(Execute, player);
        }

        protected virtual void NotifyFailedToCraft(Player player, FailureCause cause)
        {
            switch (cause)
            {
                case FailureCause.ContainerFull:
                    CharacterManager.Notifications.containerFull.Show(this.m_ResultStorageContainer.Name);
                    ExecuteEvent<ITriggerFailedToCraftCharacter>(Execute, player, FailureCause.ContainerFull);
                    break;
                case FailureCause.Unknown:
                    CharacterManager.Notifications.failedToCraft.Show(player.DisplayName);
                    ExecuteEvent<ITriggerFailedToCraftCharacter>(Execute, player, FailureCause.Unknown);
                    break;
            }
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            this.m_CallbackHandlers.Add(typeof(ITriggerFailedCraftStart), "OnFailedCraftStart");
            this.m_CallbackHandlers.Add(typeof(ITriggerCraftStart), "OnCraftStart");
            this.m_CallbackHandlers.Add(typeof(ITriggerCraftCharacter), "OnCraftItem");
            this.m_CallbackHandlers.Add(typeof(ITriggerFailedToCraftCharacter), "OnFailedToCraftItem");
            this.m_CallbackHandlers.Add(typeof(ITriggerCraftStop), "OnCraftStop");
        }
    }
}