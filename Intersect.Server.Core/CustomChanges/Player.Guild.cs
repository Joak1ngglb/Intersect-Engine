using System;
using System.Collections.Generic;
using Intersect.Server.Networking;
using Intersect.Enums;
using Intersect.Server.Localization;
using Intersect.Config;
using System.Text.Json.Serialization;
using Intersect.Server.Database.PlayerData.Players;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {

        [JsonIgnore]
        [NotMapped]
        public float GuildExpPercentage { get; set; } = 0.0f;

        public void SetGuildExpPercentage(float percentage)
        {
            GuildExpPercentage = Math.Clamp(percentage, 0.0f, 1.0f); // Asegura que esté entre 0% y 100%

            PacketSender.SendChatMsg(this, $"Has cambiado tu contribución de experiencia al gremio a {GuildExpPercentage * 100}%.", ChatMessageType.Guild);
        }


        public void GiveExperienceWithGuildShare(long amount)
        {
            if (amount <= 0) return;

            long guildExp = (long)(amount * GuildExpPercentage);
            long playerExp = amount - guildExp;

            if (IsInGuild && guildExp > 0)
            {
                DonateGuildExperience(guildExp);
            }

            GiveExperience(playerExp);
        }

        private void DonateGuildExperience(long amount)
        {
            if (Guild == null || amount <= 0) return;

            Guild.AddExperience(amount);
            PacketSender.SendChatMsg(this, $"Your guild has received {amount} XP.", ChatMessageType.Guild);
        }
    }

}