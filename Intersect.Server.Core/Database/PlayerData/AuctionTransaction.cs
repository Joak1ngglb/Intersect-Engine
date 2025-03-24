using System;
using System.ComponentModel.DataAnnotations;

namespace Intersect.Server.Database.PlayerData
{
    public class AuctionTransaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
