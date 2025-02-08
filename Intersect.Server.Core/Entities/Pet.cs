using System;
using System.Collections.Generic;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Server.Entities.Combat;
using Intersect.Server.Entities.Events;
using Intersect.Server.Entities.Pathfinding;
using Intersect.Server.Maps;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;
using Intersect.Core; // Acceso a PetBase, PetPersonality, PetRarity
using Newtonsoft.Json;
using Intersect.Server.Networking;
using Intersect.Server.Framework.Items;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Server.Database.PlayerData.Players;
using Stat = Intersect.Enums.Stat;
using Intersect.Server.Database;

namespace Intersect.Server.Entities
{
    /// <summary>
    /// Enum para los diferentes modos de comportamiento (behaviour) de la mascota.
    /// </summary>
    public enum PetBehavior
    {
        Idle,       // Descansando o sin acción activa.
        Following,  // Siguiendo al propietario.
        Aggressive, // Atacando de forma proactiva a enemigos cercanos.
        Defensive,  // Atacando solo si el propietario está en peligro.
        Fleeing     // Huyendo (por ejemplo, cuando tiene poca salud o energía).
    }

    /// <summary>
    /// La clase Pet representa una mascota en el juego.
    /// Hereda de Entity y añade propiedades específicas para el sistema de mascotas (estado, reproducción, etc.)
    /// Además, integra la lógica de combate, asignación de target, pathfinding y agro, tomando como referencia la lógica de los NPC.
    /// </summary>
    public partial class Pet : Entity
    {
        private static readonly Random random = new Random();
        private Pathfinder mPathFinder;

        #region Propiedades Específicas de la Mascota

        public PetBase PetBase { get; }
        public Player Owner { get; set; }

        /// <summary>
        /// Madurez: de 0 (bebé) a 100 (maduro). Se requiere 100 para reproducirse.
        /// </summary>
        public int Maturity { get; set; }

        /// <summary>
        /// Energía (0 a 100).
        /// </summary>
        public int Energy { get; set; }

        /// <summary>
        /// Humor (0 a 100).
        /// </summary>
        public int Mood { get; set; }

        /// <summary>
        /// Número de reproducciones realizadas.
        /// </summary>
        public int BreedCount { get; set; } = 0;

        /// <summary>
        /// Indica si la mascota es estéril.
        /// </summary>
        public bool IsSterile { get; set; } = false;

        public Gender PetGender { get; set; }
        public PetPersonality Personality { get; set; }
        public PetRarity Rarity { get; set; }

        // Datos opcionales de progresión.
        public int Level { get; set; }
        public long Experience { get; set; }

        /// <summary>
        /// Comportamiento actual de la mascota.
        /// </summary>
        public PetBehavior Behavior { get; private set; } = PetBehavior.Following;

        /// <summary>
        /// Rango de detección para buscar objetivos en combate (por ejemplo, 5 tiles).
        /// </summary>
        [NotMapped]
        public byte Range => 5; // Puedes parametrizar este valor en PetBase si lo deseas.

        public int Vital { get; set; }
        public bool IsSummoned { get; set; }
        public Guid OwnerId { get; set; }

        #endregion

        #region Campos para Gestión de Combate y Targeting

        // Tiempo de espera para buscar un nuevo objetivo.
        public long FindTargetWaitTime;
        public int FindTargetDelay = 500;
        private int mTargetFailCounter = 0;
        private int mTargetFailMax = 10;

        #endregion

        #region Constructor

        /// <summary>
        /// Crea una nueva mascota usando la plantilla (PetBase) y asignándole un dueño.
        /// Inicializa las barras de estado y configura los datos base.
        /// </summary>   // Constructor sin parámetros para Entity Framework
        public Pet() { }

