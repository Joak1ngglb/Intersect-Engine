using Intersect.Server.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intersect.Server.Database.Logging.Entities
{
    public partial class NationHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid NationId { get; private set; }

        public Guid UserId { get; set; }

        public Guid PlayerId { get; set; }

        public string Ip { get; set; }

        public DateTime TimeStamp { get; set; }

        [JsonIgnore]
        public NationActivityType Type { get; set; }

        [JsonProperty("ActivityType")]
        public string ActivityTypeName => Enum.GetName(typeof(NationActivityType), Type);

        public string Meta { get; set; }

        public Guid InitiatorId { get; set; }

        [NotMapped]
        public string Username { get; set; }

        [NotMapped]
        public string PlayerName { get; set; }

        public NationHistory()
        {
            TimeStamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Defines all different types of logged nation actions.
        /// </summary>
        public enum NationActivityType
        {
            Created,
            Joined,
            Rename
        }

        /// <summary>
        /// Logs nation activity
        /// </summary>
        /// <param name="nationId">The player to which to send a message.</param>
        /// <param name="player">The player which this activity impacts.</param>
        /// <param name="type">The type of message we are sending.</param>
        /// <param name="meta">Any other info regarding this activity</param>
        public static void LogActivity(
            Guid Id,
            Guid nationId,
            Player player,
            NationActivityType type,
            string meta = ""
        ) =>
            LogActivity(
                nationId,
                player?.UserId ?? default,
                player?.Id ?? default,
                player?.Client?.Ip ?? string.Empty,
                type,
                meta
            );

        public static void LogActivity(Guid nationId, Guid userId, Guid playerId, string playerIp, NationActivityType type, string meta = "")
        {
            if (Options.Instance.Logging.NationActivity)
            {
                DbInterface.Pool.QueueWorkItem(new Action<NationHistory>(Log), new NationHistory
                {
                    NationId = nationId,
                    TimeStamp = DateTime.UtcNow,
                    UserId = userId,
                    PlayerId = playerId,
                    Ip = playerIp,
                    Type = type,
                    Meta = meta,
                });
            }
        }

        private static void Log(NationHistory nationHistory)
        {
            using var loggingContext = DbInterface.CreateLoggingContext(readOnly: false);
            _ = loggingContext.NationHistory.Add(nationHistory);
        }
    }
}