using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Enchanting;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.Extensions;
using Intersect.GameObjects;
using MonoMod.Core.Utils;
using Newtonsoft.Json.Linq;

namespace Intersect.Client.Interface.Game.Enchanting
{
    public class OrbItemWindow
    {
        private WindowControl mOrbWindow;
        private ScrollControl mInventoryScroll;
        private ImagePanel mItemSlot;
        private ImagePanel mOrbSlot;
        private ScrollControl mProjectionContainer;
        private Button mApplyButton, mCloseButton;

        private Label lblStat, lblAmount, lblSuccessRate;
        private bool Initialized = false;
        private List<OrbInventoryItem> Items = new();
        private Item mSelectedItem;
        private Item mSelectedOrb;
        private List<Label> Values = new List<Label>();

        public int X { get; set; }
        public int Y { get; set; }

        public OrbItemWindow(Canvas gameCanvas)
        {
            mOrbWindow = new WindowControl(gameCanvas, "Usar Orbe", false, "OrbItemWindow");
            mOrbWindow.DisableResizing();
            mOrbWindow.SetSize(600, 400);
            mOrbWindow.SetPosition(100, 100);

            mInventoryScroll = new ScrollControl(mOrbWindow, "ItemContainer");
            mInventoryScroll.SetPosition(20, 20);
            mInventoryScroll.SetSize(260, 360);
            mInventoryScroll.EnableScroll(false, true);

            mItemSlot = new ImagePanel(mOrbWindow, "ItemSlot");
            mItemSlot.SetPosition(300, 60);
            mItemSlot.SetSize(80, 80);

            mOrbSlot = new ImagePanel(mOrbWindow, "OrbSlot");
            mOrbSlot.SetPosition(400, 60);
            mOrbSlot.SetSize(80, 80);

            mProjectionContainer = new ScrollControl(mOrbWindow, "ProjectionContainer");
            mProjectionContainer.SetPosition(300, 160);
            mProjectionContainer.SetSize(280, 100);
            mProjectionContainer.EnableScroll(false, true);

            lblStat = new Label(mProjectionContainer, "StatLabel");
            lblStat.SetPosition(10, 10);
            lblStat.SetSize(240, 20);

            lblAmount = new Label(mProjectionContainer, "AmountLabel");
            lblAmount.SetPosition(10, 40);
            lblAmount.SetSize(240, 20);

            lblSuccessRate = new Label(mProjectionContainer, "SuccessRateLabel");
            lblSuccessRate.SetPosition(10, 70);
            lblSuccessRate.SetSize(240, 20);

            mApplyButton = new Button(mOrbWindow, "ApplyButton");
            mApplyButton.SetPosition(300, 280);
            mApplyButton.SetSize(120, 40);
            mApplyButton.Text = "Aplicar Orbe";
            mApplyButton.Clicked += OnApplyButtonClicked;

            mCloseButton = new Button(mOrbWindow, "CloseButton");
            mCloseButton.SetPosition(440, 280);
            mCloseButton.SetSize(120, 40);
            mCloseButton.Text = "Cerrar";
            mCloseButton.Clicked += (sender, args) => mOrbWindow.IsHidden = true;
           mOrbWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            InitItemContainer();
        }

        private void InitItemContainer()
        {
            Items.Clear();
            Values.Clear();
            for (var i = 0; i < Options.MaxInvItems; i++)
            {
                Items.Add(new OrbInventoryItem(this, i));
                Items[i].Container = new ImagePanel(mInventoryScroll, "OrbItem");
                Items[i].Setup();

                Values.Add(new Label(Items[i].Container, "InventoryItemValue"));
                Values[i].Text = "";

                Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                var xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
                var yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;

                Items[i].Container.SetPosition(
                    i % (mInventoryScroll.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                    i / (mInventoryScroll.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
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

        public void SelectItem(Item item)
        {
            if (item == null || item.Base == null)
            {
                return;
            }

            mSelectedItem = item;

            // Actualizar interfaz
            var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
            mItemSlot.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture();

            // Mostrar la proyección del encantamiento
            UpdateProjection();
        }

        public void SelectOrbItem(Item item)
        {
            if (item == null || item.Base == null)
            {
                return; // Salir si el ítem no es válido
            }

           mSelectedOrb = item;

            // Actualizar el icono en el slot de currency
            var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
           mOrbSlot.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture();
            UpdateProjection();
        }

        public void UpdateProjection()
        {
            if (mSelectedItem == null || mSelectedOrb == null || mSelectedItem.ItemProperties == null)
            {
                mProjectionContainer.Hide();
                return;
            }

            mProjectionContainer.Show();

            var stat = mSelectedOrb.Base.TargetStat;
            var amount = mSelectedOrb.Base.AmountModifier;
            var baseSuccessRate = mSelectedOrb.Base.UpgradeMaterialSuccessRate > 0
                ? mSelectedOrb.Base.UpgradeMaterialSuccessRate
                : 1.0;

            // Contador actual de orbes aplicados al stat
            int orbUses = mSelectedItem.ItemProperties.StatOrbUpgradeCounts[(int)stat];
            int maxOrbUsesPerStat = 5;

            // Penalización progresiva
            double reductionPerUse = 0.05;
            double penalty = reductionPerUse * orbUses;
            double adjustedSuccessRate = baseSuccessRate - penalty;

            // Bonus/penalización según stat base
            int baseStatValue = mSelectedItem.Base.StatsGiven[(int)stat];
            if (baseStatValue > 0)
            {
                adjustedSuccessRate += 0.10;
            }
            else
            {
                adjustedSuccessRate -= 0.10;
            }

            // Clamp final entre 10% y 100%
            adjustedSuccessRate = Math.Clamp(adjustedSuccessRate, 0.10, 1.0);

            // Actualizar labels
            lblStat.Text = $"Stat: {stat}";
            lblAmount.Text = $"Cantidad: +{amount}";
            lblSuccessRate.Text = $"Éxito ajustado: {adjustedSuccessRate * 100:F1}%";

            // Crear labels adicionales para el conteo de orbes
            var lblOrbCount = new Label(mProjectionContainer, "OrbCountLabel")
            {
                Text = $"Orbes aplicados: {orbUses}/{maxOrbUsesPerStat}"
            };
            lblOrbCount.SetPosition(10, 100);
            lblOrbCount.SetSize(240, 20);
            lblOrbCount.FontName = "sourcesansproblack";
            lblOrbCount.FontSize = 10;
            lblOrbCount.SetTextColor(Color.Orange, Label.ControlState.Normal);
        }


        private void OnApplyButtonClicked(Base sender, ClickedEventArgs args)
        {
            if (mSelectedItem == null || mSelectedOrb == null)
            {
                PacketSender.SendChatMsg("Selecciona un ítem y un orbe.", 4);
                return;
            }

            int itemIndex = Globals.Me.Inventory.IndexOf(mSelectedItem);
            int orbIndex = Globals.Me.Inventory.IndexOf(mSelectedOrb);

            PacketSender.SendUpgradeStat(itemIndex, orbIndex);         
        }

        public void Show() => mOrbWindow.IsHidden = false;
        public void Hide() => mOrbWindow.IsHidden = true;
        public bool IsVisible() => !mOrbWindow.IsHidden;
    }
}
