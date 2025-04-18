using System;
using System.Collections.Generic;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.Layout;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.General;
using Intersect.Client.Items;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Client.Interface.Game.Inventory;
using Intersect.Client.Core;
using Intersect.Client.Localization;
using Intersect.Enums;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Network.Packets.Client;

namespace Intersect.Client.Interface.Game.Market
{
    public class SellMarketWindow
    {
        private WindowControl mSellWindow;
        private ScrollControl mInventoryScroll;
        private TextBoxNumeric mPriceInput;
        private TextBoxNumeric mQuantityInput;
        private Button mConfirmButton;
        private Label mInfoLabel;
        private List<Label> Values = new();
        private int mSelectedSlot = -1;
        private Guid mSelectedItemId = Guid.Empty;
        private bool Initialized = false;
        public List<InventoryItem> Items = new();
        private Label mTaxLabel;
        private Label mPriceLabel;
        private Label mQuantityLabel;
        private Label suggestedPriceLabel;

        public int X { get; set; }
        public int Y { get; set; }

        public SellMarketWindow(Canvas canvas)
        {
            mSellWindow = new WindowControl(canvas, "📤 Vender en el Mercado", false, "SellMarketWindow");
            mSellWindow.SetSize(600, 460);
            mSellWindow.SetPosition(Graphics.Renderer.GetScreenWidth() / 2 - 300, Graphics.Renderer.GetScreenHeight() / 2 - 230);
            mSellWindow.DisableResizing();

            mInventoryScroll = new ScrollControl(mSellWindow, "SellInventoryScroll");
            mInventoryScroll.SetBounds(20, 20, 280, 400);
            mInventoryScroll.EnableScroll(false, true);

            mInfoLabel = new Label(mSellWindow);
            mInfoLabel.Text = "Selecciona un objeto del inventario";

            suggestedPriceLabel = new Label(mSellWindow, "SuggestedLabel");
            suggestedPriceLabel.Text = "";
        
            suggestedPriceLabel.AutoSizeToContents = true;
    


            mQuantityLabel = new Label(mSellWindow) { Text = "Cantidad" };
            mQuantityInput = new TextBoxNumeric(mSellWindow);
            mQuantityInput.SetText("", false);
            mQuantityInput.TextChanged += (sender, args) => UpdateTaxDisplay();

            mPriceLabel = new Label(mSellWindow) { Text = "Precio" };
            mPriceInput = new TextBoxNumeric(mSellWindow);
            mPriceInput.SetText("", false);
            mPriceInput.TextChanged += (sender, args) => UpdateTaxDisplay();

            mTaxLabel = new Label(mSellWindow, "TaxLabel");
            mTaxLabel.Text = "🧾 Impuesto estimado: 0 🪙";

            mConfirmButton = new Button(mSellWindow);
            mConfirmButton.SetText("📤 Publicar");
            mConfirmButton.Disable();
            mConfirmButton.Clicked += OnConfirmClicked;

            // Layout de campos
            int startX = 320;
            mInfoLabel.SetBounds(startX, 20, 250, 20);
            suggestedPriceLabel.SetBounds(startX, 45, 250, 20);
            mQuantityLabel.SetBounds(startX, 75, 100, 20);
            mQuantityInput.SetBounds(startX, 95, 100, 30);
            mPriceLabel.SetBounds(startX + 110, 75, 100, 20);
            mPriceInput.SetBounds(startX + 110, 95, 100, 30);
            mTaxLabel.SetBounds(startX, 135, 290, 20);
            mConfirmButton.SetBounds(startX, 160, 210, 40);

            mSellWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();
        }

        private void UpdateTaxDisplay()
        {
            if (string.IsNullOrWhiteSpace(mPriceInput.Text)) return;

            if (int.TryParse(mPriceInput.Text, out var price) && price > 0)
            {
                var quantity = mQuantityInput.Value;
                if (quantity > 0)
                {
                    var tax = (int)Math.Ceiling(price * quantity * 0.02f);
                    mTaxLabel.Text = $"🧾 Impuesto estimado: {tax} 🪙";
                }

                if (mSelectedItemId != Guid.Empty &&
                    MarketPriceCache.TryGet(mSelectedItemId, out var avg, out var min, out var max))
                {
                    if (price < min || price > max)
                    {
                        PacketSender.SendChatMsg("⚠️ Precio fuera del rango permitido.", 5);
                    }
                }
            }
            else
            {
                mTaxLabel.Text = "🧾 Impuesto estimado: 0 🪙";
            }
        }

        public void Update()
        {
            if (!Initialized)
            {
                Initialized = true;
                InitItemContainer();
            }

            if (Items.Count != Options.MaxInvItems || Values.Count != Options.MaxInvItems)
            {
                InitItemContainer();
            }

            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                var slot = Globals.Me.Inventory[i];
                var item = slot?.ItemId != null ? ItemBase.Get(slot.ItemId) : null;

                if (slot == null || item == null || slot.ItemId == Guid.Empty)
                {
                    Items[i].Pnl.IsHidden = true;
                    Values[i].IsHidden = true;
                    continue;
                }

                Items[i].Pnl.IsHidden = false;
                Items[i].Container.RenderColor = CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var color) ? color : Color.White;

                if (item.IsStackable)
                {
                    Values[i].IsHidden = slot.Quantity <= 1;
                    Values[i].Text = Strings.FormatQuantityAbbreviated(slot.Quantity);
                }
                else
                {
                    Values[i].IsHidden = true;
                }

