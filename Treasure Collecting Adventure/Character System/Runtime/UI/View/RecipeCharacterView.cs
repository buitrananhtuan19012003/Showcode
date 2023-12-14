using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class RecipeCharacterView : CharacterView
    {
        /// <summary>
        /// Character container that will show crafting ingredients of the item
        /// </summary>
        [Tooltip("CharacterContainer that will show crafting ingredients of the item.")]
        [SerializeField]
        protected CharacterContainer m_Ingredients;

        [Tooltip("What recipe should be displayed?")]
        [SerializeField]
        protected RecipeType m_RecipeType = RecipeType.Crafting;

        public override void Repaint(Player player)
        {
            if (this.m_Ingredients != null)
            {
                this.m_Ingredients.RemoveCharacters();

                if (player != null)
                {
                    CraftingRecipe recipe = this.m_RecipeType == RecipeType.Crafting ? player.CraftingRecipe : player.EnchantingRecipe;
                    if (recipe != null)
                    {
                        this.m_Ingredients.gameObject.SetActive(true);
                        for (int i = 0; i < recipe.Ingredients.Count; i++)
                        {
                            Player ingredient = Instantiate(recipe.Ingredients[i].player);
                            ingredient.Stack = recipe.Ingredients[i].amount;
                            this.m_Ingredients.StackOrAdd(ingredient);
                        }
                    }
                    else
                    {
                        this.m_Ingredients.gameObject.SetActive(false);
                    }
                }
            }
        }

        public enum RecipeType
        {
            Crafting,
            Enchanting
        }
    }
}