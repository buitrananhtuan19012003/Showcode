using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public abstract class Restriction : MonoBehaviour
    {
        protected Slot slot;
        protected CharacterContainer container;

        private void Start()
        {
            slot = GetComponent<Slot>();
            if (slot != null)
            {
                container = slot.Container;
            }
            else
            {
                container = GetComponent<CharacterContainer>();
            }
        }
        public abstract bool CanAddCharacter(Player player);
    }
}