using System;
using System.Collections.Generic;
using Intersect.Server.Networking;
using Intersect.Enums;
using Intersect.Server.Localization;
using Intersect.Config;
using System.Text.Json.Serialization;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Database.PlayerData;

using Intersect.Server.Database;
using Intersect.Logging;
using Intersect.GameObjects;


namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        private Dictionary<string, DateTime> recentMessages = new Dictionary<string, DateTime>();
        private static readonly TimeSpan messageCooldown = TimeSpan.FromSeconds(120);

        public long ExpModifiedByLevel(int enemyLevel, long baseExp, int playerLevel = 0)
        {
            int levelDiff = playerLevel == 0 ? Level - enemyLevel : playerLevel - enemyLevel;
            float expMultiplier = 1.0f;
            int reductionPercentage = 0;

            if (levelDiff >= 4 && levelDiff < 6)
            {
                expMultiplier = 0.8f;
                reductionPercentage = 20;
            }
            else if (levelDiff >= 6 && levelDiff < 10)
            {
                expMultiplier = 0.6f;
                reductionPercentage = 40;
            }
            else if (levelDiff >= 10)
            {
                expMultiplier = 0.2f;
                reductionPercentage = 80;
            }

            long modifiedExp = (long)(baseExp * expMultiplier);
            ShowExpGainMessage(baseExp, modifiedExp, reductionPercentage);

            return modifiedExp;
        }

        private void ShowExpGainMessage(long baseExp, long modifiedExp, int reductionPercentage)
        {
            string message = $"You have won only {modifiedExp} experience points";
            if (reductionPercentage > 0)
            {
                message += $" (reduction of {reductionPercentage}% by level difference).";
            }

            SendMessageToParty(message, MessageType.Info);
        }

        private void SendMessageToParty(string message, MessageType type)
        {
            // Check if the player is in a party
            if (Party != null)
            {
                foreach (var member in Party)
                {
                    if (member != null)
                    {
                        member.SendMessageToPlayer(message, type);

                    }
                }
            }
            else
            {
                // If not in a party, just send the message to the player
                SendMessageToPlayer(message, type);
            }
        }

        private void SendMessageToPlayer(string message, MessageType type)
        {
            if (recentMessages.ContainsKey(message))
            {
                if ((DateTime.Now - recentMessages[message]) < messageCooldown)
                {
                    return;
                }
            }

            recentMessages[message] = DateTime.Now;
            PacketSender.SendChatMsg(this, message, (ChatMessageType)type);
        }
        public bool HasSufficientCurrency(Guid currencyId, int amountRequired)
        {
            // Verifica si el jugador tiene suficiente cantidad del ítem moneda
            return FindInventoryItemQuantity(currencyId) >= amountRequired;
        }

        public bool DeductCurrency(Guid currencyId, int amount)
        {
            // Verifica si el jugador tiene suficiente cantidad antes de deducir
            if (!HasSufficientCurrency(currencyId, amount))
            {
                return false; // No hay suficientes recursos
            }

            // Deducir el ítem del inventario
            return TryTakeItem(currencyId, amount, ItemHandling.Normal, sendUpdate: true);
        }
        public void TryUpgradeItem(int itemIndex, int level, Guid currencyId, int currencyAmountRequired, bool useAmulet = false)
        {
            if (itemIndex < 0 || itemIndex >= Items.Count)
            {
                PacketSender.SendChatMsg(this, "Índice de ítem no válido.", ChatMessageType.Error);
                return;
            }

            var item = Items[itemIndex];
            if (item == null || item.Descriptor?.ItemType != ItemType.Equipment || item.Properties == null || !item.Descriptor.CanBeEnchanted)
            {
                PacketSender.SendChatMsg(this, "El ítem no es válido o no se puede mejorar.", ChatMessageType.Error);
                return;
            }

            if (level <= item.Properties.EnchantmentLevel)
            {
                PacketSender.SendChatMsg(this, "El nivel de encantamiento debe ser superior al actual.", ChatMessageType.Error);
                return;
            }

            var currency = Items.FirstOrDefault(i => i?.ItemId == currencyId && i.Quantity >= currencyAmountRequired);
            if (currency == null || currency.Descriptor.ItemType != ItemType.Resource || currency.Descriptor.Subtype != "Rune")
            {
                PacketSender.SendChatMsg(this, "Debes usar una Runa válida para mejorar el ítem.", ChatMessageType.Error);
                return;
            }

            double successRate = currency.Descriptor.UpgradeMaterialSuccessRate;

            if (!DeductCurrency(currencyId, currencyAmountRequired))
            {
                PacketSender.SendChatMsg(this, "Error al deducir la moneda necesaria.", ChatMessageType.Error);
                return;
            }

            bool success = Random.Shared.NextDouble() <= successRate;

            using (var playerContext = DbInterface.CreatePlayerContext(readOnly: false))
            {
                try
                {
                    int[] previousStats = (int[])item.Properties.StatModifiers.Clone();
                    int previousLevel = item.Properties.EnchantmentLevel;

                    if (success)
                    {
                        item.ApplyEnchantment(level);
                        PacketSender.SendChatMsg(this, $"¡Encantamiento exitoso! {item.Descriptor.Name} ahora está en nivel +{level}.", ChatMessageType.Experience);
                     
                    }
                    else
                    {
                        if (!useAmulet)
                        {
                            int newLevel = Math.Max(0, item.Properties.EnchantmentLevel - 1);
                            item.ApplyEnchantment(newLevel);
                            PacketSender.SendChatMsg(this, $"El encantamiento falló y el nivel de {item.Descriptor.Name} ha disminuido a +{newLevel}.", ChatMessageType.Error);
                         
                        }
                        else
                        {
                            PacketSender.SendChatMsg(this, $"El encantamiento falló, pero el amuleto protegió el nivel +{previousLevel} de {item.Descriptor.Name}.", ChatMessageType.Notice);
                        }
                    }

                    playerContext.Players.Update(this);
                    playerContext.Player_Items.Update(item);
                    playerContext.SaveChanges();

                    PacketSender.SendUpdateItemLevel(this, itemIndex, item.Properties.EnchantmentLevel);
                }
                catch (Exception ex)
                {
                    PacketSender.SendChatMsg(this, "Ocurrió un error durante la mejora del ítem.", ChatMessageType.Error);
                    Log.Error(ex);
                }
            }
        }

        [JsonIgnore]
        public virtual List<MailBox> MailBoxs { get; set; } = new List<MailBox>();

        public void OpenMailBox()
        {
            InMailBox = true;
            PacketSender.SendOpenMailBox(this);
        }
        public void CloseMailBox()
        {
            if (InMailBox)
            {
                InMailBox = false;
                PacketSender.SendCloseMailBox(this);
            }
        }
        public void SendMail()
        {
            InMailBox = true;
            PacketSender.SendOpenSendMail(this);
        }

        public bool HasPermissionToTrade()
        {
            return Level >= 10; //Nivel minimo para comerciar
        }
    }

    public enum MessageType
    {
        Warning,
        Error,
        Info
    }
}