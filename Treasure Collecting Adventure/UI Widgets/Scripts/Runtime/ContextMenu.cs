using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LupinrangerPatranger.UIWidgets
{
    public class ContextMenu : UIWidget
    {
        [Header("Reference")]
        [SerializeField]
        protected MenuItem m_MenuItemPrefab = null;
        protected MenuCharacter m_MenuCharacterPrefab = null;
        protected List<MenuItem> itemCache = new List<MenuItem>();
        protected List<MenuCharacter> playerCache = new List<MenuCharacter>();

        public override void Show()
        {
            m_RectTransform.position = Input.mousePosition;
            base.Show();
        }

        protected override void Update()
        {
            base.Update();
            if (m_CanvasGroup.alpha > 0f && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;
                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                for (int i = 0; i < raycastResults.Count; i++)
                {
                    MenuItem item = raycastResults[i].gameObject.GetComponent<MenuItem>();
                    if (item != null)
                    {
                        item.OnPointerClick(pointer);
                        return;
                    }

                    MenuCharacter player = raycastResults[i].gameObject.GetComponent<MenuCharacter>();
                    if (player != null)
                    {
                        player.OnPointerClick(pointer);
                        return;
                    }
                }
                Close();
            }
        }

        public virtual void Clear()
        {
            for (int i = 0; i < itemCache.Count; i++)
            {
                itemCache[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < playerCache.Count; i++)
            {
                playerCache[i].gameObject.SetActive(false);
            }
        }

        public virtual MenuItem AddMenuItem(string text, UnityAction used)
        {
            MenuItem item = itemCache.Find(x => !x.gameObject.activeSelf);

            if (item == null)
            {
                item = Instantiate(m_MenuItemPrefab) as MenuItem;
                itemCache.Add(item);
            }
            Text itemText = item.GetComponentInChildren<Text>();

            if (itemText != null)
            {
                itemText.text = text;
            }
            item.onTrigger.RemoveAllListeners();
            item.gameObject.SetActive(true);
            item.transform.SetParent(m_RectTransform, false);
            item.onTrigger.AddListener(delegate ()
            {
                Close();
                if (used != null)
                {
                    used.Invoke();
                }
            });
            return item;
        }

        public virtual MenuCharacter AddMenuCharacter(string text, UnityAction used)
        {
            MenuCharacter player = playerCache.Find(x => !x.gameObject.activeSelf);

            if (player == null)
            {
                player = Instantiate(m_MenuCharacterPrefab) as MenuCharacter;
                playerCache.Add(player);
            }

            Text playerText = player.GetComponentInChildren<Text>();

            if (playerText != null)
            {
                playerText.text = text;
            }
            player.onTrigger.RemoveAllListeners();
            player.gameObject.SetActive(true);
            player.transform.SetParent(m_RectTransform, false);
            player.onTrigger.AddListener(delegate ()
            {
                Close();
                if (used != null)
                {
                    used.Invoke();
                }
            });
            return player;
        }
    }
}