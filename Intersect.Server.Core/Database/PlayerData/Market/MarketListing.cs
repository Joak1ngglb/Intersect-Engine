using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Entities;
using Intersect.Network.Packets.Server;

namespace Intersect.Server.Database.PlayerData
{
    public class MarketListing
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public virtual Player Seller { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
        [NotMapped]
        public ItemProperties ItemProperties { get; set; }

        public DateTime ListedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpireAt { get; set; } = DateTime.UtcNow.AddDays(28); // duración de publicación

        public bool IsSold { get; set; } = false;
    }
}
