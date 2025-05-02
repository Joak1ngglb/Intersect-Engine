using Intersect.Enums;
using Intersect.GameObjects;
public static class ItemBreakHelper
{
    private static readonly Dictionary<Stat, List<ItemBase>> StatToOrbs = new();

    public static void InitializeOrbs()
    {
        StatToOrbs.Clear();

        foreach (var item in ItemBase.Lookup.Values)
        {
            if (item is ItemBase itemBase && itemBase.Subtype == "Orb")
            {
                if (!StatToOrbs.ContainsKey(itemBase.TargetStat))
                    StatToOrbs[itemBase.TargetStat] = new List<ItemBase>();

                StatToOrbs[itemBase.TargetStat].Add(itemBase);
            }
        }
    }

    public static List<ItemBase> CalculateOrbsFromItem(ItemBase item)
    {
        var result = new List<ItemBase>();

        int itemRarity = item.Rarity;
        int rarityMultiplier = 1 + itemRarity;   // ejemplo: común=1, raro=2, épico=3...  
        int maxOrbsPerStat = 5;

        for (int i = 0; i < item.StatsGiven.Length; i++)
        {
            int baseStat = item.StatsGiven[i];
            int percentStat = item.PercentageStatsGiven[i];
            int totalStatValue = baseStat + (int)Math.Floor(baseStat * (percentStat / 100f));

            if (totalStatValue <= 0) continue;

            if (!StatToOrbs.TryGetValue((Stat)i, out var orbsForStat)) continue;

            var filteredOrbs = orbsForStat
                .Where(o => IsOrbAllowedForRarity(o, itemRarity))
                .OrderBy(o => o.AmountModifier)
                .ToList();

            if (filteredOrbs.Count == 0) continue;

            int guaranteedCount = (totalStatValue / 10) * rarityMultiplier;
            double extraChance = (totalStatValue % 10) / 10.0;

            guaranteedCount = Math.Min(guaranteedCount, maxOrbsPerStat);

            for (int j = 0; j < guaranteedCount; j++)
            {
                var chosenOrb = RandomOrb(filteredOrbs);
                result.Add(chosenOrb);
            }

            if (Random.Shared.NextDouble() < extraChance && result.Count < maxOrbsPerStat)
            {
                var chosenOrb = RandomOrb(filteredOrbs);
                result.Add(chosenOrb);
            }
        }

        return result;
    }

    private static bool IsOrbAllowedForRarity(ItemBase orb, int rarity)
    {
        if (orb.AmountModifier > 3 && rarity < 3) return false;
        return true;
    }

    private static ItemBase RandomOrb(List<ItemBase> orbs)
    {
        int index = Random.Shared.Next(orbs.Count);
        return orbs[index];
    }
}

