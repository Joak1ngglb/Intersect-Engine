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

     
        public MarketWindow(Canvas parent)
        {
            Instance = this;

            mMarketWindow = new WindowControl(parent, "📦 Mercado Global", false, "MarketWindow");
            mMarketWindow.SetSize(800, 600);
            
            mMarketWindow.DisableResizing();
            Interface.InputBlockingElements.Add(mMarketWindow);
          
            mTitle = new Label(mMarketWindow, "MarketTitle");
            mTitle.Text = "📦 Mercado Global";
            mTitle.SetBounds(20, 10, 400, 30);

            mSearchBox = new TextBox(mMarketWindow);
            mSearchBox.SetBounds(20, 50, 200, 30);
            mSearchBox.SetText("");

            mItemTypeList = new ListBox(mMarketWindow);
            mItemTypeList.SetBounds(230, 50, 150, 100);
            mItemTypeList.AddRow("Todos", "all", "all");
            mItemTypeList.SelectByUserData("all");
            foreach (var type in Enum.GetValues(typeof(ItemType)))
            {
                mItemTypeList.AddRow(type.ToString(), type.ToString(), type);
            }

            mMinPriceBox = new TextBoxNumeric(mMarketWindow);
            mMinPriceBox.SetBounds(390, 50, 80, 30);
            mMinPriceBox.SetText("", false);

            mMaxPriceBox = new TextBoxNumeric(mMarketWindow);
            mMaxPriceBox.SetBounds(480, 50, 80, 30);
            mMaxPriceBox.SetText("", false);

            mSearchButton = new Button(mMarketWindow);
            mSearchButton.SetBounds(570, 50, 100, 30);
            mSearchButton.SetText("🔍 Buscar");
            mSearchButton.Clicked += (s, a) => SendSearch();

            mListingScroll = new ScrollControl(mMarketWindow, "MarketListingScroll");
            mListingScroll.EnableScroll(false, true);
            mListingScroll.SetBounds(20, 150, 760, 420);
            mSellButton = new Button(mMarketWindow);
            mSellButton.SetBounds(680, 50, 100, 30);
            mSellButton.SetText("📤 Vender");
            mSellButton.Clicked += SellMarket_Clicked;
            mMarketWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());


            PacketSender.SendSearchMarket();
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
            int? minPrice = int.TryParse(mMinPriceBox.Text, out var minVal) ? minVal : null;
            int? maxPrice = int.TryParse(mMaxPriceBox.Text, out var maxVal) ? maxVal : null;
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
                marketItem.Setup();
                marketItem.Container.SetPosition(0, offsetY);
                offsetY += 44;

                mCurrentItems.Add(marketItem);
                mListingScroll.AddChild(marketItem.Container);
            }
        }

        public void RefreshAfterPurchase()
        {
            PacketSender.SendSearchMarket();
        }

        public void UpdateTransactionHistory(List<MarketTransactionPacket> transactions)
        {
            // Implementar si deseas mostrar historial en pestaña futura
        }

        public void Close() => mMarketWindow?.Close();
    }
}
