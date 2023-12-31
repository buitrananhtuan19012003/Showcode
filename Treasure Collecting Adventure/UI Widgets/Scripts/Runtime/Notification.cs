﻿using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.Text;

namespace LupinrangerPatranger.UIWidgets
{
    public class Notification : UIContainer<NotificationOptions>
    {
        public bool fade = true;
        public string timeFormat = "HH:mm:ss";

        public virtual bool AddItem(NotificationOptions item, params string[] replacements)
        {
            NotificationOptions options = new NotificationOptions(item);
            for (int i = 0; i < replacements.Length; i++)
            {
                options.text = options.text.Replace("{" + i + "}", replacements[i]);
            }
            return base.AddItem(options);
        }

        public virtual bool AddItem(string text, params string[] replacements)
        {
            NotificationOptions options = new NotificationOptions();
            options.text = text;
            for (int i = 0; i < replacements.Length; i++)
            {
                options.text = options.text.Replace("{" + i + "}", replacements[i]);
            }
            return base.AddItem(options);
        }

        public virtual bool AddCharacter(NotificationOptions player, params string[] replacements)
        {
            NotificationOptions options = new NotificationOptions(player);
            for (int i = 0; i < replacements.Length; i++)
            {
                options.text = options.text.Replace("{" + i + "}", replacements[i]);
            }
            return base.AddCharacter(options);
        }

        public virtual bool AddCharacter(string text, params string[] replacements)
        {
            NotificationOptions options = new NotificationOptions();
            options.text = text;
            for (int i = 0; i < replacements.Length; i++)
            {
                options.text = options.text.Replace("{" + i + "}", replacements[i]);
            }
            return base.AddCharacter(options);
        }

        public override bool CanAddItem(NotificationOptions item, out UISlot<NotificationOptions> slot, bool createSlot = false)
        {
            slot = null;
            return gameObject.activeInHierarchy && base.CanAddItem(item, out slot, createSlot);
        }
        public override bool CanAddCharacter(NotificationOptions player, out UISlot<NotificationOptions> slot, bool createSlot = false)
        {
            slot = null;
            return gameObject.activeInHierarchy && base.CanAddCharacter(player, out slot, createSlot);
        }
    }
}