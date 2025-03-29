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
using Intersect.Client.Interface.Game.Mail;
using Intersect.Client.Framework.Gwen.Control.EventArguments;

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
        private ListBox mItemTypeList;
        private Button mSearchButton;
        private Button mSellButton;
        private Label mNoResultsLabel;

        public MarketWindow(Canvas parent)
        {
            Instance = this;

            mMarketWindow = new WindowControl(parent, "📦 Mercado Global", false, "MarketWindow");
            mMarketWindow.SetSize(800, 600);
            mMarketWindow.DisableResizing();
            mMarketWindow.Focus();

            mTitle = new Label(mMarketWindow, "MarketTitle");
            mTitle.Text = "📦 Mercado Global";
            mTitle.SetBounds(20, 10, 400, 30);

            // 🔍 Filtros
            var nameLabel = new Label(mMarketWindow) { Text = "🔍 Nombre del ítem:" };
            nameLabel.SetBounds(20, 45, 140, 20);
            mSearchBox = new TextBox(mMarketWindow);
            mSearchBox.SetBounds(160, 45, 160, 25);

            var typeLabel = new Label(mMarketWindow) { Text = "📦 Tipo:" };
            typeLabel.SetBounds(340, 45, 50, 20);
            mItemTypeList = new ListBox(mMarketWindow);
            mItemTypeList.SetBounds(390, 45, 150, 80);
            mItemTypeList.AddRow("Todos", "all", "all");
            mItemTypeList.SelectByUserData("all");
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (type != ItemType.Currency)
                {
                    mItemTypeList.AddRow(type.ToString(), type.ToString(), type);
                }
            }

            var minLabel = new Label(mMarketWindow) { Text = "💰 Precio Mínimo:" };
            minLabel.SetBounds(20, 130, 120, 20);
            mMinPriceBox = new TextBoxNumeric(mMarketWindow);
            mMinPriceBox.SetBounds(140, 130, 80, 25);
         
            mMinPriceBox.SetText("", false);
            var maxLabel = new Label(mMarketWindow) { Text = "💰 Precio Máximo:" };
            maxLabel.SetBounds(230, 130, 120, 20);
            mMaxPriceBox = new TextBoxNumeric(mMarketWindow);
            mMaxPriceBox.SetBounds(350, 130, 80, 25);
            mMaxPriceBox.SetText("", false);

            mSearchButton = new Button(mMarketWindow);
            mSearchButton.SetBounds(450, 130, 100, 30);
            mSearchButton.SetText("🔍 Buscar");
            mSearchButton.Clicked += (s, a) => SendSearch();

            mSellButton = new Button(mMarketWindow);
            mSellButton.SetBounds(560, 130, 100, 30);
            mSellButton.SetText("📤 Vender");
            mSellButton.Clicked += SellMarket_Clicked;

            mListingScroll = new ScrollControl(mMarketWindow, "MarketListingScroll");
            mListingScroll.EnableScroll(false, true);
            mListingScroll.SetBounds(20, 180, 760, 400);
            mNoResultsLabel = new Label(mMarketWindow);
            mNoResultsLabel.SetText("❌ No se encontraron resultados.");
            mNoResultsLabel.SetBounds(220, 300, 300, 30);
            mNoResultsLabel.Hide(); // Oculto por defecto

            mMarketWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();

            PacketSender.SendSearchMarket();
        }

        private void InitItemContainer()
        {
            mListingScroll.DeleteAllChildren();
            mCurrentItems.Clear();
        }

        private void SellMarket_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (mMarketWindow.Parent is Canvas parentCanvas)
            {
                var sellWindow = new SellMarketWindow(parentCanvas);
                sellWindow.Show();
                sellWindow.Update();
            }
        }

        private void SendSearch()
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
            if (mItemTypeList.SelectedRow?.UserData?.ToString() != "all")
            {
                if (Enum.TryParse<ItemType>(mItemTypeList.SelectedRow?.UserData?.ToString(), out var parsed))
                {
                    type = parsed;
                }
            }

            PacketSender.SendSearchMarket(name, minPrice, maxPrice, type);
        }


        public void UpdateListings(List<MarketListingPacket> listings)
        {
            // Ocultamos siempre el mensaje de "no resultados" al comenzar
            mNoResultsLabel.Hide();

            // Si no hay nada que mostrar
            if (listings.Count == 0)
            {
                foreach (var item in mCurrentItems)
                {
                    item.Container.Dispose();
                }

                mCurrentItems.Clear();
                mListingScroll.DeleteAllChildren();

                mNoResultsLabel.Show();
                return;
            }

            // Si cambió la cantidad de ítems, reiniciamos todo
            if (listings.Count != mCurrentItems.Count)
            {
                foreach (var item in mCurrentItems)
                {
                    item.Container.Dispose();
                }

                mCurrentItems.Clear();
                mListingScroll.DeleteAllChildren();

                int offsetY = 0;
                foreach (var listing in listings)
                {
                    var marketItem = new MarketItem(this, listing);
                    marketItem.Container = new ImagePanel(mListingScroll, "MarketItemRow");
                    marketItem.Setup();
                    marketItem.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
                    marketItem.Container.SetPosition(0, offsetY);
                    marketItem.Container.Show();

                    offsetY += 44;
                    mCurrentItems.Add(marketItem);
                }

                return;
            }

            // Si no cambió la cantidad, solo actualizamos visualmente
            for (int i = 0; i < listings.Count; i++)
            {
                mCurrentItems[i].Update(listings[i]);
            }
        }


        public void RefreshAfterPurchase()
        {
            PacketSender.SendSearchMarket();
        }

        public void UpdateTransactionHistory(List<MarketTransactionPacket> transactions)
        {
            // Futuro: historial de transacciones
        }

        public void Close() => mMarketWindow?.Close();
    }
}
