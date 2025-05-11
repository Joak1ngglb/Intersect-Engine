using System;
using System.Collections.Generic;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Server.General;
using Intersect.Server.Maps;
using Intersect.Utilities;

namespace Intersect.Server.Entities.Combat
{
    public class AreaEffectInstance
    {
        public Guid Id = Guid.NewGuid();
        public Entity Owner;
        public SpellBase Spell;
        public Guid MapId;
        public Guid MapInstanceId;
        public byte X, Y, Z;
        public long EndTime;
        private AreaShape Shape;
        private Direction Direction; // útil para Line y Cone

        private HashSet<Guid> AlreadyHit = new();

        public AreaEffectInstance(Entity owner, SpellBase spell, byte x, byte y, byte z, AreaShape shape = AreaShape.Circle, Direction direction = Direction.None)
        {
            Owner = owner;
            Spell = spell;
            MapId = owner.MapId;
            MapInstanceId = owner.MapInstanceId;
            X = x;
            Y = y;
            Z = z;

            Shape = shape;
            Direction = direction;

            EndTime = Timing.Global.Milliseconds + spell.Combat.AoeDuration;
        }


        public void Update()
        {
            if (Timing.Global.Milliseconds > EndTime)
            {
                if (MapController.TryGetInstanceFromMap(MapId, MapInstanceId, out var instance))
                {
                    instance.RemoveAreaEffect(this);
                }
                return;
            }

            if (MapController.TryGetInstanceFromMap(MapId, MapInstanceId, out var map))
            {
                var targets = map.GetEntities();
                foreach (var entity in targets)
                {
                    if (entity.IsDead() || AlreadyHit.Contains(entity.Id) || entity.Z != Z)
                        continue;

                    if (!IsInShape(entity.X, entity.Y))
                        continue;

                    if (!Owner.CanAttack(entity, Spell))
                        continue;

                    Owner.TryAttack(entity, Spell);
                    AlreadyHit.Add(entity.Id);
                }

            }
        }

        private bool IsInShape(int tx, int ty)
        {
            var dx = tx - X;
            var dy = ty - Y;
            var absDx = Math.Abs(dx);
            var absDy = Math.Abs(dy);

            switch (Shape)
            {
                case AreaShape.Circle:
                    return Globals.GetDistance(X, Y, tx, ty) <= Spell.Combat.HitRadius;

                case AreaShape.Square:
                    return absDx <= Spell.Combat.HitRadius && absDy <= Spell.Combat.HitRadius;

                case AreaShape.Cross:
                    return (absDx == 0 && absDy <= Spell.Combat.HitRadius) ||
                           (absDy == 0 && absDx <= Spell.Combat.HitRadius);

                case AreaShape.Line:
                    return Direction switch
                    {
                        Direction.Up => dx == 0 && dy < 0 && absDy <= Spell.Combat.HitRadius,
                        Direction.Down => dx == 0 && dy > 0 && dy <= Spell.Combat.HitRadius,
                        Direction.Left => dy == 0 && dx < 0 && absDx <= Spell.Combat.HitRadius,
                        Direction.Right => dy == 0 && dx > 0 && dx <= Spell.Combat.HitRadius,
                        _ => false
                    };

                case AreaShape.Cone:
                    return Direction switch
                    {
                        Direction.Up => dy <= 0 && absDy <= Spell.Combat.HitRadius && absDx <= absDy,
                        Direction.Down => dy >= 0 && dy <= Spell.Combat.HitRadius && absDx <= dy,
                        Direction.Left => dx <= 0 && absDx <= Spell.Combat.HitRadius && absDy <= absDx,
                        Direction.Right => dx >= 0 && dx <= Spell.Combat.HitRadius && absDy <= dx,
                        _ => false
                    };

                default:
                    return false;
            }
        }

    }
}
