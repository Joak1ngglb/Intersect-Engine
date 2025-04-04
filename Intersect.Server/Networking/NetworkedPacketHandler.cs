using Intersect.Core;
using Intersect.Enums;
using Intersect.Framework.Core;
using Intersect.Framework.Core.GameObjects.Animations;
using Intersect.Framework.Core.GameObjects.Crafting;
using Intersect.Framework.Core.GameObjects.Events;
using Intersect.Framework.Core.GameObjects.Items;
using Intersect.Framework.Core.GameObjects.Mapping.Tilesets;
using Intersect.Framework.Core.GameObjects.Maps;
using Intersect.Framework.Core.GameObjects.Maps.MapList;
using Intersect.Framework.Core.GameObjects.NPCs;
using Intersect.Framework.Core.GameObjects.PlayerClass;
using Intersect.Framework.Core.GameObjects.Resources;
using Intersect.Framework.Core.GameObjects.Variables;
using Intersect.Framework.Core.Security;
using Intersect.GameObjects;
using Intersect.Models;
using Intersect.Network.Packets.Client;
using Intersect.Server.Admin.Actions;
using Intersect.Server.Core;
using Intersect.Server.Database;
using Intersect.Server.Database.Logging.Entities;
using Intersect.Server.Database.PlayerData;
using Intersect.Server.Database.PlayerData.Security;
using Intersect.Server.Entities;
using Intersect.Server.General;
using Intersect.Server.Localization;
using Intersect.Server.Maps;
using Intersect.Server.Notifications;
using Intersect.Utilities;

namespace Intersect.Server.Networking;

internal sealed partial class NetworkedPacketHandler
{
    //AdminActionPacket
    public void HandlePacket(Client client, AdminActionPacket packet)
    {
        var player = client?.Entity;
        if (player == null || client.Power == UserRights.None)
        {
            return;
        }

        ActionProcessing.ProcessAction(player, (dynamic) packet.Action);
    }

    //OpenAdminWindowPacket
    public void HandlePacket(Client client, OpenAdminWindowPacket packet)
    {
        if (client.PermissionSet.IsGranted(Permission.WindowAdmin))
        {
            PacketSender.SendMapList(client);
        }
    }

    //RequestPasswordResetPacket
    public void HandlePacket(Client client, RequestPasswordResetPacket packet)
    {
        if (client.TimeoutMs > Timing.Global.Milliseconds)
        {
            PacketSender.SendError(client, Strings.Errors.ErrorTimeout, Strings.General.NoticeError);
            client.ResetTimeout();

            return;
        }

        if (!Options.Instance.SmtpValid)
        {
            client.FailedAttempt();
            return;
        }

        //Find account with that name or email
        var user = User.FindFromNameOrEmail(packet.NameOrEmail.Trim());
        if (user == null)
        {
            client.FailedAttempt();
            return;
        }

        if (!PasswordResetEmail.TryCreate(user, out var passwordResetEmail))
        {
            ApplicationContext.CurrentContext.Logger.LogError(
                "Failed to create password reset email for {UserName} ({UserId})",
                user.Name,
                user.Id
            );
            PacketSender.SendError(client, Strings.Account.EmailFail, Strings.General.NoticeError);
            return;
        }

        if (!passwordResetEmail.TrySend())
        {
            PacketSender.SendError(client, Strings.Account.EmailFail, Strings.General.NoticeError);
            return;
        }

        ApplicationContext.CurrentContext.Logger.LogInformation("Send password reset email to {UserId}", user.Id);
    }

        #region "Editor Packets"

        //PingPacket
        public void HandlePacket(Client client, Network.Packets.Editor.PingPacket packet)
        {
        }

        //LoginPacket
        public void HandlePacket(Client client, Network.Packets.Editor.LoginPacket packet)
        {
            if (client.AccountAttempts > 3 && client.TimeoutMs > Timing.Global.Milliseconds)
            {
                PacketSender.SendError(client, Strings.Errors.ErrorTimeout, Strings.General.NoticeError);
                client.ResetTimeout();

                return;
            }

            client.ResetTimeout();

            if (!User.TryLogin(packet.Username, packet.Password, out var user, out var failureReason))
            {
                UserActivityHistory.LogActivity(
                    Guid.Empty,
                    Guid.Empty,
                    client.Ip,
                    UserActivityHistory.PeerType.Editor,
                    UserActivityHistory.UserAction.FailedLogin,
                    $"{packet.Username},{failureReason.Type}"
                );

                if (failureReason.Type == LoginFailureType.InvalidCredentials)
                {
                    client.FailedAttempt();
                    PacketSender.SendError(client, Strings.Account.BadLogin, Strings.General.NoticeError);
                }
                else
                {
                    PacketSender.SendError(client, Strings.Account.UnknownServerErrorRetryLogin, Strings.General.NoticeError);
                }

                return;
            }

            if (!user.Power.Editor)
            {
                client.FailedAttempt();
                PacketSender.SendError(client, Strings.Account.BadAccess, Strings.General.NoticeError);

                return;
            }

            client.IsEditor = true;
            if (client.IsEditor)
            {
                //Is Editor
                client.PacketFloodingThresholds = Options.Instance.Security.Packets.EditorThresholds;
            }


            client.SetUser(user);

            lock (Client.GlobalLock)
            {
                var clients = Client.Instances.ToArray();
                foreach (var cli in clients)
                {
                    if (cli.Name != null &&
                        cli.Name.ToLower() == packet.Username.ToLower() &&
                        cli != client &&
                        cli.IsEditor)
                    {
                        cli.Disconnect();
                    }
                }
            }

            // PacketSender.SendServerConfig(client); // TODO: We already send this when the client is initialized, why do we send it again here?

            //Editor doesn't receive packet before login
            PacketSender.SendJoinGame(client);

            PacketSender.SendTimeBaseTo(client);
            PacketSender.SendMapList(client);
        }

