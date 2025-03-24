using System;
using System.Collections.Generic;
using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public class AuctionTransactionHistoryResponsePacket : IntersectPacket
{
    [Key(0)] public List<AuctionTransactionInfo> Transactions { get; set; }

    public AuctionTransactionHistoryResponsePacket() { }

    public AuctionTransactionHistoryResponsePacket(List<AuctionTransactionInfo> transactions)
    {
        Transactions = transactions;
    }
}
[MessagePackObject]
public class AuctionTransactionInfo
{
    [Key(0)] public Guid BuyerId { get; set; }
    [Key(1)] public Guid SellerId { get; set; }
    [Key(2)] public Guid ItemId { get; set; }
    [Key(3)] public int Quantity { get; set; }
    [Key(4)] public int TotalPrice { get; set; }
    [Key(5)] public DateTime Timestamp { get; set; }

    public AuctionTransactionInfo() { }

    public AuctionTransactionInfo(Guid buyerId, Guid sellerId, Guid itemId, int quantity, int totalPrice, DateTime timestamp)
    {
        BuyerId = buyerId;
        SellerId = sellerId;
        ItemId = itemId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Timestamp = timestamp;
    }
}