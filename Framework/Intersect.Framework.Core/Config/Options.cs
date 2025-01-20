using Intersect.Config;
using Intersect.Config.Guilds;
using Intersect.Config;
using Intersect.Config.Guilds;
using Intersect.Framework.Core.Config;
using Intersect.Logging;
using Intersect.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Intersect;

public partial class Options
{
    //Caching Json
    private static string optionsCompressed = string.Empty;

    [JsonProperty("AdminOnly", Order = -3)]
    protected bool _adminOnly = false;

    //Constantly Animated Sprites
    [JsonProperty("AnimatedSprites")]
    protected List<string> _animatedSprites = new List<string>();

    [JsonProperty("BlockClientRegistrations", Order = -2)]
    protected bool _blockClientRegistrations = false;

    [JsonProperty("ValidPasswordResetTimeMinutes")]
    protected ushort _passResetExpirationMin = 30;

    [JsonProperty("OpenPortChecker", Order = 0)]
    protected bool _portChecker = true;

    [JsonProperty(NullValueHandling = NullValueHandling.Include)]
    public string? PortCheckerUrl { get; set; }

    [JsonProperty("MaxClientConnections")]
    public int MaxClientConnections { get; set; }= 100;

    [JsonProperty("MaximumLoggedinUsers")]
    protected int _maxUsers = 50;

    [JsonProperty("UPnP", Order = -1)]
    protected bool _upnp = true;

    [JsonProperty("Chat")]
    public ChatOptions ChatOpts = new ChatOptions();

    [JsonProperty("Combat")]
    public CombatOptions CombatOpts = new CombatOptions();

    [JsonProperty("Equipment")]
    public EquipmentOptions EquipmentOpts = new EquipmentOptions();

    [JsonProperty("EventWatchdogKillThreshold")]
    public int EventKillTheshhold = 5000;

    [JsonProperty("Map")]
    public MapOptions MapOpts = new MapOptions();

    public DatabaseOptions GameDatabase = new DatabaseOptions();

    public DatabaseOptions LoggingDatabase = new DatabaseOptions();

    public DatabaseOptions PlayerDatabase = new DatabaseOptions();

    [JsonProperty("Player")]
    public PlayerOptions PlayerOpts = new PlayerOptions();
    [JsonProperty("Jobs")]
    public JobOptions JobOpts = new JobOptions(); // Integración de JobOptions
    [JsonProperty("Party")]
    public PartyOptions PartyOpts = new PartyOptions();

    [JsonProperty("Security")]
    public SecurityOptions SecurityOpts = new SecurityOptions();

    [JsonProperty("Loot")]
    public LootOptions LootOpts = new LootOptions();

    public ProcessingOptions Processing = new ProcessingOptions();

    public SpriteOptions Sprites = new SpriteOptions();

    [JsonProperty("Npc")]
    public NpcOptions NpcOpts = new NpcOptions();

    public MetricsOptions Metrics = new MetricsOptions();

    public PacketOptions Packets = new PacketOptions();

    public SmtpSettings SmtpSettings = new SmtpSettings();

    public QuestOptions Quest = new QuestOptions();

    public GuildOptions Guild = new GuildOptions();

    public LoggingOptions Logging = new LoggingOptions();

    public BankOptions Bank = new BankOptions();

    public InstancingOptions Instancing = new InstancingOptions();

    public ItemOptions Items = new ItemOptions();

    [JsonIgnore]
    public string OptionsData { get; private set; } = string.Empty;

    public static Options Instance { get; private set; }

    [JsonIgnore]
    public bool SendingToClient { get; set; } = true;

    [JsonProperty(Order = -3)]
    public bool AdminOnly { get; set; }

    public List<string> AnimatedSprites { get; set; } = [];

    [JsonProperty(Order = -2)]
    public bool BlockClientRegistrations { get; set; }

    public ushort ValidPasswordResetTimeMinutes { get; set; } = 30;

    [JsonProperty(Order = 0)]
    public bool OpenPortChecker { get; set; } = true;

    [JsonProperty(NullValueHandling = NullValueHandling.Include)]
    public string? PortCheckerUrl { get; set; }

    public int MaxClientConnections { get; set; } = 100;

    /// <summary>
    /// Defines the maximum amount of logged-in users our server is allowed to handle.
    /// </summary>
    public int MaximumLoggedInUsers { get; set; } = 50;

    [JsonProperty(Order = -1)]
    public bool UPnP { get; set; } = true;

