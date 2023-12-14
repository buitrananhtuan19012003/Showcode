using LupinrangerPatranger.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.InventorySystem
{
    public class SwapItems : MonoBehaviour
    {
        public KeyCode key = KeyCode.T;
        public ItemSlot first;
        public ItemSlot second;

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                first.Container.SwapItems(first, second);
            }
        }
    }
}