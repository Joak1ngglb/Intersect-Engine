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
            }
            else
            {
                Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Resource, mMySprite);
            }

            mHasRenderBounds = false;
        }
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

        var map = Maps.MapInstance.Get(MapId);
        LatestMap = map;
        if (map == null || !map.InView())
        {
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
            renderList.Add(this);
        }

        return renderList;
    }

    private void CalculateRenderBounds()
    {
        var map = MapInstance;
        if (map == null || BaseResource == default)
        {
            return;
        }

        if (Texture != null)
        {
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
            mDestRectangle.Y = (int)(map.Y + Y * Options.TileHeight + OffsetY);
            mDestRectangle.X = (int)(map.X + X * Options.TileWidth + OffsetX);
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