        public Pet(Player owner, PetBase petBase) : base()
        {
            Owner = owner;
            PetBase = petBase;

            Name = petBase.Name;
            Sprite = petBase.Sprite;
            Level = petBase.Level; // Se puede iniciar en 1 o en el nivel requerido.
            Experience = 0;

            Maturity = 0; // Inicia como bebé.
            Energy = 100;
            Mood = 100;
            BreedCount = 0;
            IsSterile = false;

            // Asignar género aleatoriamente.
            PetGender = random.NextDouble() < 0.5 ? Gender.Male : Gender.Female;
            Personality = petBase.DefaultPersonality;
            Rarity = petBase.Rarity;

            // Inicializamos el pathfinder (reutilizando la lógica de NPC).
            mPathFinder = new Pathfinder(this);
            for (var i = 0; i < Enum.GetValues<Stat>().Length; i++)
            {
                BaseStats[i] = PetBase.PetStats[i];
                Stat[i] = new Combat.Stat((Stat)i, this);
            }

            var spellSlot = 0;
            for (var I = 0; I < PetBase.Spells.Count; I++)
            {
                var slot = new SpellSlot(spellSlot);
                slot.Set(new Spell(PetBase.Spells[I]));
                Spells.Add(slot);
                spellSlot++;
            }

            for (var i = 0; i < Enum.GetValues<Vital>().Length; i++)
            {
                SetMaxVital(i, PetBase.MaxVital[i]);
                SetVital(i, PetBase.MaxVital[i]);
            }
        }

        #endregion

        #region Métodos de Interacción (Feed, Play, Train, etc.)

        public void Feed(int foodAmount)
        {
            Energy += foodAmount;
            if (Energy > 100)
                Energy = 100;
            PacketSender.SendChatMsg(Owner, $"Tu mascota se ha alimentado y su energía es ahora {Energy}.", ChatMessageType.Notice);
        }

        public void Play()
        {
            Mood += 10;
            if (Mood > 100)
                Mood = 100;
            PacketSender.SendChatMsg(Owner, $"Tu mascota se ha divertido y su humor es ahora {Mood}.", ChatMessageType.Notice);
        }

        public void Train()
        {
            Experience += 50;
            Maturity += 5; // Cada entrenamiento incrementa la madurez.
            if (Maturity > 100)
                Maturity = 100;
            CheckForLevelUp();
            PacketSender.SendChatMsg(Owner, $"Tu mascota ha entrenado. Nivel: {Level}, Madurez: {Maturity}.", ChatMessageType.Notice);
        }

        private void CheckForLevelUp()
        {
            int threshold = 1000;
            while (Experience >= threshold)
            {
                Experience -= threshold;
                Level++;
                PacketSender.SendChatMsg(Owner, $"¡Tu mascota ha subido al nivel {Level}!", ChatMessageType.Notice);
            }
        }

        #endregion

        #region Métodos de Reproducción

