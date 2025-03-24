using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Intersect.Enums;
using Intersect.Network.Packets.Server;
using Intersect.Server.Entities;
using Intersect.Server.Localization;
using Intersect.Server.Networking;
using Intersect.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Intersect.Server.Database.PlayerData
{
    public class AuctionHouseManager
    {
        public AuctionHouseManager() { }

        public AuctionHouseManager(Guid orderId, Guid seller, Guid itemId, int quantity, ItemProperties properties, int price)
        {
            OrderId = orderId;
            SellerId = seller;
            ItemId = itemId;
            Quantity = quantity;
            Properties = properties;
            Price = price;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }
        public Guid OrderId { get; set; } = Guid.Empty;
        public Guid SellerId { get; set; } = Guid.Empty;
        public Guid ItemId { get; set; } = Guid.Empty;
        public int Quantity { get; set; }

        [Column("Properties")]
        [JsonIgnore]
        public string PropertiesJson
        {
            get => JsonConvert.SerializeObject(Properties);
            set => Properties = JsonConvert.DeserializeObject<ItemProperties>(value) ?? new ItemProperties();
        }

        [NotMapped]
        public ItemProperties Properties { get; set; } = new ItemProperties();

        public int Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #region DatabaseRequest

        public static void DeletePlayerOrders(Guid sellerId)
        {
            using var context = DbInterface.CreatePlayerContext(false);
            context.AuctionHouse.RemoveRange(context.AuctionHouse.Where(o => o.SellerId == sellerId));
            context.SaveChanges();
        }

        public static void RemoveOrder(AuctionHouseManager order)
        {
            using var context = DbInterface.CreatePlayerContext(false);
            context.AuctionHouse.Remove(order);
            context.SaveChanges();
        }

        public static void AddOrder(AuctionHouseManager order)
        {
            using var context = DbInterface.CreatePlayerContext(false);
            context.AuctionHouse.Add(order);
            context.SaveChanges();
        }

        public static IEnumerable<AuctionHouseManager> ListOrders(Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                return null;
            }
            using var context = DbInterface.CreatePlayerContext();
            return QueryOrdersByItemId(context, itemId);
        }

        public static AuctionHouseManager GetOrder(Guid id)
        {
            using var context = DbInterface.CreatePlayerContext();
            return QueryOrderById(context, id);
        }

        public static void AddSellOrder(AuctionHouseManager newOrder)
        {
            using var context = DbInterface.CreatePlayerContext(false);

            try
            {
                // Agregar la orden a la base de datos
                context.AuctionHouse.Add(newOrder);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la orden de venta: {ex.Message}");
            }
        }

        public static int GetActiveOrders(Guid sellerId)
        {
            using var context = DbInterface.CreatePlayerContext();

            try
            {
                // Contar las órdenes activas del jugador
                return context.AuctionHouse.Count(o => o.SellerId == sellerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al contar órdenes activas: {ex.Message}");
                return 0;
            }
        }


        private static readonly Func<PlayerContext, Guid, IEnumerable<AuctionHouseManager>> QueryOrdersByItemId =
            EF.CompileQuery(
                (PlayerContext context, Guid itemId) => context.AuctionHouse.Where(o => o.ItemId == itemId)
            );

        private static readonly Func<PlayerContext, Guid, AuctionHouseManager> QueryOrderById =
            EF.CompileQuery(
                (PlayerContext context, Guid orderId) => context.AuctionHouse.FirstOrDefault(o => o.Id == orderId)
            );

        #endregion

 
    }
}
