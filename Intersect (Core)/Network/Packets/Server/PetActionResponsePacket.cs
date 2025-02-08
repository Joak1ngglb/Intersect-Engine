using MessagePack;
using System;
using Intersect.Network;

namespace Intersect.Network.Packets.Server
{
    [MessagePackObject]
    public partial class PetActionResponsePacket : IntersectPacket
    {
        public PetActionResponsePacket() { }

        public PetActionResponsePacket(Guid petId, string action, bool success, string message)
        {
            PetId = petId;
            Action = action;
            Success = success;
            Message = message;
        }

        [Key(0)]
        public Guid PetId { get; set; }

        [Key(1)]
        public string Action { get; set; }  // Ejemplo: "Summon", "Despawn", "Feed", "Play", "Breed", "Release"

        [Key(2)]
        public bool Success { get; set; }

        [Key(3)]
        public string Message { get; set; } // Mensaje para el cliente
    }
}
