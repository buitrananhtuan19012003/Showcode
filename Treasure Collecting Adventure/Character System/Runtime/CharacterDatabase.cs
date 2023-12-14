using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class CharacterDatabase : ScriptableObject
    {
        public List<Player> players = new List<Player>();
        public List<Currency> currencies = new List<Currency>();
        public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();
        public List<Rarity> raritys = new List<Rarity>();
        public List<Category> categories = new List<Category>();
        //public List<EquipmentRegion> equipments = new List<EquipmentRegion>();
        public List<Configuration.Settings> settings = new List<Configuration.Settings>();

        public List<Player> allPlayers
        {
            get
            {
                List<Player> all = new List<Player>(players);
                all.AddRange(currencies);
                return all;
            }
        }

        public GameObject GetCharacterPrefab(string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                if (player != null && player.Prefab != null && player.Prefab.name == name)
                {
                    return player.Prefab;
                }
            }
            return null;
        }

        public void Merge(CharacterDatabase database)
        {
            players.AddRange(database.players.Where(y => !players.Any(z => z.Name == y.Name)));
            currencies.AddRange(database.currencies.Where(y => !currencies.Any(z => z.Name == y.Name)));
            raritys.AddRange(database.raritys.Where(y => !raritys.Any(z => z.Name == y.Name)));
            categories.AddRange(database.categories.Where(y => !categories.Any(z => z.Name == y.Name)));
            craftingRecipes.AddRange(database.craftingRecipes.Where(y => !craftingRecipes.Any(z => z.Name == y.Name)));
        }

        public void RemoveNullReferences()
        {
            this.players.RemoveAll(x => x == null);
            this.currencies.RemoveAll(x => x == null);
            this.raritys.RemoveAll(x => x == null);
            this.categories.RemoveAll(x => x == null);
            this.craftingRecipes.RemoveAll(x => x == null);
        }
    }
}