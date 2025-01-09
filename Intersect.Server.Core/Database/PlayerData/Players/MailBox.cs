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
            Sender = sender;
            Player = to;
            Title = title;
            Message = msg;
            Attachments = attachments ?? new List<MailAttachment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get;  set; }
        

        [JsonProperty(nameof(Player))]
        private Guid JsonPlayerId => Player?.Id ?? Guid.Empty;

        [JsonProperty(nameof(Sender))]
        private Guid JsonTargetId => Sender?.Id ?? Guid.Empty;

        [JsonIgnore]
        public virtual Player Player { get;  set; }

        [JsonIgnore]
        public virtual Player Sender { get;  set; }

        public string Title { get;  set; }
        public string Message { get;  set; }

        [Column("Attachments")]
        public string AttachmentsJson
        {
            get => JsonConvert.SerializeObject(Attachments);
            set => Attachments = JsonConvert.DeserializeObject<List<MailAttachment>>(value) ?? new List<MailAttachment>();
        }

        [NotMapped]
        public List<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();

        public void AddAttachment(Item item)
        {
            Attachments.Add(new MailAttachment
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                Properties = item.Properties
            });
        }

        public List<MailAttachment> GetAttachments()
        {
            return Attachments;
        }

        public static void GetMails(PlayerContext context, Player player)
        {
            var mails = context.Player_MailBox
                .Where(p => player.Id == p.Player.Id)
                .Include(p => p.Sender)
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
        public ItemProperties Properties { get; set; } = new ItemProperties();
    }
}