                if (Items[i].IsDragging)
                {
                    Items[i].Pnl.IsHidden = true;
                    Values[i].IsHidden = true;
                }

                Items[i].Update();
            }
        }

        private void InitItemContainer()
        {
            Items.Clear();
            Values.Clear();
            mInventoryScroll.DeleteAllChildren();

            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                Items.Add(new InventoryItem(this, i));
                Items[i].Container = new ImagePanel(mInventoryScroll, "SellInvItem");
                Items[i].Setup();

                Values.Add(new Label(Items[i].Container, "InventoryItemValue"));
                Values[i].Text = "";

                Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                int xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
                int yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;

                Items[i].Container.SetPosition(
                    i % (mInventoryScroll.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                    i / (mInventoryScroll.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
                );

                Items[i].Container.DoubleClicked += (sender, args) => SelectItem(Items[i], i);
            }
        }

        public void SelectItem(InventoryItem itemSlot, int slotIndex)
        {
            var itemId = Globals.Me.Inventory[slotIndex]?.ItemId;
            if (itemId == null || itemId == Guid.Empty)
            {
                PacketSender.SendChatMsg("⚠️ No se ha seleccionado un ítem válido.", 4);
                return;
            }

            mSelectedSlot = slotIndex;
            mSelectedItemId = itemId.Value;

            mConfirmButton.Enable();

            var item = ItemBase.Get(itemId.Value);
            mInfoLabel.Text = $"Publicar: {item?.Name}";

            mQuantityInput.SetText(Globals.Me.Inventory[slotIndex].Quantity.ToString(), false);
            mPriceInput.SetText("", false);

            UpdateSuggestedPrice(mSelectedItemId);

            PacketSender.SendRequestMarketInfo(itemId.Value);
            PacketSender.SendChatMsg($"📦 Ítem seleccionado: {item?.Name}", 5);
        }

        public void UpdateSuggestedPrice(Guid itemId)
        {
            MarketPriceCache.TryGet(mSelectedItemId, out var avg, out var min, out var max) ;
            PacketSender.SendChatMsg($" Promedio: {avg} \n📊 Rango: {min} - {max} 🪙", 5);
            suggestedPriceLabel = new Label(mSellWindow, "SuggestedLabel");
            suggestedPriceLabel.Text = $" Promedio: {avg} \n📊 Rango: {min} - {max} 🪙";
            suggestedPriceLabel.Show();
            suggestedPriceLabel.SetTextColor(Color.Orange,Label.ControlState.Normal);
            suggestedPriceLabel.SetBounds(300, 45, 250, 20);
            if (int.TryParse(mPriceInput.Text, out var price))
            {
                if (price < min || price > max)
                {
                    PacketSender.SendChatMsg("⚠️ Precio fuera del rango permitido.", 4);
                }
            }
        }

        private void OnConfirmClicked(Base sender, EventArgs args)
        {
            if (mSelectedSlot < 0 || mSelectedItemId == Guid.Empty)
            {
                PacketSender.SendChatMsg("❌ Slot inválido o sin ítem.", 5);
                return;
            }

            if (!int.TryParse(mQuantityInput.Text, out var qty) || qty <= 0)
            {
                PacketSender.SendChatMsg("❌ Cantidad inválida.", 5);
                return;
            }

            if (!int.TryParse(mPriceInput.Text, out var price) || price <= 0)
            {
                PacketSender.SendChatMsg("❌ Precio inválido.", 5);
                return;
            }

            if (MarketPriceCache.TryGet(mSelectedItemId, out var avg, out var min, out var max))
            {
                if (price < min || price > max)
                {
                    PacketSender.SendChatMsg("❌ El precio está fuera del rango permitido.", 5);
                    return;
                }
            }

            var slotData = Globals.Me.Inventory[mSelectedSlot];
            if (slotData == null)
            {
                PacketSender.SendChatMsg("❌ No se encontró el slot en inventario.", 5);
                return;
            }

            var item = ItemBase.Get(slotData.ItemId);
            if (item == null)
            {
                PacketSender.SendChatMsg("❌ ItemBase no encontrado.", 5);
                return;
            }

            var itemName = item.Name;
            var properties = slotData.ItemProperties ?? new Network.Packets.Server.ItemProperties();

            PacketSender.SendCreateMarketListing(slotData.ItemId, qty, price, properties);
            PacketSender.SendChatMsg($"📤 Publicado: {itemName} x{qty} por {price} 🪙", 5);

            mPriceInput.SetText("", false);
            mQuantityInput.SetText("", false);
            mInfoLabel.Text = "✅ ¡Ítem publicado con éxito!";
        }

        public void Show() => mSellWindow?.Show();
        public void Hide() => mSellWindow?.Hide();
        public void Close() => mSellWindow?.Close();
        public bool IsVisible() => mSellWindow?.IsHidden == false;
        public Guid GetSelectedItemId() => mSelectedItemId;
    }

    public static class MarketPriceCache
    {
        private static readonly Dictionary<Guid, (int Avg, int Min, int Max)> _cache = new();
        static int average { get; set; }
        public static void Update(Guid itemId, int avg, int min, int max)
        {
            _cache[itemId] = (avg, min, max);
        }

        public static bool TryGet(Guid itemId, out int avg, out int min, out int max)
        {
            if (_cache.TryGetValue(itemId, out var tuple))
            {
                avg = tuple.Avg;
                min = tuple.Min;
                max = tuple.Max;
                return true;
            }

            avg = min = max = 0;
            return false;
        }
    }
}
