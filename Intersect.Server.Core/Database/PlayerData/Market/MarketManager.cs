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
        /// <summary>
        /// Publicar un ítem en el mercado.
        /// </summary>
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

            var currencyBase = GetDefaultCurrency();
            if (currencyBase == null)
            {
                PacketSender.SendChatMsg(seller, "Currency item not configured!", ChatMessageType.Error, CustomColors.Alerts.Error);
                return false;
            }

            // Calcular impuesto
            var tax = (int)Math.Ceiling(price * GetMarketTaxPercentage());

            // Verificar si puede pagar la comisión
            if (!seller.TryTakeItem(currencyBase.Id, tax))
            {
                PacketSender.SendChatMsg(seller, Strings.Market.notenoughmoney, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            // Verificar si tiene el ítem y puede quitarlo
            if (!seller.TryTakeItem(item.ItemId, quantity))
            {
                // Reembolsar impuesto
                seller.TryGiveItem(currencyBase.Id, tax);
                PacketSender.SendChatMsg(seller, Strings.Market.cannotremoveitem, ChatMessageType.Error, CustomColors.Alerts.Declined);
                return false;
            }

            // Crear publicación
            var listing = new MarketListing
            {
                Seller = seller,
                ItemId = item.ItemId,
                Quantity = quantity,
                Price = price,
                ItemProperties = item.Properties,
                ListedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(28),
                IsSold = false
            };

            using var context = DbInterface.CreatePlayerContext(readOnly: false);

            // Adjuntar seller y su User para prevenir conflictos de tracking
            context.Attach(seller);
            context.Attach(seller.User);

            // Agregar la publicación al contexto
            context.Market_Listings.Add(listing);

            // Prevenir errores por usuarios duplicados
            context.StopTrackingUsersExcept(seller.User);

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
                    Log.Error($"[Market] Error de concurrencia al guardar la publicación. Intentos restantes: {retries - 1}", ex);
                    retries--;

                    if (retries == 0)
                    {
                        PacketSender.SendChatMsg(seller, "❌ Error al crear la publicación en el mercado.", ChatMessageType.Error, CustomColors.Alerts.Declined);
                        return false;
                    }
                }
            }

            PacketSender.SendChatMsg(seller, Strings.Market.listingcreated, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
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

        public static bool TryBuyListing(Player buyer, Guid listingId)
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: false);
            var listing = context.Market_Listings
                .Where(l => l.Id == listingId)
                .Select(l => l)
                .FirstOrDefault();

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

            var currencyAmount = buyer.FindInventoryItemQuantity(currencyBase.Id);
            if (currencyAmount < listing.Price)
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

            // Remover el dinero de forma segura
            var currencySlots = buyer.FindInventoryItemSlots(currencyBase.Id);
            var remainingCost = listing.Price;
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

                if (remainingCost <= 0)
                    break;
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

            // Dar ítem comprado
            buyer.TryGiveItem(itemToGive, -1);

            listing.IsSold = true;

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

            // Enviar oro por correo al vendedor
            var goldAttachment = new MailAttachment
            {
                ItemId = currencyBase.Id,
                Quantity = listing.Price,
                Properties = null
            };

            var mail = new MailBox(
                sender: buyer,
                to: listing.Seller,
                title: Strings.Market.salecompleted,
                msg: Strings.Market.yoursolditem.ToString(ItemBase.GetName(listing.ItemId)),
                attachments: new List<MailAttachment> { goldAttachment }
            );

            if (Player.FindOnline(listing.Seller.Name) is Player sellerOnline)
            {
                sellerOnline.MailBoxs.Add(mail);
                PacketSender.SendChatMsg(sellerOnline, Strings.Market.yoursolditem, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
                PacketSender.SendOpenMailBox(sellerOnline);
            }
            else
            {
                context.Player_MailBox.Add(mail);
            }

            context.SaveChanges();
            PacketSender.SendChatMsg(buyer, Strings.Market.itempurchased, ChatMessageType.Trading, CustomColors.Alerts.Accepted);
            return true;
        }

        /// <summary>
        /// Limpiar publicaciones expiradas y devolver ítems por correo.
        /// </summary>
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
    }
}
