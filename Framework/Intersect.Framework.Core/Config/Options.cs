using System.ComponentModel;
using Intersect.Config;
using Intersect.Config.Guilds;
using Intersect.Framework.Core.Config;
using Intersect.Logging;
using Intersect.Core;
using Intersect.Framework.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Intersect;

public partial record Options
{
    private static readonly JsonSerializerSettings PrivateIndentedSerializerSettings = new()
    {
        ContractResolver = new OptionsContractResolver(true, false),
        Formatting = Formatting.Indented,
    };

    private static readonly JsonSerializerSettings PrivateSerializerSettings = new()
    {
        ContractResolver = new OptionsContractResolver(true, false),
    };

    private static readonly JsonSerializerSettings PublicSerializerSettings = new()
    {
        ContractResolver = new OptionsContractResolver(false, true),
    };

    #region Constants

    public const string DefaultGameName = "Intersect";

    public const int DefaultServerPort = 5400;

    public const string CategoryCore = nameof(CategoryCore);

    public const string CategoryDatabase = nameof(CategoryDatabase);

    public const string CategoryGameAccess = nameof(CategoryGameAccess);

    public const string CategoryLoggingAndMetrics = nameof(CategoryLoggingAndMetrics);

    public const string CategoryNetworkVisibility = nameof(CategoryNetworkVisibility);

    public const string CategorySecurity = nameof(CategorySecurity);

    #endregion

    #region Static Properties

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

    public static Options Instance { get; private set; }

    public static Options? PendingChanges { get; private set; }

    public static bool IsLoaded => Instance != null;

    #endregion Static Properties

    #region Transient Properties

    [Ignore]
    [JsonIgnore]
    public string OptionsData { get; private set; } = string.Empty;

    [Ignore]
    public bool SmtpValid { get; private set; }

    #endregion Transient Properties

    #region Configuration Properties

    #region Game Core

    [Category(CategoryCore)]
    [JsonProperty(Order = -100)]
    [RequiresRestart]
    public string GameName { get; set; } = DefaultGameName;

    [Category(CategoryCore)]
    [JsonProperty(Order = -100)]
    [RequiresRestart]
    public ushort ServerPort { get; set; } = DefaultServerPort;

    #endregion Game Core

    #region Game Access

    [Category(CategoryGameAccess)]
    [JsonProperty(Order = -99)]
    public bool AdminOnly { get; set; }


    [Category(CategoryGameAccess)]
    [JsonProperty(Order = -99)]
    public bool BlockClientRegistrations { get; set; }


    [Category(CategoryGameAccess)]
    [JsonProperty(Order = -99)]
    public int MaxClientConnections { get; set; } = 100;

    /// <summary>
    /// Defines the maximum amount of logged-in users our server is allowed to handle.
    /// </summary>
    [Category(CategoryGameAccess)]
    [JsonProperty(Order = -99)]
    public int MaximumLoggedInUsers { get; set; } = 50;

    #endregion Game Access

    public static int MaxLevel => Instance.PlayerOpts.MaxLevel;
    public static int MaxInvItems => Instance.PlayerOpts.MaxInventory;
    #region Network Visibility

    [Category(CategoryNetworkVisibility)]
    [JsonProperty(Order = -91)]
    [RequiresRestart]
    public bool UPnP { get; set; } = true;

    [Category(CategoryNetworkVisibility)]
    [JsonProperty(Order = -91)]
    [RequiresRestart]
    public bool OpenPortChecker { get; set; } = true;

    [Category(CategoryNetworkVisibility)]
    [JsonProperty(Order = -91, NullValueHandling = NullValueHandling.Include)]
    [RequiresRestart]
    public string? PortCheckerUrl { get; set; }

    #endregion Network Visibility

    #region Logging and Metrics

    [Category(CategoryLoggingAndMetrics)]
    [JsonProperty(Order = -80)]
    public LoggingOptions Logging { get; set; } = new();

    [Category(CategoryLoggingAndMetrics)]
    [JsonProperty(Order = -80)]
    public MetricsOptions Metrics { get; set; } = new();

    #endregion Logging and Metrics

    #region Database

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
    [Category(CategoryDatabase)]
    [JsonProperty(Order = -70)]
    [RequiresRestart]
    public DatabaseOptions GameDatabase { get; set; } = new();

    [Category(CategoryDatabase)]
    [JsonProperty(Order = -70)]
    [RequiresRestart]
    public DatabaseOptions LoggingDatabase { get; set; } = new();

