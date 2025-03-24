using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.Server.Database.PlayerData;
using Intersect.Server.Networking;
using Intersect.Utilities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Intersect.GameObjects;
using Intersect.Server.Database;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Network.Packets.Server;
using Intersect.Enums;


namespace Intersect.Server.Entities;
public partial class Auction_House
{
    public static void CreateSellOrder(Guid sellerId, Guid itemId, int quantity, ItemProperties properties, int price)
    {
        using var context = DbInterface.CreatePlayerContext(false);
        var newOrder = new AuctionHouseManager(Guid.NewGuid(), sellerId, itemId, quantity, properties, price);
        context.AuctionHouse.Add(newOrder);
        context.SaveChanges();
    }

    public static IEnumerable<AuctionHouseManager> SearchOrders(Guid itemId)
    {
        using var context = DbInterface.CreatePlayerContext();
        return context.AuctionHouse.Where(o => o.ItemId == itemId).ToList();
    }

    private static Guid GetCurrencyItemId()
    {
        // Implementación de ejemplo, reemplazar con la lógica real para obtener el ID del ítem de currency
        return new Guid("cc16ab76-7cbb-491b-8e04-483a669847c3");
    }
    
    public static void BuyItem(Guid buyerId, Guid orderId, int quantity)
    {
        using var context = DbInterface.CreatePlayerContext(false);
        var order = context.AuctionHouse.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null || order.Quantity < quantity)
        {
            PacketSender.SendChatMsg(Player.Find(buyerId), "La cantidad solicitada no está disponible.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }
        if (buyerId == order.SellerId)
        {
            PacketSender.SendChatMsg(Player.Find(buyerId), "No puedes comprar tus propios ítems.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

       
        var currencyItemId = GetCurrencyItemId();
        int totalPrice = quantity * order.Price;

        // Verificar si el comprador tiene suficiente currency
        var currencySlots = context.Player_Items.Where(i => i.PlayerId == buyerId && i.ItemId == currencyItemId).ToList();
        int totalCurrency = currencySlots.Sum(i => i.Quantity);

        if (totalCurrency < totalPrice)
        {
            PacketSender.SendChatMsg(Player.Find(buyerId), "No tienes suficiente currency.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        // Deducir la cantidad de currency
        int remainingCost = totalPrice;
        foreach (var slot in currencySlots)
        {
            int amountToRemove = Math.Min(slot.Quantity, remainingCost);
            slot.Quantity -= amountToRemove;
            remainingCost -= amountToRemove;

            if (slot.Quantity == 0)
            {
                context.Player_Items.Remove(slot);
            }

            if (remainingCost <= 0)
            {
                break;
            }
        }

        // Ajustar cantidad de ítems en la orden
        order.Quantity -= quantity;
        if (order.Quantity == 0)
        {
            context.AuctionHouse.Remove(order);
        }

        var seller = context.Players.FirstOrDefault(p => p.Id == order.SellerId);

        // Enviar los ítems comprados al comprador
        var mailToBuyer = new MailBox
        {
            Player = context.Players.FirstOrDefault(p => p.Id == buyerId),
            Title = "Compra en Auction House",
            Message = $"Has comprado {quantity} unidades de {ItemBase.Get(order.ItemId).Name}.",
            Attachments = new List<MailAttachment>
        {
            new MailAttachment { ItemId = order.ItemId, Quantity = quantity }
        }
        };
        context.Player_MailBox.Add(mailToBuyer);

        // Enviar currency al vendedor
        if (seller != null)
        {
            var mailToSeller = new MailBox
            {
                Player = seller,
                Title = "Venta en Auction House",
                Message = $"Has vendido {quantity} unidades de {ItemBase.Get(order.ItemId).Name} por {totalPrice} {ItemBase.Get(currencyItemId).Name}.",
                Attachments = new List<MailAttachment>
            {
                new MailAttachment { ItemId = currencyItemId, Quantity = totalPrice }
            }
            };
            context.Player_MailBox.Add(mailToSeller);
        }

        // **Registrar la transacción**
        AuctionHouseTransactions(buyerId, order.SellerId, order.ItemId, quantity, totalPrice);

        context.SaveChanges();
    }


    public static void SellItem(Guid sellerId, Guid itemId, int quantity, int price)
    {
        var seller = Player.FindOnline(sellerId);
        if (seller == null)
        {
            return;
        }

        // Verificar si el jugador tiene suficientes ítems en su inventario
        if (!seller.CanTakeItem(itemId, quantity))
        {
            PacketSender.SendChatMsg(seller, "No tienes suficientes unidades de este ítem.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        if (price <= 0)
        {
            PacketSender.SendChatMsg(seller, "El precio debe ser mayor a 0.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        int activeOrders = AuctionHouseManager.GetActiveOrders(sellerId);
        if (activeOrders >= 10) // Máximo de 10 órdenes activas
        {
            PacketSender.SendChatMsg(seller, "Has alcanzado el límite de órdenes activas.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        // Intentar remover el ítem del inventario del jugador usando métodos existentes
        if (!seller.TryTakeItem(itemId, quantity, ItemHandling.Normal, sendUpdate: true))
        {
            PacketSender.SendChatMsg(seller, "No se pudo retirar el ítem del inventario.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        // Registrar la orden en la casa de subastas
        var newOrder = new AuctionHouseManager(Guid.NewGuid(), sellerId, itemId, quantity, new ItemProperties(), price);
        AuctionHouseManager.AddSellOrder(newOrder);

        PacketSender.SendChatMsg(seller, "Ítem listado exitosamente en la Auction House.", ChatMessageType.Inventory, CustomColors.Alerts.Accepted);
    }


    public static void CancelOrder(Guid sellerId, Guid orderId)
    {
        using var context = DbInterface.CreatePlayerContext(false);
        var order = context.AuctionHouse.FirstOrDefault(o => o.OrderId == orderId && o.SellerId == sellerId);

        if (order == null)
        {
            PacketSender.SendChatMsg(Player.Find(sellerId), "No tienes una orden con este ID.", ChatMessageType.Inventory, CustomColors.Alerts.Error);
            return;
        }

        // Devolver los ítems al vendedor
        var mailToSeller = new MailBox
        {
            Player = context.Players.FirstOrDefault(p => p.Id == sellerId),
            Title = "Orden Cancelada",
            Message = $"Has cancelado tu venta de {order.Quantity} unidades de {ItemBase.Get(order.ItemId).Name}.",
            Attachments = new List<MailAttachment>
        {
            new MailAttachment { ItemId = order.ItemId, Quantity = order.Quantity }
        }
        };
        context.Player_MailBox.Add(mailToSeller);

        // Eliminar la orden de la base de datos
        context.AuctionHouse.Remove(order);
        context.SaveChanges();

        PacketSender.SendChatMsg(Player.Find(sellerId), "Orden cancelada y ítems devueltos.", ChatMessageType.Inventory, CustomColors.Alerts.Accepted);
    }

    public static void AuctionHouseTransactions(Guid buyerId, Guid sellerId, Guid itemId, int quantity, int totalPrice)
    {
        using var context = DbInterface.CreatePlayerContext(false);

        var transaction = new AuctionTransaction
        {
            BuyerId = buyerId,
            SellerId = sellerId,
            ItemId = itemId,
            Quantity = quantity,
            TotalPrice = totalPrice,
            Timestamp = DateTime.UtcNow
        };

        context.AuctionTransactions.Add(transaction);
        context.SaveChanges();
    }

    public static IEnumerable<AuctionHouseManager> SearchOrders(Guid itemId, int minPrice = 0, int maxPrice = int.MaxValue, int minQuantity = 1)
    {
        using var context = DbInterface.CreatePlayerContext();
        return context.AuctionHouse
            .Where(o => o.ItemId == itemId && o.Price >= minPrice && o.Price <= maxPrice && o.Quantity >= minQuantity)
            .OrderBy(o => o.Price)
            .ToList();
    }
    public static IEnumerable<AuctionTransaction> GetTransactionHistory(Guid playerId, bool isBuyer)
    {
        using var context = DbInterface.CreatePlayerContext();
        return isBuyer
            ? context.AuctionTransactions.Where(t => t.BuyerId == playerId).OrderByDescending(t => t.Timestamp).ToList()
            : context.AuctionTransactions.Where(t => t.SellerId == playerId).OrderByDescending(t => t.Timestamp).ToList();
    }

}