        public Pet Breed(Pet partner)
        {
            if (this.PetBase.Species != partner.PetBase.Species)
                throw new InvalidOperationException("Las mascotas deben ser de la misma especie.");

            if (this.Maturity < this.PetBase.RequiredMaturity || partner.Maturity < partner.PetBase.RequiredMaturity)
                throw new InvalidOperationException("Ambas mascotas deben estar completamente maduras.");

            if (this.Energy < this.PetBase.RequiredEnergy || partner.Energy < partner.PetBase.RequiredEnergy)
                throw new InvalidOperationException("Ambas mascotas deben tener suficiente energía.");
            if (this.Mood < this.PetBase.RequiredMood || partner.Mood < partner.PetBase.RequiredMood)
                throw new InvalidOperationException("Ambas mascotas deben tener buen humor.");

            if (this.PetGender == partner.PetGender)
                throw new InvalidOperationException("Las mascotas deben tener géneros complementarios.");

            if (this.IsSterile || partner.IsSterile ||
                this.BreedCount >= this.PetBase.MaxBreedCount || partner.BreedCount >= partner.PetBase.MaxBreedCount)
            {
                this.IsSterile = true;
                partner.IsSterile = true;
                throw new InvalidOperationException("Una de las mascotas ha alcanzado el límite de reproducciones y es estéril.");
            }

            this.BreedCount++;
            partner.BreedCount++;
            if (this.BreedCount >= this.PetBase.MaxBreedCount)
                this.IsSterile = true;
            if (partner.BreedCount >= partner.PetBase.MaxBreedCount)
                partner.IsSterile = true;

            this.Energy -= this.PetBase.RequiredEnergy;
            partner.Energy -= partner.PetBase.RequiredEnergy;
            this.Mood -= this.PetBase.RequiredMood / 2;
            partner.Mood -= partner.PetBase.RequiredMood / 2;

            PetPersonality childPersonality;
            double mutationChance = 0.15; // 15% de probabilidad de mutación.
            if (random.NextDouble() < mutationChance)
            {
                var personalities = Enum.GetValues(typeof(PetPersonality));
                do
                {
                    childPersonality = (PetPersonality)personalities.GetValue(random.Next(personalities.Length));
                } while (childPersonality == this.Personality || childPersonality == partner.Personality);
            }
            else
            {
                childPersonality = random.NextDouble() < 0.5 ? this.Personality : partner.Personality;
            }

            Gender childGender = random.NextDouble() < 0.5 ? Gender.Male : Gender.Female;

            int childLevel = 1;
            long childExperience = 0;
            int childMaturity = 0;
            int childEnergy = (this.Energy + partner.Energy) / 2;
            int childMood = (this.Mood + partner.Mood) / 2;
            int childBreedCount = 0;
            bool childIsSterile = false;

            PetBase childPetBase = this.PetBase;

            Pet child = new Pet(this.Owner, childPetBase)
            {
                Level = childLevel,
                Experience = childExperience,
                Maturity = childMaturity,
                Energy = childEnergy,
                Mood = childMood,
                BreedCount = childBreedCount,
                IsSterile = childIsSterile,
                PetGender = childGender,
                Personality = childPersonality,
                Rarity = childPetBase.Rarity
            };

            PacketSender.SendChatMsg(Owner, "¡Reproducción exitosa! Se ha generado una nueva mascota.", ChatMessageType.Notice);
            return child;
        }

        protected override EntityItemSource AsItemSource()
        {
            return new EntityItemSource
            {
                EntityType = EntityType.GlobalEntity
            };
        }

        #endregion

        #region Movimiento y Comportamiento en Combate

