using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Intersect.Network.Packets.Client;
[MessagePackObject]

public class AuctionTransactionHistoryPacket : IntersectPacket
{
    [Key(0)] public Guid PlayerId { get; set; }
    [Key(1)] public bool IsBuyer { get; set; }

    public AuctionTransactionHistoryPacket() { }

    public AuctionTransactionHistoryPacket(Guid playerId, bool isBuyer)
    {
        PlayerId = playerId;
        IsBuyer = isBuyer;
    }
}

