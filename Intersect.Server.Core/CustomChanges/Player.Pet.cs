using System;
using System.Collections.Generic;
using System.Linq;
using Intersect.Server.Networking;
using Intersect.Enums;
using Intersect.Server.Localization;
using Intersect.Core;
using Intersect.Server.Database;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.GameObjects;

namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        // Mascota actualmente equipada
        [NotMapped]
        public Pet EquippedPet { get; private set; }

        // Lista de mascotas en posesión del jugador
        public virtual List<Pet> Pets { get; set; } = new List<Pet>();

        public Guid Id { get; set; }

        /// <summary>
        /// Equipa una mascota a partir de un PetItem
        /// </summary>
        public void EquipPet(Item petItem)
        {
            if (petItem == null || !petItem.Descriptor.IsPet)
                return;

            // Si ya hay una mascota equipada, primero la desequipamos
            if (EquippedPet != null)
            {
                UnequipPet();
            }

            // Buscar la mascota en la lista del jugador
            var pet = Pets.FirstOrDefault(p => p.PetBase.Id == petItem.Descriptor.PetBaseId);

            // Si no la encuentra, la creamos a partir de PetBase
            if (pet == null)
            {
                var petBase = PetBase.Get(petItem.Descriptor.PetBaseId);
                if (petBase == null)
                    return;

                pet = new Pet(this, petBase);
                Pets.Add(pet);
            }

            // Equipar la mascota
            EquippedPet = pet;

            // Eliminar el PetItem del inventario
            Items.Remove((Database.PlayerData.Players.InventorySlot)petItem);

            PacketSender.SendChatMsg(this, $"Has invocado a {EquippedPet.Name}!", ChatMessageType.Notice);
        }

        /// <summary>
        /// Desequipa la mascota y la guarda como un PetItem
        /// </summary>
        public void UnequipPet()
        {
            if (EquippedPet == null)
                return;

            // Convertir la mascota en un PetItem
            var petItem = new Item(EquippedPet.PetBase.Id, 1);
            Items.Add((Database.PlayerData.Players.InventorySlot)petItem);

            PacketSender.SendChatMsg(this, $"{EquippedPet.Name} ha sido guardada en tu inventario.", ChatMessageType.Notice);

            // Quitar la mascota equipada
            EquippedPet = null;
        }

        /// <summary>
        /// Cargar las mascotas del jugador desde la base de datos
        /// </summary>
        public void LoadPets()
        {
            using (var context = DbInterface.CreatePlayerContext())
            {
                Pets = context.Player_Pets
                    .Where(p => p.PlayerId == Id)
                    .Select(p => new Pet(this, p.PetBase)
                    {
                        Name = p.Name,
                        Level = p.Level,
                        Experience = p.Experience,
                        PetGender = p.Gender,
                        Personality = p.Personality,
                        Energy = p.Energy,
                        Mood = p.Mood,
                        Maturity = p.Maturity,
                        BreedCount = p.BreedCount,
                        IsSterile = p.IsSterile,
                      //  Stat = JsonConvert.DeserializeObject<int[]>(p.StatsJson) ?? new int[Enum.GetValues<Stat>().Length],
                        //Vital = JsonConvert.DeserializeObject<int[]>(p.VitalJson) ?? new int[Enum.GetValues<Vital>().Length]
                    })
                    .ToList();
            }
        }

        /// <summary>
        /// Guardar las mascotas del jugador en la base de datos
        /// </summary>
        public void SavePets()
        {
            using (var context = DbInterface.CreatePlayerContext())
            {
                foreach (var pet in Pets)
                {
                    var existingPet = context.Player_Pets.FirstOrDefault(p => p.PetId == pet.Id);
                    if (existingPet != null)
                    {
                        existingPet.Name = pet.Name;
                        existingPet.Level = pet.Level;
                        existingPet.Experience = pet.Experience;
                        existingPet.Gender = pet.PetGender;
                        existingPet.Personality = pet.Personality;
                        existingPet.Energy = pet.Energy;
                        existingPet.Mood = pet.Mood;
                        existingPet.Maturity = pet.Maturity;
                        existingPet.BreedCount = pet.BreedCount;
                        existingPet.IsSterile = pet.IsSterile;
                        existingPet.StatsJson = JsonConvert.SerializeObject(pet.Stat);
                        existingPet.VitalJson = JsonConvert.SerializeObject(pet.Vital);
                    }
                    else
                    {
                        context.Player_Pets.Add(new PlayerPet
                        {
                            PetId = pet.Id,
                            PlayerId = this.Id,
                            PetBaseId = pet.PetBase.Id,
                            Name = pet.Name,
                            Level = pet.Level,
                            Experience = pet.Experience,
                            Gender = pet.PetGender,
                            Personality = pet.Personality,
                            Energy = pet.Energy,
                            Mood = pet.Mood,
                            Maturity = pet.Maturity,
                            BreedCount = pet.BreedCount,
                            IsSterile = pet.IsSterile,
                            StatsJson = JsonConvert.SerializeObject(pet.Stat),
                            VitalJson = JsonConvert.SerializeObject(pet.Vital)
                        });
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
