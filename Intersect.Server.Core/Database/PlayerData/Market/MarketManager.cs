using System;
using System.Collections.Generic;
using System.Linq;
using Intersect.Server.Database;
using Intersect.Server.Entities;
using Intersect.Server.Localization;
using Intersect.Network.Packets.Server;
using Intersect.Enums;

using Intersect.GameObjects;
using Intersect.Server.Networking;
using Intersect.Server.Database.PlayerData.Players;
using Microsoft.EntityFrameworkCore;
using Intersect.Logging;

namespace Intersect.Server.Database.PlayerData.Players

{
    public static class MarketManager
    {
        public static List<MarketListing> SearchMarket(
     string name = "",
     int? minPrice = null,
     int? maxPrice = null,
     ItemType? type = null
 )
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: true);

            // 🔎 Consulta base: solo ítems no vendidos y dentro de fecha de expiración
            var query = context.Market_Listings
                .Where(l => !l.IsSold && l.ExpireAt > DateTime.UtcNow);

            // 💰 Filtros de precio
            if (minPrice.HasValue)
            {
                query = query.Where(l => l.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= maxPrice.Value);
            }

            var result = query.ToList();

            // 🧠 Filtrado avanzado (no puede hacerse en EF)
            if (!string.IsNullOrWhiteSpace(name))
            {
                result = result.Where(l =>
                {
                    var itemBase = ItemBase.Get(l.ItemId);
                    return itemBase != null &&
                           itemBase.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
                }).ToList();
            }

            if (type.HasValue)
            {
                result = result.Where(l =>
                {
                    var itemBase = ItemBase.Get(l.ItemId);
                    return itemBase != null &&
                           itemBase.Type == GameObjectType.Item &&
                           itemBase.ItemType == type.Value;
                }).ToList();
            }

