using Intersect.Enums;
using MessagePack;
using System;

namespace Intersect.Network.Packets.Client
{
    [MessagePackObject]
    public partial class UpdateNationMemberPacket : IntersectPacket
    {
        /// <summary>
        /// Parameterless Constructor for MessagePack
        /// </summary>
        public UpdateNationMemberPacket()
        {

        }

        public UpdateNationMemberPacket(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

    }
}