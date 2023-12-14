using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class PriceCharacterView : CharacterView
    {
        /// <summary>
        /// Character Container that will show the price of item.
        /// </summary>
        [Tooltip("CharacterContainer reference to display the price.")]
        [SerializeField]
        protected CharacterContainer m_Price;
        [Tooltip("What price should be displayed?")]
        [SerializeField]
        protected PriceType m_PriceType = PriceType.Buy;

        public override void Repaint(Player player)
        {
            if (this.m_Price != null)
            {
                this.m_Price.RemoveCharacters();
                if (player != null && player.IsSellable)
                {
                    this.m_Price.gameObject.SetActive(true);
                    Currency price = Instantiate(this.m_PriceType == PriceType.Buy ? player.BuyCurrency : player.SellCurrency);
                    price.Stack = Mathf.RoundToInt(this.m_PriceType == PriceType.Buy ? player.BuyPrice : player.SellPrice);
                    this.m_Price.StackOrAdd(price);
                }
                else
                {
                    this.m_Price.gameObject.SetActive(false);
                }
            }
        }

        public enum PriceType
        {
            Buy,
            Sell
        }
    }
}