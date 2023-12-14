using System.Security.Cryptography;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    public class SwapCharacters : MonoBehaviour
    {
        public CharacterSlot first;
        public CharacterSlot second;
        public CharacterSlot third;
        public CharacterSlot fourth;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                first.Container.SwapCharacters(first, second);
            }
            //if (Input.GetKeyDown(KeyNumber.2))
            //{
            //    first.Container.SwapCharacters(first, second);
            //}
        }
    }
}