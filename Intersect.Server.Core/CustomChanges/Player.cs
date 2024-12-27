using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.Server.Networking;
using Intersect.Enums;

namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        private Dictionary<string, DateTime> recentMessages = new Dictionary<string, DateTime>();
        private static readonly TimeSpan messageCooldown = TimeSpan.FromSeconds(120); // Ajusta el intervalo de tiempo según sea necesario

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

            if (levelDiff >= 4)
            {
                ShowLowLevelKillMessage();
            }

            return modifiedExp;
        }

        private void ShowExpGainMessage(long baseExp, long modifiedExp, int reductionPercentage)
        {
            string message = $"You have won only {modifiedExp} experience points";
            if (reductionPercentage > 0)
            {
                message += $" (reduction of {reductionPercentage}% by level difference).";
            }
            SendMessageToPlayer(message, MessageType.Info);
        }

        private void ShowLowLevelKillMessage()
        {
            SendMessageToPlayer("You are killing monsters that are much weaker than you.", MessageType.Warning);
        }

        private void SendMessageToPlayer(string message, MessageType type)
        {
            if (recentMessages.ContainsKey(message))
            {
                // Si el mensaje ya ha sido enviado recientemente, verifica el tiempo transcurrido
                if ((DateTime.Now - recentMessages[message]) < messageCooldown)
                {
                    // Si el tiempo transcurrido es menor que el cooldown, no enviar el mensaje
                    return;
                }
            }

            // Actualiza el diccionario con el tiempo actual
            recentMessages[message] = DateTime.Now;

            // Envía el mensaje al jugador
            PacketSender.SendChatMsg(this, message, (ChatMessageType)type);
        }
    }

    public enum MessageType
    {
        Warning,
        Error,
        Info
    }
}