using MessagePack;

namespace Intersect.Network.Packets.Server
{
    [MessagePackObject]
    public class UpdateItemLevelPacket : IntersectPacket
    {
        [Key(0)]
        public Guid ItemId { get; set; }

        [Key(1)]
        public int NewEnchantmentLevel { get; set; }

        public UpdateItemLevelPacket() { }

        public UpdateItemLevelPacket(Guid itemId, int newEnchantmentLevel)
        {
            ItemId = itemId;
            NewEnchantmentLevel = newEnchantmentLevel;
        }
    }
}
