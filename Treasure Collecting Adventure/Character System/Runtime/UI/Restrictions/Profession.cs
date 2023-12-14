using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem.Restrictions
{
    public class Profession : Restriction
    {
        public override bool CanAddCharacter(Player player)
        {
            string profession = PlayerPrefs.GetString("Profession");

            //if (player == null || !(player is EquipmentItem equipmentItem)) { return false; }

            if (string.IsNullOrEmpty(profession)) return true;

            ObjectProperty property = player.FindProperty("Profession");
            if (property == null) return true;

            string[] professions = property.stringValue.Split(';');
            for (int i = 0; i < professions.Length; i++)
            {
                if (PlayerPrefs.GetString("Profession") == professions[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}