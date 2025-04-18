using System;
using System.Collections.Generic;
using System.Linq;
using Intersect.GameObjects;
using Intersect.Server.Database.PlayerData;

public class MarketStatistics
{
    public Guid ItemId { get; set; }
    public int TotalSold { get; set; }
    public int TotalRevenue { get; set; }
    public int NumberOfSales { get; set; }

    public float AveragePrice => NumberOfSales > 0 ? (float)TotalRevenue / NumberOfSales : 0;

    public MarketStatistics(Guid itemId, IEnumerable<MarketTransaction> transactions)
    {
        ItemId = itemId;

        foreach (var tx in transactions)
        {
            AddTransaction(tx);
        }
    }

    public MarketStatistics(Guid itemId)
    {
        ItemId = itemId;
    }

    public int GetMinAllowedPrice(float marginPercent = 0.5f)
    {
        return (int)Math.Floor(AveragePrice * (1f - marginPercent));
    }

    public int GetMaxAllowedPrice(float marginPercent = 0.5f)
    {
        return (int)Math.Ceiling(AveragePrice * (1f + marginPercent));
    }

    public void AddTransaction(MarketTransaction tx)
    {
        if (tx == null) return;

        NumberOfSales++;
        TotalSold += tx.Quantity;
        TotalRevenue += tx.Price;
    }
}
