using System;
using Intersect.Client.Entities;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;

namespace Intersect.Client.Entities
{
    public class Pet : Entity
    {
        public Guid OwnerId { get; set; }
        public int Level { get; set; }
        public int[] CurrentStats { get; set; }
        public float MovementSpeed { get; set; }

        // Propiedades adicionales
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public PetRarity Rarity { get; set; }
        public PetPersonality Personality { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Hunger { get; set; }
        public int Mood { get; set; }
        public int Maturity { get; set; }
        public int BreedCount { get; set; }
        public bool IsSummoned { get; set; }

        public Pet(Guid id, PetPacket packet) : base(id, packet, EntityType.Pet)
        {
            Load(packet);
        }

        public override void Load(EntityPacket packet)
        {
            base.Load(packet);
            var petPacket = (PetPacket)packet;
            OwnerId = petPacket.OwnerId;
            Level = petPacket.Level;
            CurrentStats = petPacket.CurrentStats;
            MovementSpeed = petPacket.MovementSpeed;

            Name = petPacket.Name;
            Gender = petPacket.Gender;
            Rarity = petPacket.Rarity;
            Personality = petPacket.Personality;
            Health = petPacket.Health;
            MaxHealth = petPacket.MaxHealth;
            Hunger = petPacket.Hunger;
            Mood = petPacket.Mood;
            Maturity = petPacket.Maturity;
            BreedCount = petPacket.BreedCount;
            IsSummoned = petPacket.IsSummoned;
        }

        public override bool Update()
        {
            if (IsMoving)
            {
                MoveTimer = Timing.Global.Milliseconds + (long)GetMovementTime();
            }
            return base.Update();
        }

        public override float GetMovementTime()
        {
            return MovementSpeed;
        }

        internal string GetStatsSummary()
        {
            var statsSummary = new List<string>();

            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            {
                int statValue = CurrentStats[(int)stat];
                if (statValue > 0)
                {
                    statsSummary.Add($"{stat}: {statValue}");
                }
            }

            return statsSummary.Count > 0 ? string.Join(" | ", statsSummary) : "No stats available";
        }

        internal string GetVitalsSummary()
        {
            var vitalsSummary = new List<string>();

            foreach (Vital vital in Enum.GetValues(typeof(Vital)))
            {
             long currentVital = GetVital(vital);
                long maxVital = GetMaxVital(vital);

                if (maxVital > 0) // Solo mostrar si tiene un valor máximo válido
                {
                    vitalsSummary.Add($"{vital}: {currentVital}/{maxVital}");
                }
            }

            return vitalsSummary.Count > 0 ? string.Join(" | ", vitalsSummary) : "No vitals available";
        }

        private long GetVital(Vital vital)
        {
            return vital switch
            {
                Enums.Vital.Health => Health,
                Enums.Vital.Mana => 0, // Asumiendo que el mana no está implementado
                _ => 0
            };
        }

        private long GetMaxVital(Vital vital)
        {
            return vital switch
            {
                Enums.Vital.Health => MaxHealth,
                Enums.Vital.Mana => 0, // Asumiendo que el mana no está implementado
                _ => 0
            };
        }
    }
}
