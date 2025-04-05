using System;
using System.Collections.Generic;
using Intersect.Server.Networking;
using Intersect.Enums;
using Intersect.Server.Localization;
using Intersect.Config;
using System.Text.Json.Serialization;
using Intersect.Server.Database.PlayerData.Players;

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
    }

    public enum MessageType
    {
        Warning,
        Error,
        Info
    }
}