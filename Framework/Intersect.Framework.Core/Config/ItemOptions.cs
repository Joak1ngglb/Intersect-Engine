using System.Diagnostics.CodeAnalysis;
using Intersect.Enums;
using Newtonsoft.Json;

namespace Intersect.Config;

/// <summary>
/// Configuration options for items.
/// </summary>
public class ItemOptions
{
    /// <summary>
    /// The available rarity tiers.
    /// </summary>
    /// <remarks>This is not intended to be a localized string, please see Strings for localization.</remarks>
    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public List<string> RarityTiers { get; set; } =
    [
        @"None",
        @"Common",
        @"Uncommon",
        @"Rare",
        @"Epic",
        @"Legendary",
    ];

    public bool TryGetRarityName(int rarityLevel, [NotNullWhen(true)] out string? rarityName)
    {
        rarityName = RarityTiers.Skip(rarityLevel).FirstOrDefault();
        return rarityName != default;
    }
    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public Dictionary<ItemType, List<string>> ItemSubtypes { get; set; } = new()
{
    { ItemType.Consumable, new() { "Drink", "Food", "Potion", "Scroll" } },
    { ItemType.Equipment, new() { "Axe", "Bow", "Dagger", "Hammer", "Spear", "Staff", "Sword", "Wand" } },
    { ItemType.Resource, new()
        {
            "Blood", "Bone", "Cereal", "Claw", "Crystal", "Ear", "Essence", "Eye", "Fabric", "Feather", "Fiber", "Fish",
            "Fruits", "Gem", "Hair", "Herb", "Hide", "Horn", "Ink", "Leather", "Meat", "Mushrooms", "Oil", "Ore", "Orb",
            "Paw", "Plant", "Powder", "Root", "Rune", "Scale", "Seed", "Shell", "Soul", "Tail", "Vegetables", "Wing", "Wood"
        }
    }
};

    public List<string> GetSubtypesFor(ItemType type)
    {
        return ItemSubtypes.TryGetValue(type, out var subtypes) ? subtypes : new List<string>();
    }

}
