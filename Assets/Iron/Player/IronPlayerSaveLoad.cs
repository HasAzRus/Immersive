using System;
using System.Collections.Generic;
using Oxygen;

namespace Iron
{
    public static class IronPlayerSaveLoad
    {
        private const string HealthKey = "Health";
        private const string StaminaKey = "Stamina";
        private const string EnergyKey = "Energy";
        private const string ItemNames = "Item Names";
        private const string ItemCounts = "Item Counts";
        
        public static void Save(IWritableStorage storage, IronPlayerData data)
        {
            var items = data.Items;
            var itemsCount = items.Length;
            
            var names = new string[itemsCount];
            var counts = new int[itemsCount];

            for (var i = 0; i < itemsCount; i++)
            {
                names[i] = items[i].Name;
                counts[i] = items[i].Count;
            }
            
            storage.Write(HealthKey, data.Health);
            storage.Write(StaminaKey, data.Stamina);
            storage.Write(EnergyKey, data.Energy);
            storage.Write(ItemNames, names);
            storage.Write(ItemCounts, counts);
        }

        public static IronPlayerData Load(IReadableStorage storage)
        {
            var names = storage.Read(ItemNames, Array.Empty<string>());
            var counts = storage.Read(ItemCounts, Array.Empty<int>());

            var items = new List<IItem>();

            for (var i = 0; i < names.Length; i++)
            {
               items.Add(new Item(names[i], counts[i])); 
            }

            var data = new IronPlayerData()
            {
                Health = storage.Read(HealthKey, 100f),
                Stamina = storage.Read(StaminaKey, 100f),
                Energy = storage.Read(EnergyKey, 100f),
                Items = items.ToArray()
            };

            return data;
        }
    }
}