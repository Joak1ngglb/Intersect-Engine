using Intersect.Client.Core;
using Intersect.Client.Framework.Entities;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Maps;
using Intersect.Client.General;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Client.Framework.Graphics;
using System.Diagnostics;

namespace Intersect.Client.Entities;

public partial class Resource : Entity, IResource
{
    private FloatRect _renderBoundsDest = FloatRect.Empty;
    private FloatRect _renderBoundsSrc = FloatRect.Empty;

    private bool _recalculateRenderBounds;
    private bool _waitingForTilesets;
    private const byte BEHIND_ALPHA = 120; // Nivel de transparencia cuando el jugador est� detr�s
    private const byte NORMAL_ALPHA = 255; // Nivel de transparencia normal
    private bool _playerBehind;

    private bool _isDead;
    private ResourceBase? _descriptor;

    public Resource(Guid id, ResourceEntityPacket packet) : base(id, packet, EntityType.Resource)
    {
        mRenderPriority = 0;
    }

    public ResourceBase? Descriptor
    {
        get => _descriptor;
        set => _descriptor = value;
    }

    public bool IsDepleted => IsDead;

    public bool IsDead
    {
        get => _isDead;
        set
        {
            if (value == _isDead)
            {
                return;
            }

            _isDead = value;
            _recalculateRenderBounds = true;
        }
    }

    public override string Sprite
    {
        get => _sprite;
        set
        {
            if (value == _sprite)
            {
                return;
            }

            if (Descriptor == null)
            {
                return;
            }

            _sprite = value;
            ReloadSpriteTexture();
        }
    }

