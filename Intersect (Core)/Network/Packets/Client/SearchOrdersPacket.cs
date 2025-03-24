using Intersect.Network;
using MessagePack;
namespace Intersect.Network.Packets.Client;

[MessagePackObject]
public class SearchOrdersPacket : IntersectPacket
{
    [Key(0)] public Guid ItemId { get; set; }

    public SearchOrdersPacket() { }

    public SearchOrdersPacket(Guid itemId)
    {
        ItemId = itemId;

    }
}
