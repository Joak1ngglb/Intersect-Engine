using System;
using Intersect.Server.Database.PlayerData.Players;

using Intersect.Server.Entities;
using Intersect.Network.Packets.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intersect.Server.Database.PlayerData
{
    public class MarketTransaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ListingId { get; set; }

        public string BuyerName { get; set; }

        public virtual Player Seller { get; set; }

        public Guid ItemId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
        [NotMapped] 
        public ItemProperties ItemProperties { get; set; }

        public DateTime SoldAt { get; set; } = DateTime.UtcNow;
    }
}