            return result;
        }

        public static bool TryListItem(Player seller, Item item, int quantity, int price)
        {
            if (item == null || quantity <= 0 || price <= 0)
            {
                PacketSender.SendChatMsg(seller, Strings.Market.invalidlisting, ChatMessageType.Error, CustomColors.Alerts.Error);
                return false;
            }

            var itemBase = ItemBase.Get(item.ItemId);
            if (itemBase == null || !itemBase.CanSell)
            {
                PacketSender.SendChatMsg(seller, Strings.Market.cannotlist, ChatMessageType.Error, CustomColors.Alerts.Error);
                return false;
            }
            var basePrice = itemBase.Price;
            var minAllowed = (int)(basePrice * 0.5);
            var maxAllowed = (int)(basePrice * 2);

            // Intentar obtener estadísticas previas si existen
            if (MarketStatisticsManager.TryGetStats(item.ItemId, out var stats))
            {
                minAllowed = stats.GetMinAllowedPrice();
                maxAllowed = stats.GetMaxAllowedPrice();
            }

            // ❌ Si el precio está fuera del margen permitido, rechazar publicación
            if (price < minAllowed || price > maxAllowed)
            {
                PacketSender.SendChatMsg(
                    seller,
                    $"❌ Precio fuera del rango permitido para este ítem. Rango actual: {minAllowed} - {maxAllowed} 🪙",
                    ChatMessageType.Error,
                    CustomColors.Alerts.Declined
                );
                return false;
            }


            var currencyBase = GetDefaultCurrency();
            if (currencyBase == null)
            {
                PacketSender.SendChatMsg(seller, "Currency item not configured!", ChatMessageType.Error, CustomColors.Alerts.Error);
                return false;
            }
        
            var tax = (int)Math.Ceiling(price * GetMarketTaxPercentage());
            if (!seller.TryTakeItem(currencyBase.Id, tax))
            {
                PacketSender.SendChatMsg(seller, Strings.Market.notenoughmoney, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            if (!seller.TryTakeItem(item.ItemId, quantity))
            {
                seller.TryGiveItem(currencyBase.Id, tax); // Reembolso
                PacketSender.SendChatMsg(seller, Strings.Market.cannotremoveitem, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            using var context = DbInterface.CreatePlayerContext(readOnly: false);
            context.StopTrackingUsersExcept(seller.User);

            // 💡 Adjuntar entidad `seller` correctamente
            context.Attach(seller);

            // ✅ Crear el listado después de adjuntar seller
            var listing = new MarketListing
            {
                Seller = seller,
                ItemId = item.ItemId,
                Quantity = quantity,
                Price = price,
                ItemProperties = item.Properties,
                ListedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(7),
                IsSold = false
            };

            context.Market_Listings.Add(listing);

            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    context.SaveChanges();
                    break;
                }
                catch (DbUpdateException ex)
                {
                    Log.Error($"Error al guardar listado en mercado. Reintentos restantes: {retries - 1}", ex);
                    retries--;

                    if (retries == 0)
                    {
                        PacketSender.SendChatMsg(seller, "❌ No se pudo guardar el listado en el mercado.", ChatMessageType.Error, CustomColors.Alerts.Error);
                        return false;
                    }
                }
            }

            PacketSender.SendChatMsg(seller, Strings.Market.listingcreated, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
            PacketSender.SendRefreshMarket(seller);
            return true;
        }
        public static bool TryBuyListing(Player buyer, Guid listingId)
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: false);
            context.StopTrackingUsersExcept(buyer.User); // ✅ Prevenir tracking duplicado

            // ✅ Adjuntar comprador (en caso de que lo requiera EF)
            context.Attach(buyer);

            var listing = context.Market_Listings
                .Include(l => l.Seller)
                .FirstOrDefault(l => l.Id == listingId);

            if (listing == null || listing.IsSold || listing.ExpireAt <= DateTime.UtcNow)
            {
                PacketSender.SendChatMsg(buyer, Strings.Market.listingunavailable, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            var currencyBase = GetDefaultCurrency();
            if (currencyBase == null)
            {
                PacketSender.SendChatMsg(buyer, "Currency not configured.", ChatMessageType.Error, CustomColors.Alerts.Error);
                return false;
            }

            var totalCost = listing.Price * listing.Quantity;
            var currencyAmount = buyer.FindInventoryItemQuantity(currencyBase.Id);

            if (currencyAmount < totalCost)
            {
                PacketSender.SendChatMsg(buyer, Strings.Market.notenoughmoney, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            var itemToGive = new Item(listing.ItemId, listing.Quantity) { Properties = listing.ItemProperties };
            if (!buyer.CanGiveItem(itemToGive.ItemId, itemToGive.Quantity))
            {
                PacketSender.SendChatMsg(buyer, Strings.Market.inventoryfull, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            var currencySlots = buyer.FindInventoryItemSlots(currencyBase.Id);
            var remainingCost = totalCost;
            var removedItems = new Dictionary<Guid, int>();
            var success = true;

            foreach (var itemSlot in currencySlots)
            {
                var quantityToRemove = Math.Min(remainingCost, itemSlot.Quantity);
                if (!buyer.TryTakeItem(itemSlot, quantityToRemove))
                {
                    success = false;
                    break;
                }

                removedItems[itemSlot.Id] = quantityToRemove;
                remainingCost -= quantityToRemove;

                if (remainingCost <= 0) break;
            }

            if (!success || remainingCost > 0)
            {
                foreach (var kvp in removedItems)
                {
                    buyer.TryGiveItem(kvp.Key, kvp.Value);
                }

                PacketSender.SendChatMsg(buyer, Strings.Market.transactionfailed, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            // ✅ Marcar como vendido y dar el ítem
            listing.IsSold = true;
            context.Remove(listing);
            context.Update(listing);
            buyer.TryGiveItem(itemToGive, -1);

            var transaction = new MarketTransaction
            {
                ListingId = listing.Id,
                Seller = listing.Seller,
                BuyerName = buyer.Name,
                ItemId = listing.ItemId,
                Quantity = listing.Quantity,
                Price = listing.Price,
                ItemProperties = listing.ItemProperties
            };
            context.Market_Transactions.Add(transaction);

            var goldAttachment = new MailAttachment
            {
                ItemId = currencyBase.Id,
                Quantity = totalCost
            };

            var mail = new MailBox(
                sender: buyer,
                to: listing.Seller,
                title: Strings.Market.salecompleted,
                msg: Strings.Market.yoursolditem.ToString(ItemBase.GetName(listing.ItemId)),
                attachments: new List<MailAttachment> { goldAttachment }
            );

            var sellerOnline = Player.FindOnline(listing.Seller.Name);
            if (sellerOnline != null)
            {
                sellerOnline.MailBoxs.Add(mail);
                PacketSender.SendChatMsg(sellerOnline, Strings.Market.yoursolditem, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
                PacketSender.SendOpenMailBox(sellerOnline);
            }
            else
            {
                // ❌ Ya está traqueado por EF, NO volver a hacer context.Attach(listing.Seller)
                context.Player_MailBox.Add(mail);
            }

            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    context.SaveChanges();
                    break;
                }
                catch (DbUpdateException ex)
                {
                    Log.Error($"❌ Error de concurrencia al comprar ítem. Reintentos restantes: {retries - 1}", ex);
                    retries--;

                    if (retries == 0)
                    {
                        PacketSender.SendChatMsg(buyer, Strings.Market.transactionfailed, ChatMessageType.Error, CustomColors.Alerts.Declined);
                        return false;
                    }
                }
            }
            UpdateStatistics(transaction);

            PacketSender.SendChatMsg(buyer, Strings.Market.itempurchased, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
            PacketSender.SendRefreshMarket(buyer); // Actualiza al cliente con nuevo mercado

            return true;
        }


        private const float DefaultMarketTax = 0.02f; // 2% por publicación

        public static float GetMarketTaxPercentage()
        {
            // Si en el futuro quieres hacer dinámico esto por tipo de ítem, ciudad, etc., puedes modificar aquí
            return DefaultMarketTax;
        }
        private static ItemBase GetDefaultCurrency()
        {
            return ItemBase.Lookup.Values
                .OfType<ItemBase>()
                .FirstOrDefault(i => i.ItemType == ItemType.Currency);
        }


        public static void CleanExpiredListings()
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: false);
            var expired = context.Market_Listings
                .Where(l => !l.IsSold && l.ExpireAt <= DateTime.UtcNow)
                .ToList();

            foreach (var listing in expired)
            {
                var item = new MailAttachment
                {
                    ItemId = listing.ItemId,
                    Quantity = listing.Quantity,
                    Properties = listing.ItemProperties
                };

                var mail = new MailBox(
                    sender: null,
                    to: listing.Seller,
                    title: Strings.Market.expiredlisting,
                    msg: Strings.Market.yourlistingexpired,
                    attachments: new List<MailAttachment> { item }
                );

                if (Player.FindOnline(listing.Seller.Name) is Player onlineSeller)
                {
                    onlineSeller.MailBoxs.Add(mail);
                    PacketSender.SendOpenMailBox(onlineSeller);
                }
                else
                {
                    context.Player_MailBox.Add(mail);
                }

                context.Market_Listings.Remove(listing);
            }

            if (expired.Count > 0)
            {
                context.SaveChanges();
            }
        }
        private static Dictionary<Guid, MarketStatistics> _statisticsCache = new();

        public static void UpdateStatistics(MarketTransaction tx)
        {
            if (!_statisticsCache.TryGetValue(tx.ItemId, out var stats))
            {
                stats = new MarketStatistics(tx.ItemId);
                _statisticsCache[tx.ItemId] = stats;
            }

            stats.AddTransaction(tx);
        }

        public static bool IsPriceWithinAllowedRange(Guid itemId, int price)
        {
            if (_statisticsCache.TryGetValue(itemId, out var stats) && stats.NumberOfSales > 0)
            {
                var min = stats.GetMinAllowedPrice();
                var max = stats.GetMaxAllowedPrice();
                return price >= min && price <= max;
            }

            // Si no hay stats, usar precio base
            var item = ItemBase.Get(itemId);
            if (item?.Price > 0)
            {
                var min = (int)(item.Price * 0.5);
                var max = (int)(item.Price * 1.5);
                return price >= min && price <= max;
            }

            return true; // Sin datos, dejar pasar
        }

    }
}
