using System.ComponentModel.DataAnnotations.Schema;

using Intersect.Models;
using Intersect.GameObjects.Events;

using Newtonsoft.Json;
using Intersect.GameObjects.Conditions;
using Intersect.Config;
using Intersect.Framework.Core.Config;

namespace Intersect.GameObjects.Crafting;

public partial class CraftBase : DatabaseObject<CraftBase>, IFolderable
{
    [NotMapped]
    public List<CraftIngredient> Ingredients { get; set; } = [];

    [JsonConstructor]
    public CraftBase(Guid id) : base(id)
    {
        Name = "New Craft";
    }

    //Parameterless constructor for EF
    public CraftBase()
    {
        Name = "New Craft";
    }

    [JsonIgnore]
    [Column("Ingredients")]
    public string IngredientsJson
    {
        get => JsonConvert.SerializeObject(Ingredients, Formatting.None);
        protected set => Ingredients = JsonConvert.DeserializeObject<List<CraftIngredient>>(value);
    }

    [JsonProperty(Order = -6)]
    public int ItemLossChance { get; set; } = 100;

    [JsonProperty(Order = -5)]
    public int FailureChance { get; set; } = 0;

    [JsonProperty(Order = -4)]
    public Guid ItemId { get; set; }

    [JsonProperty(Order = -3)]
    public int Quantity { get; set; } = 1;

    [JsonProperty(Order = -2)]
    public int Time { get; set; }

    /// <inheritdoc />
    public string Folder { get; set; } = string.Empty;

    [Column("Event")]
    [JsonProperty]
    public Guid EventId { get; set; }

    [NotMapped]
    [JsonIgnore]
    public EventBase Event
    {
        get => EventBase.Get(EventId);
        set => EventId = value?.Id ?? Guid.Empty;
    }
    // Propiedad para almacenar el tipo de trabajo
    public JobType Jobs { get; set; }
    // Propiedad para almacenar la cantidad de experiencia que otorga
    public int ExperienceAmount { get; set; }
    [NotMapped]
    public ConditionLists CraftingRequirements { get; set; } = new();

    //Requirements
    [Column("CraftingRequirements")]
    [JsonIgnore]
    public string JsonCraftingRequirements
    {
        get => CraftingRequirements.Data();
        set => CraftingRequirements.Load(value ?? "[]");
    }
}

public partial class CraftIngredient
{
    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public CraftIngredient(Guid itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    public ItemBase GetItem()
    {
        return ItemBase.Get(ItemId);
    }
}
