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
    private const byte BEHIND_ALPHA = 120;
    private const byte NORMAL_ALPHA = 255;
    private bool _playerBehind;

    public ResourceBase? BaseResource { get; set; }

    bool IResource.IsDepleted => IsDead;

    public bool IsDead { get; set; }

    FloatRect mDestRectangle = FloatRect.Empty;

    private bool mHasRenderBounds;

    FloatRect mSrcRectangle = FloatRect.Empty;

    public Resource(Guid id, EntityPacket packet) : base(id, packet, EntityType.Resource)
    {
        mRenderPriority = 1;
    }

    public override string Sprite
    {
        get => mMySprite;
        set
        {
            if (value == mMySprite)
            {
                return;
            }

            if (BaseResource == null)
            {
                return;
            }

            mMySprite = value;
            if (IsDead && BaseResource.Exhausted.GraphicFromTileset ||
                !IsDead && BaseResource.Initial.GraphicFromTileset)
            {
                if (GameContentManager.Current.TilesetsLoaded)
                {
                    Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Tileset, mMySprite);
                }
                else
                {
                    _waitingForTilesets = true;
                }
            }
            else
            {
                Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Resource, mMySprite);
            }

        mHasRenderBounds = false;
    }

    public override void Load(EntityPacket? packet)
    {
        base.Load(packet);
        var pkt = packet as ResourceEntityPacket;

        if (pkt == default)
        {
            return;
        }

        IsDead = pkt.IsDead;
        var baseId = pkt.ResourceId;
        BaseResource = ResourceBase.Get(baseId);

        if (BaseResource == default)
        {
            return;
        }

        HideName = true;
        if (IsDead)
        {
            Sprite = BaseResource.Exhausted.Graphic;
        }
        else
        {
            Sprite = BaseResource.Initial.Graphic;
        }
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

        if (!Maps.MapInstance.TryGet(MapId, out var map) || !map.InView())
        {
            LatestMap = map;
            Globals.EntitiesToDispose.Add(Id);

            return false;
        }

        if (!mHasRenderBounds)
        {
            CalculateRenderBounds();
        }

        if (!Graphics.WorldViewport.IntersectsWith(mDestRectangle))
        {
            if (RenderList != null)
            {
                _ = RenderList.Remove(this);
            }
            return true;
        }

        var result = base.Update();
        if (!result && RenderList != null)
        {
            _ = RenderList.Remove(this);
        }

        return result;
    }

    public override bool CanBeAttacked => !IsDead;

    public override HashSet<Entity> DetermineRenderOrder(HashSet<Entity> renderList, IMapInstance map)
    {
        base.DetermineRenderOrder(renderList, map);
        
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

        return renderList;
    }

    private void CalculateRenderBounds()
    {
        if (BaseResource == default)
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

        mSrcRectangle.X = 0;
        mSrcRectangle.Y = 0;
        if (IsDead && BaseResource.Exhausted.GraphicFromTileset)
        {
            mSrcRectangle.X = BaseResource.Exhausted.X * Options.TileWidth;
            mSrcRectangle.Y = BaseResource.Exhausted.Y * Options.TileHeight;
            mSrcRectangle.Width = (BaseResource.Exhausted.Width + 1) * Options.TileWidth;
            mSrcRectangle.Height = (BaseResource.Exhausted.Height + 1) * Options.TileHeight;
        }
        else if (!IsDead && BaseResource.Initial.GraphicFromTileset)
        {
            mSrcRectangle.X = BaseResource.Initial.X * Options.TileWidth;
            mSrcRectangle.Y = BaseResource.Initial.Y * Options.TileHeight;
            mSrcRectangle.Width = (BaseResource.Initial.Width + 1) * Options.TileWidth;
            mSrcRectangle.Height = (BaseResource.Initial.Height + 1) * Options.TileHeight;
        }
        else
        {
            mSrcRectangle.Width = Texture.Width;
            mSrcRectangle.Height = Texture.Height;
        }

        mDestRectangle.Width = mSrcRectangle.Width;
        mDestRectangle.Height = mSrcRectangle.Height;
        mDestRectangle.Y = (int) (map.Y + Y * Options.TileHeight + OffsetY);
        mDestRectangle.X = (int) (map.X + X * Options.TileWidth + OffsetX);
        if (mSrcRectangle.Height > Options.TileHeight)
        {
            mDestRectangle.Y -= mSrcRectangle.Height - Options.TileHeight;
        }

        if (mSrcRectangle.Width > Options.TileWidth)
        {
            mDestRectangle.X -= (mSrcRectangle.Width - Options.TileWidth) / 2;
        }

        mHasRenderBounds = true;
    }

    public override void Draw()
    {
        if (MapInstance == null || Texture == null)
        {
            return;
        }

        if (!mHasRenderBounds)
        {
            CalculateRenderBounds();
        }

        try
        {
            _playerBehind = IsPlayerBehindResource();
            
            var alpha = _playerBehind ? BEHIND_ALPHA : NORMAL_ALPHA;
            var renderColor = new Color(alpha, 255, 255, 255);

            Graphics.DrawGameTexture(
                Texture,
                mSrcRectangle,
                mDestRectangle,
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
            return false;
        }

        return IsNear(player, 1) && player.IsBehind(this);
    }
}
