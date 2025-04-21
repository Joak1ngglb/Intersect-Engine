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
using MonoMod.Core.Utils;
using Newtonsoft.Json.Linq;

namespace Intersect.Client.Interface.Game.Market
{
    /// <summary>
    /// Ventana para publicar objetos en el mercado.
    /// </summary>
    public sealed class SellMarketWindow
    {
        #region === UI ===

        private readonly WindowControl _window;
        private readonly ScrollControl mInventoryScroll;
        private readonly TextBoxNumeric _priceInput;
        private readonly TextBoxNumeric _quantityInput;
        private readonly Button _confirmButton;
        private readonly Label _infoLabel;
        private readonly Label _taxLabel;
        private readonly Label _suggestedPriceLabel;
        private readonly CheckBox _autoSplitCheckbox;

        // Render helpers por slot
        public readonly List<InventoryItem> Items = new();
        private readonly List<Label> Values = new();

        #endregion

        #region === State ===

        private int _selectedSlot = -1;
        private Guid _selectedItemId = Guid.Empty;

        private Label _suggestedRangeLabel;

        #endregion

        public int X { get; set; }
        public int Y { get; set; }
        private bool Initialized = false;

        public SellMarketWindow(Canvas canvas)
        {
            _window = new WindowControl(canvas, "📤 " + Strings.Market.sellwindow, false, "SellMarketWindow");
            _window.SetSize(600, 460);
            _window.SetPosition(Graphics.Renderer.GetScreenWidth() / 2 - 300, Graphics.Renderer.GetScreenHeight() / 2 - 230);
            _window.DisableResizing();

            // Panel inventario
            mInventoryScroll = new ScrollControl(_window, "SellInventoryScroll");
            mInventoryScroll.SetBounds(20, 20, 280, 400);
            mInventoryScroll.EnableScroll(false, true);

            // Etiquetas y campos de entrada
            _infoLabel = new Label(_window,"SelectedItem") { Text = Strings.Market.selectitem };
            _suggestedPriceLabel = new Label(_window, "SuggestedLabel");
            _suggestedRangeLabel = new Label(_window, "SuggestedRange");

            _quantityInput = new TextBoxNumeric(_window,"QuantityInput");
            _priceInput = new TextBoxNumeric(_window,"PriceInput");
            _quantityInput.TextChanged += (_, _) => RefreshTax();
            _priceInput.TextChanged += (_, _) => RefreshTax();

            _taxLabel = new Label(_window, "TaxLabel") { Text = Strings.Market.taxes_0 };

            _autoSplitCheckbox = new CheckBox(_window,"SplitCheckBox") { Text = Strings.Market.splitpackages };
            _autoSplitCheckbox.IsChecked = true;
      
            _confirmButton = new Button(_window,"CorfimButton") { Text = Strings.Market.publish };
            _confirmButton.Disable();
            _confirmButton.Clicked += OnConfirmClicked;

            BuildLayout();
            _window.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
           InitItemContainer();
        }

        #region === Layout ===

        private void BuildLayout()
        {
            int startX = 320,
                startY = 20,
                labelH = 20,
                inputH = 30,
                padY = 8;

            _infoLabel.SetBounds(startX, startY, 250, labelH); startY += labelH + padY;
            _suggestedPriceLabel.SetBounds(startX, startY, 250, labelH); startY += labelH + (padY * 2);
            _suggestedRangeLabel.SetBounds(startX, startY, 250, labelH); startY += labelH + (padY * 2);
            // Cantidad / Precio etiquetas
            new Label(_window,"QuantityLabel") { Text = Strings.Market.quantity }.SetBounds(startX, startY + 10, 100, labelH);
            new Label(_window,"Pricelabel") { Text = Strings.Market.price }.SetBounds(startX + 110, startY + 10, 100, labelH);
            startY += labelH;

            _quantityInput.SetBounds(startX, startY+10, 100, inputH);
            _priceInput.SetBounds(startX + 110, startY+10, 100, inputH);
            startY += inputH + (padY * 2);

            _taxLabel.SetBounds(startX, startY, 290, labelH); startY += labelH + padY;
            _autoSplitCheckbox.SetBounds(startX, startY, 250, labelH); startY += labelH + padY;
            _confirmButton.SetBounds(startX, startY, 210, 40);
        }

        #endregion

        #region === Inventory UI ===

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

        #endregion

        #region === Selection & Validation ===

