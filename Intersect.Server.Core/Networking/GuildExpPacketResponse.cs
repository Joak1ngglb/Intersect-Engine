using Intersect.Network;
using MessagePack;

namespace Intersect.Network.Packets.Server;
[MessagePackObject]
public partial class GuildExpPacketResponse: IntersectPacket

{
    public GuildExpPacketResponse()
    {
    }
    public GuildExpPacketResponse(float experience)
    {
        Experience = experience;
    }

    [Key(0)]
    public float Experience;

}