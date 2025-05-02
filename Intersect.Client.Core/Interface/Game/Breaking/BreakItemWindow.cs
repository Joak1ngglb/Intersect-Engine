using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Items;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Enums;
using Intersect.Extensions;
using Newtonsoft.Json.Linq;
using Intersect.Client.Interface.Game.Enchanting;
using Intersect.Collections.Slotting;
using MonoMod.Core.Utils;
using Intersect.Client.Localization;

namespace Intersect.Client.Interface.Game.Breaking
{
    public class BreakItemWindow
    {
        private WindowControl mWindow;
        private ScrollControl mItemContainer;
        private ImagePanel mSelectedItemPanel;
        private Button mBreakButton, mCloseButton;

        private List<BreakInventoryItem> Items = new();
        private List<Label> ItemLabels = new();
        private Item mSelectedItem;
        private List<Label> Values = new List<Label>();

        public int X { get; set; }
        public int Y { get; set; }
        private bool Initialized = false;

        public BreakItemWindow(Canvas gameCanvas)
        {
            mWindow = new WindowControl(gameCanvas, "Romper Ítem", false, "BreakItemWindow");
            mWindow.DisableResizing();
            mWindow.SetSize(600, 400);
            mWindow.SetPosition(100, 100);

            mItemContainer = new ScrollControl(mWindow, "ItemContainer");
            mItemContainer.SetPosition(20, 20);
            mItemContainer.SetSize(260, 360);
            mItemContainer.EnableScroll(false, true);

            mSelectedItemPanel = new ImagePanel(mWindow, "SelectedItemPanel");
            mSelectedItemPanel.SetPosition(300, 60);
            mSelectedItemPanel.SetSize(80, 80);

            mBreakButton = new Button(mWindow, "BreakButton");
            mBreakButton.SetPosition(300, 160);
            mBreakButton.SetSize(120, 40);
            mBreakButton.Text = "Romper";
            mBreakButton.Clicked += OnBreakButtonClicked;

            mCloseButton = new Button(mWindow, "CloseButton");
            mCloseButton.SetPosition(440, 160);
            mCloseButton.SetSize(120, 40);
            mCloseButton.Text = "Cerrar";
            mCloseButton.Clicked += (s, e) => mWindow.IsHidden = true;
            mWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();
        }
         private void InitItemContainer()
        {
            Items.Clear();
            Values.Clear();
            for (var i = 0; i < Options.MaxInvItems; i++)
            {
                        
                Items.Add(new BreakInventoryItem(this, i));
                Items[i].Container = new ImagePanel(mItemContainer, "BreakItemSlot");
                Items[i].Setup();

                Values.Add(new Label(Items[i].Container, "InventoryItemValue"));
                Values[i].Text = "";

                Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                var xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
                var yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;

                Items[i].Container.SetPosition(
                    i % (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                    i / (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
                );
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

        // Change the access modifier of the SelectItem method to public to resolve the CS0122 error.  
        public void SelectItem(Item item)
        {
            mSelectedItem = item;
            var texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
            mSelectedItemPanel.Texture = texture ?? Graphics.Renderer.GetWhiteTexture();
        }

        private void OnBreakButtonClicked(Base sender, ClickedEventArgs args)
        {
            if (mSelectedItem == null)
            {
                PacketSender.SendChatMsg("Selecciona un ítem para romper.", 4);
                return;
            }

            int itemIndex = Globals.Me.Inventory.IndexOf(mSelectedItem);
            PacketSender.SendBreakItem(itemIndex);
        }

        public void Show() => mWindow.IsHidden = false;
        public void Hide() => mWindow.IsHidden = true;
        public bool IsVisible() => !mWindow.IsHidden;
    }
}
