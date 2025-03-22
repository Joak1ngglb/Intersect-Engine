using System.Runtime.Serialization;

namespace Intersect.Config.Guilds;

/// <summary>
/// Contains all options pertaining to guilds
/// </summary>
public partial class GuildOptions
{
    /// <summary>
    /// The intersect default value for guild bank slots
    /// </summary>
    public const int DefaultBankSlots = 50;

    /// <summary>
    /// Configures whether or not to allow guild members to attack eachother.
    /// </summary>
    public bool AllowGuildMemberPvp { get; set; } = false;

    /// <summary>
    /// Configured whether the guild name should be rendered above player sprites as a tag
    /// </summary>
    public bool ShowGuildNameTagsOverMembers { get; set; } = true;

    /// <summary>
    /// How often to send guild updates to members, these updates are alongside updates whenever people log in or out
    /// </summary>
    public int GuildUpdateInterval = 10000;

    /// <summary>
    /// If set to a value > 0 then upon server boot any guilds with only 1 member that hasn't played in this number of days will be deleted
    /// </summary>
    public int DeleteStaleGuildsAfterDays { get; set; } = -1;

    /// <summary>
    /// Default number of storage slots in guild banks
    /// </summary>
    public int InitialBankSlots { get; set; } = DefaultBankSlots;

    /// <summary>
    /// Array of guild ranks that are available in this game
    /// </summary>
    public GuildRank[] Ranks { get; set; } = new GuildRank[]
    {
        new GuildRank()
        {
            Title = "Master",
            Limit = 1,
            Permissions = new GuildPermissions()
            {
                Invite = true,
                Kick = true,
                Demote = true,
                Promote = true,
                BankDeposit = true,
                BankMove = true,
                BankRetrieve = true
            }
        },
        new GuildRank()
        {
            Title = "Officer",
            Limit = 10,
            Permissions = new GuildPermissions()
            {
                Invite = true,
                Kick = true,
                Demote = true,
                Promote = true,
                BankDeposit = true,
                BankMove = true,
                BankRetrieve = true
            }
        },
        new GuildRank()
        {
            Title = "Member",
            Limit = -1,
            Permissions = new GuildPermissions()
            {
                Invite = false,
                Kick = false,
                Demote = false,
                Promote = false,
                BankDeposit = true,
                BankMove = false,
                BankRetrieve = false
            }
        },
        new GuildRank()
        {
            Title = "Newbie",
            Limit = -1,
            Permissions = new GuildPermissions()
            {
                Invite = false,
                Kick = false,
                Demote = false,
                Promote = false,
                BankDeposit = false,
                BankMove = false,
                BankRetrieve = false
            }
        }

    };
    /// <summary>
    /// La cantidad base de experiencia requerida para el nivel 2.
    /// </summary>
    public long BaseXP { get; set; } = 500000;

    /// <summary>
    /// Factor de crecimiento de experiencia requerida por nivel.
    /// </summary>
    public double GrowthFactor { get; set; } = 1.7;

    /// <summary>
    /// Define el porcentaje de experiencia obtenida por un jugador que se transfiere al gremio.
    /// </summary>
    public double GuildXpContribution { get; set; } = 0.40; // 40% de la XP obtenida va al gremio.

    /// <summary>
    /// Número inicial de miembros permitidos en un gremio.
    /// </summary>
    public int InitialMaxMembers { get; set; } = 10;
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        Validate();
    }

    public void Validate()
    {
        if (Ranks.Length < 2)
        {
            throw new Exception("Must have at least 2 guild ranks defined!");
        }

        if (Ranks[0].Limit != 1)
        {
            throw new Exception($"{Ranks[0].Title} is considered a guild leader (first rank) so the limit should be 1.");
        }

        //Leader is always going to be the first rank, just make sure they can invite or kick
        Ranks[0].Permissions.Invite = true;
        Ranks[0].Permissions.Kick = true;
    }
}
