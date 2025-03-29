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
using MonoMod.Core.Utils;
using Newtonsoft.Json.Linq;
using Intersect.Client.Localization;
using Intersect.Enums;
using Intersect.Client.Framework.GenericClasses;
using System.Diagnostics;

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
        private List<Label> Values = new List<Label>();
        private int mSelectedSlot = -1;
        private Guid mSelectedItemId = Guid.Empty;
        //Initialized Items?
        private bool Initialized= false;
        //Item List
        public List<InventoryItem> Items = new List<InventoryItem>();
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
           // Interface.InputBlockingElements.Add(mSellWindow);

            mInventoryScroll = new ScrollControl(mSellWindow, "SellInventoryScroll");
          
            mInventoryScroll.EnableScroll(false, true);

           

            mInfoLabel = new Label(mSellWindow);
      
            mInfoLabel.Text = "Selecciona un objeto del inventario";

            mQuantityLabel = new Label(mSellWindow);
           
            mQuantityLabel.Text = "Cantidad";

            mQuantityInput = new TextBoxNumeric(mSellWindow);
            
            mQuantityInput.SetText("", false);
            mQuantityInput.TextChanged += (sender, args) => UpdateTaxDisplay();

            mPriceLabel = new Label(mSellWindow);
         
            mPriceLabel.Text = "Precio";

            mPriceInput = new TextBoxNumeric(mSellWindow);
        
            mPriceInput.SetText("", false);
            mPriceInput.TextChanged += (sender, args) => UpdateTaxDisplay();

            mTaxLabel = new Label(mSellWindow,"Taxlabel");
         
            mTaxLabel.Text = "🧾 Impuesto estimado: 0 🪙";

            mConfirmButton = new Button(mSellWindow);
   
            mConfirmButton.SetText("📤 Publicar");
            mConfirmButton.Disable();
            mConfirmButton.Clicked += OnConfirmClicked;
            suggestedPriceLabel = new Label(mSellWindow,"SuggetedLabel");
            suggestedPriceLabel.SetBounds(320, 150, 210, 15);
            suggestedPriceLabel.Text = "";

            int startX = 320;
            int spacingY = 25;

            mInfoLabel.SetBounds(startX, 20, 250, 20);
            suggestedPriceLabel.SetBounds(startX, 45, 250, 20);

            mQuantityLabel.SetBounds(startX, 75, 100, 20);
            mQuantityInput.SetBounds(startX, 95, 100, 30);

            mPriceLabel.SetBounds(startX + 110, 75, 100, 20);
            mPriceInput.SetBounds(startX + 110, 95, 100, 30);

            mTaxLabel.SetBounds(startX, 135, 210, 20);
            mConfirmButton.SetBounds(startX, 160, 210, 40);
            mSellWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();
        }
        private void UpdateTaxDisplay()
        {
            if (int.TryParse(mPriceInput.Text, out var price) && price > 0)
            {
                var quantity = mQuantityInput.Value;
                if (price > 0 && quantity > 0)
                {
                    var tax = (int)Math.Ceiling(price * quantity * 0.02f); // Usa el valor real del servidor
                    mTaxLabel.Text = $"🧾 Impuesto estimado: {tax} 🪙";
                }

            }
            else
            {
                mTaxLabel.Text = "🧾 Impuesto estimado: 0 ";
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
                var inventorySlot = Globals.Me.Inventory[i];

                if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
                {
                    Items[i].Pnl.IsHidden = true;
                    Values[i].IsHidden = true;
                    continue;
                }

                var item = ItemBase.Get(inventorySlot.ItemId);

                if (item != null)
                {
                    Items[i].Pnl.IsHidden = false;

                    if (CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor))
                    {
                        Items[i].Container.RenderColor = rarityColor;
                    }
                    else
                    {
                        Items[i].Container.RenderColor = Color.White;
                    }

                    if (item.IsStackable)
                    {
                        Values[i].IsHidden = inventorySlot.Quantity <= 1;
                        Values[i].Text = Strings.FormatQuantityAbbreviated(inventorySlot.Quantity);
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
                else
                {
                    Items[i].Pnl.IsHidden = true;
                    Values[i].IsHidden = true;
                }
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

            // Por conveniencia, precargar cantidad máxima
            mQuantityInput.SetText(Globals.Me.Inventory[slotIndex].Quantity.ToString(), false);
            mPriceInput.SetText("", false);
           
            if (item!= null)
            {
                var suggestedPrice = item.Price;
                suggestedPriceLabel.Text = $"💡 Precio sugerido: {suggestedPrice} 🪙";
                if (suggestedPrice > 0 && (mPriceInput.Value < suggestedPrice * 0.5 || mPriceInput.Value > suggestedPrice * 2))
                {
                    PacketSender.SendChatMsg("⚠️ Precio fuera del rango sugerido.", 4);
                }
            }
            PacketSender.SendChatMsg($"📦 Ítem seleccionado: {item?.Name}", 5);
           

        }
        private void OnConfirmClicked(Base sender, EventArgs args)
        {
            if (mSelectedSlot < 0 || mSelectedItemId == Guid.Empty)
            {
                PacketSender.SendChatMsg("❌ Slot inválido o sin ítem.", 5);
                return;
            }

            // Validar cantidad y precio
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

      

            // Enviar el listado al servidor
            PacketSender.SendCreateMarketListing(slotData.ItemId, qty, price, properties);

            // ✅ Confirmación al jugador
            PacketSender.SendChatMsg($"📤 Publicado: {itemName} x{qty} por {price} 🪙", 5);

            // Limpiar campos
            mPriceInput.SetText("", false);
            mQuantityInput.SetText("", false);
            mInfoLabel.Text = "✅ ¡Ítem publicado con éxito!";
        }




        public void Show() => mSellWindow?.Show();
        public void Hide() => mSellWindow?.Hide();
        public void Close() => mSellWindow?.Close();
      
    }
}

