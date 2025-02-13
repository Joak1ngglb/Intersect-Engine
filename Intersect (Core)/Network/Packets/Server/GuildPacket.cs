﻿using MessagePack;

namespace Intersect.Network.Packets.Server;


/// <summary>
/// The definition of the GuildPacket sent to a player containing the online and offline members of their guilds.
/// </summary>
[MessagePackObject]
public partial class GuildPacket : IntersectPacket
{
    /// <summary>
    /// Parameterless Constructor for MessagePack
    /// </summary>
    public GuildPacket()
    {

    }
    /// <summary>
    /// Create a new instance of this class and define its contents.
    /// </summary>
    /// <param name="members">An array containing all guild members and metadata.</param>
    public GuildPacket(GuildMember[] members)
    {
        Members = members;
    }

    [Key(0)]
    public GuildMember[] Members { get; set; }

}

[MessagePackObject]
public partial class GuildMember
{
    [Key(0)]
    public Guid Id;
    [Key(1)]
    public string Name;
    [Key(2)]
    public int Rank;
    [Key(3)]
    public int Level;
    [Key(4)]
    public string ClassName;
    [Key(5)]
    public string MapName;
    [Key(6)]
    public bool Online = false;

    /// <summary>
    /// Parameterless constructor for messagepack
    /// </summary>
    public GuildMember()
    {

    }

    public GuildMember(Guid id, string name, int rank, int level, string className, string mapName)
    {
        Id = id;
        Name = name;
        Rank = rank;
        Level = level;
        ClassName = className;
        MapName = mapName;
    }
}
