using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class QuestProgressPacket : IntersectPacket
{
    //Parameterless Constructor for MessagePack
    public QuestProgressPacket()
    {
    }

    public QuestProgressPacket(Dictionary<Guid, string> quests, Guid[] hiddenQuests, Dictionary<Guid, Dictionary<Guid, int>> questRewardItems)
    {
        Quests = quests;
        HiddenQuests = hiddenQuests;
        QuestRewardItems = questRewardItems;
    }

    [Key(0)]
    public Dictionary<Guid, string> Quests { get; set; }

    [Key(1)]
    public Guid[] HiddenQuests { get; set; }
    [Key(2)]
    public Dictionary<Guid, Dictionary<Guid, int>> QuestRewardItems { get; set; }

}
