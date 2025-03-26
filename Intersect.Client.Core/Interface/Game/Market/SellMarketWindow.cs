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
  
        private Label mPriceLabel;
        private Label mQuantityLabel;
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
            mInventoryScroll.SetBounds(20, 20, 280, 400);
            mInventoryScroll.EnableScroll(false, true);

           

            mInfoLabel = new Label(mSellWindow);
            mInfoLabel.SetBounds(320, 30, 250, 20);
            mInfoLabel.Text = "Selecciona un objeto del inventario";

            mQuantityLabel = new Label(mSellWindow);
            mQuantityLabel.SetBounds(320, 55, 100, 15);
            mQuantityLabel.Text = "Cantidad";

            mQuantityInput = new TextBoxNumeric(mSellWindow);
            mQuantityInput.SetBounds(320, 70, 100, 30);
            mQuantityInput.SetText("", false);

            mPriceLabel = new Label(mSellWindow);
            mPriceLabel.SetBounds(430, 55, 100, 15);
            mPriceLabel.Text = "Precio";

            mPriceInput = new TextBoxNumeric(mSellWindow);
            mPriceInput.SetBounds(430, 70, 100, 30);
            mPriceInput.SetText("", false);

            mConfirmButton = new Button(mSellWindow);
            mConfirmButton.SetBounds(320, 120, 210, 40);
            mConfirmButton.SetText("📤 Publicar");
            mConfirmButton.Disable();
            mConfirmButton.Clicked += OnConfirmClicked;

            mSellWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();
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

            PacketSender.SendChatMsg($"📦 Ítem seleccionado: {item?.Name}", 5);
        }



        private void OnConfirmClicked(Base sender, EventArgs args)
        {
            if (mSelectedSlot < 0 || mSelectedItemId == Guid.Empty) return;

            if (!int.TryParse(mQuantityInput.Text, out var qty) || qty <= 0) return;
            if (!int.TryParse(mPriceInput.Text, out var price) || price <= 0) return;

            var slotData = Globals.Me.Inventory[mSelectedSlot];
            if (slotData == null) return;

            PacketSender.SendCreateMarketListing(slotData.ItemId, qty, price, slotData.ItemProperties);

            // 🔍 Enviar mensaje de depuración al chat
            PacketSender.SendChatMsg($"[Debug] Publicado: {slotData.ItemId} x{qty} por {price}",5);

            mPriceInput.SetText("", false);
            mQuantityInput.SetText("", false);
            mInfoLabel.Text = "✅ ¡Ítem publicado con éxito!";
        }


        public void Show() => mSellWindow?.Show();
        public void Hide() => mSellWindow?.Hide();
        public void Close() => mSellWindow?.Close();
        public FloatRect RenderBounds()
        {
            var rect = new FloatRect()
            {
                X = mSellWindow.LocalPosToCanvas(new Point(0, 0)).X -
                    (Items[0].Container.Padding.Left + Items[0].Container.Padding.Right) / 2,
                Y = mSellWindow.LocalPosToCanvas(new Point(0, 0)).Y -
                    (Items[0].Container.Padding.Top + Items[0].Container.Padding.Bottom) / 2,
                Width = mSellWindow.Width + Items[0].Container.Padding.Left + Items[0].Container.Padding.Right,
                Height = mSellWindow.Height + Items[0].Container.Padding.Top + Items[0].Container.Padding.Bottom
            };

            return rect;
        }
    }
}

