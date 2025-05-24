using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Intersect.Network.Packets.Server;
using Intersect.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Intersect.Server.Database.PlayerData.Players
{
    public class MailBox
    {
        public MailBox() { }

        public MailBox(Player sender, Player to, string title, string msg, List<MailAttachment> attachments)
        {
            Sender = sender.Name;
            Player = to;
            Title = title;
            Message = msg;
            Attachments = attachments ?? new List<MailAttachment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Sender { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        [Column("Attachments")]
        public string AttachmentsJson
        {
            get => JsonConvert.SerializeObject(Attachments);
            set => Attachments = JsonConvert.DeserializeObject<List<MailAttachment>>(value) ?? new List<MailAttachment>();
        }

        [NotMapped]
        public List<MailAttachment> Attachments { get; set; } = new();

        public void AddAttachment(Item item)
        {
            Attachments.Add(new MailAttachment
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                Properties = item.Properties
            });
        }

        public List<MailAttachment> GetAttachments() => Attachments;

        public static void GetMails(PlayerContext context, Player player)
        {
            var mails = context.Player_MailBox
                .Where(p => player.Id == p.Player.Id)
                .ToList();

            if (mails != null)
            {
                player.MailBoxs = mails;
                foreach (var mail in mails)
                {
                    mail.Attachments = JsonConvert.DeserializeObject<List<MailAttachment>>(mail.AttachmentsJson)
                                        ?? new List<MailAttachment>();
                }
            }
        }
    }

    public class MailAttachment
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public ItemProperties Properties { get; set; } = new();
    }
}
