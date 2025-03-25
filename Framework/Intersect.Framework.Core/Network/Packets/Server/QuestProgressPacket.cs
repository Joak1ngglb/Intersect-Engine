using Intersect.Framework.Core.Config;
using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class QuestProgressPacket : IntersectPacket
{
    // Constructor sin parámetros para MessagePack
    public QuestProgressPacket()
    {
    }

    public QuestProgressPacket(Dictionary<Guid, string> quests, Guid[] hiddenQuests, Dictionary<Guid, Dictionary<Guid, int>> questRewardItems, Dictionary<Guid, long> questRewardExperience, Dictionary<Guid, Dictionary<JobType, long>> questRewardJobExperience)
    {
        Quests = quests;
        HiddenQuests = hiddenQuests;
        QuestRewardItems = questRewardItems;
        QuestRewardExperience = questRewardExperience;
        QuestRewardJobExperience = questRewardJobExperience;
    }

    [Key(0)]
    public Dictionary<Guid, string> Quests { get; set; }

    [Key(1)]
    public Guid[] HiddenQuests { get; set; }

    [Key(2)]
    public Dictionary<Guid, Dictionary<Guid, int>> QuestRewardItems { get; set; }

    [Key(3)]
    public Dictionary<Guid, long> QuestRewardExperience { get; set; } // Nueva propiedad

    [Key(4)]
    public Dictionary<Guid, Dictionary<JobType, long>> QuestRewardJobExperience { get; set; } // Nueva propiedad
}
