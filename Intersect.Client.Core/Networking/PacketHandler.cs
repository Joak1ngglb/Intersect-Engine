using Intersect.Client.Core;
using Intersect.Client.Entities;
using Intersect.Client.Entities.Events;
using Intersect.Client.Entities.Projectiles;
using Intersect.Client.Framework.Entities;
using Intersect.Client.Framework.Items;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Chat;
using Intersect.Client.Interface.Menu;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Maps;
using Intersect.Configuration;
using Intersect.Core;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Maps;
using Intersect.GameObjects.Maps.MapList;
using Intersect.Logging;
using Intersect.Network;
using Intersect.Network.Packets;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;
using Intersect.Framework;
using Intersect.Models;
using Intersect.Client.Interface.Shared;
using Intersect.Network.Packets.Client;

namespace Intersect.Client.Networking;


internal sealed partial class PacketHandler
{
    private sealed partial class VirtualPacketSender : IPacketSender
    {
        public IApplicationContext ApplicationContext { get; }

        public INetwork Network => Networking.Network.Socket.Network;

        public VirtualPacketSender(IApplicationContext applicationContext) =>
            ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));

        #region Implementation of IPacketSender

        /// <inheritdoc />
        public bool Send(IPacket packet)
        {
            if (packet is IntersectPacket intersectPacket)
            {
                Networking.Network.SendPacket(intersectPacket);
                return true;
            }

            return false;
        }

        #endregion
    }

    public long Ping { get; set; } = 0;

    public long PingTime { get; set; }

    public IClientContext Context { get; }

    public ILogger Logger => Context.Logger;

    public PacketHandlerRegistry Registry { get; }

    public IPacketSender VirtualSender { get; }

    public PacketHandler(IClientContext context, PacketHandlerRegistry packetHandlerRegistry)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Registry = packetHandlerRegistry ?? throw new ArgumentNullException(nameof(packetHandlerRegistry));

        if (!Registry.TryRegisterAvailableMethodHandlers(GetType(), this, false) || Registry.IsEmpty)
        {
            throw new InvalidOperationException("Failed to register method handlers, see logs for more details.");
        }

        if (!Registry.TryRegisterAvailableTypeHandlers(GetType().Assembly))
        {
            throw new InvalidOperationException("Failed to register type handlers, see logs for more details.");
        }

        VirtualSender = new VirtualPacketSender(context);
    }

    public bool HandlePacket(IPacket packet)
    {
        if (packet is AbstractTimedPacket timedPacket)
        {
            Timing.Global.Synchronize(timedPacket.UTC, timedPacket.Offset);
        }

        if (!(packet is IntersectPacket))
        {
            return false;
        }

        if (!packet.IsValid)
        {
            return false;
        }

        if (!Registry.TryGetHandler(packet, out HandlePacketGeneric handler))
        {
            Logger.Error($"No registered handler for {packet.GetType().FullName}!");

            return false;
        }

        if (Registry.TryGetPreprocessors(packet, out var preprocessors))
        {
            if (!preprocessors.All(preprocessor => preprocessor.Handle(VirtualSender, packet)))
            {
                // Preprocessors are intended to be silent filter functions
                return false;
            }
        }

        if (Registry.TryGetPreHooks(packet, out var preHooks))
        {
            if (!preHooks.All(hook => hook.Handle(VirtualSender, packet)))
            {
                // Hooks should not fail, if they do that's an error
                Logger.Error($"PreHook handler failed for {packet.GetType().FullName}.");
                return false;
            }
        }

        if (!handler(VirtualSender, packet))
        {
            return false;
        }

        if (Registry.TryGetPostHooks(packet, out var postHooks))
        {
            if (!postHooks.All(hook => hook.Handle(VirtualSender, packet)))
            {
                // Hooks should not fail, if they do that's an error
                Logger.Error($"PostHook handler failed for {packet.GetType().FullName}.");
                return false;
            }
        }

        return true;
    }

    //PingPacket
    public void HandlePacket(IPacketSender packetSender, Intersect.Network.Packets.Server.PingPacket packet)
    {
        if (!packet.RequestingReply)
        {
            return;
        }

        PacketSender.SendPing();
        PingTime = Timing.Global.Milliseconds;
    }

    //ConfigPacket
    public void HandlePacket(IPacketSender packetSender, ConfigPacket packet)
    {
        Log.Debug("Received configuration from server.");
        Options.LoadFromServer(packet.Config);
        Globals.WaitingOnServer = false;
        MainMenu.HandleReceivedConfiguration();
        try
        {
            Strings.Load();
        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
        Graphics.InitInGame();
    }

    //JoinGamePacket
    public void HandlePacket(IPacketSender packetSender, JoinGamePacket packet)
    {
        Main.JoinGame();
        Globals.JoiningGame = true;
    }

    public void HandlePacket(IPacketSender packetSender, MapAreaPacket packet)
    {
        foreach (var map in packet.Maps)
        {
            HandleMap(packetSender, map);
        }
    }

    public void HandlePacket(IPacketSender packetSender, MapAreaIdsPacket packet)
    {
        // TODO: Background all of this?
        List<ObjectCacheKey<MapBase>> cacheKeys = new(packet.MapIds.Length);
        List<MapPacket> loadedCachedMaps = new(packet.MapIds.Length);
        foreach (var mapId in packet.MapIds)
        {
            if (ObjectDataDiskCache<MapBase>.TryLoad(mapId, out var cacheData))
            {
                ObjectCacheKey<MapBase> cacheKey = new(cacheData.Id);
                var deserializedCachedPacket = MessagePacker.Instance.Deserialize<MapPacket>(cacheData.Data, silent: true);
                if (deserializedCachedPacket != default)
                {
                    cacheKey = new ObjectCacheKey<MapBase>(
                        cacheData.Id,
                        cacheData.Checksum,
                        cacheData.Version
                    );
                    cacheKeys.Add(cacheKey);
                    loadedCachedMaps.Add(deserializedCachedPacket);
                    continue;
                }

                Log.Warn($"Failed to deserialized cached data for {cacheKey}, will fetch again");
            }

            cacheKeys.Add(new ObjectCacheKey<MapBase>(new Id<MapBase>(mapId)));
        }

        PacketSender.SendNeedMap(cacheKeys.ToArray());

        foreach (var cachedMap in loadedCachedMaps)
        {
            HandleMap(packetSender, cachedMap, skipSave: true);
        }
    }

    private void HandleMap(IPacketSender packetSender, MapPacket packet, bool skipSave = false)
    {
        var mapId = packet.MapId;

        if (!skipSave)
        {
            ObjectCacheData<MapBase> cacheData = new()
            {
                Id = new Id<MapBase>(mapId),
                Data = (packet as IntersectPacket).Data,
                Version = packet.CacheVersion,
            };
            ObjectCacheKey<MapBase> cacheKey = new(new Id<MapBase>(mapId), cacheData.Checksum, cacheData.Version);

            if (!ObjectDataDiskCache<MapBase>.TrySave(cacheData))
            {
                Log.Warn($"Failed to save cache for {cacheKey}");
            }
        }

        MapInstance.UpdateMapRequestTime(packet.MapId);

        if (MapInstance.TryGet(mapId, out var mapInstance))
        {
            if (packet.Revision == mapInstance.Revision)
            {
                return;
            }

            mapInstance.Dispose(false, false);
        }

        mapInstance = new MapInstance(mapId);
        MapInstance.Lookup.Set(mapId, mapInstance);
        lock (mapInstance.MapLock)
        {
            mapInstance.Load(packet.Data);
            mapInstance.LoadTileData(packet.TileData);
            mapInstance.AttributeData = packet.AttributeData;
            mapInstance.CreateMapSounds();
            if (mapId == Globals.Me.MapId)
            {
                Audio.PlayMusic(mapInstance.Music, ClientConfiguration.Instance.MusicFadeTimer, ClientConfiguration.Instance.MusicFadeTimer, true);
            }

            mapInstance.GridX = packet.GridX;
            mapInstance.GridY = packet.GridY;
            mapInstance.CameraHolds = packet.CameraHolds;
            mapInstance.Autotiles.InitAutotiles(mapInstance.GenerateAutotileGrid());

            if (Globals.PendingEvents.ContainsKey(mapId))
            {
                foreach (var evt in Globals.PendingEvents[mapId])
                {
                    mapInstance.AddEvent(evt.Key, evt.Value);
                }

                Globals.PendingEvents[mapId].Clear();
            }
        }

        MapInstance.OnMapLoaded?.Invoke(mapInstance);
    }

    public void HandlePacket(IPacketSender packetSender, TradeAcceptedPacket packet)
    {
        if (Globals.Me == null)
        {
            return;
        }
        Globals.TradeAccepted = true;
    }

    //MapPacket
    public void HandlePacket(IPacketSender packetSender, MapPacket packet)
    {
        HandleMap(packetSender, packet);
        Player.FetchNewMaps();
    }

    //PlayerEntityPacket
    public void HandlePacket(IPacketSender packetSender, PlayerEntityPacket packet)
    {
        var en = Globals.GetEntity(packet.EntityId, EntityType.Player);
        if (en != null)
        {
            en.Load(packet);
            if (packet.IsSelf)
            {
                Globals.Me = (Player) Globals.Entities[packet.EntityId];
            }
        }
        else
        {
            Globals.Entities.Add(packet.EntityId, new Player(packet.EntityId, packet));
            if (packet.IsSelf)
            {
                Globals.Me = (Player) Globals.Entities[packet.EntityId];
            }
        }
    }

    //PlayerRespawnPacket
    public void HandlePacket(IPacketSender packetSender, PlayerRespawnPacket packet)
    {
        Interface.Interface.GameUi?.GameMenu?.HideDeathWindow();
    }

    //NpcEntityPacket
    public void HandlePacket(IPacketSender packetSender, NpcEntityPacket packet)
    {
        var en = Globals.GetEntity(packet.EntityId, EntityType.GlobalEntity);
        if (en != null)
        {
            en.Load(packet);
            en.Aggression = packet.Aggression;
        }
        else
        {
            var entity = new Entity(packet.EntityId, packet, EntityType.GlobalEntity)
            {
                Aggression = packet.Aggression,
            };
            Globals.Entities.Add(entity.Id, entity);
        }
    }

    //ResourceEntityPacket
    public void HandlePacket(IPacketSender packetSender, ResourceEntityPacket packet)
    {
        var en = Globals.GetEntity(packet.EntityId, EntityType.Resource);
        if (en != null)
        {
            en.Load(packet);
        }
        else
        {
            var entity = new Resource(packet.EntityId, packet);
            Globals.Entities.Add(entity.Id, entity);
        }
    }

    //ProjectileEntityPacket
    public void HandlePacket(IPacketSender packetSender, ProjectileEntityPacket packet)
    {
        var en = Globals.GetEntity(packet.EntityId, EntityType.Projectile);
        if (en != null)
        {
            en.Load(packet);
        }
        else
        {
            var entity = new Projectile(packet.EntityId, packet);
            Globals.Entities.Add(entity.Id, entity);
        }
    }

    //EventEntityPacket
    public void HandlePacket(IPacketSender packetSender, EventEntityPacket packet)
    {
        var map = MapInstance.Get(packet.MapId);
        if (map != null)
        {
            map?.AddEvent(packet.EntityId, packet);
        }
        else
        {
            var dict = Globals.PendingEvents.ContainsKey(packet.MapId)
                ? Globals.PendingEvents[packet.MapId]
                : new Dictionary<Guid, EventEntityPacket>();

            if (dict.ContainsKey(packet.EntityId))
            {
                dict[packet.EntityId] = packet;
            }
            else
            {
                dict.Add(packet.EntityId, packet);
            }

            if (!Globals.PendingEvents.ContainsKey(packet.MapId))
            {
                Globals.PendingEvents.Add(packet.MapId, dict);
            }
        }
    }

    //MapEntitiesPacket
    public void HandlePacket(IPacketSender packetSender, MapEntitiesPacket packet)
    {
        var mapEntities = new Dictionary<Guid, List<Guid>>();
        foreach (var pkt in packet.MapEntities)
        {
            HandlePacket(pkt);

            if (!mapEntities.ContainsKey(pkt.MapId))
            {
                mapEntities.Add(pkt.MapId, new List<Guid>());
            }

            mapEntities[pkt.MapId].Add(pkt.EntityId);
        }

        //Remove any entities on the map that shouldn't be there anymore!
        foreach (var entities in mapEntities)
        {
            foreach (var entity in Globals.Entities)
            {
                if (entity.Value.MapId == entities.Key && !entities.Value.Contains(entity.Key))
                {
                    if (!Globals.EntitiesToDispose.Contains(entity.Key) && entity.Value != Globals.Me && !(entity.Value is Projectile))
                    {
                        Globals.EntitiesToDispose.Add(entity.Key);
                    }
                }
            }
        }
    }

    // MapInstanceChanged Packet
    public void HandlePacket(IPacketSender packetSender, MapInstanceChangedPacket packet)
    {
        var disposingEntities = new List<Guid>();
        foreach (var pkt in packet.EntitiesToDispose)
        {
            HandlePacket(pkt);
            disposingEntities.Add(pkt.EntityId);
        }

        foreach (var entity in disposingEntities)
        {
            Globals.EntitiesToDispose.Add(entity);
        }

        foreach (var mapId in packet.MapIdsToRefresh)
        {
            var map = MapInstance.Get(mapId);
            if (map != null)
            {
                Globals.EntitiesToDispose.AddRange(map.LocalEntities.Values
                    .ToList()
                    .Select(en => en.Id));
            }
        }
    }

    //EntityPositionPacket
    public void HandlePacket(IPacketSender packetSender, EntityPositionPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        Entity en;
        if (type != EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            if (MapInstance.Get(mapId) == null)
            {
                return;
            }

            if (!MapInstance.Get(mapId).LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = MapInstance.Get(mapId).LocalEntities[id];
        }

        if (en == Globals.Me)
        {
            Log.Debug($"received epp: {Timing.Global.Milliseconds}");
        }

        if (en == Globals.Me &&
            (Globals.Me.DashQueue.Count > 0 || Globals.Me.DashTimer > Timing.Global.Milliseconds))
        {
            return;
        }

        if (en == Globals.Me && Globals.Me.MapId != mapId)
        {
            Globals.Me.MapId = mapId;
            Globals.NeedsMaps = true;
            Player.FetchNewMaps();
        }
        else
        {
            en.MapId = mapId;
        }

        en.X = packet.X;
        en.Y = packet.Y;
        en.Dir = (Direction)packet.Direction;
        en.Passable = packet.Passable;
        en.HideName = packet.HideName;
    }

    //EntityLeftPacket
    public void HandlePacket(IPacketSender packetSender, EntityLeftPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        if (id == Globals.Me?.Id && type < EntityType.Event)
        {
            return;
        }

        if (type != EntityType.Event)
        {
            if (Globals.Entities?.ContainsKey(id) ?? false)
            {
                Globals.EntitiesToDispose?.Add(id);
            }
        }
        else
        {
            var map = MapInstance.Get(mapId);
            if (map?.LocalEntities?.ContainsKey(id) ?? false)
            {
                map.LocalEntities[id]?.Dispose();
                map.LocalEntities[id] = null;
                map.LocalEntities.Remove(id);
            }
        }
    }

    //ChatMsgPacket
    public void HandlePacket(IPacketSender packetSender, Intersect.Network.Packets.Server.ChatMsgPacket packet)
    {
        ChatboxMsg.AddMessage(
            new ChatboxMsg(
                packet.Message ?? "", new Color(packet.Color.A, packet.Color.R, packet.Color.G, packet.Color.B), packet.Type,
                packet.Target
            )
        );
    }

    //AnnouncementPacket
    public void HandlePacket(IPacketSender packetSender, AnnouncementPacket packet)
    {
        Interface.Interface.GameUi.AnnouncementWindow.ShowAnnouncement(packet.Message, packet.Duration);
    }

    //ActionMsgPackets
    public void HandlePacket(IPacketSender packetSender, ActionMsgPackets packet)
    {
        foreach (var pkt in packet.Packets)
        {
            HandlePacket(pkt);
        }
    }

    //ActionMsgPacket
    public void HandlePacket(IPacketSender packetSender, ActionMsgPacket packet)
    {
        var map = MapInstance.Get(packet.MapId);
        if (map != null)
        {
            map.ActionMessages.Add(
                new ActionMessage(
                    map, packet.X, packet.Y, packet.Message,
                    new Color(packet.Color.A, packet.Color.R, packet.Color.G, packet.Color.B)
                )
            );
        }
    }

    //GameDataPacket
    public void HandlePacket(IPacketSender packetSender, GameDataPacket packet)
    {
        foreach (var pkt in packet.GameObjects)
        {
            HandlePacket(pkt);
        }

        CustomColors.Load(packet.ColorsJson);
        Globals.HasGameData = true;
    }

    //MapListPacket
    public void HandlePacket(IPacketSender packetSender, MapListPacket packet)
    {
        MapList.List.JsonData = packet.MapListData;
        MapList.List.PostLoad(MapBase.Lookup, false, true);

        //TODO ? If admin window is open update it
    }

    //EntityMovementPackets
    public void HandlePacket(IPacketSender packetSender, EntityMovementPackets packet)
    {
        if (packet.GlobalMovements != null)
        {
            foreach (var pkt in packet.GlobalMovements)
            {
                HandlePacket(pkt);
            }
        }

        if (packet.LocalMovements != null)
        {
            foreach (var pkt in packet.LocalMovements)
            {
                HandlePacket(pkt);
            }
        }
    }

    //EntityMovePacket
    public void HandlePacket(IPacketSender packetSender, EntityMovePacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        Entity en;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var gameMap = MapInstance.Get(mapId);
            if (gameMap == null)
            {
                return;
            }

            if (!gameMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = gameMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        var entityMap = MapInstance.Get(en.MapId);
        if (entityMap == null)
        {
            return;
        }

        if (en is Player && Options.Combat.MovementCancelsCast)
        {
            en.CastTime = 0;
        }

        if (en.Dashing != null || en.DashQueue.Count > 0)
        {
            return;
        }

        var map = mapId;
        var x = packet.X;
        var y = packet.Y;
        Direction dir = (Direction)packet.Direction;
        var correction = packet.Correction;
        if ((en.MapId != map || en.X != x || en.Y != y) &&
            (en != Globals.Me || en == Globals.Me && correction) &&
            en.Dashing == null)
        {
            en.MapId = map;
            en.X = x;
            en.Y = y;
            en.Dir = dir;
            if (en is Player p)
            {
                p.MoveDir = dir;
            }
            en.IsMoving = true;

            switch (en.Dir)
            {
                case Direction.Up:
                    en.OffsetY = Options.TileWidth;
                    en.OffsetX = 0;

                    break;
                case Direction.Down:
                    en.OffsetY = -Options.TileWidth;
                    en.OffsetX = 0;

                    break;
                case Direction.Left:
                    en.OffsetY = 0;
                    en.OffsetX = Options.TileWidth;

                    break;
                case Direction.Right:
                    en.OffsetY = 0;
                    en.OffsetX = -Options.TileWidth;

                    break;
                case Direction.UpLeft:
                    en.OffsetY = Options.TileHeight;
                    en.OffsetX = Options.TileWidth;

                    break;
                case Direction.UpRight:
                    en.OffsetY = Options.TileHeight;
                    en.OffsetX = -Options.TileWidth;

                    break;
                case Direction.DownLeft:
                    en.OffsetY = -Options.TileHeight;
                    en.OffsetX = Options.TileWidth;

                    break;
                case Direction.DownRight:
                    en.OffsetY = -Options.TileHeight;
                    en.OffsetX = -Options.TileWidth;

                    break;
            }
        }

        // Set the Z-Dimension if the player has moved up or down a dimension.
        if (entityMap.Attributes[en.X, en.Y] != null &&
            entityMap.Attributes[en.X, en.Y].Type == MapAttributeType.ZDimension)
        {
            if (((MapZDimensionAttribute) entityMap.Attributes[en.X, en.Y]).GatewayTo > 0)
            {
                en.Z = (byte) (((MapZDimensionAttribute) entityMap.Attributes[en.X, en.Y]).GatewayTo - 1);
            }
        }
    }

    public void HandlePacket(IPacketSender packetSender, MapEntityVitalsPacket packet)
    {
        // Get our map, cancel out if it doesn't exist.
        var map = MapInstance.Get(packet.MapId);
        if (map == null)
        {
            return;
        }

        foreach (var en in packet.EntityUpdates)
        {
            Entity entity = null;

            if (en.Type < EntityType.Event)
            {
                if (!Globals.Entities.ContainsKey(en.Id))
                {
                    return;
                }

                entity = Globals.Entities[en.Id];
            }
            else
            {
                if (!map.LocalEntities.ContainsKey(en.Id))
                {
                    return;
                }

                entity = map.LocalEntities[en.Id];
            }

            if (entity == null)
            {
                return;
            }

            entity.Vital = en.Vitals;
            entity.MaxVital = en.MaxVitals;

            if (entity == Globals.Me)
            {
                if (en.CombatTimeRemaining > 0)
                {
                    Globals.Me.CombatTimer = Timing.Global.Milliseconds + en.CombatTimeRemaining;
                }
            }
        }
    }

    public void HandlePacket(IPacketSender packetSender, MapEntityStatusPacket packet)
    {
        // Get our map, cancel out if it doesn't exist.
        var map = MapInstance.Get(packet.MapId);
        if (map == null)
        {
            return;
        }

        foreach (var en in packet.EntityUpdates)
        {
            Entity entity = null;

            if (en.Type < EntityType.Event)
            {
                if (!Globals.Entities.ContainsKey(en.Id))
                {
                    return;
                }

                entity = Globals.Entities[en.Id];
            }
            else
            {
                if (!map.LocalEntities.ContainsKey(en.Id))
                {
                    return;
                }

                entity = map.LocalEntities[en.Id];
            }

            if (entity == null)
            {
                return;
            }

            //Update status effects
            entity.Status.Clear();
            foreach (var status in en.Statuses)
            {
                var instance = new Status(
                    status.SpellId, status.Type, status.TransformSprite, status.TimeRemaining, status.TotalDuration
                );

                entity.Status.Add(instance);

                if (instance.Type == SpellEffect.Stun || instance.Type == SpellEffect.Silence)
                {
                    entity.CastTime = 0;
                }
                else if (instance.Type == SpellEffect.Shield)
                {
                    instance.Shield = status.VitalShields;
                }
            }

            entity.SortStatuses();

            if (Interface.Interface.GameUi != null)
            {
                //If its you or your target, update the entity box.
                if (en.Id == Globals.Me.Id && Interface.Interface.GameUi.PlayerStatusWindow != null)
                {
                    Interface.Interface.GameUi.PlayerStatusWindow.ShouldUpdateStatuses = true;
                }
                else if (en.Id == Globals.Me.TargetIndex && Globals.Me.TargetBox != null)
                {
                    Globals.Me.TargetBox.ShouldUpdateStatuses = true;
                }
            }
        }
    }

    //EntityVitalsPacket
    public void HandlePacket(IPacketSender packetSender, EntityVitalsPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        Entity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        en.Vital = packet.Vitals;
        en.MaxVital = packet.MaxVitals;

        if (en == Globals.Me)
        {
            if (packet.CombatTimeRemaining > 0)
            {
                Globals.Me.CombatTimer = Timing.Global.Milliseconds + packet.CombatTimeRemaining;
            }
        }

        //Update status effects
        en.Status.Clear();
        foreach (var status in packet.StatusEffects)
        {
            var instance = new Status(
                status.SpellId, status.Type, status.TransformSprite, status.TimeRemaining, status.TotalDuration
            );

            en.Status.Add(instance);

            if (instance.Type == SpellEffect.Stun || instance.Type == SpellEffect.Silence)
            {
                en.CastTime = 0;
            }
            else if (instance.Type == SpellEffect.Shield)
            {
                instance.Shield = status.VitalShields;
            }
        }

        en.SortStatuses();

        if (Interface.Interface.GameUi != null)
        {
            //If its you or your target, update the entity box.
            if (id == Globals.Me.Id && Interface.Interface.GameUi.PlayerStatusWindow != null)
            {
                Interface.Interface.GameUi.PlayerStatusWindow.ShouldUpdateStatuses = true;
            }
            else if (id == Globals.Me.TargetIndex && Globals.Me.TargetBox != null)
            {
                Globals.Me.TargetBox.ShouldUpdateStatuses = true;
            }
        }
    }

    //EntityStatsPacket
    public void HandlePacket(IPacketSender packetSender, EntityStatsPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        Entity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        en.Stat = packet.Stats;
    }

    //EntityDirectionPacket
    public void HandlePacket(IPacketSender packetSender, EntityDirectionPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        Entity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        en.Dir = (Direction)packet.Direction;
    }

    //EntityAttackPacket
    public void HandlePacket(IPacketSender packetSender, EntityAttackPacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;
        var attackTimer = packet.AttackTimer;

        Entity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        var isSelf = en == Globals.Me;
        en.IsBlocking = packet.IsBlocking;

        if (attackTimer > -1)
        {
            en.AttackTimer = Timing.Global.Milliseconds + attackTimer;
            if (!isSelf)
            {
                en.AttackTime = attackTimer;
            }
        }
    }

    //EntityDiePacket
    public void HandlePacket(IPacketSender packetSender, EntityDiePacket packet)
    {
        var id = packet.Id;
        var type = packet.Type;
        var mapId = packet.MapId;

        Entity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
            {
                return;
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        // if (type == EntityType.GlobalEntity)
        // {
        //     // Create corpse
        //     var corpseId = Guid.NewGuid();
        //     Globals.Entities.Add(corpseId, new Corpse(corpseId, en));
        // }

        if (en.Id == Globals.Me.Id)
        {
            Interface.Interface.GameUi?.GameMenu?.ShowDeathWindow();
        }

        en.ClearAnimations(null);
    }

    //EventDialogPacket
    public void HandlePacket(IPacketSender packetSender, EventDialogPacket packet)
    {
        var ed = new Dialog();
        ed.Prompt = packet.Prompt;
        ed.Face = packet.Face;
        if (packet.Type != 0)
        {
            ed.Opt1 = packet.Responses[0];
            ed.Opt2 = packet.Responses[1];
            ed.Opt3 = packet.Responses[2];
            ed.Opt4 = packet.Responses[3];
        }

        ed.EventId = packet.EventId;
        Globals.EventDialogs.Add(ed);
    }

    //InputVariablePacket
    public void HandlePacket(IPacketSender packetSender, InputVariablePacket packet)
    {
        var type = InputBox.InputType.NumericInput;
        switch (packet.Type)
        {
            case VariableDataType.String:
                type = InputBox.InputType.TextInput;

                break;
            case VariableDataType.Integer:
            case VariableDataType.Number:
                type = InputBox.InputType.NumericInput;

                break;
            case VariableDataType.Boolean:
                type = InputBox.InputType.YesNo;

                break;
        }

        _ = new InputBox(
            title: packet.Title,
            prompt: packet.Prompt,
            inputType: type,
            userData: packet.EventId,
            onSuccess: PacketSender.SendEventInputVariable,
            onCancel: PacketSender.SendEventInputVariableCancel
        );
    }

    //ErrorMessagePacket
    public void HandlePacket(IPacketSender packetSender, ErrorMessagePacket packet)
    {
        Fade.FadeIn(ClientConfiguration.Instance.FadeDurationMs);
        Globals.WaitingOnServer = false;
        Interface.Interface.ShowError(packet.Error, packet.Header);
        Interface.Interface.MenuUi?.Reset();
    }

    //MapItemsPacket
    public void HandlePacket(IPacketSender packetSender, MapItemsPacket packet)
    {
        var map = MapInstance.Get(packet.MapId);
        if (map == null)
        {
            return;
        }

        map.MapItems.Clear();
        foreach(var item in packet.Items)
        {
            var mapItem = new MapItemInstance(item.TileIndex,item.Id, item.ItemId, item.BagId, item.Quantity, item.Properties);

            if (!map.MapItems.ContainsKey(mapItem.TileIndex))
            {
                map.MapItems.Add(mapItem.TileIndex, new List<IMapItemInstance>());
            }

            map.MapItems[mapItem.TileIndex].Add(mapItem);
        }
    }

    //MapItemUpdatePacket
    public void HandlePacket(IPacketSender packetSender, MapItemUpdatePacket packet)
    {
        var map = MapInstance.Get(packet.MapId);
        if (map == null)
        {
            return;
        }

        // Are we deleting this item?
        if (packet.ItemId == Guid.Empty)
        {
            // Find our item based on our unique Id and remove it.
            foreach(var location in map.MapItems.Keys)
            {
                var tempItem = map.MapItems[location].Where(item => item.Id == packet.Id).SingleOrDefault();
                if (tempItem != null)
                {
                    map.MapItems[location].Remove(tempItem);
                }
            }
        }
        else
        {
            if (!map.MapItems.ContainsKey(packet.TileIndex))
            {
                map.MapItems.Add(packet.TileIndex, new List<IMapItemInstance>());
            }

            // Check if the item already exists, if it does replace it. Otherwise just add it.
            var mapItem = new MapItemInstance(packet.TileIndex, packet.Id, packet.ItemId, packet.BagId, packet.Quantity, packet.Properties);
            if (map.MapItems[packet.TileIndex].Any(item => item.Id == mapItem.Id))
            {
                for (var index = 0; index < map.MapItems[packet.TileIndex].Count; index++)
                {
                    if (map.MapItems[packet.TileIndex][index].Id == mapItem.Id)
                    {
                        map.MapItems[packet.TileIndex][index] = mapItem;
                    }
                }
            }
            else
            {
                // Reverse the array again to match server, add item.. then  reverse again to get the right render order.
                map.MapItems[packet.TileIndex].Add(mapItem);
            }
        }
    }

    //InventoryPacket
    public void HandlePacket(IPacketSender packetSender, InventoryPacket packet)
    {
        foreach (var inv in packet.Slots)
        {
            HandlePacket(inv);
        }
    }

    //InventoryUpdatePacket
    public void HandlePacket(IPacketSender packetSender, InventoryUpdatePacket packet)
    {
        if (Globals.Me != null)
        {
            Globals.Me.Inventory[packet.Slot].Load(packet.ItemId, packet.Quantity, packet.BagId, packet.Properties);
            Globals.Me.InventoryUpdatedDelegate?.Invoke();
        }
    }

    //SpellsPacket
    public void HandlePacket(IPacketSender packetSender, SpellsPacket packet)
    {
        foreach (var spl in packet.Slots)
        {
            HandlePacket(spl);
        }
    }

    //SpellUpdatePacket
    public void HandlePacket(IPacketSender packetSender, SpellUpdatePacket packet)
    {
        if (Globals.Me != null)
        {
            Globals.Me.Spells[packet.Slot].Load(packet.SpellId);
        }
    }

    //EquipmentPacket
    public void HandlePacket(IPacketSender packetSender, EquipmentPacket packet)
    {
        var entityId = packet.EntityId;
        if (Globals.Entities.ContainsKey(entityId))
        {
            var entity = Globals.Entities[entityId];
            if (entity != null)
            {
                if (entity == Globals.Me && packet.InventorySlots != null)
                {
                    entity.MyEquipment = packet.InventorySlots;
                }
                else if (packet.ItemIds != null)
                {
                    entity.Equipment = packet.ItemIds;
                }
            }
        }
    }

    //StatPointsPacket
    public void HandlePacket(IPacketSender packetSender, StatPointsPacket packet)
    {
        if (Globals.Me != null)
        {
            Globals.Me.StatPoints = packet.Points;
        }
    }

    //HotbarPacket
    public void HandlePacket(IPacketSender packetSender, HotbarPacket packet)
    {
        for (var i = 0; i < Options.Instance.PlayerOpts.HotbarSlotCount; i++)
        {
            if (Globals.Me == null)
            {
                Log.Debug("Can't set hotbar, Globals.Me is null!");

                break;
            }

            if (Globals.Me.Hotbar == null)
            {
                Log.Debug("Can't set hotbar, hotbar is null!");

                break;
            }

            var hotbarEntry = Globals.Me.Hotbar[i];
            hotbarEntry.Load(packet.SlotData[i]);
        }
    }

    //CharacterCreationPacket
    public void HandlePacket(IPacketSender packetSender, CharacterCreationPacket packet)
    {
        Globals.WaitingOnServer = false;
        Interface.Interface.MenuUi.MainMenu.NotifyOpenCharacterCreation();
    }

    //AdminPanelPacket
    public void HandlePacket(IPacketSender packetSender, AdminPanelPacket packet)
    {
        Interface.Interface.GameUi.NotifyOpenAdminWindow();
    }

    //SpellCastPacket
    public void HandlePacket(IPacketSender packetSender, SpellCastPacket packet)
    {
        var entityId = packet.EntityId;
        var spellId = packet.SpellId;
        if (SpellBase.Get(spellId) != null && Globals.Entities.ContainsKey(entityId))
        {
            Globals.Entities[entityId].CastTime = Timing.Global.Milliseconds + SpellBase.Get(spellId).CastDuration;
            Globals.Entities[entityId].SpellCast = spellId;
        }
    }

    //SpellCooldownPacket
    public void HandlePacket(IPacketSender packetSender, SpellCooldownPacket packet)
    {
        foreach (var cd in packet.SpellCds)
        {
            var time = Timing.Global.Milliseconds + cd.Value;
            if (!Globals.Me.SpellCooldowns.ContainsKey(cd.Key))
            {
                Globals.Me.SpellCooldowns.Add(cd.Key, time);
            }
            else
            {
                Globals.Me.SpellCooldowns[cd.Key] = time;
            }
        }
    }

    //ItemCooldownPacket
    public void HandlePacket(IPacketSender packetSender, ItemCooldownPacket packet)
    {
        foreach (var cd in packet.ItemCds)
        {
            var time = Timing.Global.Milliseconds + cd.Value;
            if (!Globals.Me.ItemCooldowns.ContainsKey(cd.Key))
            {
                Globals.Me.ItemCooldowns.Add(cd.Key, time);
            }
            else
            {
                Globals.Me.ItemCooldowns[cd.Key] = time;
            }
        }
    }

    //GlobalCooldownPacket
    public void HandlePacket(IPacketSender packetSender, GlobalCooldownPacket packet)
    {
        Globals.Me.GlobalCooldown = Timing.Global.Milliseconds + packet.GlobalCooldown;
    }

    //ExperiencePacket
    public void HandlePacket(IPacketSender packetSender, ExperiencePacket packet)
    {
        if (Globals.Me != null)
        {
            Globals.Me.Experience = packet.Experience;
            Globals.Me.ExperienceToNextLevel = packet.ExperienceToNextLevel;
        }
    }

    //ProjectileDeadPacket
    public void HandlePacket(IPacketSender packetSender, ProjectileDeadPacket packet)
    {
        if (packet.ProjectileDeaths != null)
        {
            foreach (var projDeath in packet.ProjectileDeaths)
            {
                if (Globals.Entities.ContainsKey(projDeath) && Globals.Entities[projDeath] is Projectile)
                {
                    Globals.EntitiesToDispose?.Add(projDeath);
                }
            }
        }
        if (packet.SpawnDeaths != null)
        {
            foreach (var spawnDeath in packet.SpawnDeaths)
            {
                if (Globals.Entities.ContainsKey(spawnDeath.Key) && Globals.Entities[spawnDeath.Key] is Projectile projectile)
                {
                    projectile.SpawnDead(spawnDeath.Value);
                }
            }
        }
    }

    //PlayAnimationPackets
    public void HandlePacket(IPacketSender sender, PlayAnimationPackets packet)
    {
        foreach (var pkt in packet.Packets)
        {
            HandlePacket(pkt);
        }
    }

    //PlayAnimationPacket
    public void HandlePacket(IPacketSender packetSender, PlayAnimationPacket packet)
    {
        var mapId = packet.MapId;
        var animId = packet.AnimationId;
        var targetType = packet.TargetType;
        var entityId = packet.EntityId;
        if (targetType == -1)
        {
            var map = MapInstance.Get(mapId);
            if (map != null)
            {
                map.AddTileAnimation(animId, packet.X, packet.Y, packet.Direction);
            }
        }
        else if (targetType == 1)
        {
            if (Globals.Entities.ContainsKey(entityId))
            {
                if (Globals.Entities[entityId] != null && !Globals.EntitiesToDispose.Contains(entityId))
                {
                    var animBase = AnimationBase.Get(animId);
                    if (animBase != null)
                    {
                        var animInstance = new Animation(
                            animBase, false, packet.Direction != Direction.None, -1, Globals.Entities[entityId]
                        );

                        if (packet.Direction > Direction.None)
                        {
                            animInstance.SetDir(packet.Direction);
                        }

                        Globals.Entities[entityId].Animations.Add(animInstance);
                    }
                }
            }
        }
        else if (targetType == 2)
        {
            var map = MapInstance.Get(mapId);
            if (map != null)
            {
                if (map.LocalEntities.ContainsKey(entityId))
                {
                    if (map.LocalEntities[entityId] != null)
                    {
                        var animBase = AnimationBase.Get(animId);
                        if (animBase != null)
                        {
                            var animInstance = new Animation(
                                animBase, false, packet.Direction == Direction.None, -1,
                                map.LocalEntities[entityId]
                            );

                            if (packet.Direction > Direction.None)
                            {
                                animInstance.SetDir(packet.Direction);
                            }

                            map.LocalEntities[entityId].Animations.Add(animInstance);
                        }
                    }
                }
            }
        }
    }

    //HoldPlayerPacket
    public void HandlePacket(IPacketSender packetSender, HoldPlayerPacket packet)
    {
        var eventId = packet.EventId;
        var mapId = packet.MapId;
        if (!packet.Releasing)
        {
            if (!Globals.EventHolds.ContainsKey(eventId))
            {
                Globals.EventHolds.Add(eventId, mapId);
            }
        }
        else
        {
            if (Globals.EventHolds.ContainsKey(eventId))
            {
                Globals.EventHolds.Remove(eventId);
            }
        }
    }

    //PlayMusicPacket
    public void HandlePacket(IPacketSender packetSender, PlayMusicPacket packet)
    {
        Audio.PlayMusic(packet.BGM, ClientConfiguration.Instance.MusicFadeTimer, ClientConfiguration.Instance.MusicFadeTimer, true);
    }

    //StopMusicPacket
    public void HandlePacket(IPacketSender packetSender, StopMusicPacket packet)
    {
        Audio.StopMusic(ClientConfiguration.Instance.MusicFadeTimer);
    }

    //PlaySoundPacket
    public void HandlePacket(IPacketSender packetSender, PlaySoundPacket packet)
    {
        Audio.AddGameSound(packet.Sound, false);
    }

    //StopSoundsPacket
    public void HandlePacket(IPacketSender packetSender, StopSoundsPacket packet)
    {
        Audio.StopAllSounds();
    }

    //ShowPicturePacket
    public void HandlePacket(IPacketSender packetSender, ShowPicturePacket packet)
    {
        PacketSender.SendClosePicture(Globals.Picture?.EventId ?? Guid.Empty);
        packet.ReceiveTime = Timing.Global.Milliseconds;
        Globals.Picture = packet;
    }

    //HidePicturePacket
    public void HandlePacket(IPacketSender packetSender, HidePicturePacket packet)
    {
        PacketSender.SendClosePicture(Globals.Picture?.EventId ?? Guid.Empty);
        Globals.Picture = null;
    }

    //ShopPacket
    public void HandlePacket(IPacketSender packetSender, ShopPacket packet)
    {
        if (Interface.Interface.GameUi == null)
        {
            throw new ArgumentNullException(nameof(Interface.Interface.GameUi));
        }

        if (packet == null)
        {
            throw new ArgumentNullException(nameof(packet));
        }

        if (packet.ShopData != null)
        {
            Globals.GameShop = new ShopBase();
            Globals.GameShop.Load(packet.ShopData);
            Interface.Interface.GameUi.NotifyOpenShop();
        }
        else
        {
            Globals.GameShop = null;
            Interface.Interface.GameUi.NotifyCloseShop();
        }
    }

    //CraftingTablePacket
    public void HandlePacket(IPacketSender packetSender, CraftingTablePacket packet)
    {
        if (!packet.Close)
        {
            Globals.ActiveCraftingTable = new CraftingTableBase();
            Globals.ActiveCraftingTable.Load(packet.TableData);
            Interface.Interface.GameUi.NotifyOpenCraftingTable(packet.JournalMode);
        }
        else
        {
            Interface.Interface.GameUi.NotifyCloseCraftingTable();
        }
    }

    //BankPacket
    public void HandlePacket(IPacketSender packetSender, BankPacket packet)
    {
        if (!packet.Close)
        {
            Globals.GuildBank = packet.Guild;
            Globals.Bank = new Item[packet.Slots];
            foreach (var itm in packet.Items)
            {
                HandlePacket(itm);
            }
            Globals.BankSlots = packet.Slots;
            Interface.Interface.GameUi.NotifyOpenBank();
        }
        else
        {
            Interface.Interface.GameUi.NotifyCloseBank();
        }
    }

    //BankUpdatePacket
    public void HandlePacket(IPacketSender packetSender, BankUpdatePacket packet)
    {
        var slot = packet.Slot;
        if (packet.ItemId != Guid.Empty)
        {
            Globals.Bank[slot] = new Item();
            Globals.Bank[slot].Load(packet.ItemId, packet.Quantity, packet.BagId, packet.Properties);
        }
        else
        {
            Globals.Bank[slot] = null;
        }
    }

    // Screen Shake packet
    public void HandlePacket(IPacketSender packetSender, ShakeScreenPacket packet)
    {
        if (Globals.Me == null) return;
        if (Graphics.CurrentShake < packet.Intensity)
        {
            Graphics.CurrentShake = packet.Intensity;
        }
    }
    // The client will flash the screen with the given params
    public void HandlePacket(IPacketSender packetSender, FlashScreenPacket packet)
    {
        if (Globals.Me == null) return;
        Flash.FlashScreen(packet.Duration, packet.FlashColor, packet.Intensity);

        var flashSound = packet.SoundFile;
        if (flashSound != null)
        {
            Audio.AddGameSound(flashSound, false);
        }
    }

    // Combat Effect packet
    public void HandlePacket(IPacketSender packetSender, CombatEffectPacket packet)
    {
        if (Globals.Me == null) return;
        var someTarget = packet.TargetId != null && packet.TargetId != Guid.Empty && Globals.Entities.ContainsKey(packet.TargetId);
        if (Graphics.CurrentShake < packet.ShakeAmount && packet.ShakeAmount > 0.0f)
        {
            Graphics.CurrentShake = packet.ShakeAmount;
        }
        if (packet.FlashColor != null)
        {
            Flash.FlashScreen(packet.FlashDuration, packet.FlashColor, packet.FlashIntensity);
        }
        // Flash entity
        if (someTarget)
        {
            Entity affectedTarget = Globals.Entities[packet.TargetId];
            affectedTarget.Flash = true;
            affectedTarget.FlashColor = packet.EntityFlashColor;
            affectedTarget.FlashEndTime = Timing.Global.Milliseconds + 200; // TODO config
            if (!string.IsNullOrEmpty(packet.Sound))
            {
                Audio.AddMapSound(packet.Sound, affectedTarget.X, affectedTarget.Y, affectedTarget.MapId, false, 0, 10);
            }
        }
        else // If we don't have a specific target handle this as if the hit-effects are happening to the packet-receiving player
        {
            if (!string.IsNullOrEmpty(packet.Sound))
            {
                Audio.AddGameSound(packet.Sound, false);
            }
            Globals.Me.Flash = true;
            Globals.Me.FlashColor = packet.EntityFlashColor;
            Globals.Me.FlashEndTime = Timing.Global.Milliseconds + 200; // TODO config
        }
    }

    //GameObjectPacket
    public void HandlePacket(IPacketSender packetSender, GameObjectPacket packet)
    {
        var type = packet.Type;
        var id = packet.Id;
        var another = packet.AnotherFollowing;
        var deleted = packet.Deleted;
        var json = "";
        if (!deleted)
        {
            json = packet.Data;
        }

        switch (type)
        {
            case GameObjectType.Map:
                //Handled in a different packet
                break;
            case GameObjectType.Tileset:
                var obj = new TilesetBase(id);
                obj.Load(json);
                TilesetBase.Lookup.Set(id, obj);
                if (Globals.HasGameData && !another)
                {
                    Globals.ContentManager.LoadTilesets(TilesetBase.GetNameList());
                }

                break;
            case GameObjectType.Event:
                //Clients don't store event data, im an idiot.
                break;
            default:
                var lookup = type.GetLookup();
                if (deleted)
                {
                    lookup.Get(id)?.Delete();
                }
                else
                {
                    lookup.DeleteAt(id);
                    var item = lookup.AddNew(type.GetObjectType(), id);
                    item.Load(json);
                }

                break;
        }
    }

    //EntityDashPacket
    public void HandlePacket(IPacketSender packetSender, EntityDashPacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.EntityId))
        {
            Globals.Entities[packet.EntityId]
                .DashQueue.Enqueue(
                    new Dash(
                        packet.EndMapId, packet.EndX, packet.EndY,
                        packet.DashTime, packet.Direction
                    )
                );
        }
    }

    //MapGridPacket
    public void HandlePacket(IPacketSender packetSender, MapGridPacket packet)
    {
        Globals.MapGridWidth = packet.Grid.GetLength(0);
        Globals.MapGridHeight = packet.Grid.GetLength(1);
        var clearKnownMaps = packet.ClearKnownMaps;
        Globals.MapGrid = new Guid[Globals.MapGridWidth, Globals.MapGridHeight];
        if (clearKnownMaps)
        {
            foreach (var map in MapInstance.Lookup.Values.ToArray())
            {
                ((MapInstance) map).Dispose();
            }
        }

        Globals.NeedsMaps = true;
        Globals.GridMaps.Clear();
        for (var x = 0; x < Globals.MapGridWidth; x++)
        {
            for (var y = 0; y < Globals.MapGridHeight; y++)
            {
                Globals.MapGrid[x, y] = packet.Grid[x, y];
                if (Globals.MapGrid[x, y] != Guid.Empty)
                {
                    Globals.GridMaps.Add(Globals.MapGrid[x, y]);
                    // MapInstance.UpdateMapRequestTime(Globals.MapGrid[x, y]);
                }
            }
        }

        if (Globals.Me != null)
        {
            Player.FetchNewMaps();
        }

        Graphics.GridSwitched = true;
    }

    //TimePacket
    public void HandlePacket(IPacketSender packetSender, TimePacket packet)
    {
        Time.LoadTime(
            packet.Time, Color.FromArgb(packet.Color.A, packet.Color.R, packet.Color.G, packet.Color.B), packet.Rate
        );
    }

    //PartyPacket
    public void HandlePacket(IPacketSender packetSender, PartyPacket packet)
    {
        if (Globals.Me == null || Globals.Me.Party == null)
        {
            return;
        }

        Globals.Me.Party.Clear();
        for (var i = 0; i < packet.MemberData.Length; i++)
        {
            var mem = packet.MemberData[i];
            Globals.Me.Party.Add(new PartyMember(mem.Id, mem.Name, mem.Vital, mem.MaxVital, mem.Level));
        }
    }

    //PartyUpdatePacket
    public void HandlePacket(IPacketSender packetSender, PartyUpdatePacket packet)
    {
        var index = packet.MemberIndex;
        if (index < Globals.Me.Party.Count)
        {
            var mem = packet.MemberData;
            Globals.Me.Party[index] = new PartyMember(mem.Id, mem.Name, mem.Vital, mem.MaxVital, mem.Level);
        }
    }

    //PartyInvitePacket
    public void HandlePacket(IPacketSender packetSender, Intersect.Network.Packets.Server.PartyInvitePacket packet)
    {
        _ = new InputBox(
            title: Strings.Parties.PartyInvite,
            prompt: Strings.Parties.InvitePrompt.ToString(packet.LeaderName),
            inputType: InputBox.InputType.YesNo,
            userData: packet.LeaderId,
            onSuccess: PacketSender.SendPartyAccept,
            onCancel: PacketSender.SendPartyDecline
        );
    }

    //ChatBubblePacket
    public void HandlePacket(IPacketSender packetSender, ChatBubblePacket packet)
    {
        var id = packet.EntityId;
        var type = packet.Type;
        var mapId = packet.MapId;
        IEntity en = null;
        if (type < EntityType.Event)
        {
            if (!Globals.Entities.ContainsKey(id))
            {
                return;
            }

            en = Globals.Entities[id];
        }
        else
        {
            var entityMap = MapInstance.Get(mapId);
            if (entityMap == null)
            {
                return;
            }

            if (!entityMap.LocalEntities.ContainsKey(id))
                return;
            {
            }

            en = entityMap.LocalEntities[id];
        }

        if (en == null)
        {
            return;
        }

        en.AddChatBubble(packet.Text);
    }

    //QuestOfferPacket
    public void HandlePacket(IPacketSender packetSender, QuestOfferPacket packet)
    {
        if (!Globals.QuestOffers.Contains(packet.QuestId))
        {
            Globals.QuestOffers.Add(packet.QuestId);
        }
    }

    //QuestProgressPacket
    public void HandlePacket(IPacketSender packetSender, QuestProgressPacket packet)
    {
        if (Globals.Me != null)
        {
            foreach (var quest in packet.Quests)
            {
                if (quest.Value == null)
                {
                    if (Globals.Me.QuestProgress.ContainsKey(quest.Key))
                    {
                        Globals.Me.QuestProgress.Remove(quest.Key);
                    }
                }
                else
                {
                    if (Globals.Me.QuestProgress.ContainsKey(quest.Key))
                    {
                        Globals.Me.QuestProgress[quest.Key] = new QuestProgress(quest.Value);
                    }
                    else
                    {
                        Globals.Me.QuestProgress.Add(quest.Key, new QuestProgress(quest.Value));
                    }
                }
            }

            Globals.Me.HiddenQuests = packet.HiddenQuests;

            if (Interface.Interface.GameUi != null)
            {
                Interface.Interface.GameUi.NotifyQuestsUpdated();
            }
        }
    }

    //TradePacket
    public void HandlePacket(IPacketSender packetSender, TradePacket packet)
    {
        if (!string.IsNullOrEmpty(packet.TradePartner))
        {
            Globals.Trade = new Item[2, Options.MaxInvItems];

            //Gotta initialize the trade values
            for (var x = 0; x < 2; x++)
            {
                for (var y = 0; y < Options.MaxInvItems; y++)
                {
                    Globals.Trade[x, y] = new Item();
                }
            }

            Interface.Interface.GameUi.NotifyOpenTrading(packet.TradePartner);
        }
        else
        {
            Interface.Interface.GameUi.NotifyCloseTrading();
        }
    }

    //TradeUpdatePacket
    public void HandlePacket(IPacketSender packetSender, TradeUpdatePacket packet)
    {
        var side = 0;

        if (packet.TraderId != Globals.Me.Id)
        {
            side = 1;
        }

        var slot = packet.Slot;
        if (packet.ItemId == Guid.Empty)
        {
            Globals.Trade[side, slot] = null;
        }
        else
        {
            Globals.Trade[side, slot] = new Item();
            Globals.Trade[side, slot].Load(packet.ItemId, packet.Quantity, packet.BagId, packet.Properties);
        }
    }

    //TradeRequestPacket
    public void HandlePacket(IPacketSender packetSender, Intersect.Network.Packets.Server.TradeRequestPacket packet)
    {
        _ = new InputBox(
            title: Strings.Trading.TradeRequest,
            prompt: Strings.Trading.RequestPrompt.ToString(packet.PartnerName),
            inputType: InputBox.InputType.YesNo,
            userData: packet.PartnerId,
            onSuccess: PacketSender.SendTradeRequestAccept,
            onCancel: PacketSender.SendTradeRequestDecline
        );
    }

    //NpcAggressionPacket
    public void HandlePacket(IPacketSender packetSender, NpcAggressionPacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.EntityId))
        {
            Globals.Entities[packet.EntityId].Aggression = packet.Aggression;
        }
    }

    //PlayerDeathPacket
    public void HandlePacket(IPacketSender packetSender, PlayerDeathPacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.PlayerId))
        {
            //Clear all dashes.
            Globals.Entities[packet.PlayerId].DashQueue.Clear();
            Globals.Entities[packet.PlayerId].Dashing = null;
            Globals.Entities[packet.PlayerId].DashTimer = 0;
        }
    }

    //EntityZDimensionPacket
    public void HandlePacket(IPacketSender packetSender, EntityZDimensionPacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.EntityId))
        {
            Globals.Entities[packet.EntityId].Z = packet.Level;
        }
    }

    //BagPacket
    public void HandlePacket(IPacketSender packetSender, BagPacket packet)
    {
        if (!packet.Close)
        {
            Globals.Bag = new Item[packet.Slots];
            Interface.Interface.GameUi.NotifyOpenBag();
        }
        else
        {
            Interface.Interface.GameUi.NotifyCloseBag();
        }
    }

    //BagUpdatePacket
    public void HandlePacket(IPacketSender packetSender, BagUpdatePacket packet)
    {
        if (packet.ItemId == Guid.Empty)
        {
            Globals.Bag[packet.Slot] = null;
        }
        else
        {
            Globals.Bag[packet.Slot] = new Item();
            Globals.Bag[packet.Slot].Load(packet.ItemId, packet.Quantity, packet.BagId, packet.Properties);
        }
    }

    //MoveRoutePacket
    public void HandlePacket(IPacketSender packetSender, MoveRoutePacket packet)
    {
        Globals.MoveRouteActive = packet.Active;
    }

    //FriendsPacket
    public void HandlePacket(IPacketSender packetSender, FriendsPacket packet)
    {
        Globals.Me?.Friends.Clear();

        foreach (var friend in packet.OnlineFriends)
        {
            var f = new FriendInstance()
            {
                Name = friend.Key,
                Map = friend.Value,
                Online = true
            };

            Globals.Me?.Friends.Add(f);
        }

        foreach (var friend in packet.OfflineFriends)
        {
            var f = new FriendInstance()
            {
                Name = friend,
                Online = false
            };

            Globals.Me?.Friends.Add(f);
        }

        Interface.Interface.GameUi?.NotifyUpdateFriendsList();
    }

    //FriendRequestPacket
    public void HandlePacket(IPacketSender packetSender, FriendRequestPacket packet)
    {
        _ = new InputBox(
            title: Strings.Friends.Request,
            prompt: Strings.Friends.RequestPrompt.ToString(packet.FriendName),
            inputType: InputBox.InputType.YesNo,
            userData: packet.FriendId,
            onSuccess: PacketSender.SendFriendRequestAccept,
            onCancel: PacketSender.SendFriendRequestDecline
        );
    }

    //CharactersPacket
    public void HandlePacket(IPacketSender packetSender, CharactersPacket packet)
    {
        var characters = new List<Character>();

        foreach (var chr in packet.Characters)
        {
            characters.Add(
                new Character(chr.Id, chr.Name, chr.Sprite, chr.Face, chr.Level, chr.ClassName, chr.Equipment)
            );
        }

        if (packet.FreeSlot)
        {
            characters.Add(null);
        }

        Globals.WaitingOnServer = false;
        Interface.Interface.MenuUi.MainMenu.NotifyOpenCharacterSelection(characters);
    }

    //PasswordResetResultPacket
    public void HandlePacket(IPacketSender packetSender, PasswordResetResultPacket packet)
    {
        if (packet.Succeeded)
        {
            // Show Success Message and Open Login Screen
            Interface.Interface.ShowError(Strings.ResetPass.Success, Strings.ResetPass.SuccessMessage);
            Interface.Interface.MenuUi.MainMenu.NotifyOpenLogin();
        }
        else
        {
            Interface.Interface.ShowError(Strings.ResetPass.Error, Strings.ResetPass.ErrorMessage);
        }

        Globals.WaitingOnServer = false;
    }

    //TargetOverridePacket
    public void HandlePacket(IPacketSender packetSender, TargetOverridePacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.TargetId))
        {
            Globals.Me.TryTarget(Globals.Entities[packet.TargetId], true);
        }
    }

    //EnteringGamePacket
    public void HandlePacket(IPacketSender packetSender, EnteringGamePacket packet)
    {
        //Fade out, we're finally loading the game world!
        Fade.FadeOut(ClientConfiguration.Instance.FadeDurationMs);
    }

    //CancelCastPacket
    public void HandlePacket(IPacketSender packetSender, CancelCastPacket packet)
    {
        if (Globals.Entities.ContainsKey(packet.EntityId))
        {
            Globals.Entities[packet.EntityId].CastTime = 0;
            Globals.Entities[packet.EntityId].SpellCast = Guid.Empty;
        }
    }

    //GuildPacket
    public void HandlePacket(IPacketSender packetSender, GuildPacket packet)
    {
        if (Globals.Me == null || Globals.Me.Guild == null)
        {
            return;
        }

        var updatedGuildMembers = packet.Members.OrderByDescending(m => m.Online).ThenBy(m => m.Rank)
            .ThenBy(m => m.Name).ToArray();

        var currentGuildMembers = Globals.Me.GuildMembers;
        var hasUpdates = currentGuildMembers?.Length != updatedGuildMembers.Length;
        if (!hasUpdates)
        {
            for (var index = 0; index < currentGuildMembers.Length; ++index)
            {
                var currentGuildMember = currentGuildMembers[index];
                var updatedGuildMember = updatedGuildMembers[index];

                if (currentGuildMember.Id != updatedGuildMember.Id)
                {
                    hasUpdates = true;
                    break;
                }

                if (currentGuildMember.Online != updatedGuildMember.Online)
                {
                    hasUpdates = true;
                    break;
                }

                if (currentGuildMember.Rank != updatedGuildMember.Rank)
                {
                    hasUpdates = true;
                    break;
                }

                if (!string.Equals(currentGuildMember.Name, updatedGuildMember.Name))
                {
                    hasUpdates = true;
                    break;
                }
            }
        }

        if (hasUpdates)
        {
            Globals.Me.GuildMembers = updatedGuildMembers;
            Interface.Interface.GameUi.NotifyUpdateGuildList();
        }
    }


    //GuildInvitePacket
    public void HandlePacket(IPacketSender packetSender, GuildInvitePacket packet)
    {
        _ = new InputBox(
            title: Strings.Guilds.InviteRequestTitle,
            prompt: Strings.Guilds.InviteRequestPrompt.ToString(packet.Inviter, packet.GuildName),
            inputType: InputBox.InputType.YesNo,
            onSuccess: PacketSender.SendGuildInviteAccept,
            onCancel: PacketSender.SendGuildInviteDecline
        );
    }

    public void HandlePacket(IPacketSender packetSender, FadePacket packet)
    {
        switch (packet.FadeType)
        {
            case GameObjects.Events.FadeType.None:
                Fade.Cancel();
                break;
            case GameObjects.Events.FadeType.FadeIn:
                Fade.FadeIn(packet.DurationMs, packet.WaitForCompletion);
                break;
            case GameObjects.Events.FadeType.FadeOut:
                Fade.FadeOut(packet.DurationMs, packet.WaitForCompletion);
                break;
        }
    }

}
