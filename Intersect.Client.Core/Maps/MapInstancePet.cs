using System.Collections.Concurrent;
using Intersect.Client.Entities;
using Intersect.GameObjects.Maps;

namespace Intersect.Client.Maps
{
    public partial class MapInstance : MapBase
    {
        // Diccionario para manejar mascotas en el mapa
        public Dictionary<Guid, Pet> LocalPets { get; private set; } = new Dictionary<Guid, Pet>();

        /// <summary>
        /// Agrega una mascota al mapa.
        /// </summary>
        public void SpawnPet(Pet pet)
        {
            if (pet == null || LocalPets.ContainsKey(pet.Id))
            {
                return;
            }

            pet.MapId = Id;
            LocalPets.Add(pet.Id, pet);
            LocalEntities.Add(pet.Id, pet);
        }

        /// <summary>
        /// Elimina una mascota del mapa.
        /// </summary>
        public void DespawnPet(Guid petId)
        {
            if (LocalPets.ContainsKey(petId))
            {
                LocalPets.Remove(petId);
                LocalEntities.Remove(petId);
            }
        }

        /// <summary>
        /// Actualiza todas las mascotas en el mapa.
        /// </summary>
        private void UpdatePets()
        {
            foreach (var pet in LocalPets.Values)
            {
                pet.Update();
            }
        }

        /// <summary>
        /// Sobrecarga de `AddEntity` para soportar mascotas.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            if (entity is Pet pet)
            {
                SpawnPet(pet);
            }
            else
            {
                LocalEntities[entity.Id] = entity;
            }
        }

        /// <summary>
        /// Sobrecarga de `RemoveEntity` para manejar mascotas.
        /// </summary>
        public void RemoveEntity(Guid entityId)
        {
            if (LocalPets.ContainsKey(entityId))
            {
                DespawnPet(entityId);
            }
            else
            {
                LocalEntities.Remove(entityId);
            }
        }

    }
}
