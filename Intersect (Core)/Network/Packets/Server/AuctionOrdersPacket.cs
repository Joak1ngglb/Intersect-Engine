using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;


namespace Intersect.Network.Packets.Server;
[MessagePackObject]
public class AuctionOrdersPacket : IntersectPacket
{
    [Key(0)] public List<AuctionHouseOrderInfo> Orders { get; set; }

    public AuctionOrdersPacket() { }

    public AuctionOrdersPacket(List<AuctionHouseOrderInfo> orders)
    {
        Orders = orders;
    }
}
[MessagePackObject]
public class AuctionHouseOrderInfo
{
    [Key(0)] public Guid OrderId { get; set; }
    [Key(1)] public Guid ItemId { get; set; }
    [Key(2)] public int Quantity { get; set; }
    [Key(3)] public int Price { get; set; }

    public AuctionHouseOrderInfo() { }

    public AuctionHouseOrderInfo(Guid orderId, Guid itemId, int quantity, int price)
    {
        OrderId = orderId;
        ItemId = itemId;
        Quantity = quantity;
        Price = price;
    }
}


