using Intersect.Network.Packets.Server;
using Intersect.Network;
using MessagePack;
namespace Intersect.Network.Packets.Client;

[MessagePackObject]
public class CreateSellOrderPacket : IntersectPacket
{
    [Key(0)] public Guid SellerId { get; set; }
    [Key(1)] public Guid ItemId { get; set; }
    [Key(2)] public int Quantity { get; set; }
    [Key(3)] public int Price { get; set; }
 
    public CreateSellOrderPacket() { }

    public CreateSellOrderPacket(Guid sellerId, Guid itemId, int quantity, int price)
    {
        SellerId = sellerId;
        ItemId = itemId;
        Quantity = quantity;
        Price = price;
      
    }
}

