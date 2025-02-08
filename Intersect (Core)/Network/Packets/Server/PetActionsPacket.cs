using MessagePack;
using System;

namespace Intersect.Network.Packets.Client;

[MessagePackObject]
public partial class BreedPetPacket : IntersectPacket
{
    public BreedPetPacket() { }

    public BreedPetPacket(Guid petId1, Guid petId2)
    {
        PetId1 = petId1;
        PetId2 = petId2;
    }

    [Key(0)]
    public Guid PetId1 { get; set; }

    [Key(1)]
    public Guid PetId2 { get; set; }
}

[MessagePackObject]
public partial class ReleasePetPacket : IntersectPacket
{
    public ReleasePetPacket() { }

    public ReleasePetPacket(Guid petId)
    {
        PetId = petId;
    }

    [Key(0)]
    public Guid PetId { get; set; }
}

[MessagePackObject]
public partial class PlayWithPetPacket : IntersectPacket
{
    public PlayWithPetPacket() { }

    public PlayWithPetPacket(Guid petId)
    {
        PetId = petId;
    }

    [Key(0)]
    public Guid PetId { get; set; }
}

[MessagePackObject]
public partial class FeedPetPacket : IntersectPacket
{
    public FeedPetPacket() { }

    public FeedPetPacket(Guid petId)
    {
        PetId = petId;
    }

    [Key(0)]
    public Guid PetId { get; set; }
}

[MessagePackObject]
public partial class TogglePetSummonPacket : IntersectPacket
{
    public TogglePetSummonPacket() { }

    public TogglePetSummonPacket(Guid petId)
    {
        PetId = petId;
    }

    [Key(0)]
    public Guid PetId { get; set; }
}
