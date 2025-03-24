using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Intersect.Network.Packets.Client;
[MessagePackObject]
public class BuyItemtoAuctionPacket : IntersectPacket
{
    [Key(0)] public Guid BuyerId { get; set; }
    [Key(1)] public Guid OrderId { get; set; }
    [Key(2)] public int Quantity { get; set; }

    public BuyItemtoAuctionPacket() { }

    public BuyItemtoAuctionPacket(Guid buyerId, Guid orderId, int quantity)
    {
        BuyerId = buyerId;
        OrderId = orderId;
        Quantity = quantity;
    }
}
