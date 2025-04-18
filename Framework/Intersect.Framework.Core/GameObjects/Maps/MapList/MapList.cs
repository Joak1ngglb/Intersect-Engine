using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Collections;
using Intersect.Framework.Core.Serialization;
using Newtonsoft.Json;

namespace Intersect.Framework.Core.GameObjects.Maps.MapList;

public partial class MapList
{
    //So EF will save this :P
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [NotMapped]
    public List<MapListItem> Items { get; set; } = [];

    public static MapList List { get; set; } = new();

    public static List<MapListMap> OrderedMaps { get; } = [];

    [JsonIgnore]
    [Column("JsonData")]
    public string JsonData
    {
        get => JsonConvert.SerializeObject(
            this,
            new JsonSerializerSettings()
            {
                SerializationBinder = new IntersectTypeSerializationBinder(),
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            }
        );
        set => JsonConvert.PopulateObject(
            value, this,
            new JsonSerializerSettings()
            {
                SerializationBinder = new IntersectTypeSerializationBinder(),
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            }
        );
    }

    public void PostLoad(DatabaseObjectLookup gameMaps, bool isServer = true, bool isTopLevel = false)
    {
        if (isTopLevel)
        {
            OrderedMaps.Clear();
        }

        foreach (var itm in Items.ToArray())
        {
            if (itm.Type == 0)
            {
                var dirItm = (MapListFolder)itm;
                dirItm.PostLoad(gameMaps, isServer);
            }
            else
            {
                var mapItm = (MapListMap)itm;
                var removed = false;
                if (isServer)
                {
                    if (gameMaps.Get<MapDescriptor>(mapItm.MapId) == null)
                    {
                        Items.Remove(itm);
                        removed = true;
                    }
                }

                if (!removed)
                {
                    mapItm.PostLoad(gameMaps, isServer);
                    OrderedMaps.Add(mapItm);
                }
            }
        }

        if (isTopLevel)
        {
            OrderedMaps.Sort();
        }
    }

    public void AddMap(Guid mapId, long timeCreated, DatabaseObjectLookup gameMaps)
    {
        if (!gameMaps.Keys.Contains(mapId))
        {
            return;
        }

        var tmp = new MapListMap()
        {
            Name = gameMaps[mapId].Name,
            MapId = mapId,
            TimeCreated = timeCreated
        };

        Items.Add(tmp);
    }

    public void AddFolder(string folderName)
    {
        var tmp = new MapListFolder() { Name = folderName, FolderId = Guid.NewGuid() };
        Items.Add(tmp);
    }

    public MapListFolder FindFolder(Guid folderId)
    {
        foreach (var item in Items)
        {
            if (item is MapListFolder folder)
            {
                if (folder.FolderId == folderId)
                {
                    return folder;
                }

                if (folder.Children.TryFindFolder(folderId, out var subfolder))
                {
                    return subfolder;
                }
            }
        }

        return default;
    }

    public bool TryFindFolder(Guid folderId, out MapListFolder mapListFolder)
    {
        mapListFolder = FindFolder(folderId);
        return mapListFolder != default;
    }

    public MapListMap FindMap(Guid mapId)
    {
        foreach (var item in Items)
        {
            switch (item)
            {
                case MapListFolder folder:
                    if (folder.Children.TryFindMap(mapId, out var mapListMap))
                    {
                        return mapListMap;
                    }
                    break;

                case MapListMap map:
                    if (map.MapId == mapId)
                    {
                        return map;
                    }
                    break;
            }
        }

        return default;
    }

    public bool TryFindMap(Guid mapId, out MapListMap mapListMap)
    {
        mapListMap = FindMap(mapId);
        return mapListMap != default;
    }

