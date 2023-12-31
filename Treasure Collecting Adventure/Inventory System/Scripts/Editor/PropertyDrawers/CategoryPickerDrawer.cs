﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace LupinrangerPatranger.InventorySystem
{
    [CustomPropertyDrawer(typeof(Category))]
    public class CategoryPickerDrawer : PickerDrawer<Category>
    {
        protected override List<Category> GetItems(ItemDatabase database)
        {
            return database.categories;
        }
    }
}