    public static int MaxLevel => Instance.PlayerOpts.MaxLevel;
    public static int MaxInvItems => Instance.PlayerOpts.MaxInventory;

    public EquipmentOptions Equipment = new();

    public int EventWatchdogKillThreshold { get; set; } = 5000;

    public static int RequestTimeout => Instance.PlayerOpts.RequestTimeout;

    public static int TradeRange => Instance.PlayerOpts.TradeRange;

    public static int WeaponIndex => Instance.EquipmentOpts.WeaponSlot;

    public static int ShieldIndex => Instance.EquipmentOpts.ShieldSlot;

    public static List<string> EquipmentSlots => Instance.EquipmentOpts.Slots;

    public static List<string>[] PaperdollOrder => Instance.EquipmentOpts.Paperdoll.Directions;

    public static bool CombatFlashes => Instance.CombatOpts.CombatFlashes;
    public static float CriticalHitFlashIntensity => Instance.CombatOpts.CriticalHitFlashIntensity;
    public static float HitFlashDuration => Instance.CombatOpts.HitFlashDuration;
    public static string CriticalHitReceivedSound => Instance.CombatOpts.CriticalHitReceivedSound;
    public static float DamageTakenFlashIntensity => Instance.CombatOpts.DamageTakenFlashIntensity;
    public static float DamageTakenShakeAmount => Instance.CombatOpts.DamageTakenShakeAmount;
    public static float DamageGivenShakeAmount => Instance.CombatOpts.DamageGivenShakeAmount;
    public static float MaxDamageShakeDistance => Instance.CombatOpts.MaxDamageShakeDistance;
    public static string GenericDamageGivenSound => Instance.CombatOpts.GenericDamageGivenSound;
    public static string GenericDamageReceivedSound => Instance.CombatOpts.GenericDamageReceivedSound;
    public static float ResourceDestroyedShakeAmount => Instance.CombatOpts.ResourceDestroyedShakeAmount;
    public static string CriticalHitDealtSound => Instance.CombatOpts.CriticalHitDealtSound;

    public static List<string> ToolTypes => Instance.EquipmentOpts.ToolTypes;

    public static List<string> AnimatedSprites => Instance._animatedSprites;

    public static int RegenTime => Instance.CombatOpts.RegenTime;

    public static int CombatTime => Instance.CombatOpts.CombatTime;

    public static int MinAttackRate => Instance.CombatOpts.MinAttackRate;

    public static int MaxAttackRate => Instance.CombatOpts.MaxAttackRate;

    public static int BlockingSlow => Instance.CombatOpts.BlockingSlow;

    public static int MaxDashSpeed => Instance.CombatOpts.MaxDashSpeed;

    public static int GameBorderStyle => Instance.MapOpts.GameBorderStyle;

    public static bool ZDimensionVisible => Instance.MapOpts.ZDimensionVisible;

    public static int MapWidth => Instance?.MapOpts?.MapWidth ?? 32;

    public static int MapHeight => Instance?.MapOpts?.MapHeight ?? 26;

    public static int TileWidth => Instance.MapOpts.TileWidth;

    public static int TileHeight => Instance.MapOpts.TileHeight;

    public static int EventWatchdogKillThreshhold => Instance.EventKillTheshhold;

    public static int MaxChatLength => Instance.ChatOpts.MaxChatLength;

    public static int MinChatInterval => Instance.ChatOpts.MinIntervalBetweenChats;

    public static LootOptions Loot => Instance.LootOpts;

    public static NpcOptions Npc => Instance.NpcOpts;

    public static PartyOptions Party => Instance.PartyOpts;

    public static ChatOptions Chat => Instance.ChatOpts;

    public static bool UPnP => Instance._upnp;

    public static bool OpenPortChecker => Instance._portChecker;

    public static SmtpSettings Smtp => Instance.SmtpSettings;

    public static int PasswordResetExpirationMinutes => Instance._passResetExpirationMin;

    public static bool AdminOnly
    {
        get => Instance._adminOnly;
        set => Instance._adminOnly = value;
    }

    public static bool BlockClientRegistrations
    {
        get => Instance._blockClientRegistrations;
        set => Instance._blockClientRegistrations = value;
    }

    public static PlayerOptions Player => Instance.PlayerOpts;
    public static JobOptions Jobs => Instance.JobOpts; // Propiedad estática para acceder fácilmente a JobOptions
    public static string RecipesId => Instance.JobOpts.RecipesId; // Propiedad estática para acceder fácilmente al ID de las recetas
    public static int MaxJobLevel => Instance.JobOpts.MaxJobLevel; // Propiedad estática para el nivel máximo de los trabajos
    public static Dictionary<JobType, long> JobBaseExp => Instance.JobOpts.JobBaseExp;

