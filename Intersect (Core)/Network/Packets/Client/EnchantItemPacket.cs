using MessagePack;

namespace Intersect.Network.Packets.Client;

[MessagePackObject]
public partial class EnchantItemPacket : IntersectPacket
{
    // Constructor sin parámetros para MessagePack
    public EnchantItemPacket()
    {
    }

    // Constructor para inicializar propiedades
    public EnchantItemPacket(Guid itemId, int targetLevel, Guid currencyId, int currencyAmount, bool useAmulet)
    {
        ItemId = itemId;
        TargetLevel = targetLevel;
        CurrencyId = currencyId;
        CurrencyAmount = currencyAmount;
        UseAmulet = useAmulet;
    }

    [Key(0)]
    public Guid ItemId { get; set; }

    [Key(1)]
    public int TargetLevel { get; set; }

    [Key(2)]
    public Guid CurrencyId { get; set; }

    [Key(3)]
    public int CurrencyAmount { get; set; }

    [Key(4)]
    public bool UseAmulet { get; set; }
}
