using System;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Interface.Game.Market;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Client.Networking;
using Intersect.Client.General;
using Intersect;

public class MarketItem
{
    public ImagePanel Container;

    private ImagePanel mIconPanel;
    private Label mNameLabel;
    private Label mQuantityLabel;
    private Label mPriceLabel;
    private Button mBuyButton;

    private MarketWindow mMarketWindow;
    private MarketListingPacket mListing;
    private ItemDescriptionWindow mDescWindow;
    private ItemBase mItemBase;
    private Button cancelButton;

    public MarketItem(MarketWindow marketWindow, MarketListingPacket listing)
    {
        mMarketWindow = marketWindow;
        mListing = listing;
    }

    public void Setup()
    {
        mItemBase = ItemBase.Get(mListing.ItemId);

        mIconPanel = new ImagePanel(Container, "MarketItemIcon");
        mIconPanel.SetBounds(5, 4, 32, 32);

        if (mItemBase != null)
        {
            mIconPanel.Texture = Globals.ContentManager.GetTexture(Intersect.Client.Framework.Content.TextureType.Item, mItemBase.Icon);
            mIconPanel.RenderColor = mItemBase.Color;
        }

        mIconPanel.HoverEnter += OnHoverEnter;
        mIconPanel.HoverLeave += OnHoverLeave;
        mIconPanel.Clicked += OnClick;

        mNameLabel = new Label(Container, "MarketItemName")
        {
            Text = mItemBase?.Name ?? "???"
        };
        mNameLabel.SetBounds(42, 5, 200, 30);

        mQuantityLabel = new Label(Container, "MarketItemQuantity")
        {
            Text = $"x{mListing.Quantity}"
        };
        mQuantityLabel.SetBounds(250, 5, 50, 30);

        mPriceLabel = new Label(Container, "MarketItemPrice")
        {
            Text = $"{mListing.Price} 🪙"
        };
        mPriceLabel.SetBounds(310, 5, 100, 30);

        mBuyButton = new Button(Container, "BuyMarketItemButton");
        mBuyButton.SetText("🛒 Comprar");
        mBuyButton.SetBounds(500, 5, 100, 30);
        mBuyButton.Clicked += OnBuyClick;
        if (mListing.SellerName == Globals.Me.Name)
        {
            mBuyButton.IsHidden = true;

            cancelButton = new Button(Container, "CancelMarketItemButton");
            cancelButton.SetText("❌ Cancelar");
            cancelButton.SetBounds(500, 5, 100, 30);
            cancelButton.Clicked += OnCancelClick;
        }

    }
    private void OnCancelClick(Base sender, ClickedEventArgs args)
    {
        PacketSender.SendCancelMarketListing(mListing.ListingId);
    }

    private void OnBuyClick(Base sender, ClickedEventArgs args)
    {
        if (mItemBase == null) return;

        // Cadena sin etiquetas HTML, sólo un salto de línea
        var mensaje = $"¿Estás seguro de que quieres comprar {mItemBase.Name} x{mListing.Quantity}\npor {mListing.Price} Boins?";

        var confirmation = new ConfirmBox(
            Container?.Parent ?? Container,
            mensaje,
            "🛒 Confirmar Compra"
        );

        confirmation.SetSize(320, 220);

        confirmation.SetTextScale(1.1f);

        confirmation.Confirmed += (ctrl, evt) =>
        {
            if (evt.Result == ConfirmBox.ConfirmResult.Yes)
            {
                PacketSender.SendBuyMarketListing(mListing.ListingId);
            }
            // si es No o Closed, no hacemos nada
        };

        confirmation.Show();
    }



    private void OnHoverEnter(Base sender, EventArgs args)
    {
        if (mItemBase != null && mDescWindow == null)
        {
            mDescWindow = new ItemDescriptionWindow(mItemBase, mListing.Quantity, 0, 0, mListing.Properties);
        }
    }

    private void OnHoverLeave(Base sender, EventArgs args)
    {
        mDescWindow?.Dispose();
        mDescWindow = null;
    }

    private void OnClick(Base sender, ClickedEventArgs args)
    {
        PacketSender.SendBuyMarketListing(mListing.ListingId);
    }

    public void Update(MarketListingPacket newListing)
    {
        mListing = newListing;
        mItemBase = ItemBase.Get(mListing.ItemId);

        if (mItemBase == null)
        {
            mNameLabel.Text = "???";
            mIconPanel.Texture = null;
            return;
        }

        mNameLabel.Text = mItemBase.Name;
        mQuantityLabel.Text = $"x{mListing.Quantity}";
        mPriceLabel.Text = $"{mListing.Price}";
        mIconPanel.Texture = Globals.ContentManager.GetTexture(Intersect.Client.Framework.Content.TextureType.Item, mItemBase.Icon);
        mIconPanel.RenderColor = mItemBase.Color;
    }
}
