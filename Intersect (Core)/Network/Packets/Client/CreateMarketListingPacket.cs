using System;
using Intersect.Network.Packets.Server;
using MessagePack;

namespace Intersect.Network.Packets.Client
{
    [MessagePackObject]
    public class CreateMarketListingPacket : IntersectPacket
    {
        [Key(0)] public Guid ItemId { get; set; }
        [Key(1)] public int Quantity { get; set; }
        [Key(2)] public int Price { get; set; }
        [Key(3)] public ItemProperties Properties { get; set; }
    }
}