        //MapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.MapUpdatePacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            if (!MapController.TryGet(packet.MapId, out var map))
            {
                return;
            }

            map.Load(packet.JsonData, map.Revision + 1);
            MapList.List.UpdateMap(packet.MapId);

            //Event Fixing
            var removedEvents = new List<Guid>();
            foreach (var id in map.EventIds)
            {
                if (!map.LocalEvents.ContainsKey(id))
                {
                    var evt = EventDescriptor.Get(id);
                    if (evt != null)
                    {
                        DbInterface.DeleteGameObject(evt);
                    }

                    removedEvents.Add(id);
                }
            }

            foreach (var id in removedEvents)
            {
                map.EventIds.Remove(id);
            }

            foreach (var evt in map.LocalEvents)
            {
                var dbObj = EventDescriptor.Get(evt.Key);
                if (dbObj == null)
                {
                    dbObj = (EventDescriptor)DbInterface.AddGameObject(GameObjectType.Event, evt.Key);
                }

                dbObj.Load(evt.Value.JsonData);
                DbInterface.SaveGameObject(dbObj);
                if (!map.EventIds.Contains(evt.Key))
                {
                    map.EventIds.Add(evt.Key);
                }
            }

            map.LocalEvents.Clear();

            if (packet.TileData != null && map.TileData != null)
            {
                map.TileData = packet.TileData;
            }

            map.AttributeData = packet.AttributeData;

            DbInterface.SaveGameObject(map);

            map.Initialize();
            var players = new List<Player>();
            foreach (var surrMap in map.GetSurroundingMaps(true))
            {
                players.AddRange(surrMap.GetPlayersOnAllInstances().ToArray());
            }

            foreach (var plyr in players)
            {
                plyr.Warp(plyr.MapId, (byte) plyr.X, (byte) plyr.Y, plyr.Dir, false, (byte) plyr.Z, true);
                PacketSender.SendMap(plyr.Client, packet.MapId);
            }

