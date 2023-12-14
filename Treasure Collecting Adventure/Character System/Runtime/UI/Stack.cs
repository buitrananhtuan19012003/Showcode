using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [RequireComponent(typeof(Spinner))]
    public class Stack : UIWidget
    {
        [HideInInspector]
        public Player player;
        private Canvas canvas;
        private Spinner spinner;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.canvas = GetComponentInParent<Canvas>();
            this.spinner = GetComponent<Spinner>();
        }

        public void SetCharacter(Player player)
        {
            if (player != null)
            {
                this.player = player;
                this.spinner.min = 1;
                this.spinner.max = player.Stack;
                this.spinner.step = 1;

                Show();
                UpdatePosition();
            }
        }

        public void Unstack()
        {
            if (player != null)
            {
                int amount = Mathf.RoundToInt(spinner.current);
                player.Stack -= amount;
                Player newCharacter = (Player)Instantiate(player);
                newCharacter.Rarity = player.Rarity;
                newCharacter.Stack = amount;
                player = newCharacter;
                UICursor.Set(player.Icon);
                base.Close();
            }
        }

        public void Cancel()
        {
            player = null;
        }

        private void UpdatePosition()
        {
            Vector3 currentPosition = GetCurrentPosition();
            transform.position = currentPosition;
            Focus();
        }

        private Vector3 GetCurrentPosition()
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            Vector2 offset = Vector2.zero;

            if (Input.mousePosition.x < m_RectTransform.sizeDelta.x)
            {
                offset += new Vector2(m_RectTransform.sizeDelta.x * 0.5f, 0);
            }
            else
            {
                offset += new Vector2(-m_RectTransform.sizeDelta.x * 0.5f, 0);
            }
            if (Screen.height - Input.mousePosition.y > m_RectTransform.sizeDelta.y)
            {
                offset += new Vector2(0, m_RectTransform.sizeDelta.y * 0.5f);
            }
            else
            {
                offset += new Vector2(0, -m_RectTransform.sizeDelta.y * 0.5f);
            }
            pos = pos + offset;

            return canvas.transform.TransformPoint(pos);
        }
    }
}