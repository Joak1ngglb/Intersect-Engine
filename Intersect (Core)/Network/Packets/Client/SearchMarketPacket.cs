using Intersect.Enums;
using MessagePack;

namespace Intersect.Network.Packets.Client
{
    [MessagePackObject]
    public class SearchMarketPacket : IntersectPacket
    {
        [Key(0)] public string ItemName { get; set; } = "";
        [Key(1)] public int? MinPrice { get; set; }
        [Key(2)] public int? MaxPrice { get; set; }
        [Key(3)] public ItemType? Type { get; set; }
    }
}
