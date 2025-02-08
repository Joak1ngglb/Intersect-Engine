using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Server.Entities;
namespace Intersect.Server.Database.PlayerData.Players
{
    public class PlayerPet
    {
        [Key]  // Define la clave primaria
        public Guid PetId { get; set; } = Guid.NewGuid();

        public Guid PlayerId { get; set; }  // Relación con el jugador
        public virtual Player Player { get; set; }

        public Guid PetBaseId { get; set; }  // Relación con la especie de mascota
        public virtual PetBase PetBase { get; set; }

        public string Name { get; set; } = "Mascota";  // Nombre personalizado
        public int Level { get; set; } = 1;
        public long Experience { get; set; } = 0;
        public Gender Gender { get; set; }
        public PetPersonality Personality { get; set; }
        public int Energy { get; set; } = 100;
        public int Mood { get; set; } = 100;
        public int Maturity { get; set; } = 0;
        public int BreedCount { get; set; } = 0;
        public bool IsSterile { get; set; } = false;

        [Column("Stats")]
        public string StatsJson
        {
            get => JsonConvert.SerializeObject(Stats);
            set => Stats = JsonConvert.DeserializeObject<int[]>(value) ?? new int[Enum.GetValues<Stat>().Length];
        }

        [NotMapped]
        public int[] Stats { get; set; } = new int[Enum.GetValues<Stat>().Length];
        [Column("Vital")]
        public string VitalJson
        {
            get => JsonConvert.SerializeObject(Vital);
            set => Vital = JsonConvert.DeserializeObject<long[]>(value) ?? new long[Enum.GetValues<Vital>().Length];
        }
        [NotMapped]
        public long[] Vital { get; set; } = new long[Enum.GetValues<Vital>().Length];

        [Column("Personality")]
        public string PersonalityJson
        {
            get => JsonConvert.SerializeObject(Personality);
            set => Personality = JsonConvert.DeserializeObject<PetPersonality>(value);
        }
    }
}