    private void ReloadSpriteTexture()
    {
        if (Descriptor == null)
        {
            return;
        }

        if (IsDead && Descriptor.Exhausted.GraphicFromTileset ||
            !IsDead && Descriptor.Initial.GraphicFromTileset)
        {
            if (GameContentManager.Current.TilesetsLoaded)
            {
                Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Tileset, _sprite);
            }
            else
            {
                _waitingForTilesets = true;
            }
        }
        else
        {
            Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Resource, _sprite);
        }

        _recalculateRenderBounds = true;
    }

    public override void Load(EntityPacket? packet)
    {
        base.Load(packet);

        _recalculateRenderBounds = true;

        if (packet is not ResourceEntityPacket resourceEntityPacket)
        {
            return;
        }

        IsDead = resourceEntityPacket.IsDead;

        if (!ResourceBase.TryGet(resourceEntityPacket.ResourceId, out _descriptor))
        {
            return;
        }

        UpdateFromDescriptor(_descriptor);
    }

    private void UpdateFromDescriptor(ResourceBase? descriptor)
    {
        if (descriptor == null)
        {
            return;
        }

        var updatedSprite = IsDead ? descriptor.Exhausted.Graphic : descriptor.Initial.Graphic;
        _sprite = updatedSprite;
        ReloadSpriteTexture();
    }

    public override void Dispose()
    {
        if (RenderList != null)
        {
            _ = RenderList.Remove(this);
            RenderList = null;
        }

        ClearAnimations(null);
        GC.SuppressFinalize(this);
        mDisposed = true;
    }

    public override bool Update()
    {
        if (mDisposed)
        {
            LatestMap = null;

            return false;
        }

        if (Descriptor is { IsDeleted: true } deletedDescriptor)
        {
            _ = ResourceBase.TryGet(deletedDescriptor.Id, out _descriptor);
            UpdateFromDescriptor(_descriptor);
        }

        if (!Maps.MapInstance.TryGet(MapId, out var map) || !map.InView())
        {
            LatestMap = map;
            Globals.EntitiesToDispose.Add(Id);

            return false;
        }

        if (_recalculateRenderBounds)
        {
            CalculateRenderBounds();
        }

        if (!Graphics.WorldViewport.IntersectsWith(_renderBoundsDest))
        {
            if (RenderList != null)
            {
                _ = RenderList.Remove(this);
            }

            return true;
        }

        var result = base.Update();
        if (!result)
        {
            if (RenderList != null)
            {
                _ = RenderList.Remove(this);
            }
        }

        return result;
    }

    /// <inheritdoc />
    public override bool CanBeAttacked => !IsDead;

    public override HashSet<Entity>? DetermineRenderOrder(HashSet<Entity>? renderList, IMapInstance? map)
    {
        if (Descriptor == default ||
            (IsDead && !Descriptor.Exhausted.RenderBelowEntities) ||
            (!IsDead && !Descriptor.Initial.RenderBelowEntities)
        )
        {
            return base.DetermineRenderOrder(renderList, map);
        }

        //Otherwise we are alive or dead and we want to render below players/npcs
        if (renderList != null)
        {
            _ = renderList.Remove(this);
        }

        if (map == null)
        {
            return null;
        }

        if (Globals.MapGrid == default)
        {
            return null;
        }

        if (Globals.Me?.MapInstance == null)
        {
            return null;
        }

        var gridX = Globals.Me.MapInstance.GridX;
        var gridY = Globals.Me.MapInstance.GridY;
        for (var x = gridX - 1; x <= gridX + 1; x++)
        {
            for (var y = gridY - 1; y <= gridY + 1; y++)
            {
                if (x >= 0 &&
                    x < Globals.MapGridWidth &&
                    y >= 0 &&
                    y < Globals.MapGridHeight &&
                    Globals.MapGrid[x, y] != Guid.Empty)
                {
                    if (Globals.MapGrid[x, y] == MapId)
                    {
                        var priority = mRenderPriority;
                        if (Z != 0)
                        {
                            priority += 3;
                        }

                        HashSet<Entity> renderSet;

                        if (y == gridY - 1)
                        {
                            renderSet = Graphics.RenderingEntities[priority, Y];
                        }
                        else if (y == gridY)
                        {
                            renderSet = Graphics.RenderingEntities[priority, Options.MapHeight + Y];
                        }
                        else
                        {
                            renderSet = Graphics.RenderingEntities[priority, Options.MapHeight * 2 + Y];
                        }

                        _ = renderSet.Add(this);
                        renderList = renderSet;

                        return renderList;

                    }
                }
            }
        }

        return renderList;
    }

    private void CalculateRenderBounds()
    {
        if (Descriptor == default)
        {
            return;
        }

        if (MapInstance is not { } map)
        {
            return;
        }

        if (_waitingForTilesets)
        {
            if (GameContentManager.Current.TilesetsLoaded)
            {
                ReloadSpriteTexture();
                _waitingForTilesets = false;
            }
            else
            {
                // No textures yet
                return;
            }
        }

        if (Texture == null)
        {
            return;
        }

        _renderBoundsSrc.X = 0;
        _renderBoundsSrc.Y = 0;
        if (IsDead && Descriptor.Exhausted.GraphicFromTileset)
        {
            _renderBoundsSrc.X = Descriptor.Exhausted.X * Options.TileWidth;
            _renderBoundsSrc.Y = Descriptor.Exhausted.Y * Options.TileHeight;
            _renderBoundsSrc.Width = (Descriptor.Exhausted.Width + 1) * Options.TileWidth;
            _renderBoundsSrc.Height = (Descriptor.Exhausted.Height + 1) * Options.TileHeight;
        }
        else if (!IsDead && Descriptor.Initial.GraphicFromTileset)
        {
            _renderBoundsSrc.X = Descriptor.Initial.X * Options.TileWidth;
            _renderBoundsSrc.Y = Descriptor.Initial.Y * Options.TileHeight;
            _renderBoundsSrc.Width = (Descriptor.Initial.Width + 1) * Options.TileWidth;
            _renderBoundsSrc.Height = (Descriptor.Initial.Height + 1) * Options.TileHeight;
        }
        else
        {
            _renderBoundsSrc.Width = Texture.Width;
            _renderBoundsSrc.Height = Texture.Height;
        }

        _renderBoundsDest.Width = _renderBoundsSrc.Width;
        _renderBoundsDest.Height = _renderBoundsSrc.Height;
        _renderBoundsDest.Y = (int) (map.Y + Y * Options.TileHeight + OffsetY);
        _renderBoundsDest.X = (int) (map.X + X * Options.TileWidth + OffsetX);
        if (_renderBoundsSrc.Height > Options.TileHeight)
        {
            _renderBoundsDest.Y -= _renderBoundsSrc.Height - Options.TileHeight;
        }

        if (_renderBoundsSrc.Width > Options.TileWidth)
        {
            _renderBoundsDest.X -= (_renderBoundsSrc.Width - Options.TileWidth) / 2;
        }

        _recalculateRenderBounds = false;
    }

    //Rendering Resources
    public override void Draw()
    {
        if (MapInstance == null || Texture == null)
        {
            return;
        }

        if (!_recalculateRenderBounds)
        {
            CalculateRenderBounds();
        }

        try
        {
            // Determinar si el jugador est� detr�s del recurso
            _playerBehind = IsPlayerBehindResource();

            // Ajustar la transparencia seg�n la posici�n del jugador
            var alpha = _playerBehind ? BEHIND_ALPHA : NORMAL_ALPHA;
            var renderColor = new Color(alpha, 255, 255, 255);

            // Dibujar la textura del recurso con la transparencia ajustada
            Graphics.DrawGameTexture(
                Texture,
                _renderBoundsSrc,
                _renderBoundsDest,
                renderColor
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al dibujar recurso: {ex.Message}");
        }
    }
    private bool IsPlayerBehindResource()
    {
        var player = Globals.Me;
        if (player == null || player.MapId != MapId)
        {
            Graphics.DrawGameTexture(Texture, _renderBoundsSrc, _renderBoundsDest, Intersect.Color.White);
        }

        return IsNear(player, 1) && player.IsBehind(this);
    }
}