using System.Diagnostics.CodeAnalysis;
using Intersect.Client.Entities;
using Intersect.Client.Entities.Events;
using Intersect.Client.Framework.Database;
using Intersect.Client.Framework.Entities;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.Framework.Input;
using Intersect.Client.Framework.Sys;
using Intersect.Client.Items;
using Intersect.Client.Maps;
using Intersect.Client.Plugins.Interfaces;
using Intersect.Core;
using Intersect.Enums;
using Intersect.Framework.Core.Config;
using Intersect.Framework.Core.GameObjects.Crafting;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Microsoft.Extensions.Logging;

namespace Intersect.Client.General;

public static partial class Globals
{

    /// Indica si el otro jugador ha aceptado el intercambio
    public static bool TradeAccepted { get; set; } = false;

    //Only need 1 table, and that is the one we see at a given moment in time.
    public static CraftingTableBase ActiveCraftingTable;

    public static int AnimFrame = 0;

    //Bag
    public static Item[] Bag = null;

    //Bank
    public static Item[] Bank;
    public static bool GuildBank;
    public static int BankSlots;

    public static bool ConnectionLost;
    // Mail
    public static List<Mail> Mails = new List<Mail>();
    /// <summary>
    /// This is used to prevent the client from showing unnecessary disconnect messages
    /// </summary>
    public static bool SoftLogout;

    //Game Systems
    public static GameContentManager ContentManager;

    public static int CurrentMap = -1;

    public static GameDatabase Database;

    //Entities and stuff
    //public static List<Entity> Entities = new List<Entity>();
    public static readonly Dictionary<Guid, Entity> Entities = [];

    public static readonly List<Guid> EntitiesToDispose = [];

    //Control Objects
    public static readonly List<Dialog> EventDialogs = [];

    public static readonly Dictionary<Guid, Guid> EventHolds = [];

    //Game Lock
    public static readonly object GameLock = new();

    //Crucial game variables

    internal static readonly List<IClientLifecycleHelper> ClientLifecycleHelpers = [];

    private static GameStates mGameState = GameStates.Intro;

    public static readonly Dictionary<Guid, Point> GridMaps = [];

    public static bool HasGameData = false;

    public static bool InBag = false;

    public static bool InBank = false;

    //Crafting station
    public static bool InCraft = false;

    public static bool InTrade = false;

    public static GameInput InputManager;

    public static bool IntroComing = true;

    public static readonly long IntroDelay = 2000;

    //Engine Progression
    public static int IntroIndex = 0;

    public static long IntroStartTime = -1;

    public static bool IsRunning = true;

    public static bool JoiningGame = false;

    public static bool LoggedIn = false;

    //Map/Chunk Array
    public static Guid[,]? MapGrid;

    public static long MapGridHeight;

    public static long MapGridWidth;

    //Local player information
    public static Player? Me;

    public static bool MoveRouteActive = false;

    public static bool NeedsMaps = true;

    //Event Guid and the Map its associated with
    public static readonly Dictionary<Guid, Dictionary<Guid, EventEntityPacket>> PendingEvents = new();

    //Event Show Pictures
    public static ShowPicturePacket? Picture;

    public static readonly List<Guid> QuestOffers = new();

    public static readonly Random Random = new();

    public static GameSystem System;

    //Trading (Only 2 people can trade at once)
    public static Item[,] Trade;

    //Scene management

    //Only need 1 table, and that is the one we see at a given moment in time.
    public static CraftingTableDescriptor? ActiveCraftingTable { get; set; }

    public static int AnimationFrame { get; set; }

    //Bag
    public static Item[]? BagSlots { get; set; }

    //Bank
    public static Item[]? BankSlots { get; set; }
    public static bool IsGuildBank { get; set; }
    public static int BankSlotCount { get; set; }

    public static bool ConnectionLost { get; set; }

    /// <summary>
    ///     This is used to prevent the client from showing unnecessary disconnect messages
    /// </summary>
    public static bool SoftLogout { get; set; }

    //Game Systems
    public static GameContentManager? ContentManager { get; set; }

    public static GameDatabase? Database { get; set; }

    //Game Shop
    //Only need 1 shop, and that is the one we see at a given moment in time.
    public static ShopDescriptor? GameShop { get; set; }

    /// <see cref="GameStates" />
    public static GameStates GameState
    {
        get => mGameState;
        set
        {
            mGameState = value;
            EmitLifecycleChangingState();
        }
    }

    public static bool InShop => GameShop != null;

    public static bool CanCloseInventory => !(InBag || InBank || InCraft || InShop || InTrade);

    public static bool HoldToSoftRetargetOnSelfCast { get; set; }

    public static bool ShouldSoftRetargetOnSelfCast =>
        HoldToSoftRetargetOnSelfCast || Database.AutoSoftRetargetOnSelfCast;

    public static bool WaitingOnServer { get; set; }

    internal static void EmitLifecycleChangingState()
    {
        foreach (var clientLifecycleHelper in ClientLifecycleHelpers)
        {
            clientLifecycleHelper.EmitLifecycleChangingState(GameState);
        }
    }

    internal static void EmitLifecycleChangedState()
    {
        foreach (var clientLifecycleHelper in ClientLifecycleHelpers)
        {
            clientLifecycleHelper.EmitLifecycleChangedState(GameState);
        }
    }

