using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterEventData : CallbackEventData
    {
        public Player player;
        public Slot slot;
        public GameObject gameObject;

        public CharacterEventData(Player player)
        {
            this.player = player;
            if (player != null)
            {
                this.slot = player.Slot;
                this.gameObject = player.Prefab;
            }
        }
    }
}