            PacketSender.SendMap(client, packet.MapId, true); //Sends map to everyone/everything in proximity
            PacketSender.SendMapListToAll();
        }

        //CreateMapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.CreateMapPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            lock (ServerContext.Instance.LogicService.LogicLock)
            {
                ServerContext.Instance.LogicService.LogicPool.WaitForIdle();
                var newMapId = Guid.Empty;
                MapController newMap = null;
                var tmpMap = new MapController(true);
                if (!packet.AttachedToMap)
                {
                    var destType = (int)packet.MapListParentType;
                    newMap = (MapController)DbInterface.AddGameObject(GameObjectType.Map);
                    newMapId = newMap.Id;
                    tmpMap = MapController.Get(newMapId);
                    DbInterface.GenerateMapGrids();
                    PacketSender.SendMap(client, newMapId, true);
                    PacketSender.SendMapGridToAll(tmpMap.MapGrid);

                    //FolderDirectory parent = null;
                    destType = -1;
                    if (destType == -1)
                    {
                        MapList.List.AddMap(newMapId, tmpMap.TimeCreated, MapDescriptor.Lookup);
                    }

                    DbInterface.SaveMapList();

                    PacketSender.SendMapListToAll();
                    /*else if (destType == 0)
                    {
                        parent = Database.MapStructure.FindDir(bf.ReadInteger());
                        if (parent == null)
                        {
                            Database.MapStructure.AddMap(newMap);
                        }
                        else
                        {
                            parent.Children.AddMap(newMap);
                        }
                    }
                    else if (destType == 1)
                    {
                        var mapNum = bf.ReadInteger();
                        parent = Database.MapStructure.FindMapParent(mapNum, null);
                        if (parent == null)
                        {
                            Database.MapStructure.AddMap(newMap);
                        }
                        else
                        {
                            parent.Children.AddMap(newMap);
                        }
                    }*/
                }
                else
                {
                    var relativeMap = packet.MapId;
                    switch (packet.AttachDir)
                    {
                        case 0:
                            if (MapController.Get(MapController.Get(relativeMap).Up) == null)
                            {
                                newMap = (MapController)DbInterface.AddGameObject(GameObjectType.Map);
                                newMapId = newMap.Id;
                                tmpMap = MapController.Get(newMapId);
                                tmpMap.MapGrid = MapController.Get(relativeMap).MapGrid;
                                tmpMap.MapGridX = MapController.Get(relativeMap).MapGridX;
                                tmpMap.MapGridY = MapController.Get(relativeMap).MapGridY - 1;
                                MapController.Get(relativeMap).Up = newMapId;
                                DbInterface.SaveGameObject(MapController.Get(relativeMap));
                            }

                            break;

                        case 1:
                            if (MapController.Get(MapController.Get(relativeMap).Down) == null)
                            {
                                newMap = (MapController)DbInterface.AddGameObject(GameObjectType.Map);
                                newMapId = newMap.Id;
                                tmpMap = MapController.Get(newMapId);
                                tmpMap.MapGrid = MapController.Get(relativeMap).MapGrid;
                                tmpMap.MapGridX = MapController.Get(relativeMap).MapGridX;
                                tmpMap.MapGridY = MapController.Get(relativeMap).MapGridY + 1;
                                MapController.Get(relativeMap).Down = newMapId;
                                DbInterface.SaveGameObject(MapController.Get(relativeMap));
                            }

                            break;

                        case 2:
                            if (MapController.Get(MapController.Get(relativeMap).Left) == null)
                            {
                                newMap = (MapController)DbInterface.AddGameObject(GameObjectType.Map);
                                newMapId = newMap.Id;
                                tmpMap = MapController.Get(newMapId);
                                tmpMap.MapGrid = MapController.Get(relativeMap).MapGrid;
                                tmpMap.MapGridX = MapController.Get(relativeMap).MapGridX - 1;
                                tmpMap.MapGridY = MapController.Get(relativeMap).MapGridY;
                                MapController.Get(relativeMap).Left = newMapId;
                                DbInterface.SaveGameObject(MapController.Get(relativeMap));
                            }

                            break;

                        case 3:
                            if (MapController.Get(MapController.Get(relativeMap).Right) == null)
                            {
                                newMap = (MapController)DbInterface.AddGameObject(GameObjectType.Map);
                                newMapId = newMap.Id;
                                tmpMap = MapController.Get(newMapId);
                                tmpMap.MapGrid = MapController.Get(relativeMap).MapGrid;
                                tmpMap.MapGridX = MapController.Get(relativeMap).MapGridX + 1;
                                tmpMap.MapGridY = MapController.Get(relativeMap).MapGridY;
                                MapController.Get(relativeMap).Right = newMapId;
                                DbInterface.SaveGameObject(MapController.Get(relativeMap));
                            }

                            break;
                    }

                    if (newMapId != Guid.Empty)
                    {
                        var grid = DbInterface.GetGrid(tmpMap.MapGrid);
                        if (tmpMap.MapGridX >= 0 && tmpMap.MapGridX < grid.Width)
                        {
                            if (tmpMap.MapGridY + 1 < grid.Height)
                            {
                                tmpMap.Down = grid.MapIdGrid[tmpMap.MapGridX, tmpMap.MapGridY + 1];

                                if (tmpMap.Down != Guid.Empty)
                                {
                                    MapController.Get(tmpMap.Down).Up = newMapId;
                                    DbInterface.SaveGameObject(MapController.Get(tmpMap.Down));
                                }
                            }

                            if (tmpMap.MapGridY - 1 >= 0)
                            {
                                tmpMap.Up = grid.MapIdGrid[tmpMap.MapGridX, tmpMap.MapGridY - 1];

                                if (tmpMap.Up != Guid.Empty)
                                {
                                    MapController.Get(tmpMap.Up).Down = newMapId;
                                    DbInterface.SaveGameObject(MapController.Get(tmpMap.Up));
                                }
                            }
                        }

                        if (tmpMap.MapGridY >= 0 && tmpMap.MapGridY < grid.Height)
                        {
                            if (tmpMap.MapGridX - 1 >= 0)
                            {
                                tmpMap.Left = grid.MapIdGrid[tmpMap.MapGridX - 1, tmpMap.MapGridY];

                                if (tmpMap.Left != Guid.Empty)
                                {
                                    MapController.Get(tmpMap.Left).Right = newMapId;
                                    DbInterface.SaveGameObject(MapController.Get(tmpMap.Left));
                                }
                            }

                            if (tmpMap.MapGridX + 1 < grid.Width)
                            {
                                tmpMap.Right = grid.MapIdGrid[tmpMap.MapGridX + 1, tmpMap.MapGridY];

                                if (tmpMap.Right != Guid.Empty)
                                {
                                    MapController.Get(tmpMap.Right).Left = newMapId;
                                    DbInterface.SaveGameObject(MapController.Get(tmpMap.Right));
                                }
                            }
                        }

                        DbInterface.SaveGameObject(newMap);

                        DbInterface.GenerateMapGrids();
                        PacketSender.SendMap(client, newMapId, true);
                        PacketSender.SendMapGridToAll(MapController.Get(newMapId).MapGrid);
                        PacketSender.SendEnterMap(client, newMapId);
                        var folderDir = MapList.List.FindMapParent(relativeMap, null);
                        if (folderDir != null)
                        {
                            folderDir.Children.AddMap(newMapId, MapController.Get(newMapId).TimeCreated, MapDescriptor.Lookup);
                        }
                        else
                        {
                            MapList.List.AddMap(newMapId, MapController.Get(newMapId).TimeCreated, MapDescriptor.Lookup);
                        }

                        DbInterface.SaveMapList();
                        PacketSender.SendMapListToAll();
                    }
                }
            }
        }

        //MapListUpdatePacket
        public void HandlePacket(Client client, Network.Packets.Editor.MapListUpdatePacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            MapListFolder parent = default;
            MapList currentList;
            switch (packet.UpdateType)
            {
                case MapListUpdate.MoveItem:
                    MapList.List.HandleMove(packet.TargetType, packet.TargetId, packet.ParentType, packet.ParentId);
                    break;

                case MapListUpdate.AddFolder:
                    if (packet.ParentId == Guid.Empty)
                    {
                        parent = default;
                        MapList.List.AddFolder(Strings.Mapping.NewFolder);
                        break;
                    }

                    switch (packet.ParentType)
                    {
                        case 0:
                            parent = MapList.List.FindFolder(packet.ParentId);
                            break;
                        case 1:
                            parent = MapList.List.FindMapParent(packet.ParentId, null);
                            break;
                    }

                    currentList = parent?.Children ?? MapList.List;
                    currentList.AddFolder(Strings.Mapping.NewFolder);
                    break;

                case MapListUpdate.Rename:
                    if (packet.TargetType == 0)
                    {
                        parent = MapList.List.FindFolder(packet.TargetId);
                        if (parent == default)
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to rename {nameof(MapListFolder)} {packet.TargetId} but it was not found.");
                            return;
                        }

                        parent.Name = packet.Name;
                        break;
                    }

                    if (packet.TargetType == 1)
                    {
                        var mapListMap = MapList.List.FindMap(packet.TargetId);
                        if (mapListMap == default)
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to rename {nameof(MapListMap)} {packet.TargetId} but it was not found in the map list.");
                            return;
                        }

                        if (!MapController.TryGet(packet.TargetId, out var mapToRename))
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to rename {nameof(MapListMap)} {packet.TargetId} but the map itself could not be found.");
                            return;
                        }

                        mapListMap.Name = packet.Name;
                        mapToRename.Name = packet.Name;
                        DbInterface.SaveGameObject(mapToRename);
                    }

                    break;

                case MapListUpdate.Delete:
                    if (packet.TargetType == 0)
                    {
                        MapList.List.DeleteFolder(packet.TargetId);
                        break;
                    }

                    if (packet.TargetType == 1)
                    {
                        if (MapController.Lookup == default)
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to delete {nameof(MapListMap)} {packet.TargetId} but the {nameof(MapController)}.{nameof(MapController.Lookup)} was null?");
                            return;
                        }

                        if (MapController.Lookup.Count < 2)
                        {
                            PacketSender.SendError(client, Strings.Mapping.LastMapError, Strings.Mapping.LastMap);
                            return;
                        }

                        var logicService = ServerContext.Instance?.LogicService;
                        var logicLock = logicService?.LogicLock;
                        if (logicLock == default)
                        {
                            ApplicationContext.Context.Value?.Logger.LogError($"{nameof(ServerContext)}.{nameof(ServerContext.Instance)}.{nameof(ServerContext.LogicService)}.{nameof(LogicService.LogicLock)} was unexpectedly null when try to delete {nameof(MapListMap)} {packet.TargetId}.");
                            return;
                        }

                        var logicPool = logicService.LogicPool;
                        if (logicPool == default)
                        {
                            ApplicationContext.Context.Value?.Logger.LogError($"{nameof(ServerContext)}.{nameof(ServerContext.Instance)}.{nameof(ServerContext.LogicService)}.{nameof(LogicService.LogicPool)} was unexpectedly null when try to delete {nameof(MapListMap)} {packet.TargetId}.");
                            return;
                        }

                        if (!MapController.TryGet(packet.TargetId, out var mapToDelete))
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to delete {nameof(MapListMap)} {packet.TargetId} but the map itself could not be found.");
                            return;
                        }

                        if (!(mapToDelete is MapController mapController))
                        {
                            ApplicationContext.Context.Value?.Logger.LogWarning($"Tried to delete {nameof(MapListMap)} {packet.TargetId} but {nameof(MapController)}.{nameof(MapController.TryGet)} returned something other than a {nameof(MapController)}.");
                            return;
                        }

                        lock (logicLock)
                        {
                            logicPool.WaitForIdle();

                            // Warp players off the map being deleted
                            var players = mapController.GetPlayersOnAllInstances();
                            foreach (var player in players)
                            {
                                player.WarpToSpawn();
                            }

                            MapList.List.DeleteMap(mapController.Id);
                            DbInterface.DeleteGameObject(mapController);
                            DbInterface.GenerateMapGrids();
                        }

                        PacketSender.SendMapToEditors(mapController.Id);
                    }

                    break;

                default:
                    throw new IndexOutOfRangeException($"{nameof(packet.UpdateType)} was not a valid value or has not been implemented: {packet.UpdateType}");
            }

            DbInterface.SaveMapList();
            PacketSender.SendMapListToAll();
        }

        //UnlinkMapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.UnlinkMapPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var mapId = packet.MapId;
            var curMapId = packet.CurrentMapId;
            var mapGrid = 0;
            if (MapController.Lookup.Keys.Contains(mapId))
            {
                if (client.IsEditor)
                {
                    lock (ServerContext.Instance.LogicService.LogicLock)
                    {
                        ServerContext.Instance.LogicService.LogicPool.WaitForIdle();
                        var map = MapController.Get(mapId);
                        if (map != null)
                        {
                            map.ClearConnections();

                            var grid = DbInterface.GetGrid(map.MapGrid);
                            var gridX = map.MapGridX;
                            var gridY = map.MapGridY;

                            //Up
                            if (gridY - 1 >= 0 && grid.MapIdGrid[gridX, gridY - 1] != Guid.Empty)
                            {
                                MapController.Get(grid.MapIdGrid[gridX, gridY - 1])?.ClearConnections(Direction.Down);
                            }

                            //Down
                            if (gridY + 1 < grid.Height && grid.MapIdGrid[gridX, gridY + 1] != Guid.Empty)
                            {
                                MapController.Get(grid.MapIdGrid[gridX, gridY + 1])?.ClearConnections(Direction.Up);
                            }

                            //Left
                            if (gridX - 1 >= 0 && grid.MapIdGrid[gridX - 1, gridY] != Guid.Empty)
                            {
                                MapController.Get(grid.MapIdGrid[gridX - 1, gridY])?.ClearConnections(Direction.Right);
                            }

                            //Right
                            if (gridX + 1 < grid.Width && grid.MapIdGrid[gridX + 1, gridY] != Guid.Empty)
                            {
                                MapController.Get(grid.MapIdGrid[gridX + 1, gridY])?.ClearConnections(Direction.Left);
                            }

                            DbInterface.GenerateMapGrids();
                            if (MapController.Lookup.Keys.Contains(curMapId))
                            {
                                mapGrid = MapController.Get(curMapId).MapGrid;
                            }
                        }

                        PacketSender.SendMapGridToAll(mapGrid);
                    }
                }
            }
        }

        //LinkMapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.LinkMapPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var adjacentMapId = packet.AdjacentMapId;
            var linkMapId = packet.LinkMapId;
            var adjacentMap = MapController.Get(packet.AdjacentMapId);
            var linkMap = MapController.Get(packet.LinkMapId);
            long gridX = packet.GridX;
            long gridY = packet.GridY;
            var canLink = true;

            lock (ServerContext.Instance.LogicService.LogicLock)
            {
                ServerContext.Instance.LogicService.LogicPool.WaitForIdle();
                if (adjacentMap != null && linkMap != null)
                {
                    //Clear to test if we can link.
                    var linkGrid = DbInterface.GetGrid(linkMap.MapGrid);
                    var adjacentGrid = DbInterface.GetGrid(adjacentMap.MapGrid);
                    if (linkGrid != adjacentGrid && linkGrid != null && adjacentGrid != null)
                    {
                        var xOffset = linkMap.MapGridX - gridX;
                        var yOffset = linkMap.MapGridY - gridY;
                        for (var x = 0; x < adjacentGrid.Width; x++)
                        {
                            for (var y = 0; y < adjacentGrid.Height; y++)
                            {
                                if (x + xOffset >= 0 &&
                                    x + xOffset < linkGrid.Width &&
                                    y + yOffset >= 0 &&
                                    y + yOffset < linkGrid.Height)
                                {
                                    if (adjacentGrid.MapIdGrid[x, y] != Guid.Empty &&
                                        linkGrid.MapIdGrid[x + xOffset, y + yOffset] != Guid.Empty)
                                    {
                                        //Incompatible Link!
                                        PacketSender.SendError(
                                            client,
                                            Strings.Mapping.LinkFailureError.ToString(
                                                MapDescriptor.GetName(linkMapId), MapDescriptor.GetName(adjacentMapId),
                                                MapDescriptor.GetName(adjacentGrid.MapIdGrid[x, y]),
                                                MapDescriptor.GetName(linkGrid.MapIdGrid[x + xOffset, y + yOffset])
                                            ), Strings.Mapping.LinkFailure
                                        );

                                        return;
                                    }
                                }
                            }
                        }

                        if (canLink)
                        {
                            var updatedMaps = new HashSet<MapController>();
                            for (var x = -1; x < adjacentGrid.Width + 1; x++)
                            {
                                for (var y = -1; y < adjacentGrid.Height + 1; y++)
                                {
                                    if (x + xOffset >= 0 &&
                                        x + xOffset < linkGrid.Width &&
                                        y + yOffset >= 0 &&
                                        y + yOffset < linkGrid.Height)
                                    {
                                        if (linkGrid.MapIdGrid[x + xOffset, y + yOffset] != Guid.Empty)
                                        {
                                            var inXBounds = x > -1 && x < adjacentGrid.Width;
                                            var inYBounds = y > -1 && y < adjacentGrid.Height;
                                            if (inXBounds && inYBounds)
                                            {
                                                adjacentGrid.MapIdGrid[x, y] = linkGrid.MapIdGrid[x + xOffset, y + yOffset];
                                            }

                                            if (inXBounds && y - 1 >= 0 && adjacentGrid.MapIdGrid[x, y - 1] != Guid.Empty)
                                            {
                                                MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]).Up = adjacentGrid.MapIdGrid[x, y - 1];
                                                updatedMaps.Add(MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]));

                                                MapController.Get(adjacentGrid.MapIdGrid[x, y - 1]).Down = linkGrid.MapIdGrid[x + xOffset, y + yOffset];
                                                updatedMaps.Add(MapController.Get(adjacentGrid.MapIdGrid[x, y - 1]));
                                            }

                                            if (inXBounds &&
                                                y + 1 < adjacentGrid.Height &&
                                                adjacentGrid.MapIdGrid[x, y + 1] != Guid.Empty)
                                            {
                                                MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]).Down = adjacentGrid.MapIdGrid[x, y + 1];
                                                updatedMaps.Add(MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]));

                                                MapController.Get(adjacentGrid.MapIdGrid[x, y + 1]).Up = linkGrid.MapIdGrid[x + xOffset, y + yOffset];
                                                updatedMaps.Add(MapController.Get(adjacentGrid.MapIdGrid[x, y + 1]));
                                            }

                                            if (inYBounds && x - 1 >= 0 && adjacentGrid.MapIdGrid[x - 1, y] != Guid.Empty)
                                            {
                                                MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]).Left = adjacentGrid.MapIdGrid[x - 1, y];
                                                updatedMaps.Add(MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]));

                                                MapController.Get(adjacentGrid.MapIdGrid[x - 1, y]).Right = linkGrid.MapIdGrid[x + xOffset, y + yOffset];
                                                updatedMaps.Add(MapController.Get(adjacentGrid.MapIdGrid[x - 1, y]));
                                            }

                                            if (inYBounds &&
                                                x + 1 < adjacentGrid.Width &&
                                                adjacentGrid.MapIdGrid[x + 1, y] != Guid.Empty)
                                            {
                                                MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]).Right = adjacentGrid.MapIdGrid[x + 1, y];
                                                updatedMaps.Add(MapController.Get(linkGrid.MapIdGrid[x + xOffset, y + yOffset]));

                                                MapController.Get(adjacentGrid.MapIdGrid[x + 1, y]).Left = linkGrid.MapIdGrid[x + xOffset, y + yOffset];
                                                updatedMaps.Add(MapController.Get(adjacentGrid.MapIdGrid[x + 1, y]));
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (var map in updatedMaps)
                            {
                                DbInterface.SaveGameObject(map);
                            }

                            DbInterface.GenerateMapGrids();
                            PacketSender.SendMapGridToAll(adjacentMap.MapGrid);
                        }
                    }
                }
            }
        }

        //CreateGameObjectPacket
        public void HandlePacket(Client client, Network.Packets.Editor.CreateGameObjectPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var type = packet.Type;
            var obj = DbInterface.AddGameObject(type);

            var changed = false;
            switch(type)
            {
                case GameObjectType.Event:
                    ((EventDescriptor)obj).CommonEvent = true;
                    changed = true;

                    break;
                case GameObjectType.Item:
                    ((ItemDescriptor)obj).DropChanceOnDeath = Options.Instance.Player.ItemDropChance;
                    changed = true;

                    break;
            }

            if (changed)
            {
                DbInterface.SaveGameObject(obj);
            }

            PacketSender.CacheGameDataPacket();

            PacketSender.SendGameObjectToAll(obj);
        }

        //RequestOpenEditorPacket
        public void HandlePacket(Client client, Network.Packets.Editor.RequestOpenEditorPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var type = packet.Type;
            PacketSender.SendGameObjects(client, type);
            PacketSender.SendOpenEditor(client, type);
        }

        //DeleteGameObjectPacket
        public void HandlePacket(Client client, Network.Packets.Editor.DeleteGameObjectPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var type = packet.Type;
            var id = packet.Id;

            // TODO: YO COME DO THIS
            IDatabaseObject obj = null;
            switch (type)
            {
                case GameObjectType.Animation:
                    obj = AnimationDescriptor.Get(id);

                    break;

                case GameObjectType.Class:
                    if (ClassDescriptor.Lookup.Count == 1)
                    {
                        PacketSender.SendError(client, Strings.Classes.LastClassError, Strings.Classes.LastClass);

                        return;
                    }

                    obj = DatabaseObject<ClassDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Item:
                    obj = ItemDescriptor.Get(id);

                    break;
                case GameObjectType.Npc:
                    obj = NPCDescriptor.Get(id);

                    break;

                case GameObjectType.Projectile:
                    obj = ProjectileDescriptor.Get(id);

                    break;

                case GameObjectType.Quest:
                    obj = QuestDescriptor.Get(id);

                    break;

                case GameObjectType.Resource:
                    obj = ResourceDescriptor.Get(id);

                    break;

                case GameObjectType.Shop:
                    obj = ShopDescriptor.Get(id);

                    break;

                case GameObjectType.Spell:
                    obj = SpellDescriptor.Get(id);

                    break;

                case GameObjectType.CraftTables:
                    obj = DatabaseObject<CraftingTableDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Crafts:
                    obj = DatabaseObject<CraftingRecipeDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Map:
                    break;

                case GameObjectType.Event:
                    obj = EventDescriptor.Get(id);

                    break;

                case GameObjectType.PlayerVariable:
                    obj = PlayerVariableDescriptor.Get(id);

                    break;

                case GameObjectType.ServerVariable:
                    obj = ServerVariableDescriptor.Get(id);

                    break;

                case GameObjectType.Tileset:
                    break;

                case GameObjectType.Time:
                    break;

                case GameObjectType.GuildVariable:
                    obj = GuildVariableDescriptor.Get(id);

                    break;

                case GameObjectType.UserVariable:
                    obj = UserVariableDescriptor.Get(id);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (obj != null)
            {
                switch (obj)
                {
                    //if Item or Resource, kill all global entities of that kind
                    case ItemDescriptor itemDescriptor:
                        MapController.DespawnInstancesOf(itemDescriptor);
                        break;
                    case NPCDescriptor npcDescriptor:
                        MapController.DespawnInstancesOf(npcDescriptor);
                        break;
                    case ProjectileDescriptor projectileDescriptor:
                        MapController.DespawnInstancesOf(projectileDescriptor);
                        break;
                    case ResourceDescriptor resourceDescriptor:
                        MapController.DespawnInstancesOf(resourceDescriptor);
                        break;
                }

                DbInterface.DeleteGameObject(obj);

                PacketSender.CacheGameDataPacket();

                PacketSender.SendGameObjectToAll(obj, true);
            }
        }

        //SaveGameObjectPacket
        public void HandlePacket(Client client, Network.Packets.Editor.SaveGameObjectPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var type = packet.Type;
            var id = packet.Id;
            IDatabaseObject obj = null;
            switch (type)
            {
                case GameObjectType.Animation:
                    obj = AnimationDescriptor.Get(id);

                    break;

                case GameObjectType.Class:
                    obj = DatabaseObject<ClassDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Item:
                    obj = ItemDescriptor.Get(id);

                    break;
                case GameObjectType.Npc:
                    obj = NPCDescriptor.Get(id);

                    break;

                case GameObjectType.Projectile:
                    obj = ProjectileDescriptor.Get(id);

                    break;

                case GameObjectType.Quest:
                    obj = QuestDescriptor.Get(id);

                    break;

                case GameObjectType.Resource:
                    obj = ResourceDescriptor.Get(id);

                    break;

                case GameObjectType.Shop:
                    obj = ShopDescriptor.Get(id);

                    break;

                case GameObjectType.Spell:
                    obj = SpellDescriptor.Get(id);

                    break;

                case GameObjectType.CraftTables:
                    obj = DatabaseObject<CraftingTableDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Crafts:
                    obj = DatabaseObject<CraftingRecipeDescriptor>.Lookup.Get(id);

                    break;

                case GameObjectType.Map:
                    break;

                case GameObjectType.Event:
                    obj = EventDescriptor.Get(id);

                    break;

                case GameObjectType.PlayerVariable:
                    obj = PlayerVariableDescriptor.Get(id);

                    break;

                case GameObjectType.ServerVariable:
                    obj = ServerVariableDescriptor.Get(id);

                    break;

                case GameObjectType.Tileset:
                    break;

                case GameObjectType.Time:
                    break;

                case GameObjectType.GuildVariable:
                    obj = GuildVariableDescriptor.Get(id);

                    break;

                case GameObjectType.UserVariable:
                    obj = UserVariableDescriptor.Get(id);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (obj != null)
            {
                lock (ServerContext.Instance.LogicService.LogicLock)
                {
                    ServerContext.Instance.LogicService.LogicPool.WaitForIdle();

                    //if Item or Resource, kill all global entities of that kind
                    switch (obj)
                    {
                        case ItemDescriptor itemDescriptor:
                            MapController.DespawnInstancesOf(itemDescriptor);
                            break;
                        case NPCDescriptor npcDescriptor:
                            MapController.DespawnInstancesOf(npcDescriptor);
                            break;
                        case ProjectileDescriptor projectileDescriptor:
                            MapController.DespawnInstancesOf(projectileDescriptor);
                            break;
                        case ResourceDescriptor resourceDescriptor:
                            MapController.DespawnInstancesOf(resourceDescriptor);
                            break;
                    }

                    obj.Load(packet.Data);

                    if (type == GameObjectType.Quest)
                    {
                        var qst = (QuestDescriptor)obj;
                        foreach (var evt in qst.RemoveEvents)
                        {
                            var evtb = EventDescriptor.Get(evt);
                            if (evtb != null)
                            {
                                DbInterface.DeleteGameObject(evtb);
                            }
                        }

                        foreach (var evt in qst.AddEvents)
                        {
                            var evtb = (EventDescriptor)DbInterface.AddGameObject(GameObjectType.Event, evt.Key);
                            evtb.Load(evt.Value.JsonData);

                            foreach (var tsk in qst.Tasks)
                            {
                                if (tsk.Id == evt.Key)
                                {
                                    tsk.CompletionEvent = evtb;
                                }
                            }

                            DbInterface.SaveGameObject(evtb);
                        }

                        qst.AddEvents.Clear();
                        qst.RemoveEvents.Clear();
                    }
                    else if (type == GameObjectType.PlayerVariable)
                    {
                        DbInterface.CachePlayerVariableEventTextLookups();
                    }
                    else if (type == GameObjectType.ServerVariable)
                    {
                        Player.StartCommonEventsWithTriggerForAll(CommonEventTrigger.ServerVariableChange, "", obj.Id.ToString());
                        DbInterface.CacheServerVariableEventTextLookups();
                    }
                    else if (type == GameObjectType.GuildVariable)
                    {
                        DbInterface.CacheGuildVariableEventTextLookups();
                    }
                    else if (type == GameObjectType.UserVariable)
                    {
                        DbInterface.CacheUserVariableEventTextLookups();
                    }

                    DbInterface.SaveGameObject(obj);
                    // Only replace the modified object
                    PacketSender.CacheGameDataPacket();

                    PacketSender.SendGameObjectToAll(obj, false);
                }

                if (type == GameObjectType.Item)
                {
                    foreach (var player in Player.OnlinePlayers)
                    {
                        player.CacheEquipmentTriggers();
                    }
                }
            }
        }

        //SaveTimeDataPacket
        public void HandlePacket(Client client, Network.Packets.Editor.SaveTimeDataPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            DaylightCycleDescriptor.Instance.LoadFromJson(packet.TimeJson);
            DbInterface.SaveTime();
            Time.Init();
            PacketSender.SendTimeBaseToAllEditors();
        }

        //AddTilesetsPacket
        public void HandlePacket(Client client, Network.Packets.Editor.AddTilesetsPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            foreach (var tileset in packet.Tilesets)
            {
                var value = tileset.Trim().ToLower();
                var found = false;
                foreach (var tset in TilesetDescriptor.Lookup)
                {
                    if (tset.Value.Name.Trim().ToLower() == value)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    var obj = DbInterface.AddGameObject(GameObjectType.Tileset);
                    ((TilesetDescriptor)obj).Name = value;
                    DbInterface.SaveGameObject(obj);
                    PacketSender.CacheGameDataPacket();
                    PacketSender.SendGameObjectToAll(obj);
                }
            }
        }

        //RequestGridPacket
        public void HandlePacket(Client client, Network.Packets.Editor.RequestGridPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            if (MapController.Lookup.Keys.Contains(packet.MapId))
            {
                if (client.IsEditor)
                {
                    PacketSender.SendMapGrid(client, MapController.Get(packet.MapId).MapGrid);
                }
            }
        }

        //OpenMapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.EnterMapPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            client.EditorMap = packet.MapId;
        }

        //NeedMapPacket
        public void HandlePacket(Client client, Network.Packets.Editor.NeedMapPacket packet)
        {
            if (!client.IsEditor)
            {
                return;
            }

            var map = MapController.Get(packet.MapId);
            if (map != null)
            {
                PacketSender.SendMap(client, packet.MapId);
            }
        }

        #endregion
}