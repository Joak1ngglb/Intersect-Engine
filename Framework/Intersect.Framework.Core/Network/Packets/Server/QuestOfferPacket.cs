using Intersect.Framework.Core.Config;
using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class QuestOfferPacket : IntersectPacket
{
    // Constructor sin parámetros para MessagePack
    public QuestOfferPacket()
    {
    }

    public QuestOfferPacket(Guid questId, Dictionary<Guid, int> rewardItems, long rewardExperience, Dictionary<JobType, long> rewardJobExperience)
    {
        QuestId = questId;
        RewardItems = rewardItems;
        RewardExperience = rewardExperience;
        RewardJobExperience = rewardJobExperience;
    }

    [Key(0)]
    public Guid QuestId { get; set; }

    [Key(1)]
    public Dictionary<Guid, int> RewardItems { get; set; }

    [Key(2)]
    public long RewardExperience { get; set; } // Nueva propiedad

    [Key(3)]
    public Dictionary<JobType, long> RewardJobExperience { get; set; } // Nueva propiedad
}
