using System;
using System.Collections.Generic;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.Layout;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.General;
using Intersect.Client.Core;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using MonoMod.Core.Utils;


namespace Intersect.Client.Interface.Game.Market
{
    public class MarketWindow
    {
        public static MarketWindow Instance;

        private WindowControl mMarketWindow;
        private ScrollControl mListingScroll;
        private Label mTitle;
        private List<MarketItem> mCurrentItems = new();

        private TextBox mSearchBox;
        private TextBoxNumeric mMinPriceBox;
        private TextBoxNumeric mMaxPriceBox;
        private ComboBox mItemTypeCombo;
        private Button mSearchButton;
        private Button mSellButton;
        private Label mNoResultsLabel;
        private Label mMaxLabel;
        private Label mMinLabel;
        private Label mNameLabel;
        private Label mTypeLabel;
        private Label mHeaderNameLabel;
        private Label mHeaderQuantityLabel;
        private Label mHeaderPriceLabel;
        private ImagePanel mListContainer;
        private bool Initialized=false;
        private MarketItem marketItem;

        public MarketWindow(Canvas parent)
        {
            Instance = this;
            mMarketWindow = new WindowControl(parent, "📦 Mercado Global", false, "MarketWindow");
            mMarketWindow.SetSize(800, 600);
            mMarketWindow.DisableResizing();
            mMarketWindow.Focus();

            mNameLabel = new Label(mMarketWindow, "MarketNameLabel");
            mNameLabel.Text = "Nombre del ítem:";
            mNameLabel.SetBounds(20, 45, 140, 20);

            mSearchBox = new TextBox(mMarketWindow, "MarketSearchBox");
           
            mSearchBox.Focus();
            Interface.FocusElements.Add(mSearchBox);
            mTypeLabel = new Label(mMarketWindow, "MarketTypeLabel");
            mTypeLabel.Text = "Tipo:";
            

            mItemTypeCombo = new ComboBox(mMarketWindow, "MarketItemTypeCombo");
           
            mItemTypeCombo.AddItem("All", "all", "all");
            mItemTypeCombo.SelectByUserData("all");
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (type != ItemType.Currency)
                {
                    mItemTypeCombo.AddItem(type.ToString(), type.ToString(),type.ToString());
                }
            }

            mMinLabel = new Label(mMarketWindow, "MarketMinLabel");
            mMinLabel.Text = "Precio Mínimo:";
          

            mMinPriceBox = new TextBoxNumeric(mMarketWindow, "MarketMinPriceBox");
         
            mMinPriceBox.SetText("", false);

            mMaxLabel = new Label(mMarketWindow, "MarketMaxLabel");
            mMaxLabel.Text = "Precio Máximo:";
           

            mMaxPriceBox = new TextBoxNumeric(mMarketWindow, "MarketMaxPriceBox");
          
            mMaxPriceBox.SetText("", false);

            mSearchButton = new Button(mMarketWindow, "MarketSearchButton");
          
            mSearchButton.SetText("Buscar");
            mSearchButton.Clicked += (s, a) => SendSearch();

            mSellButton = new Button(mMarketWindow, "MarketSellButton");
           
            mSellButton.SetText("Vender");
            mSellButton.Clicked += SellMarket_Clicked;
            // Fila 1
            mNameLabel.SetBounds(20, 40, 140, 20);
            mSearchBox.SetBounds(160, 40, 160, 25);

            mTypeLabel.SetBounds(340, 40, 50, 20);
            mItemTypeCombo.SetBounds(390, 40, 160, 25);

            // Fila 2
            mMinLabel.SetBounds(20, 75, 120, 20);
            mMinPriceBox.SetBounds(140, 75, 80, 25);

            mMaxLabel.SetBounds(230, 75, 120, 20);
            mMaxPriceBox.SetBounds(350, 75, 80, 25);

            mSearchButton.SetBounds(450, 75, 100, 30);
            mSellButton.SetBounds(560, 75, 100, 30);
            // 📦 Contenedor general del listado (incluye encabezados + scroll)
            mListContainer = new ImagePanel(mMarketWindow, "MarketContainer");
            mListContainer.SetBounds(20, 115, 760, 450); // Estira casi hasta el fondo


            // 🧾 Encabezados dentro del contenedor
            mHeaderNameLabel = new Label(mListContainer, "MarketHeaderName");
            mHeaderNameLabel.Text = "Nombre del ítem";
            mHeaderNameLabel.SetBounds(10, 0, 200, 20);

