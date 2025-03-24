using Intersect.Enums;
using Intersect.GameObjects.Ranges;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace Intersect.Framework.Core.GameObjects.Items;

public partial class EquipmentProperties
{
    [JsonIgnore]
    public ItemRange StatRange_Attack
    {
        get => StatRanges.TryGetValue(Stat.Attack, out var range) ? range : StatRange_Attack = new ItemRange();
        set => StatRanges[Stat.Attack] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_AbilityPower
    {
        get =>
            StatRanges.TryGetValue(Stat.AbilityPower, out var range) ? range : StatRange_AbilityPower = new ItemRange();
        set => StatRanges[Stat.AbilityPower] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_Defense
    {
        get => StatRanges.TryGetValue(Stat.Defense, out var range) ? range : StatRange_Defense = new ItemRange();
        set => StatRanges[Stat.Defense] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_MagicResist
    {
        get =>
            StatRanges.TryGetValue(Stat.MagicResist, out var range) ? range : StatRange_MagicResist = new ItemRange();
        set => StatRanges[Stat.MagicResist] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_Speed
    {
        get => StatRanges.TryGetValue(Stat.Speed, out var range) ? range : StatRange_Speed = new ItemRange();
        set => StatRanges[Stat.Speed] = value;
    }
    public ItemRange StatRange_ArmorPenetration
    {
        get => StatRanges.TryGetValue(Stat.ArmorPenetration, out var range) ? range : StatRange_ArmorPenetration = new ItemRange();
        set => StatRanges[Stat.ArmorPenetration] = value;
    }
    public ItemRange StatRange_Vitality
    {
        get => StatRanges.TryGetValue(Stat.Vitality, out var range) ? range : StatRange_Vitality = new ItemRange();
        set => StatRanges[Stat.Vitality] = value;
    }
    public ItemRange StatRange_Wisdom
    {
        get => StatRanges.TryGetValue(Stat.Wisdom, out var range) ? range : StatRange_Wisdom = new ItemRange();
        set => StatRanges[Stat.Wisdom] = value;
    }
}