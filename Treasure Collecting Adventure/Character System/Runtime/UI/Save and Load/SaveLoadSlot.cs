using UnityEngine;
using UnityEngine.EventSystems;
using LupinrangerPatranger.UIWidgets;
using ContextMenu = LupinrangerPatranger.UIWidgets.ContextMenu;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LupinrangerPatranger.CharacterSystem
{
    public class SaveLoadSlot : MonoBehaviour, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            string key = GetComponentInChildren<Text>().text;
            DialogBox dialogBox = CharacterManager.UI.dialogBox;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ContextMenu menu = CharacterManager.UI.contextMenu;
                menu.Clear();
                menu.AddMenuCharacter("Load", () =>
                {
                    dialogBox.Show("Load", "Are you sure you want to load this save? ", null, (int result) =>
                    {
                        if (result != 0) return;
                        CharacterManager.Load(key);
                    }, "Yes", "No");

                });
                menu.AddMenuCharacter("Save", () =>
                {
                    dialogBox.Show("Save", "Are you sure you want to overwrite this save? ", null, (int result) =>
                    {
                        if (result != 0) return;
                        List<string> keys = PlayerPrefs.GetString("CharacterSystemSavedKeys").Split(';').ToList();
                        int index = keys.IndexOf(key);
                        CharacterManager.Delete(key);
                        CharacterManager.Save(DateTime.UtcNow.ToString(), index);
                    }
                    , "Yes", "No");
                });
                menu.AddMenuCharacter("Delete", () =>
                {
                    dialogBox.Show("Delete", "Are you sure you want to delete this save? ", null, (int result) =>
                    {
                        if (result != 0) return;
                        CharacterManager.Delete(key);
                        DestroyImmediate(gameObject);
                    }, "Yes", "No");
                });
                menu.Show();
            }
            else
            {
                dialogBox.Show("Load", "Are you sure you want to load this save? ", null, (int result) =>
                {
                    if (result != 0) return;
                    CharacterManager.Load(key);
                }, "Yes", "No");

            }
        }
    }
}