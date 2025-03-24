using System;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Client.Interface.Game.AuctionHouse
{
    public class SellAuctionItem
    {
        public ImagePanel Container;
        public ImagePanel Pnl;
        public int Slot => mSlot;

        private int mSlot;
        private Guid mItemId;
        private AuctionHouseWindow mAuctionHouseWindow;
        private ItemDescriptionWindow mDescWindow;
        private long mClickTime;
        private bool mMouseOver;
        private int mMouseX = -1;
        private int mMouseY = -1;
        private Guid mCurrentItemId;
        private int mCurrentAmt;
        private bool mIconCd;
        private string mTexLoaded = "";
        private Label mCooldownLabel;

        public SellAuctionItem(AuctionHouseWindow auctionHouseWindow, int slot)
        {
            mAuctionHouseWindow = auctionHouseWindow;
            mSlot = slot;

            // Inicializar el contenedor
            Container = new ImagePanel(auctionHouseWindow.RootWindow, "SellAuctionItem");
            Container.SetSize(50, 50);
        }

        public void Setup()
        {
            Pnl = new ImagePanel(Container, "SellAuctionItemIcon");
            Pnl.SetSize(32, 32);
            Pnl.SetPosition(0, 0);

            mCooldownLabel = new Label(Container, "SellAuctionItemCooldownLabel");
            mCooldownLabel.IsHidden = true;
            mCooldownLabel.TextColor = new Color(0, 255, 255, 255);

            Pnl.Clicked += pnl_Clicked;
            Pnl.RightClicked += pnl_RightClicked;
            Pnl.HoverEnter += pnl_HoverEnter;
            Pnl.HoverLeave += pnl_HoverLeave;
        }

        private void pnl_Clicked(Base sender, ClickedEventArgs arguments)
        {
            mClickTime = Timing.Global.MillisecondsUtc + 500;
            var itemSlot = Globals.Me.Inventory[mSlot];
            if (itemSlot != null)
            {
                mItemId = itemSlot.ItemId;
                mAuctionHouseWindow.SelectItemForSale(mSlot, mItemId);
            }
        }

        private void pnl_RightClicked(Base sender, ClickedEventArgs arguments)
        {
            var itemSlot = Globals.Me.Inventory[mSlot];
            if (itemSlot != null)
            {
                mItemId = itemSlot.ItemId;
                mAuctionHouseWindow.SelectItemForSale(mSlot, mItemId);
            }
        }

        private void pnl_HoverLeave(Base sender, EventArgs arguments)
        {
            mMouseOver = false;
            mMouseX = -1;
            mMouseY = -1;
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
        }

        private void pnl_HoverEnter(Base sender, EventArgs arguments)
        {
            if (InputHandler.MouseFocus != null) return;
            mMouseOver = true;

            if (Globals.InputManager.MouseButtonDown(MouseButtons.Left))
            {
                return;
            }

            var itemSlot = Globals.Me.Inventory[mSlot];
            if (itemSlot == null || itemSlot.ItemId == Guid.Empty)
                return;

            mItemId = itemSlot.ItemId;
            var itemData = ItemBase.Get(mItemId);

            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }

            if (itemData != null)
            {
                int posX = mAuctionHouseWindow.RootWindow.X;
                int posY = mAuctionHouseWindow.RootWindow.Y;
                mDescWindow = new ItemDescriptionWindow(
                    itemData,
                    itemSlot.Quantity,
                    posX,
                    posY,
                    itemSlot.ItemProperties
                );
            }
        }

        public FloatRect RenderBounds()
        {
            var rect = new FloatRect()
            {
                X = Pnl.LocalPosToCanvas(new Point(0, 0)).X,
                Y = Pnl.LocalPosToCanvas(new Point(0, 0)).Y,
                Width = Pnl.Width,
                Height = Pnl.Height
            };

            return rect;
        }

        public void Update()
        {
            var slotData = Globals.Me.Inventory[mSlot];
            if (slotData == null) return;

            var item = ItemBase.Get(slotData.ItemId);
            if (item != null)
            {
                // Aplicar color de rareza
                Container.RenderColor = CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor)
                    ? rarityColor
                    : Color.White;
            }

            // Verificar si hay cambios en el ítem o cooldown antes de actualizar
            if (slotData.ItemId != mCurrentItemId ||
                slotData.Quantity != mCurrentAmt ||
                (item != null && mTexLoaded != item.Icon) ||
                mIconCd != Globals.Me.IsItemOnCooldown(mSlot))
            {
                mCurrentItemId = slotData.ItemId;
                mCurrentAmt = slotData.Quantity;
                mIconCd = Globals.Me.IsItemOnCooldown(mSlot);

                if (item != null)
                {
                    var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
                    if (itemTex != null)
                    {
                        Pnl.Texture = itemTex;
                        Pnl.RenderColor = mIconCd ? new Color(100, item.Color.R, item.Color.G, item.Color.B) : item.Color;
                    }
                    else
                    {
                        Pnl.Texture = null;
                    }

                    mTexLoaded = item.Icon;

                    if (mIconCd)
                    {
                        var remaining = Globals.Me.GetItemRemainingCooldown(mSlot);
                        mCooldownLabel.IsHidden = false;
                        mCooldownLabel.Text = TimeSpan.FromMilliseconds(remaining).WithSuffix("0.0");
                    }
                    else
                    {
                        mCooldownLabel.IsHidden = true;
                    }
                }
                else
                {
                    Pnl.Texture = null;
                    mTexLoaded = "";
                }

                // Si se mostraba una descripción, actualizarla
                if (mDescWindow != null)
                {
                    mDescWindow.Dispose();
                    mDescWindow = null;
                    pnl_HoverEnter(null, null);
                }
            }
        }
    }
}