        public void SelectItem(InventoryItem itemSlot, int slotIndex)
        {
            var slot = Globals.Me.Inventory[slotIndex];
            if (slot?.ItemId == Guid.Empty)
            {
                PacketSender.SendChatMsg(Strings.Market.invalidItem, (byte)ChatMessageType.Error);
                return;
            }

            _selectedSlot = slotIndex;
            _selectedItemId = slot.ItemId;
            _confirmButton.Enable();

            var item = ItemBase.Get(slot.ItemId);
            _infoLabel.Text = Strings.Market.publish_colon + " " + item?.Name;
            _quantityInput.SetText(string.Empty, false);
            _priceInput.SetText(string.Empty, false);

            PacketSender.SendRequestMarketInfo(slot.ItemId);
            UpdateSuggestedPrice(slot.ItemId);
        }

        private void RefreshTax()
        {
            if (!int.TryParse(_priceInput.Text, out var unitPrice) || unitPrice <= 0) { _taxLabel.Text = Strings.Market.taxes_0; return; }
            int qty = (int)_quantityInput.Value;
            if (qty <= 0) { _taxLabel.Text = Strings.Market.taxes_0; return; }

            int tax = (int)Math.Ceiling(unitPrice * qty * 0.02f);
            _taxLabel.Text = Strings.Market.taxes_estimated.ToString(tax);
        }
        public void UpdateSuggestedPrice(Guid itemId)
        {
            if (!MarketPriceCache.TryGet(itemId, out int avg, out int min, out int max))
            {
                      
                return;
            }

            _suggestedPriceLabel.SetText(Strings.Market.pricehint.ToString(avg));
            _suggestedRangeLabel.SetText(Strings.Market.pricerange.ToString(min, max)); // 

            _suggestedPriceLabel.Show();
            _suggestedRangeLabel.Show();

            _suggestedPriceLabel.SetTextColor(Color.Orange, Label.ControlState.Normal);
            _suggestedRangeLabel.SetTextColor(Color.Orange, Label.ControlState.Normal);
        }

        #endregion

        #region === Publish ===

        private void OnConfirmClicked(Base _, EventArgs __)
        {
            if (_selectedSlot < 0 || _selectedItemId == Guid.Empty) { PacketSender.SendChatMsg(Strings.Market.noItemSelected, 0); return; }
            if (!int.TryParse(_quantityInput.Text, out int qty) || qty <= 0) { PacketSender.SendChatMsg(Strings.Market.invalidQuantity, 0); return; }
            if (!int.TryParse(_priceInput.Text, out int price) || price <= 0) { PacketSender.SendChatMsg(Strings.Market.invalidPrice, 0 ); return; }

            var slotData = Globals.Me.Inventory[_selectedSlot];
            if (slotData == null || slotData.ItemId != _selectedItemId || slotData.Quantity < qty)
            {
                PacketSender.SendChatMsg(Strings.Market.quantityExceeds, 0); return;
            }

            if (MarketPriceCache.TryGet(_selectedItemId, out var avg, out var min, out var max))
            {
                if (price < min || price > max)
                {
                    PacketSender.SendChatMsg(
               string.Format(Strings.Market.priceOutOfRange.ToString(), min, max),
               0
           );
                    return;
                }
            }

            // Enviar al servidor
            var props = slotData.ItemProperties ?? new Network.Packets.Server.ItemProperties();
            PacketSender.SendCreateMarketListing(_selectedItemId, qty, price, props, _autoSplitCheckbox.IsChecked);

            ResetSelection();
        }

        private void ResetSelection()
        {
            _selectedSlot = -1;
            _selectedItemId = Guid.Empty;
            _infoLabel.Text = Strings.Market.selectitem;
            _priceInput.SetText(string.Empty, false);
            _quantityInput.SetText(string.Empty, false);
            _confirmButton.Disable();
            _suggestedPriceLabel.SetText("");
            _suggestedRangeLabel.SetText("");
            _taxLabel.SetText("");
           InitItemContainer(); // refrescar grid inmediatamente para que el ítem desaparezca si el servidor ya lo descontó
            Update();
        }

        #endregion

        #region === Public helpers ===

        public void Show() => _window.Show();
        public void Hide() => _window.Hide();
        public void Close() => _window.Close();
        public bool IsVisible() => !_window.IsHidden;
        public Guid GetSelectedItemId() => _selectedItemId;

        #endregion
    }

    /// <summary>
    /// Cache local de precios de mercado.
    /// </summary>
    public static class MarketPriceCache
    {
        private static readonly Dictionary<Guid, (int Avg, int Min, int Max)> Cache = new();

        public static void Update(Guid itemId, int avg, int min, int max) => Cache[itemId] = (avg, min, max);

        public static bool TryGet(Guid itemId, out int avg, out int min, out int max)
        {
            if (Cache.TryGetValue(itemId, out var t))
            {
                (avg, min, max) = t;
                return true;
            }

            avg = min = max = 0;
            return false;
        }
    }
}