            mHeaderQuantityLabel = new Label(mListContainer, "MarketHeaderQuantity");
            mHeaderQuantityLabel.Text = "Cantidad";
            mHeaderQuantityLabel.SetBounds(250, 0, 100, 20);

            mHeaderPriceLabel = new Label(mListContainer, "MarketHeaderPrice");
            mHeaderPriceLabel.Text = "Precio";
            mHeaderPriceLabel.SetBounds(360, 0, 100, 20);

        
            // ❌ Mensaje si no hay resultados (fuera del contenedor)
            mNoResultsLabel = new Label(mMarketWindow);
            mNoResultsLabel.SetText("❌ No se encontraron resultados.");
            mNoResultsLabel.SetBounds(250, 300, 300, 30);

            mNoResultsLabel.Hide(); // Oculto por defecto
            InitScrollPanel();

            mMarketWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
      
         
            PacketSender.SendSearchMarket();
            SendSearch(); // Ejecutar búsqueda inicial
           
        }

        public void InitScrollPanel()
        {
            // 🔽 Scroll dentro del contenedor
            mListingScroll = new ScrollControl(mListContainer, "MarketListingScroll");
            mListingScroll.GetVerticalScrollBar();
            mListingScroll.EnableScroll(false, true);
            mListingScroll.SetBounds(0, 25, 760, 425); // Más alto, acorde al nuevo mListContainer

            mListingScroll.Show(); // Forzar creación visual

        }
        private SellMarketWindow mSellWindow;

        private void SellMarket_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (mSellWindow != null && mSellWindow.IsVisible())
            {
               return;
            }

            if (mMarketWindow.Parent is Canvas parentCanvas)
            {
                mSellWindow = new SellMarketWindow(parentCanvas);
                mSellWindow.Show();
                mSellWindow.Update();
            }
        }

        public void SendSearch()
        {
            var name = mSearchBox.Text?.Trim() ?? "";

            int? minPrice = null;
            if (!string.IsNullOrWhiteSpace(mMinPriceBox.Text) && int.TryParse(mMinPriceBox.Text, out var minVal))
            {
                minPrice = minVal;
            }

            int? maxPrice = null;
            if (!string.IsNullOrWhiteSpace(mMaxPriceBox.Text) && int.TryParse(mMaxPriceBox.Text, out var maxVal))
            {
                maxPrice = maxVal;
            }
            ItemType? type = null;
            if (mItemTypeCombo.SelectedItem?.UserData?.ToString() != "all")
            {
                if (Enum.TryParse<ItemType>(mItemTypeCombo.SelectedItem.UserData.ToString(), out var parsed))
                {
                    type = parsed;
                }
            }

            PacketSender.SendSearchMarket(name, minPrice, maxPrice, type);
        }

        public void UpdateListings(List<MarketListingPacket> listings)
        {
            mListingScroll.DeleteAllChildren();
            mCurrentItems.Clear();

            // 🔽 Scroll dentro del contenedor
            mListingScroll = new ScrollControl(mListContainer, "MarketListingScroll");
            mListingScroll.GetVerticalScrollBar();
            mListingScroll.EnableScroll(false, true);
            mListingScroll.SetBounds(0, 25, 760, 425); // Más alto, acorde al nuevo mListContainer

            mListingScroll.Show(); // Forzar creación visual

           
            int offsetY = 0;
            const int itemHeight = 44;

            if (listings.Count == 0)
            {
                mNoResultsLabel.Show();
                return;
            }

            mNoResultsLabel.Hide();

            foreach (var listing in listings)
            {
                var marketItem = new MarketItem(this, listing);
                marketItem.Container = new ImagePanel(mListingScroll, "MarketItemRow");
                marketItem.Setup();
                marketItem.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
                marketItem.Container.SetBounds(0, offsetY, 750, itemHeight);
                marketItem.Container.Show();

                //offsetY += itemHeight;
                mCurrentItems.Add(marketItem);
            }

            for (int i = 0; i < listings.Count; i++)
            {
                mCurrentItems[i].Update(listings[i]);
                mCurrentItems[i].Container.SetBounds(0, offsetY, 750, itemHeight);
                mCurrentItems[i].Container.Show();
                offsetY += itemHeight;
            }
            // mListingScroll.SetInnerSize(700, offsetY);
            mListingScroll.UpdateScrollBars();
        }

        public void RefreshAfterPurchase()
        {
            SendSearch();
        }

        public void UpdateTransactionHistory(List<MarketTransactionPacket> transactions)
        {
            // Futuro: historial de transacciones
        }

        public void Close() => mMarketWindow?.Close();
        public void Show()
        {
            mMarketWindow?.Show();
        }
    }
}
