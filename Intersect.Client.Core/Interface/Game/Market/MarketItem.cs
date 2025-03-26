using System;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Interface.Game.Market
{
    public class MarketItem
    {
        public ImagePanel Container;
        public ImagePanel Pnl;

        private Label mNameLabel;
        private Label mQuantityLabel;
        private Label mPriceLabel;
        private Button mBuyButton;

        private MarketWindow mMarketWindow;
        private MarketListingPacket mListing;
        private ItemDescriptionWindow mDescWindow;

        public MarketItem(MarketWindow marketWindow, MarketListingPacket listing)
        {
            mMarketWindow = marketWindow;
            mListing = listing;
        }

        public void Setup()
        {
            Container = new ImagePanel(null, "MarketItemRow");
            Container.SetBounds(0, 0, 760, 40);

            Pnl = new ImagePanel(Container, "MarketItemIcon");
            Pnl.SetBounds(5, 4, 32, 32);

            var itemData = ItemBase.Get(mListing.ItemId);
            if (itemData != null)
            {
                Pnl.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, itemData.Icon);
            }

            Pnl.HoverEnter += Pnl_HoverEnter;
            Pnl.HoverLeave += Pnl_HoverLeave;
            Pnl.DoubleClicked += Pnl_DoubleClicked;

            mNameLabel = new Label(Container, "MarketItemName");
            mNameLabel.Text = itemData?.Name ?? "???";
            mNameLabel.SetBounds(42, 5, 200, 30);

            mQuantityLabel = new Label(Container, "MarketItemQuantity");
            mQuantityLabel.Text = $"x{mListing.Quantity}";
            mQuantityLabel.SetBounds(250, 5, 50, 30);

            mPriceLabel = new Label(Container, "MarketItemPrice");
            mPriceLabel.Text = $"{mListing.Price} 🪙";
            mPriceLabel.SetBounds(310, 5, 100, 30);

            mBuyButton = new Button(Container, "BuyMarketItemButton");
            mBuyButton.SetText("🛒 Comprar");
            mBuyButton.SetBounds(620, 5, 100, 30);
            mBuyButton.Clicked += OnBuyClicked;
        }

        private void OnBuyClicked(Base sender, ClickedEventArgs args)
        {
            var itemData = ItemBase.Get(mListing.ItemId);
            var confirmation = new MessageBox(
                Container?.Parent ?? Container,
                $"¿Comprar {itemData?.Name} por {mListing.Price} 🪙?",
                "Confirmar Compra"
            );

            confirmation.Dismissed += (s, e) =>
            {
                PacketSender.SendBuyMarketListing(mListing.ListingId);
            };

            confirmation.Show();
        }

        private void Pnl_HoverEnter(Base sender, EventArgs args)
        {
            var itemData = ItemBase.Get(mListing.ItemId);
            if (itemData != null && mDescWindow == null)
            {
                mDescWindow = new ItemDescriptionWindow(itemData, mListing.Quantity, 0, 0, mListing.Properties);
            }
        }

        private void Pnl_HoverLeave(Base sender, EventArgs args)
        {
            mDescWindow?.Dispose();
            mDescWindow = null;
        }

        private void Pnl_DoubleClicked(Base sender, ClickedEventArgs args)
        {
            PacketSender.SendBuyMarketListing(mListing.ListingId);
        }

        public void UpdateItem(MarketListingPacket listing)
        {
            mListing = listing;

            var itemData = ItemBase.Get(mListing.ItemId);
            mNameLabel.Text = itemData?.Name ?? "???";
            mQuantityLabel.Text = $"x{mListing.Quantity}";
            mPriceLabel.Text = $"{mListing.Price} 🪙";

            if (itemData != null)
            {
                Pnl.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, itemData.Icon);
            }
        }
    }
}