    public static double GainBaseExponent => Instance.JobOpts.ExpGrowthRate;
    public static EquipmentOptions Equipment => Instance.EquipmentOpts;

    public static CombatOptions Combat => Instance.CombatOpts;

    public static MapOptions Map => Instance.MapOpts;

    public static bool Loaded => Instance != null;

    [JsonProperty("GameName", Order = -5)]
    public string GameName { get; set; } = DEFAULT_GAME_NAME;

    [JsonProperty(Order = -4)]
    public ushort ServerPort { get; set; } = DEFAULT_SERVER_PORT;

    /// <summary>
    /// Passability configuration by map zone
    /// </summary>
    public Passability Passability { get; } = new();

    public MapOptions Map = new();

    public DatabaseOptions GameDatabase = new();

    public DatabaseOptions LoggingDatabase = new();

    public DatabaseOptions PlayerDatabase = new();

    public PlayerOptions Player = new();

    public PartyOptions Party = new();

    public SecurityOptions Security = new();

    public LootOptions Loot = new();

    public ProcessingOptions Processing = new();

    public SpriteOptions Sprites = new();

    public NpcOptions Npc = new();

    public MetricsOptions Metrics = new();

    public PacketOptions Packets = new();

    public SmtpSettings SmtpSettings = new();

    public QuestOptions Quest = new();

    public GuildOptions Guild = new();

    public LoggingOptions Logging = new();

    public BankOptions Bank = new();

    public InstancingOptions Instancing = new();

    public ItemOptions Items = new();

    public static Options Instance { get; private set; }

    public static bool IsLoaded => Instance != null;

    public bool SmtpValid { get; set; }

    public void FixAnimatedSprites()
    {
        for (var i = 0; i < AnimatedSprites.Count; i++)
        {
            AnimatedSprites[i] = AnimatedSprites[i].ToLower();
        }
    }

    public static string ResourcesDirectory { get; set; } = "resources";

    public static bool LoadFromDisk()
    {
        Options instance = new();
        Instance = instance;

        var pathToServerConfig = Path.Combine(ResourcesDirectory, "config.json");
        if (!Directory.Exists(ResourcesDirectory))
        {
            Directory.CreateDirectory(ResourcesDirectory);
        }
        else if (File.Exists(pathToServerConfig))
        {
            instance = JsonConvert.DeserializeObject<Options>(File.ReadAllText(pathToServerConfig)) ?? instance;
            Instance = instance;
        }

        instance.SmtpValid = instance.SmtpSettings.IsValid();
        instance.FixAnimatedSprites();

        SaveToDisk();

        return true;
    }

    public static void SaveToDisk()
    {
        if (Instance is not { } instance)
        {
            ApplicationContext.Context.Value?.Logger.LogError("Tried to save null instance to disk");
            return;
        }

        if (!Directory.Exists(ResourcesDirectory))
        {
            Directory.CreateDirectory(ResourcesDirectory);
        }

        var pathToServerConfig = Path.Combine(ResourcesDirectory, "config.json");

        instance.SendingToClient = false;
        try
        {
            File.WriteAllText(
                pathToServerConfig,
                JsonConvert.SerializeObject(instance, Formatting.Indented)
            );
        }
        catch (Exception exception)
        {
            ApplicationContext.Context.Value?.Logger.LogError(
                exception,
                "Failed to save options to {OptionsPath}",
                pathToServerConfig
            );
        }
        instance.SendingToClient = true;
        instance.OptionsData = JsonConvert.SerializeObject(instance);
    }

    public static void LoadFromServer(string data)
    {
        Instance = JsonConvert.DeserializeObject<Options>(data);
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeGameDatabase()
    {
        return !SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeLoggingDatabase()
    {
        return !SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeLogging()
    {
        return !SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializePlayerDatabase()
    {
        return !SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeSmtpSettings()
    {
        return !SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeSmtpValid()
    {
        return SendingToClient;
    }

    // ReSharper disable once UnusedMember.Global
    public bool ShouldSerializeSecurity()
    {
        return !SendingToClient;
    }

    #region Constants

    // TODO: Clean these up
    //Values that cannot easily be changed:

    public const string DEFAULT_GAME_NAME = "Intersect";

    public const int DEFAULT_SERVER_PORT = 5400;

    #endregion
}