    [Category(CategoryDatabase)]
    [JsonProperty(Order = -70)]
    [RequiresRestart]
    public DatabaseOptions PlayerDatabase { get; set; } = new();

    #endregion Database

    #region Security

    [Category(CategorySecurity)]
    [JsonProperty(Order = -60)]
    [RequiresRestart]
    public SecurityOptions Security { get; set; } = new();

    [Category(CategorySecurity)]
    [JsonProperty(Order = -60)]
    [RequiresRestart]
    public SmtpSettings SmtpSettings { get; set; } = new();

    #endregion Security

    #region Other Game Properties

    [RequiresRestart]
    public List<string> AnimatedSprites { get; set; } = [];

    [RequiresRestart]
    public PacketOptions Packets { get; set; } = new();

    public ChatOptions Chat { get; set; } = new();

    [RequiresRestart]
    public CombatOptions Combat { get; set; } = new();

    [RequiresRestart]
    public EquipmentOptions Equipment { get; set; } = new();

    [RequiresRestart]
    public int EventWatchdogKillThreshold { get; set; } = 5000;

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

    [JsonProperty("ServerPort", Order = -4)]
    public ushort _serverPort { get; set; } = DEFAULT_SERVER_PORT;

    /// <summary>
    /// Passability configuration by map zone
    /// </summary>
    public PassabilityOptions Passability { get; set; } = new();

    public ushort ValidPasswordResetTimeMinutes { get; set; } = 30;

    public MapOptions Map { get; set; } = new();

    public PlayerOptions Player { get; set; } = new();

    public PartyOptions Party { get; set; } = new();

    public LootOptions Loot { get; set; } = new();

    public ProcessingOptions Processing { get; set; } = new();

    public SpriteOptions Sprites { get; set; } = new();

    public NpcOptions Npc { get; set; } = new();

    public QuestOptions Quest { get; set; } = new();

    public GuildOptions Guild { get; set; } = new();

    public BankOptions Bank { get; set; } = new();

    public InstancingOptions Instancing { get; set; } = new();

    public ItemOptions Items { get; set; } = new();

    #endregion Other Game Properties

    #endregion Configuration Properties

    public void FixAnimatedSprites()
    {
        for (var i = 0; i < AnimatedSprites.Count; i++)
        {
            AnimatedSprites[i] = AnimatedSprites[i].ToLower();
        }
    }

    public static bool LoadFromDisk()
    {
        var instance = EnsureCreated();

        var pathToServerConfig = Path.Combine(ResourcesDirectory, "config.json");
        if (!Directory.Exists(ResourcesDirectory))
        {
            Directory.CreateDirectory(ResourcesDirectory);
        }
        else if (File.Exists(pathToServerConfig))
        {
            var rawJson = File.ReadAllText(pathToServerConfig);
            instance = JsonConvert.DeserializeObject<Options>(rawJson, PrivateSerializerSettings) ?? instance;
            Instance = instance;
        }

        instance.SmtpValid = instance.SmtpSettings.IsValid();
        instance.FixAnimatedSprites();

        SaveToDisk();

        return true;
    }

    internal static Options EnsureCreated()
    {
        Options instance = new();
        Instance = instance;
        return instance;
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

        try
        {
            var serializedPrivateConfiguration = JsonConvert.SerializeObject(instance, PrivateIndentedSerializerSettings);
            File.WriteAllText(pathToServerConfig, serializedPrivateConfiguration);
        }
        catch (Exception exception)
        {
            ApplicationContext.Context.Value?.Logger.LogError(
                exception,
                "Failed to save options to {OptionsPath}",
                pathToServerConfig
            );
        }

        instance.OptionsData = JsonConvert.SerializeObject(instance, PublicSerializerSettings);
    }

    public static void LoadFromServer(string data)
    {
        try
        {
            var loadedOptions = JsonConvert.DeserializeObject<Options>(data, PublicSerializerSettings);
            Instance = loadedOptions;
            OptionsLoaded?.Invoke(loadedOptions);
        }
        catch (Exception exception)
        {
            ApplicationContext.CurrentContext.Logger.LogError(exception, "Failed to load options from server");
            throw;
        }
    }

    public static event OptionsLoadedEventHandler? OptionsLoaded;

    public Options DeepClone() => JsonConvert.DeserializeObject<Options>(
        JsonConvert.SerializeObject(this, PrivateSerializerSettings)
    );
}