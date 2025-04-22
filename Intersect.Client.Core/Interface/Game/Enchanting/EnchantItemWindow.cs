using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Inventory;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.Extensions;
using Intersect.GameObjects;


namespace Intersect.Client.Interface.Game.Enchanting
{
    public class EnchantItemWindow
    {
        private WindowControl mEnchantWindow;

        // Controles de la sección izquierda
        private ScrollControl mProjectionContainer;
        private Label mLblProjection;

        // Controles de la sección derecha
        private ImagePanel mItemSlot;
        private ImagePanel mCurrencySlot;
        private CheckBox mUseAmuletCheckbox;
        private ScrollControl mInventoryScroll;
        private Button mEnchantButton, mCloseButton;

        private bool Initialized = false;
        private List<InventoryItem> Items = new List<InventoryItem>();
        private List<Label> Values = new List<Label>();

        private Item mSelectedItem;
        private Item mSelectedCurrency;
        // Sección de proyección de encantamiento
  
        public int Y { get; set; }
        public int X { get;  set; }

        public EnchantItemWindow(Canvas gameCanvas)
        {
            // Crear la ventana principal
            mEnchantWindow = new WindowControl(gameCanvas, Strings.Enchanting.WindowTitle, false, "EnchantItemWindow");
            mEnchantWindow.DisableResizing();
            mEnchantWindow.SetSize(600, 500);
            mEnchantWindow.SetPosition(100, 100);

            // Sección izquierda: Proyección
            mLblProjection = new Label(mEnchantWindow, "ProjectionLabel");
            mLblProjection.SetPosition(20, 20);
            mLblProjection.SetSize(260, 30);
            mLblProjection.Text = Strings.Enchanting.Projection;

            mProjectionContainer = new ScrollControl(mEnchantWindow, "ProjectionContainer");
            mProjectionContainer.SetPosition(20, 60);
            mProjectionContainer.SetSize(260, 400);
            mProjectionContainer.EnableScroll(false, true);

            // Sección derecha: Contenedor de ítems y slots
            mInventoryScroll = new ScrollControl(mEnchantWindow, "ItemContainer");
            mInventoryScroll.SetPosition(300, 20);
            mInventoryScroll.SetSize(260, 200);
            mInventoryScroll.EnableScroll(false, true);

            // Slot del ítem a encantar
            mItemSlot = new ImagePanel(mEnchantWindow, "ItemSlot");
            mItemSlot.SetPosition(300, 240);
            mItemSlot.SetSize(80, 80);
            mItemSlot.Texture = Graphics.Renderer.GetWhiteTexture();

            // Slot de la moneda de encantamiento
            mCurrencySlot = new ImagePanel(mEnchantWindow, "CurrencySlot");
            mCurrencySlot.SetPosition(400, 240);
            mCurrencySlot.SetSize(80, 80);
            mCurrencySlot.Texture = Graphics.Renderer.GetWhiteTexture();

            // Checkbox para amuleto protector
            mUseAmuletCheckbox = new CheckBox(mEnchantWindow, "UseAmuletCheckbox");
            mUseAmuletCheckbox.SetPosition(300, 340);
            mUseAmuletCheckbox.SetSize(260, 30);
            mUseAmuletCheckbox.Text = Strings.Enchanting.UseAmulet;

            // Botones
            mEnchantButton = new Button(mEnchantWindow, "EnchantButton");
            mEnchantButton.SetPosition(300, 380);
            mEnchantButton.SetSize(120, 40);
            mEnchantButton.Text = Strings.Enchanting.Enchant;
            mEnchantButton.Clicked += OnEnchantButtonClicked;

            mCloseButton = new Button(mEnchantWindow, "CloseButton");
            mCloseButton.SetPosition(440, 380);
            mCloseButton.SetSize(120, 40);
            mCloseButton.Text = Strings.Enchanting.Close;
            mCloseButton.Clicked += (sender, args) => mEnchantWindow.IsHidden = true;
            mEnchantWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            // Inicializar contenedor de ítems
            InitItemContainer();
            CreateProjectionSection();
        }
        private void CreateProjectionSection()
        {
            // Etiqueta de proyección
            mLblProjection = new Label(mEnchantWindow, "ProjectionLabel");
            mLblProjection.SetPosition(20, 20);
            mLblProjection.SetSize(260, 30);
            mLblProjection.Text = Strings.Enchanting.Projection;

            // Contenedor para la proyección
            mProjectionContainer = new ScrollControl(mEnchantWindow, "ProjectionContainer");
            mProjectionContainer.SetPosition(20, 60);
            mProjectionContainer.SetSize(260, 400);
            mProjectionContainer.EnableScroll(false, true);
        }
        private void InitItemContainer()
        {
            Items.Clear();
            Values.Clear();
            mInventoryScroll.DeleteAllChildren();

            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                Items.Add(new InventoryItem(this, i));
                Items[i].Container = new ImagePanel(mInventoryScroll, "Enchanttem");
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

                // Agregar un evento para seleccionar un ítem al hacer clic
                Items[i].Container.Clicked += (sender, args) =>
                {
                    var selectedItem = Globals.Me.Inventory[i];

                    // Validar que el ítem seleccionado es válido
                    if (selectedItem != null)
                    {
                        mSelectedItem = (Item)selectedItem;

                        SelectItem((Item)selectedItem);
                    }
                };
                // Agregar evento para seleccionar el ítem de currency con clic derecho
                Items[i].Container.RightClicked += (sender, args) =>
                {
                    var selectedItem = Globals.Me.Inventory[i];
                    if (selectedItem != null)
                    {
                        SelectCurrencyItem((Item)selectedItem);
                    }
                };

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

        public void SelectCurrencyItem(Item item)
        {
            if (item == null || item.Base == null)
            {
                return; // Salir si el ítem no es válido
            }

            mSelectedCurrency = item;

            // Actualizar el icono en el slot de currency
            var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
            mCurrencySlot.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture();
        }

        private void UpdateProjection()
        {
            if (mSelectedItem == null || mSelectedItem.Base == null)
            {
                mProjectionContainer.Hide(); // Limpia la vista si no hay ítem seleccionado
                return;
            }

          // mProjectionContainer.RemoveChild(base,true); // Reiniciar la vista previa

            var projectedLevel = mSelectedItem.EnchantmentLevel + 1;
            var successRate = mSelectedItem.Base.GetUpgradeSuccessRate(projectedLevel);

            // Si se usa un potenciador de éxito, aumenta la tasa de éxito
            /*if (mSelectedRateBoostItem != null)
            {
                successRate += mSelectedRateBoostItem.Base.GetBoostAmount();
            }*/

            // Agregar nivel actual y proyectado
            var lblCurrentLevel = new Label(mProjectionContainer, "CurrentLevelLabel")
            {
                Text = $"Nivel Actual: {mSelectedItem.EnchantmentLevel}",
                TextColor = Color.White
            };

            var lblProjectedLevel = new Label(mProjectionContainer, "ProjectedLevelLabel")
            {
                Text = $"Nivel Proyectado: {projectedLevel}",
                TextColor = Color.Green
            };

            var lblSuccessRate = new Label(mProjectionContainer, "SuccessRateLabel")
            {
                Text = $"Tasa de éxito: {successRate * 100:F1}%",
                TextColor = successRate >= 1.0 ? Color.Green : Color.Yellow
            };

            // Agregar las estadísticas proyectadas
            for (var i = 0; i < Enum.GetValues<Stat>().Length; i++)
            {
                var statName = Strings.ItemDescription.StatCounts[i];
                var currentStat = mSelectedItem.Base.StatsGiven[i] + (mSelectedItem.ItemProperties?.StatModifiers[i] ?? 0);
                var projectedStat = currentStat + (int)(currentStat * 0.05 * projectedLevel); // Mejora basada en nivel

                var statColor = projectedStat > currentStat ? Color.Green : (projectedStat < currentStat ? Color.Red : Color.White);
                var lblStat = new Label(mProjectionContainer, $"StatLabel_{i}")
                {
                    Text = $"{statName}: {currentStat} → {projectedStat}",
                    TextColor = statColor
                };
            }

            // Ajustar la vista
            mProjectionContainer.SizeToChildren(true, true);
        }
        /* public void SelectRateBoostItem(Item item)
         {
             if (item == null || item.Base == null) return;

             mSelectedRateBoostItem = item;
             var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
             mRateBoostSlot.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture();

             // Actualizar proyección con el nuevo éxito modificado
             UpdateProjection();
         }*/
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

        private void OnEnchantButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (mSelectedItem == null)
            {
                PacketSender.SendChatMsg("No item selected for enchanting.", 4);
                return;
            }

            if (mSelectedCurrency == null)
            {
                PacketSender.SendChatMsg("No currency item selected for enchanting.", 4);
                return;
            }

            // Obtener el índice del ítem seleccionado
            var itemIndex = Globals.Me.Inventory.IndexOf(mSelectedItem);

            if (itemIndex < 0)
            {
                PacketSender.SendChatMsg("Error identifying selected item.", 4);
                return;
            }

            // Obtener datos necesarios
            var currencyId = mSelectedCurrency.ItemId; // Usar ItemId para la moneda
            var targetLevel = mSelectedItem.EnchantmentLevel + 1;
            var currencyAmount = mSelectedItem.Base.GetUpgradeCost(targetLevel);
            var useAmulet = mUseAmuletCheckbox.IsChecked;

            // Validar nivel máximo de encantamiento
            if (targetLevel > 10)
            {
                PacketSender.SendChatMsg("Item is already at max enchantment level.", 4);
                return;
            }

            // Enviar paquete al servidor con el índice del ítem y el ItemId de la moneda
            PacketSender.SendEnchantItem(itemIndex, targetLevel, currencyId, currencyAmount, useAmulet);
        }



        public void Show()
        {
            mEnchantWindow.IsHidden = false;
        }

        public void Hide()
        {
            mEnchantWindow.IsHidden = true;
        }

        public bool IsVisible()
        {
            return !mEnchantWindow.IsHidden;
        }
    }
}