        /// <summary>
        /// Permite que la mascota siga a su dueño mediante pathfinding.
        /// Si el dueño está en otro mapa, se realiza un warp.
        /// </summary>
        public void FollowOwner(long timeMs)
        {
            if (Owner == null || Owner.IsDisposed)
                return;

            if (Owner.MapId != this.MapId)
            {
                Warp(Owner.MapId, Owner.X, Owner.Y, Owner.Dir);
                return;
            }

            const int followDistance = 3;
            if (InRangeOf(Owner, followDistance))
                return;

            var currentTarget = mPathFinder.GetTarget();
            if (currentTarget == null ||
                currentTarget.TargetMapId != Owner.MapId ||
                currentTarget.TargetX != Owner.X ||
                currentTarget.TargetY != Owner.Y)
            {
                mPathFinder.SetTarget(new PathfinderTarget(Owner.MapId, Owner.X, Owner.Y, Owner.Z));
            }

            var result = mPathFinder.Update(timeMs);
            switch (result.Type)
            {
                case PathfinderResultType.Success:
                    {
                        Direction moveDir = mPathFinder.GetMove();
                        if (moveDir != Direction.None && CanMoveInDirection(moveDir))
                        {
                            Move(moveDir, Owner);
                        }
                        break;
                    }
                case PathfinderResultType.OutOfRange:
                case PathfinderResultType.NoPathToTarget:
                    {
                        Warp(Owner.MapId, Owner.X, Owner.Y, Owner.Dir);
                        break;
                    }
                case PathfinderResultType.Failure:
                    {
                        break;
                    }
                case PathfinderResultType.Wait:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Evalúa el entorno y el estado de la mascota para determinar el comportamiento.
        /// Si el dueño está en combate se activa el modo agresivo o defensivo; si la energía es baja, se queda idle; de lo contrario sigue al dueño.
        /// </summary>
        public void EvaluateBehavior(long timeMs)
        {
            if (Owner != null && Owner.IsUnderAttack())
            {
                // Según la personalidad, cambia a agresivo o defensivo.
                if (Personality == PetPersonality.Aggressive || Personality == PetPersonality.Impulsive)
                    SetBehavior(PetBehavior.Aggressive);
                else
                    SetBehavior(PetBehavior.Defensive);
            }
            else if (Energy < 30)
            {
                SetBehavior(PetBehavior.Idle);
            }
            else
            {
                SetBehavior(PetBehavior.Following);
            }
        }

        /// <summary>
        /// Cambia el comportamiento de la mascota y notifica al propietario.
        /// </summary>
        public void SetBehavior(PetBehavior newBehavior)
        {
            if (Behavior != newBehavior)
            {
                Behavior = newBehavior;
                PacketSender.SendChatMsg(Owner, $"Tu mascota ahora está en modo {Behavior}.", ChatMessageType.Notice);
            }
        }

        /// <summary>
        /// En modo Defensive, si el dueño está siendo atacado, la mascota ataca al objetivo del dueño.
        /// En modo Aggressive, busca cualquier objetivo hostil cercano.
        /// </summary>
        public void EngageCombat(long timeMs)
        {
            if (Behavior == PetBehavior.Defensive && Owner.Target != null && !Owner.Target.IsDead())
            {
                // Asiste al dueño atacando el mismo objetivo.
                Entity ownerTarget = Owner.Target;
                if (!InRangeOf(ownerTarget, 1))
                {
                    Direction moveDir = DirectionToTarget(ownerTarget);
                    if (CanMoveInDirection(moveDir))
                        Move(moveDir, Owner);
                }
                else
                {
                    if (!IsFacingTarget(ownerTarget))
                        ChangeDir(DirectionToTarget(ownerTarget));
                    TryAttack(ownerTarget);
                }
            }
            else if (Behavior == PetBehavior.Aggressive)
            {
                // Busca cualquier objetivo hostil cercano dentro de un rango definido.
                int searchRange = 5;
                Entity chosenTarget = null;
                int closestDistance = int.MaxValue;
                foreach (var instance in MapController.GetSurroundingMapInstances(MapId, MapInstanceId, true))
                {
                    foreach (var entity in instance.GetCachedEntities())
                    {
                        if (entity == null || entity.IsDead() || entity == this || entity == Owner)
                            continue;
                        if (IsHostile(entity))
                        {
                            int dist = GetDistanceTo(entity);
                            if (dist < searchRange && dist < closestDistance)
                            {
                                closestDistance = dist;
                                chosenTarget = entity;
                            }
                        }
                    }
                }
                if (chosenTarget != null)
                {
                    if (!InRangeOf(chosenTarget, 1))
                    {
                        Direction moveDir = DirectionToTarget(chosenTarget);
                        if (CanMoveInDirection(moveDir))
                            Move(moveDir, Owner);
                    }
                    else
                    {
                        if (!IsFacingTarget(chosenTarget))
                            ChangeDir(DirectionToTarget(chosenTarget));
                        TryAttack(chosenTarget);
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza la mascota: evalúa su comportamiento y ejecuta acciones en consecuencia.
        /// </summary>
        public override void Update(long timeMs)
        {
            base.Update(timeMs);
            EvaluateBehavior(timeMs);
            switch (Behavior)
            {
                case PetBehavior.Following:
                    FollowOwner(timeMs);
                    break;
                case PetBehavior.Aggressive:
                case PetBehavior.Defensive:
                    EngageCombat(timeMs);
                    break;
                case PetBehavior.Idle:
                    // No se mueve.
                    break;
                case PetBehavior.Fleeing:
                    // Lógica para huir (por implementar).
                    break;
                default:
                    FollowOwner(timeMs);
                    break;
            }
        }

        /// <summary>
        /// Determina si un objetivo es hostil para la mascota.
        /// Se considera hostil a:
        ///   - Un jugador que no sea el propietario y que no esté en zona segura.
        ///   - Un NPC que tenga al propietario como objetivo.
        /// </summary>
        private bool IsHostile(Entity target)
        {
            if (target is Player p)
            {
                if (p.Id != Owner.Id && MapController.Get(p.MapId).ZoneType != MapZone.Safe)
                    return true;
            }
            else if (target is Npc npc)
            {
                if (npc.Target != null && npc.Target.Id == Owner.Id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Busca un nuevo objetivo hostil cercano, usando lógica similar a la de los NPC.
        /// </summary>
        public void TryFindNewTarget(long timeMs, Guid avoidId = default(Guid), bool ignoreTimer = false, Entity attackedBy = null)
        {
            if (!ignoreTimer && FindTargetWaitTime > timeMs)
                return;

            var possibleTargets = new List<Entity>();
            int closestRange = Range + 1;
            int closestIndex = -1;

            // Recorre las instancias de mapas adyacentes para buscar objetivos.
            foreach (var instance in MapController.GetSurroundingMapInstances(MapId, MapInstanceId, true))
            {
                foreach (var entity in instance.GetCachedEntities())
                {
                    if (entity == null || entity.IsDead() || entity == this || entity.Id == avoidId)
                        continue;
                    if (IsHostile(entity))
                    {
                        int dist = GetDistanceTo(entity);
                        if (dist <= Range && dist < closestRange)
                        {
                            possibleTargets.Add(entity);
                            closestRange = dist;
                            closestIndex = possibleTargets.Count - 1;
                        }
                    }
                }
            }
            if (possibleTargets.Count > 0)
            {
                AssignTarget(possibleTargets[closestIndex]);
            }
            else
            {
                mTargetFailCounter++;
                if (mTargetFailCounter > mTargetFailMax)
                {
                    RemoveTarget();
                    mTargetFailCounter = 0;
                }
            }
            FindTargetWaitTime = timeMs + FindTargetDelay;
        }

        private void RemoveTarget()
        {
            Target = null;
            mTargetFailCounter = 0;
            PacketSender.SendChatMsg(Owner, "La mascota ha perdido su objetivo.", ChatMessageType.Notice);
        }

        /// <summary>
        /// Asigna un objetivo a la mascota.
        /// </summary>
        public void AssignTarget(Entity target)
        {
            if (target != null && target != Target)
            {
                Target = target;
                CombatTimer = Timing.Global.Milliseconds + Options.CombatTime;
                // Notificar al entorno que la mascota ha cambiado de objetivo (puedes crear un paquete similar a SendNpcAggressionToProximity).
                PacketSender.SendChatMsg(Owner, $"La mascota ahora ataca a {target.Name}.", ChatMessageType.Notice);
                mTargetFailCounter = 0;
            }
            else
            {
                Target = target;
            }
        }

        #endregion

        #region Métodos de Combate (TryAttack, Attack, etc.)

        public override int CalculateAttackTime()
        {
            if (PetBase.AttackSpeedModifier == 1)
            {
                return PetBase.AttackSpeedValue;
            }
            return base.CalculateAttackTime();
        }

        /// <summary>
        /// Intenta atacar a un objetivo en combate (cuando está a 1 tile de distancia y mirando al objetivo).
        /// Se utiliza la lógica de ataque de la entidad (heredada de Entity) y los parámetros de combate de la plantilla.
        /// </summary>
        public override void TryAttack(Entity target)
        {
            if (target == null || target.IsDisposed)
                return;
            if (!CanAttack(target, null))
                return;
            if (!IsOneBlockAway(target))
                return;
            if (!IsFacingTarget(target))
            {
                ChangeDir(DirectionToTarget(target));
                return;
            }
            if (IsAttacking)
                return;

            var deadAnimations = new List<KeyValuePair<Guid, Direction>>();
            var aliveAnimations = new List<KeyValuePair<Guid, Direction>>();

            // Utiliza los parámetros de combate definidos en la plantilla de la mascota.
            Attack(target, PetBase.Damage, 0, (DamageType)PetBase.DamageType, (Enums.Stat)PetBase.ScalingStat,
                   PetBase.Scaling, PetBase.CritChance, PetBase.CritMultiplier, deadAnimations, aliveAnimations, true);
            PacketSender.SendEntityAttack(this, CalculateAttackTime());
        }

        /// <summary>
        /// Método de ataque: delega en la implementación base (Entity.Attack) para calcular daños, aplicar efectos, etc.
        /// Se pueden agregar lógicas adicionales específicas para mascotas si se desea.
        /// </summary>
        public void Attack(Entity enemy,
                           long baseDamage,
                           long secondaryDamage,
                           DamageType damageType,
                           Enums.Stat scalingStat,
                           int scaling,
                           int critChance,
                           double critMultiplier,
                           List<KeyValuePair<Guid, Direction>> deadAnimations,
                           List<KeyValuePair<Guid, Direction>> aliveAnimations,
                           bool isAutoAttack)
        {
            // Llamamos al método base para procesar el ataque.
            base.Attack(enemy, baseDamage, secondaryDamage, damageType, scalingStat, scaling, critChance, critMultiplier,
                        deadAnimations, aliveAnimations, isAutoAttack);
        }
        public override void ProcessRegen()
        {
            base.ProcessRegen();
            // Lógica adicional para la regeneración de la mascota, si es necesario.
        }

        public long GetVital(Vital vital)
        {
            return base.GetVital(vital);
        }

        public override long GetMaxVital(Vital vital)
        {
            return base.GetMaxVital(vital);
        }
        public void Summon()
        {
            if (IsSummoned)
            {
                PacketSender.SendChatMsg(Owner, $"{Name} ya está invocado.", ChatMessageType.Notice);
                return;
            }

            if (!MapController.TryGetInstanceFromMap(Owner.MapId, Owner.MapInstanceId, out var mapInstance))
            {
                PacketSender.SendChatMsg(Owner, "Error: No se encontró la instancia del mapa.", ChatMessageType.Notice);
                return;
            }

            if (!mapInstance.TileBlocked(Owner.X, Owner.Y))
            {
                MapId = Owner.MapId;
                X = Owner.X;
                Y = Owner.Y;
                Dir = Owner.Dir;

                mapInstance.AddEntity(this);
                IsSummoned = true;

                PacketSender.SendEntityDataToProximity(this);
                PacketSender.SendChatMsg(Owner, $"Has invocado a {Name}!", ChatMessageType.Notice);
            }
            else
            {
                PacketSender.SendChatMsg(Owner, "No hay suficiente espacio para invocar a tu mascota.", ChatMessageType.Notice);
            }
        }
        public void Despawn()
        {
            if (!IsSummoned)
            {
                PacketSender.SendChatMsg(Owner, $"{Name} ya está guardado.", ChatMessageType.Notice);
                return;
            }

            if (!MapController.TryGetInstanceFromMap(Owner.MapId, Owner.MapInstanceId, out var mapInstance))
            {
                PacketSender.SendChatMsg(Owner, "Error: No se encontró la instancia del mapa.", ChatMessageType.Notice);
                return;
            }

            mapInstance.RemoveEntity(this);
            IsSummoned = false;

            PacketSender.SendEntityLeave(this);
            PacketSender.SendChatMsg(Owner, $"{Name} ha sido guardado.", ChatMessageType.Notice);
        }


        #endregion

        // Otros métodos (como ProcessRegen, GetVital, etc.) se heredan de Entity.
    }
}
