using System.Linq;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem.Restrictions
{
    public class Category : Restriction
    {
        [SerializeField]
        private LupinrangerPatranger.CharacterSystem.Category[] m_Categories = null;
        [SerializeField]
        private bool invert = false;
        public override bool CanAddCharacter(Player player)
        {
            if (this.m_Categories.Contains(null))
            {
                Debug.LogWarning("The restriction Category has a null reference. This can happen when you delete the category in database but not update your slots/container. Remove the restriction or add a reference.");
                return true;
            }

            if (player == null) { return false; }

            for (int i = 0; i < this.m_Categories.Length; i++)
            {
                if (this.m_Categories[i].IsAssignable(player.Category))
                {
                    return !invert;
                }
            }
            return invert;
        }
    }
}