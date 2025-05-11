using System;
using System.Collections.Generic;
using Intersect.Client.Framework.Graphics;
using Intersect.Enums;

using System.Drawing;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.General;
using Intersect.Client.Core;
using Intersect.Client.Framework.Content;
using Intersect.Client.Networking;
using Intersect.Utilities;

namespace Intersect.Client.Entities.Combat
{
    public static class SpellPreviewManager
    {
        private static bool _active;
        private static AreaShape _shape;
        private static int _radius;
        private static Direction _direction;
        private static int _centerX;
        private static int _centerY;
        private static long _previewEndTime = 0;

        private static readonly List<Point> _tiles = new();

        public static bool IsPreviewActive => _active;

        public static void ShowPreview(AreaShape shape, int radius, int centerX, int centerY, Direction direction, long durationMs)
        {
            _active = true;
            _shape = shape;
            _radius = radius;
            _centerX = centerX;
            _centerY = centerY;
            _direction = direction;
            _previewEndTime = Timing.Global.Milliseconds + durationMs;

            _tiles.Clear();
            _tiles.AddRange(GetAoEArea(centerX, centerY, shape, radius, direction));
        }


        public static void ClearPreview()
        {
            _active = false;
            _tiles.Clear();
        }

        private static GameTexture _previewTexture;

        public static void Draw()
        {
            if (!_active || _tiles.Count == 0 || Globals.Me?.MapInstance == null)
                return;

            if (Timing.Global.Milliseconds > _previewEndTime)
            {
                ClearPreview();
                return;
            }

            if (_previewTexture == null)
            {
                _previewTexture = Globals.ContentManager.GetTexture(TextureType.Misc, "spellpreview.png");
            }

            var map = Globals.Me.MapInstance;
            foreach (var tile in _tiles)
            {
                var x = map.X + tile.X * Options.TileWidth;
                var y = map.Y + tile.Y * Options.TileHeight;

                Graphics.DrawGameTexture(
                    _previewTexture,
                    new FloatRect(0, 0, _previewTexture.Width, _previewTexture.Height),
                    new FloatRect(x, y, Options.TileWidth, Options.TileHeight),
                    Color.White
                );
            }
        }

        private static List<Point> GetAoEArea(int centerX, int centerY, AreaShape shape, int range, Direction dir)
        {
            var points = new List<Point>();

            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = -range; dy <= range; dy++)
                {
                    int tx = centerX + dx;
                    int ty = centerY + dy;

                    if (dx == 0 && dy == 0)
                        continue;

                    switch (shape)
                    {
                        case AreaShape.Circle:
                            if (dx * dx + dy * dy <= range * range)
                                points.Add(new Point(tx, ty));
                            break;

                        case AreaShape.Square:
                            points.Add(new Point(tx, ty));
                            break;

                        case AreaShape.Cross:
                            if (dx == 0 || dy == 0)
                                points.Add(new Point(tx, ty));
                            break;

                        case AreaShape.Line:
                            if ((dir == Direction.Up && dx == 0 && dy < 0) ||
                                (dir == Direction.Down && dx == 0 && dy > 0) ||
                                (dir == Direction.Left && dy == 0 && dx < 0) ||
                                (dir == Direction.Right && dy == 0 && dx > 0))
                                points.Add(new Point(tx, ty));
                            break;

                        case AreaShape.Cone:
                            {
                                var distance = Math.Sqrt(dx * dx + dy * dy);
                                if (distance > range)
                                    continue;

                                var angle = (Math.Atan2(dy, dx) * 180 / Math.PI + 360) % 360;

                                int startAngle, endAngle;
                                switch (dir)
                                {
                                    case Direction.Up: startAngle = 225; endAngle = 315; break;
                                    case Direction.Down: startAngle = 45; endAngle = 135; break;
                                    case Direction.Left: startAngle = 135; endAngle = 225; break;
                                    case Direction.Right: startAngle = 315; endAngle = 45; break;
                                    case Direction.UpLeft: startAngle = 180; endAngle = 270; break;
                                    case Direction.UpRight: startAngle = 270; endAngle = 360; break;
                                    case Direction.DownLeft: startAngle = 90; endAngle = 180; break;
                                    case Direction.DownRight: startAngle = 0; endAngle = 90; break;
                                    default: continue;
                                }

                                bool inCone = startAngle < endAngle
                                    ? angle >= startAngle && angle <= endAngle
                                    : angle >= startAngle || angle <= endAngle;

                                if (inCone)
                                    points.Add(new Point(tx, ty));

                                break;
                            }
                    }
                }
            }

            return points;
        }
    }
}
