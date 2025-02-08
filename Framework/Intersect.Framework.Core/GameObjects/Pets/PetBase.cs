using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Conditions;   // Opcional, si se desea agregar condiciones
using Intersect.GameObjects.Events;        // Opcional, si se van a disparar eventos
using Intersect.Models;
using Intersect.Utilities;
using Newtonsoft.Json;

namespace Intersect.GameObjects
{
    /// <summary>
    /// La clase PetBase define la plantilla o configuración base de una mascota.
    /// Se basa en la estructura de NpcBase pero adaptada para el sistema de mascotas.
    /// Aquí se definen la especie, requisitos reproductivos, estadísticas base, 
    /// regeneración y valores máximos de vitales, propiedades de combate y aspectos visuales.
    /// </summary>
    public partial class PetBase : DatabaseObject<PetBase>, IFolderable
    {
        /// <summary>
        /// Constructor sin parámetros (usado por EF y para valores por defecto).
        /// </summary>


        // Constructor sin parámetros para EF
        public PetBase() : base(Guid.NewGuid())
        {
            Name = "New Pet";
        }

        // Constructor para Json
        [JsonConstructor]
        public PetBase(Guid id) : base(id)
        {
            Name = "New Pet";
        }

        #region Propiedades de Reproducción y Especie

        /// <summary>
        /// Nombre de la especie (por ejemplo, "Canino", "Felino", etc.).
        /// </summary>
        public string Species { get; set; }

        /// <summary>
        /// Porcentaje de madurez requerido para que la mascota pueda reproducirse.
        /// </summary>
        public int RequiredMaturity { get; set; }

        /// <summary>
        /// Nivel mínimo de energía requerido para la reproducción.
        /// </summary>
        public int RequiredEnergy { get; set; }

        /// <summary>
        /// Nivel mínimo de humor (estado de ánimo) requerido para la reproducción.
        /// </summary>
        public int RequiredMood { get; set; }

        /// <summary>
        /// Número máximo de reproducciones que la mascota puede realizar antes de volverse estéril.
        /// </summary>
        public int MaxBreedCount { get; set; }

        /// <summary>
        /// Personalidad por defecto de la mascota (se asigna al nacer).
        /// </summary>
        public PetPersonality DefaultPersonality { get; set; }

        /// <summary>
        /// Rareza de la mascota.
        /// </summary>
        public PetRarity Rarity { get; set; }

        #endregion

        #region Estadísticas y Vitales

        /// <summary>
        /// Estadísticas base para la mascota (por ejemplo, ataque, defensa, velocidad, etc.).
        /// Su longitud depende del número de Stat definidos en el enum.
        /// </summary>
        [NotMapped]
        public int[] PetStats { get; set; }

        /// <summary>
        /// Porcentaje o cantidad base de regeneración para cada vital (por ejemplo, salud, maná).
        /// </summary>
        [NotMapped]
        public long[] VitalRegen { get; set; }

        /// <summary>
        /// Valores máximos para cada vital.
        /// </summary>
        [NotMapped]
        public long[] MaxVital { get; set; }

        #endregion
        // Propiedades de inmunidades y habilidades
        [NotMapped]
        public List<SpellEffect> Immunities { get; set; } = new List<SpellEffect>();

        [JsonIgnore]
        [Column("Immunities")]
        public string ImmunitiesJson
        {
            get => JsonConvert.SerializeObject(Immunities);
            set => Immunities = JsonConvert.DeserializeObject<List<SpellEffect>>(value ?? "") ?? new List<SpellEffect>();
        }
        #region Propiedades de Combate

        /// <summary>
        /// Daño base que inflige la mascota (para ataques físicos).
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Tipo de daño que inflige la mascota.
        /// </summary>
        public int DamageType { get; set; }

        /// <summary>
        /// Probabilidad de realizar un ataque crítico (en porcentaje).
        /// </summary>
        public int CritChance { get; set; }

        /// <summary>
        /// Multiplicador del daño crítico.
        /// </summary>
        public double CritMultiplier { get; set; }

        /// <summary>
        /// Modificador de velocidad de ataque (para determinar el intervalo entre ataques).
        /// </summary>
        public int AttackSpeedModifier { get; set; }

        /// <summary>
        /// Valor base de velocidad de ataque en milisegundos.
        /// </summary>
        public int AttackSpeedValue { get; set; }

        // Animaciones
        [Column("AttackAnimation")]
        public Guid AttackAnimationId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AnimationBase AttackAnimation
        {
            get => AnimationBase.Get(AttackAnimationId);
            set => AttackAnimationId = value?.Id ?? Guid.Empty;
        }

        [Column("DeathAnimation")]
        public Guid DeathAnimationId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AnimationBase DeathAnimation
        {
            get => AnimationBase.Get(DeathAnimationId);
            set => DeathAnimationId = value?.Id ?? Guid.Empty;
        }


        #endregion

        #region Propiedades Visuales y de Organización

        /// <summary>
        /// Ruta o nombre del sprite que representa la mascota.
        /// </summary>
        public string Sprite { get; set; }

        /// <summary>
        /// Propiedad de color para efectos visuales (en formato ARGB).
        /// Se almacena en la base de datos en formato JSON.
        /// </summary>
        [Column("Color")]
        [JsonIgnore]
        public string JsonColor
        {
            get => JsonConvert.SerializeObject(Color);
            set => Color = !string.IsNullOrWhiteSpace(value)
                ? JsonConvert.DeserializeObject<Color>(value)
                : new Color(255, 255, 255, 255);
        }

        /// <summary>
        /// Color de la mascota (ARGB).
        /// </summary>
        [NotMapped]
        public Color Color { get; set; } = new Color(255, 255, 255, 255);

        /// <summary>
        /// Carpeta o categoría en la que se organiza la plantilla en el editor.
        /// </summary>

        [Column("AggroList")]
        [JsonIgnore]
        public string JsonAggroList
        {
            get => JsonConvert.SerializeObject(AggroList);
            set => AggroList = JsonConvert.DeserializeObject<List<Guid>>(value);
        }

        [NotMapped]
        public List<Guid> AggroList { get; set; } = new List<Guid>();

        public bool AttackAllies { get; set; }
        public string Folder { get; set; }
        public int Level { get; set; }=1;
        public int Scaling { get; set; } = 100;

        public int ScalingStat { get; set; }

        public int SightRange { get; set; }
        // Spells
        [NotMapped]
        public DbList<SpellBase> Spells { get; set; } = new DbList<SpellBase>();

        [JsonIgnore]
        [Column("Spells")]
        public string SpellsJson
        {
            get => JsonConvert.SerializeObject(Spells, Formatting.None);
            protected set => Spells = JsonConvert.DeserializeObject<DbList<SpellBase>>(value);
        }
        public int DefaultBehavior { get; set; }



        #endregion
    }

    public enum PetPersonality
    {
        Joyful,
        Serious,
        Playful,
        Shy,
        Brave,
        Protective,
        Energetic,
        Leader,
        Mysterious,
        Wise,
        Fierce,
        Aggressive,
        Impulsive
    }
    public enum PetRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    public enum PetType 
    {
        Normal,
        Hybrid,
        Legendary
    }
    public enum PetBehavior
    {
        Passive,
        Aggressive,
        Defensive,
        Supportive
    }
}
