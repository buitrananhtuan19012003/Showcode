using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CurrencySlot : Slot
    {
        [SerializeField]
        protected Currency m_Currency;
        /// <summary>
        /// Hides the empty currency slot
        /// </summary>
        [SerializeField]
        protected bool m_HideEmptySlot;

        public Currency GetDefaultCurrency()
        {
            Currency currency = Instantiate(this.m_Currency);
            currency.Stack = 0;
            return currency;
        }

        public override void Repaint()
        {
            base.Repaint();

            if (this.m_HideEmptySlot)
            {
                gameObject.SetActive(!(ObservedCharacter == null || ObservedCharacter.Stack == 0));
            }
        }

        public override bool CanAddCharacter(Player player)
        {
            return base.CanAddCharacter(player) && typeof(Currency).IsAssignableFrom(player.GetType());
        }

        public override bool CanUse()
        {
            return false;
        }
    }
}