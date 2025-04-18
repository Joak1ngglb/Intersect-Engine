using System;
using System.Collections.Generic;
using System.Linq;
using Intersect.Server.Database;
using Intersect.Server.Database.PlayerData;

public static class MarketStatisticsManager
{
    private static readonly Dictionary<Guid, MarketStatistics> _statisticsCache = new();

  
    public static void LoadFromDatabase()
    {
        using var context = Intersect.Server.Database.DbInterface.CreatePlayerContext(readOnly: true);

        var allTransactions = context.Market_Transactions.ToList();

        var grouped = allTransactions
            .GroupBy(tx => tx.ItemId);

        foreach (var group in grouped)
        {
            var stats = new MarketStatistics(group.Key, group);
            _statisticsCache[group.Key] = stats;
        }

        Intersect.Logging.Log.Info($"[MarketStatistics] Se cargaron estadísticas de {grouped.Count()} ítems del historial de transacciones.");
    }

    /// <summary>
    /// Intenta obtener las estadísticas de un ítem específico.
    /// </summary>
    public static bool TryGetStats(Guid itemId, out MarketStatistics stats)
    {
        return _statisticsCache.TryGetValue(itemId, out stats);
    }

    /// <summary>
    /// Actualiza las estadísticas al realizar una nueva venta.
    /// </summary>
    public static void UpdateStatistics(MarketTransaction tx)
    {
        if (!_statisticsCache.TryGetValue(tx.ItemId, out var stats))
        {
            stats = new MarketStatistics(tx.ItemId, new List<MarketTransaction>());
            _statisticsCache[tx.ItemId] = stats;
        }

        stats.AddTransaction(tx);
    }
    public static MarketStatistics GetStatistics(Guid itemId)
    {
        if (_statisticsCache.TryGetValue(itemId, out var stats))
        {
            return stats;
        }

        // Si no está en caché, lo cargamos desde la base de datos
        using var context = DbInterface.CreatePlayerContext(readOnly: true);
        var transactions = context.Market_Transactions
            .Where(t => t.ItemId == itemId)
            .ToList();

        stats = new MarketStatistics(itemId, transactions);
        _statisticsCache[itemId] = stats;

        return stats;
    }

}