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
            // Validar que el índice sea válido
            if (itemIndex < 0 || itemIndex >= Items.Count)
            {
                PacketSender.SendChatMsg(this, "Índice de ítem no válido.", ChatMessageType.Error);
                return;
            }

            // Obtener el ítem por su índice
            var item = Items[itemIndex];

            // Validar si el ítem es válido para mejorar
            if (item == null || item.Descriptor?.ItemType != ItemType.Equipment || level <= item.EnchantmentLevel)
            {
                PacketSender.SendChatMsg(this, "El ítem no es válido o no se puede mejorar.", ChatMessageType.Error);
                return;
            }

            // Verificar si el jugador tiene suficiente moneda
            if (!HasSufficientCurrency(currencyId, currencyAmountRequired))
            {
                PacketSender.SendChatMsg(this, "No tienes suficientes recursos para mejorar el ítem.", ChatMessageType.Error);
                return;
            }

            // Deduce la cantidad necesaria de moneda
            if (!DeductCurrency(currencyId, currencyAmountRequired))
            {
                PacketSender.SendChatMsg(this, "Error al deducir la moneda necesaria.", ChatMessageType.Error);
                return;
            }

            // Determinar si la mejora es exitosa
            var successRate = item.Descriptor.GetUpgradeSuccessRate(level);
            if (Random.Shared.NextDouble() <= successRate)
            {
                // Aplicar la mejora
                item.ApplyEnchantment(level);

                PacketSender.SendChatMsg(this, $"¡Encantamiento exitoso! El ítem ahora está en nivel +{level}.", ChatMessageType.Experience);

                // Guardar cambios en la base de datos
                using (var playerContext = DbInterface.CreatePlayerContext(readOnly: false))
                {
                    playerContext.Players.Update(this); // Actualizar al jugador
                    playerContext.Player_Items.Update(item); // Actualizar el ítem en la base de datos
                    playerContext.SaveChanges();
                }

                // Notificar al cliente sobre la actualización del nivel del ítem
                PacketSender.SendUpdateItemLevel(this, itemIndex, level);
            }
            else if (!useAmulet)
            {
                // Fallo: Reducir nivel de encantamiento
                item.EnchantmentLevel = Math.Max(0, item.EnchantmentLevel - 1);
                item.ApplyEnchantment(item.EnchantmentLevel);

                PacketSender.SendChatMsg(this, "El encantamiento falló y el nivel del ítem ha disminuido.", ChatMessageType.Error);

                // Notificar al cliente sobre la actualización del nivel y stats
                PacketSender.SendUpdateItemLevel(this, itemIndex, item.EnchantmentLevel);
            }
            else
            {
                PacketSender.SendChatMsg(this, "El encantamiento falló, pero el amuleto protegió el nivel del ítem.", ChatMessageType.Notice);
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

        public void OpenMarket()
        {
            PacketSender.SendOpenMarketWindow(this);
        }

        public void OpenSellMarket()
        {
            PacketSender.SendOpenSellMarketWindow(this);
        }
    }

    public enum MessageType
    {
        Warning,
        Error,
        Info
    }
}