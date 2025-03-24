using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Intersect.Network.Packets.Client;
[MessagePackObject]
public class CancelOrderPacket : IntersectPacket
{
    [Key(0)] public Guid OrderId { get; set; }

    public CancelOrderPacket() { }

    public CancelOrderPacket(Guid orderId)
    {
        OrderId = orderId;
    }
}