    public MapListFolder FindMapParent(Guid mapId, MapListFolder parent)
    {
        foreach (var item in Items)
        {
            switch (item)
            {
                case MapListFolder folder:
                    var mapParent = folder.Children.FindMapParent(mapId, folder);
                    if (mapParent != default)
                    {
                        return mapParent;
                    }
                    break;

                case MapListMap map:
                    if (map.MapId == mapId)
                    {
                        return parent;
                    }
                    break;
            }
        }

        return default;
    }

    public MapListFolder FindFolderParent(Guid folderId, MapListFolder parent)
    {
        foreach (var item in Items)
        {
            if (item is MapListFolder folder)
            {
                if (folder.FolderId == folderId)
                {
                    return parent;
                }

                var folderParent = folder.Children.FindFolderParent(folderId, folder);
                if (folderParent != default)
                {
                    return folderParent;
                }
            }
        }

        return default;
    }

    public void HandleMove(int srcType, Guid srcId, int destType, Guid destId)
    {
        MapListFolder sourceParent = null;
        MapListFolder destParent = null;
        MapList targetList = null;
        MapList sourceList = null;
        MapListItem source = null;
        MapListItem dest = null;
        if (destType == 0)
        {
            destParent = FindFolderParent(destId, null);
            if (destParent == null)
            {
                targetList = List;
            }
            else
            {
                targetList = destParent.Children;
            }

            dest = FindFolder(destId);
        }
        else
        {
            destParent = FindMapParent(destId, null);
            if (destParent == null)
            {
                targetList = List;
            }
            else
            {
                targetList = destParent.Children;
            }

            dest = FindMap(destId);
        }

        if (srcType == 0)
        {
            sourceParent = FindFolderParent(srcId, null);
            if (sourceParent == null)
            {
                sourceList = List;
            }
            else
            {
                sourceList = sourceParent.Children;
            }

            source = FindFolder(srcId);
        }
        else
        {
            sourceParent = FindMapParent(srcId, null);
            if (sourceParent == null)
            {
                sourceList = List;
            }
            else
            {
                sourceList = sourceParent.Children;
            }

            source = FindMap(srcId);
        }

        if (targetList != null && dest != null && sourceList != null && source != null)
        {
            if (destType == 0)
            {
                ((MapListFolder)dest).Children.Items.Add(source);
                sourceList.Items.Remove(source);
            }
            else
            {
                sourceList.Items.Remove(source);
                targetList.Items.Insert(targetList.Items.IndexOf(dest), source);
            }
        }
        else if (targetList != null && sourceList != null && source != null)
        {
            if (destType == -1)
            {
                targetList.Items.Add(source);
                sourceList.Items.Remove(source);
            }
        }

        //Save Map List
        //PacketSender.SendMapListToEditors();
    }

    public void DeleteFolder(Guid folderId)
    {
        var parent = FindFolderParent(folderId, null);
        var self = FindFolder(folderId);
        if (parent == null)
        {
            List.Items.AddRange(self.Children.Items);
            List.Items.Remove(self);
        }
        else
        {
            parent.Children.Items.AddRange(self.Children.Items);
            parent.Children.Items.Remove(self);
        }
    }

    public void DeleteMap(Guid mapid)
    {
        var parent = FindMapParent(mapid, null);
        var self = FindMap(mapid);
        if (parent == null)
        {
            List.Items.Remove(self);
        }
        else
        {
            parent.Children.Items.Remove(self);
        }
    }

    public void UpdateMap(Guid mapId)
    {
        var map = FindMap(mapId);
        var mapInstance = MapDescriptor.Get(mapId);
        if (map != null && mapInstance != null)
        {
            map.Name = mapInstance.Name;
            map.TimeCreated = mapInstance.TimeCreated;
        }
    }

    public Guid FindFirstMap()
    {
        var lowestMap = Guid.Empty;

        if (OrderedMaps.Count > 0)
        {
            lowestMap = OrderedMaps.OrderBy(m => m.TimeCreated).FirstOrDefault()?.MapId ?? default;
        }

        return lowestMap;
    }
}
