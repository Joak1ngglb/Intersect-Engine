using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class BagUpdatePacket : InventoryUpdatePacket
{
    //Parameterless Constructor for MessagePack
    public BagUpdatePacket() : base(0, Guid.Empty, 0, null, null,0)
    {
    }

    public BagUpdatePacket(int slot, Guid id, int quantity, Guid? bagId, ItemProperties properties,int enchantlevel) : base(
        slot, id, quantity, bagId, properties, enchantlevel
    )
    {
    }

}
