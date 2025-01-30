using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class QuestOfferPacket : IntersectPacket
{
    //Parameterless Constructor for MessagePack
    public QuestOfferPacket()
    {
    }

    public QuestOfferPacket(Guid questId, Dictionary<Guid, int> rewardItems)
    {
        QuestId = questId;
        RewardItems = rewardItems;
    }

    [Key(0)]
    public Guid QuestId { get; set; }
    [Key(1)]
    public Dictionary<Guid, int> RewardItems { get; set; }

}
