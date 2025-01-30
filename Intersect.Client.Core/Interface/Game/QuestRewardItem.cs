
using Intersect.GameObjects;
using System.ComponentModel;

using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game.Quest;

public partial class QuestRewardItem
{
    public ImagePanel Container;

    private IQuestWindow mQuestWindow;
    public Guid mCurrentItemId;
    private ItemDescriptionWindow mDescWindow;
    private int mItemQuantity;

    private ImagePanel Pnl;

    public QuestRewardItem(IQuestWindow questWindow, Guid itemId, int itemQuantity)
    {
        mQuestWindow = questWindow;
        mCurrentItemId = itemId;
        mItemQuantity = itemQuantity;
    }

    public void Setup()
    {
        Pnl = new ImagePanel(Container, "RewardItemIcon");
        Pnl.HoverEnter += pnl_HoverEnter;
        Pnl.HoverLeave += pnl_HoverLeave;
    }

    private void pnl_HoverLeave(Base sender, EventArgs arguments)
    {
        if (mDescWindow != null)
        {
            mDescWindow.Dispose();
            mDescWindow = null;
        }
    }

    private void pnl_HoverEnter(Base sender, EventArgs arguments)
    {
        if (InputHandler.MouseFocus != null)
        {
            return;
        }

        if (Globals.InputManager.MouseButtonDown(MouseButtons.Left))
        {
            return;
        }

        if (mDescWindow != null)
        {
            mDescWindow.Dispose();
            mDescWindow = null;
        }

        var item = ItemBase.Get(mCurrentItemId);
        if (item != null)
        {
            mDescWindow = new ItemDescriptionWindow(
                item,
                mItemQuantity,
                mQuestWindow.X,
                mQuestWindow.Y,
                null
            );
        }
    }

    public void Update()
    {
        var item = ItemBase.Get(mCurrentItemId);
        if (item != null)
        {
            var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
            if (itemTex != null)
            {
                Pnl.Texture = itemTex;
                Pnl.RenderColor = item.Color;
            }
            else
            {
                if (Pnl.Texture != null)
                {
                    Pnl.Texture = null;
                }
            }
        }
        else
        {
            if (Pnl.Texture != null)
            {
                Pnl.Texture = null;
            }
        }
    }
}
