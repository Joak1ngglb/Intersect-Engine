using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Intersect.Network.Packets.Server
{
    /// <summary>
    /// The definition of the NationPacket sent to a player containing the online and offline members of their nations.
    /// </summary>
    [MessagePackObject]
    public partial class NationPacket : IntersectPacket
    {
        /// <summary>
        /// Parameterless Constructor for MessagePack
        /// </summary>
        public NationPacket()
        {
        }
        /// <summary>
        /// Create a new instance of this class and define its contents.
        /// </summary>
        /// <param name="members">An array containing all nation members and metadata.</param>
        public NationPacket(NationMember[] members)
        {
            Members = members;
        }
        [Key(0)]
        public NationMember[] Members { get; set; }
    }
    [MessagePackObject]
    public partial class NationMember
    {
        [Key(0)]
        public Guid Id;
        [Key(1)]
        public string Name;
        [Key(2)]
        public int Level;
        [Key(3)]
        public string Class;
        [Key(4)]
        public string Map;
        [Key(5)]
        public bool Online = false;
        /// <summary>
        /// Parameterless constructor for messagepack
        /// </summary>
        public NationMember()
        {
        }
        public NationMember(Guid id, string name, int level, string cls, string map)
        {
            Id = id;
            Name = name;
            Level = level;
            Class = cls;
            Map = map;
        }
    }
}