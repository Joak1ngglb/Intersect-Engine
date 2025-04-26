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
        private List<EnchantInventoryItem> Items = new List<EnchantInventoryItem>();
        private List<Label> Values = new List<Label>();

        private Item mSelectedItem;
        private Item mSelectedCurrency;
        private Label lblCurrentLevel;
        private Label lblProjectedLevel;
        private Label lblSuccessRate;
        private Label lblCost;
        private Label lblStat;

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
     
    // Crear controles para los labels en el contenedor de proyección
            lblCurrentLevel = new Label(mProjectionContainer, "CurrentLevelLabel")
            {
                
                TextColor = Color.Yellow
            };
            lblCurrentLevel.SetPosition(10, 10);
            lblCurrentLevel.SetSize(240, 20);
            lblCurrentLevel.FontName = "sourcesansproblack";
            lblCurrentLevel.FontSize = 10;

            lblProjectedLevel = new Label(mProjectionContainer, "ProjectedLevelLabel")
            {
           
                TextColor = Color.Green
            };
            lblProjectedLevel.SetPosition(10, 40);
            lblProjectedLevel.SetSize(240, 20);
            lblProjectedLevel.FontName = "sourcesansproblack";
            lblProjectedLevel.FontSize = 10;

            lblSuccessRate = new Label(mProjectionContainer, "SuccessRateLabel")
            {
              
                TextColor = Color.Yellow
            };
            lblSuccessRate.SetPosition(10, 70);
            lblSuccessRate.SetSize(240, 20);
            lblSuccessRate.FontName = "sourcesansproblack";
            lblSuccessRate.FontSize = 10;

            lblCost = new Label(mProjectionContainer, "UpgradeCostLabel")
            {
               
                TextColor = Color.Yellow
            };
            lblCost.SetPosition(10, 100);
            lblCost.SetSize(240, 20);
            lblCost.FontName = "sourcesansproblack";
            lblCost.FontSize = 10;

            lblStat = new Label(mProjectionContainer, "StatLabel")
            {
       
                TextColor = Color.White
            };
            lblStat.SetPosition(10, 130);
            lblStat.SetSize(240, 20);
            lblStat.FontName = "sourcesansproblack";
            lblStat.FontSize = 10;
            mEnchantWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            // Inicializar contenedor de ítems
            InitItemContainer();
     
        }
       
        private void InitItemContainer()
        {
            Items.Clear();
            Values.Clear();
            mInventoryScroll.DeleteAllChildren();

            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                Items.Add(new EnchantInventoryItem(this, i));
                Items[i].Container = new ImagePanel(mInventoryScroll, "Enchanttem");
                Items[i].Setup();

                Values.Add(new Label(Items[i].Container, "InventoryItemValue"));
                Values[i].Text = "";

                Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                int xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
                int yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;

                int containerWidth = Items[i].Container.Width + xPadding;
                int containerHeight = Items[i].Container.Height + yPadding;
                int scrollWidth = Math.Max(1, mInventoryScroll.Width); // Evita división por cero

                int columns = Math.Max(1, scrollWidth / containerWidth); // Asegura al menos una columna

                Items[i].Container.SetPosition(
                    (i % columns) * containerWidth + xPadding,
                    (i / columns) * containerHeight + yPadding
                );

                // Captura segura del índice
                int index = i;

                Items[index].Container.Clicked += (sender, args) =>
                {
                    var selectedItem = Globals.Me.Inventory[index];

                    if (selectedItem != null)
                    {
                        mSelectedItem = (Item)selectedItem;
                        SelectItem((Item)selectedItem);
                    }
                };

                Items[index].Container.RightClicked += (sender, args) =>
                {
                    var selectedItem = Globals.Me.Inventory[index];
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

        public void UpdateProjection()
        {
            if (mSelectedItem == null || mSelectedItem.Base == null)
            {
                mProjectionContainer.Hide();
                return;
            }

            foreach (var child in mProjectionContainer.Children.ToList())
            {
                mProjectionContainer.RemoveChild(child, true);
            }
            mProjectionContainer.Show();

            var projectedLevel = mSelectedItem.ItemProperties.EnchantmentLevel + 1;
            var successRate = mSelectedItem.Base.GetUpgradeSuccessRate(projectedLevel);
            var upgradeCost = mSelectedItem.Base.GetUpgradeCost(projectedLevel); // Supuesto método para obtener costo

            int yOffset = 0;
            int spacing = 25;
            int labelWidth = 240;
            int labelHeight = 20;
            // Nivel Actual
            lblCurrentLevel = new Label(mProjectionContainer, "CurrentLevelLabel")
            {
                Text = $"Nivel Actual: {mSelectedItem.ItemProperties.EnchantmentLevel}"
            };
            lblCurrentLevel.SetPosition(10, yOffset);
            lblCurrentLevel.SetSize(labelWidth, labelHeight);
            lblCurrentLevel.SetTextColor(Color.Yellow, Label.ControlState.Normal);
            lblCurrentLevel.FontName = "sourcesansproblack";
            lblCurrentLevel.FontSize = 10;

            yOffset += spacing;

            // Nivel Proyectado
            lblProjectedLevel = new Label(mProjectionContainer, "ProjectedLevelLabel")
            {
                Text = $"Nivel Proyectado: {projectedLevel}"
            };
            lblProjectedLevel.SetPosition(10, yOffset);
            lblProjectedLevel.SetSize(labelWidth, labelHeight);
            lblProjectedLevel.SetTextColor(Color.Green, Label.ControlState.Normal);
            lblProjectedLevel.FontName = "sourcesansproblack";
            lblProjectedLevel.FontSize = 10;

            yOffset += spacing;

            // Tasa de éxito
            lblSuccessRate = new Label(mProjectionContainer, "SuccessRateLabel")
            {
                Text = $"Tasa de éxito: {successRate * 100:F1}%"
            };
            lblSuccessRate.SetPosition(10, yOffset);
            lblSuccessRate.SetSize(labelWidth, labelHeight);
            lblSuccessRate.SetTextColor(Color.Yellow, Label.ControlState.Normal);
            lblSuccessRate.FontName = "sourcesansproblack";
            lblSuccessRate.FontSize = 10;

            yOffset += spacing;

            // Costo de encantamiento
            lblCost = new Label(mProjectionContainer, "UpgradeCostLabel")
            {
                Text = $"Costo: {upgradeCost} monedas"
            };
            lblCost.SetPosition(10, yOffset);
            lblCost.SetSize(labelWidth, labelHeight);
            lblCost.SetTextColor(Color.Yellow, Label.ControlState.Normal);
            lblCost.FontName = "sourcesansproblack";
            lblCost.FontSize = 10;

            yOffset += spacing;


            yOffset += spacing * 2;

            for (var i = 0; i < Enum.GetValues<Stat>().Length; i++)
            {
                var statName = Strings.ItemDescription.StatCounts[i];
                int baseStat = mSelectedItem.Base.StatsGiven[i];
                int modStat = mSelectedItem.ItemProperties?.StatModifiers[i] ?? 0;
                int currentStat = baseStat + modStat;

                int projectedStat = currentStat;

           
                double bonusFactor = 0.05;

                for (int lvl = mSelectedItem.ItemProperties.EnchantmentLevel + 1; lvl <= projectedLevel; lvl++)
                {
                    int bonus = (int)Math.Ceiling(projectedStat * bonusFactor);
                    projectedStat += bonus;

                    // Solo simulación visual, no modificar EnchantmentRolls reales si no es necesario
                }

                if (currentStat == 0 && projectedStat == 0)
                {
                    continue;
                }

                var statColor = projectedStat > currentStat ? Color.Green :
                                (projectedStat < currentStat ? Color.Red : Color.White);

                var lblStat = new Label(mProjectionContainer, $"StatLabel_{i}")
                {
                    Text = $"{statName}: {currentStat} → {projectedStat}",
                };
                lblStat.SetPosition(10, yOffset);
                lblStat.SetSize(labelWidth, labelHeight);
                lblStat.SetTextColor(statColor, Label.ControlState.Normal);
                lblStat.FontName = "sourcesansproblack";
                lblStat.FontSize = 10;

                yOffset += spacing;
            }

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