    internal static void OnGameUpdate(TimeSpan deltaTime)
    {
        // Gather all known entities before passing them on to the plugins!
        // The global entity list is incomplete and lacks events.
        var knownEntities = new Dictionary<Guid, IEntity>();

        if (Entities != null)
        {
            foreach (var en in Entities)
            {
                if (!knownEntities.ContainsKey(en.Key))
                {
                    knownEntities.Add(en.Key, en.Value);
                }
            }
        }

        if (MapGrid != null)
        {
            for (var x = 0; x < MapGridWidth; x++)
            {
                for (var y = 0; y < MapGridHeight; y++)
                {
                    var map = MapInstance.Get(MapGrid[x, y]);
                    if (map != null)
                    {
                        foreach (var en in map.LocalEntities)
                        {
                            if (!knownEntities.ContainsKey(en.Key))
                            {
                                knownEntities.Add(en.Key, en.Value);
                            }
                        }
                    }
                }
            }
        }

        ClientLifecycleHelpers.ForEach(
            clientLifecycleHelper => clientLifecycleHelper?.EmitGameUpdate(GameState, Me, knownEntities, deltaTime)
        );
    }

    internal static void OnGameDraw(DrawStates state, TimeSpan deltaTime)
    {
        ClientLifecycleHelpers.ForEach(clientLifecycleHelper => clientLifecycleHelper?.EmitGameDraw(state, deltaTime));
    }

    internal static void OnGameDraw(DrawStates state, IEntity entity, TimeSpan deltaTime)
    {
        ClientLifecycleHelpers.ForEach(
            clientLifecycleHelper => clientLifecycleHelper?.EmitGameDraw(state, entity, deltaTime)
        );
    }

    public static bool TryGetEntity(EntityType entityType, Guid entityId, [NotNullWhen(true)] out Entity? entity)
    {
        if (!Entities.TryGetValue(entityId, out entity))
        {
            return false;
        }
    
    public static Dictionary<Guid, Dictionary<Guid, int>> QuestRewards = new Dictionary<Guid, Dictionary<Guid, int>>();
    public static Dictionary<Guid, long> QuestExperience = new Dictionary<Guid, long>();
    public static Dictionary<Guid, Dictionary<JobType, long>> QuestJobExperience = new Dictionary<Guid, Dictionary<JobType, long>>();

    public static List<Guid> GridMaps = new List<Guid>();

    public static bool HasGameData = false;

    public static bool InBag = false;

    public static bool InBank = false;

    //Crafting station
    public static bool InCraft = false;

    public static bool InShop => GameShop != null;

    public static bool InTrade = false;

    public static bool CanCloseInventory => !(InBag || InBank || InCraft || InShop || InTrade);

    public static GameInput InputManager;

    public static bool IntroComing = true;

    public static long IntroDelay = 2000;

    //Engine Progression
    public static int IntroIndex = 0;

    public static long IntroStartTime = -1;

    public static bool IsRunning = true;

    public static bool JoiningGame = false;

    public static bool LoggedIn = false;

    //Map/Chunk Array
    public static Guid[,]? MapGrid;

    public static long MapGridHeight;

    public static long MapGridWidth;

    //Local player information
    public static Player? Me;

    public static bool MoveRouteActive = false;

    public static bool NeedsMaps = true;

    //Event Guid and the Map its associated with
    public static Dictionary<Guid, Dictionary<Guid, EventEntityPacket>> PendingEvents =
        new Dictionary<Guid, Dictionary<Guid, EventEntityPacket>>();

    //Event Show Pictures
    public static ShowPicturePacket? Picture;

    public static List<Guid> QuestOffers = new List<Guid>();
    public static Dictionary<Guid, Dictionary<Guid, int>> QuestRewards = new Dictionary<Guid, Dictionary<Guid, int>>();
    public static Dictionary<Guid, long> QuestExperience = new Dictionary<Guid, long>();
    public static Dictionary<Guid, Dictionary<JobType, long>> QuestJobExperience = new Dictionary<Guid, Dictionary<JobType, long>>();


    public static Random Random = new Random();

    public static GameSystem System;

    //Trading (Only 2 people can trade at once)
    public static Item[,] Trade;

    //Scene management
    private static bool _waitingOnServer;

    public static bool WaitingOnServer
    {
        get => _waitingOnServer;
        set => _waitingOnServer = value;
    }
 
    public static Entity GetEntity(Guid id, EntityType type)
    {
        if (Entities.ContainsKey(id))
        {
            Entities.Remove(entityId);
            entity = null;
            return false;
        }

        if (entity.Type != entityType)
        {
            ApplicationContext.CurrentContext.Logger.LogError(
                "Found instance of {ActualEntityType} registered to {EntityId} but it was expected to be {ExpectedEntityType}",
                entity.Type,
                entityId,
                entityType
            );
            Entities.Remove(entityId);
            entity.Dispose();
            entity = null;
            return false;
        }

        EntitiesToDispose.Remove(entityId);
        return true;
    }

    public static Entity? GetEntity(Guid id, EntityType type) => TryGetEntity(type, id, out var entity) ? entity : null;
}