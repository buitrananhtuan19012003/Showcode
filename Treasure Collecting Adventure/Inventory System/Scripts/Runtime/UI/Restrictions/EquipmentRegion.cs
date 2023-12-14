using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LupinrangerPatranger.InventorySystem.Restrictions
{
    public class EquipmentRegion : Restriction
    {
        public LupinrangerPatranger.InventorySystem.EquipmentRegion region;

        public override bool CanAddItem(Item item)
        {
            if (region == null) {
                Debug.LogWarning("The restriction EquipmentRegion has a null reference. This can happen when you delete the region in database but not update your slots. Remove the restriction or add a reference.");
                return true; 
            }
            if (item == null || !(item is EquipmentItem equipmentItem)) { return false; }

            List<LupinrangerPatranger.InventorySystem.EquipmentRegion> requiredRegions = new List<LupinrangerPatranger.InventorySystem.EquipmentRegion>(equipmentItem.Region);

            Restrictions.EquipmentRegion[] restrictions = GetComponents<Restrictions.EquipmentRegion>();
            for (int i = requiredRegions.Count - 1; i >= 0; i--)
            {
                if (restrictions.Select(x => x.region.Name).Contains(requiredRegions[i].Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}