using LupinrangerPatranger.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class EnchantingTrigger : CraftingTrigger
    {

        public override bool OverrideUse(Slot slot, Player player)
        {
            if (!slot.MoveCharacter())
            {
                StartCrafting(player, 1);
            }
            return true;
        }

        protected override CraftingRecipe GetCraftingRecipe(Player player)
        {
            return player.EnchantingRecipe;
        }

        protected override IEnumerator CraftCharacter(Player player)
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

            recipe.CraftingModifier.Modify(player);
            for (int i = 0; i < player.ReferencedSlots.Count; i++)
            {
                player.ReferencedSlots[i].Repaint();
            }

            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                this.m_RequiredIngredientsContainer.RemoveCharacter(recipe.Ingredients[i].player, recipe.Ingredients[i].amount);
            }
            NotifyCharacterCrafted(player);
        }

        protected override void NotifyAlreadyCrafting(Player player)
        {
            CharacterManager.Notifications.alreadyEnchanting.Show();
            ExecuteEvent<ITriggerFailedCraftStart>(Execute, player, FailureCause.InUse);
        }

        protected override void NotifyMissingIngredients(Player player)
        {
            CharacterManager.Notifications.missingMaterials.Show();
            ExecuteEvent<ITriggerFailedCraftStart>(Execute, player, FailureCause.Requirement);
        }

        protected override void NotifyCharacterCrafted(Player player)
        {
            CharacterManager.Notifications.enchantedCharacter.Show(UnityTools.ColorString(player.Name, player.Rarity.Color));
            ExecuteEvent<ITriggerCraftCharacter>(Execute, player);
        }

        protected override void NotifyFailedToCraft(Player player, FailureCause cause)
        {
            switch (cause)
            {
                case FailureCause.ContainerFull:
                    CharacterManager.Notifications.containerFull.Show(this.m_ResultStorageContainer.Name);
                    ExecuteEvent<ITriggerFailedToCraftCharacter>(Execute, player, FailureCause.ContainerFull);
                    break;
                case FailureCause.Unknown:
                    CharacterManager.Notifications.failedToEnchant.Show(player.DisplayName);
                    ExecuteEvent<ITriggerFailedToCraftCharacter>(Execute, player, FailureCause.Unknown);
                    break;
            }
        }

    }
}