using System;
using System.Collections.Generic;
using System.Linq;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Inventory;
using Intersect.Client.Networking;
using Intersect.Client.Utilities;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Network.Packets.Client;
using Intersect.Network.Packets.Server;
using MonoMod.Core.Platforms;

namespace Intersect.Client.Interface.Game.AuctionHouse
{
    public class AuctionHouseWindow
    {
        #region Constantes de Diseño

        private const int WindowWidth = 800;
        private const int WindowHeight = 600;
        private const int Margin = 10;
        private const int TopBarHeight = 50;      // Espacio para pestañas y otros controles superiores
        private const int TabButtonWidth = 100;
        private const int TabButtonHeight = 30;
        public WindowControl RootWindow
        {
            get { return mAuctionWindow; }
        }

        // Panel de compra (BuyPanel)
        private static readonly int BuyPanelWidth = WindowWidth - 2 * Margin;  // 800 - 20 = 780
        private static readonly int BuyPanelHeight = WindowHeight - TopBarHeight - Margin; // 600 - 50 - 10 = 540

        // Controles de la pestaña de compra
        private const int CategoryFilterWidth = 150;
        private const int CategoryFilterHeight = 25;
        private const int SearchBoxWidth = 200;
        private const int SearchBoxHeight = 25;
        private const int SearchButtonWidth = 100;
        private const int SearchButtonHeight = 25;
        private const int SortFilterWidth = 150;
        private const int SortFilterHeight = 25;
        // La lista de ítems se ubica debajo de estos controles
        private static readonly int ItemListX = Margin;
        private static readonly int ItemListY = Margin + CategoryFilterHeight + Margin; // 10 + 25 + 10 = 45
        private static readonly int ItemListWidth = BuyPanelWidth - 2 * Margin;          // 780 - 20 = 760
        private static readonly int ItemListHeight = BuyPanelHeight - (CategoryFilterHeight + 2 * Margin); // 540 - 45 = 495

        // Panel de venta (SellPanel) usa las mismas dimensiones que el BuyPanel
        private static readonly int SellPanelWidth = BuyPanelWidth;  // 780
        private static readonly int SellPanelHeight = BuyPanelHeight; // 540

        // Controles de la pestaña de venta
        // Contenedor del inventario (lado izquierdo)
        private const int InventoryContainerWidth = 370;
        private const int InventoryContainerHeight = 400;
        // Lista de ítems en venta (lado derecho)
        private const int SellItemListWidth = 380;
        private const int SellItemListHeight = 400;
        // Caja de precio y botón para confirmar la venta
        private const int SellPriceBoxWidth = 150;
        private const int SellPriceBoxHeight = 25;
        private const int ConfirmSellButtonWidth = 150;
        private const int ConfirmSellButtonHeight = 25;

        // Para los ítems del inventario en grilla
        private const int InventoryItemSize = 50;
        private const int InventoryItemMargin = 5;
        private const int InventoryItemsPerRow = 5;

        // Para el botón de cerrar la ventana
        private const int CloseButtonSize = 30;

        #endregion

        #region Controles Principales

        private WindowControl mAuctionWindow;
        private Dictionary<AuctionHouseTab, Button> mTabButtons;
        private AuctionHouseTab mCurrentTab = AuctionHouseTab.Buy;
        private Button mCloseButton;

        // Paneles
        private ImagePanel mBuyPanel;
        private ImagePanel mSellPanel;

        // Controles de compra
        private ComboBox mCategoryFilter;
        private TextBox mSearchBox;
        private Button mSearchButton;
        private ComboBox mSortFilter;
        private ListBox mItemList;

        // Controles de venta
        private ScrollControl mInventoryContainer;
        private ListBox mSellItemList;
        private TextBoxNumeric mSellPriceBox;
        private Button mConfirmSellButton;
        private Guid mSelectedItemId;

        // Menú contextual (compartido para la pestaña de compra)
        private Framework.Gwen.Control.Menu mContextMenu;

        // Lista global de órdenes (se actualizará con la información recibida del servidor)
        private List<AuctionHouseOrderInfo> allAuctionOrders = new List<AuctionHouseOrderInfo>();

        private List<SellAuctionItem> SellItems = new List<SellAuctionItem>();
        private bool mInitializedSellItems = false;

        #endregion

        #region Constructor e Inicialización

        public AuctionHouseWindow(Canvas gameCanvas)
        {
            // Crear la ventana principal y asignar tamaño y posición
            mAuctionWindow = new WindowControl(gameCanvas, "Auction House", false, "AuctionHouseWindow");
            mAuctionWindow.SetSize(WindowWidth, WindowHeight);
            mAuctionWindow.SetPosition(100, 100); // Posición de ejemplo (se puede centrar en el canvas)
            mAuctionWindow.DisableResizing();

            // Botón de cierre (en la esquina superior derecha)
            mCloseButton = new Button(mAuctionWindow, "CloseButton");
            mCloseButton.SetText("❌");
            mCloseButton.SetSize(CloseButtonSize, CloseButtonSize);
            mCloseButton.SetPosition(WindowWidth - Margin - CloseButtonSize, Margin);
            mCloseButton.Clicked += (sender, args) => mAuctionWindow.IsHidden = true;

            // Inicializar pestañas, paneles y menú contextual
            InitializeTabs();
            InitializeBuyPanel();
            InitializeSellPanel();
            InitializeContextMenu();

            // Mostrar inicialmente la pestaña de compra
            mBuyPanel.IsHidden = false;
            mSellPanel.IsHidden = true;

            // Cargar la configuración de la UI (desde JSON, si se requiere)
            mAuctionWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }

        private void InitializeTabs()
        {
            mTabButtons = new Dictionary<AuctionHouseTab, Button>();
            int tabIndex = 0;
            foreach (AuctionHouseTab tab in Enum.GetValues(typeof(AuctionHouseTab)))
            {
                if (tab == AuctionHouseTab.Count)
                    continue;

                var tabButton = new Button(mAuctionWindow, $"{tab}TabButton");
                tabButton.SetText(tab.ToString());
                tabButton.SetSize(TabButtonWidth, TabButtonHeight);
                // Ubicar cada pestaña en la parte superior (separadas por margen)
                int posX = Margin + tabIndex * (TabButtonWidth + Margin);
                tabButton.SetPosition(posX, Margin);
                tabButton.UserData = tab;
                tabButton.Clicked += OnTabClicked;
                mTabButtons.Add(tab, tabButton);
                tabIndex++;
            }
        }

        private void InitializeBuyPanel()
        {
            mBuyPanel = new ImagePanel(mAuctionWindow, "BuyPanel");
            // Ubicar el panel justo debajo de las pestañas
            mBuyPanel.SetPosition(Margin, TopBarHeight);
            mBuyPanel.SetSize(BuyPanelWidth, BuyPanelHeight);

            // Filtro de categoría (arriba a la izquierda del panel)
            mCategoryFilter = new ComboBox(mBuyPanel, "CategoryFilter");
            mCategoryFilter.SetSize(CategoryFilterWidth, CategoryFilterHeight);
            mCategoryFilter.SetPosition(Margin, Margin);
            foreach (AuctionCategory category in Enum.GetValues(typeof(AuctionCategory)))
            {
                if (category == AuctionCategory.Count)
                    continue;
                mCategoryFilter.AddItem(category.ToString());
            }

            // Caja de búsqueda (a la derecha del filtro de categoría)
            mSearchBox = new TextBox(mBuyPanel, "SearchBox");
            int searchBoxX = Margin + CategoryFilterWidth + Margin; // 10 + 150 + 10 = 170
            mSearchBox.SetSize(SearchBoxWidth, SearchBoxHeight);
            mSearchBox.SetPosition(searchBoxX, Margin);

            // Botón de búsqueda (a la derecha de la caja de búsqueda)
            mSearchButton = new Button(mBuyPanel, "SearchButton");
            mSearchButton.SetText("🔍 Buscar");
            int searchButtonX = searchBoxX + SearchBoxWidth + Margin; // 170 + 200 + 10 = 380
            mSearchButton.SetSize(SearchButtonWidth, SearchButtonHeight);
            mSearchButton.SetPosition(searchButtonX, Margin);
            mSearchButton.Clicked += OnSearchClicked;

            // ComboBox de ordenamiento (en la esquina superior derecha del panel)
            mSortFilter = new ComboBox(mBuyPanel, "SortFilter");
            mSortFilter.SetSize(SortFilterWidth, SortFilterHeight);
            int sortFilterX = BuyPanelWidth - Margin - SortFilterWidth; // 780 - 10 - 150 = 620
            mSortFilter.SetPosition(sortFilterX, Margin);
            mSortFilter.AddItem("🔼 Precio Ascendente", "", 0);
            mSortFilter.AddItem("🔽 Precio Descendente", "", 1);
            mSortFilter.AddItem("🔼 Cantidad Ascendente", "", 2);
            mSortFilter.AddItem("🔽 Cantidad Descendente", "", 3);
            mSortFilter.AddItem("📜 Tipo de Ítem", "", 4);
            mSortFilter.ItemSelected += OnSortFilterChanged;

            // Lista de ítems en subasta
            mItemList = new ListBox(mBuyPanel, "ItemList");
            mItemList.SetPosition(ItemListX, ItemListY);
            mItemList.SetSize(ItemListWidth, ItemListHeight);
        }

        private void InitializeSellPanel()
        {
            mSellPanel = new ImagePanel(mAuctionWindow, "SellPanel");
            mSellPanel.SetPosition(Margin, TopBarHeight);
            mSellPanel.SetSize(SellPanelWidth, SellPanelHeight);

            // Contenedor del inventario (lado izquierdo)
            mInventoryContainer = new ScrollControl(mSellPanel, "InventoryContainer");
            mInventoryContainer.SetSize(InventoryContainerWidth, InventoryContainerHeight);
            mInventoryContainer.SetPosition(Margin, Margin);

            // Lista de ítems actualmente en venta (lado derecho)
            mSellItemList = new ListBox(mSellPanel, "SellItemList");
            int sellItemListX = Margin + InventoryContainerWidth + Margin; // 10 + 370 + 10 = 390
            mSellItemList.SetSize(SellItemListWidth, SellItemListHeight);
            mSellItemList.SetPosition(sellItemListX, Margin);

            // Caja para ingresar el precio de venta (debajo del contenedor de inventario)
            mSellPriceBox = new TextBoxNumeric(mSellPanel, "SellPriceBox");
            int sellPriceBoxY = Margin + InventoryContainerHeight + Margin; // 10 + 400 + 10 = 420
            mSellPriceBox.SetSize(SellPriceBoxWidth, SellPriceBoxHeight);
            mSellPriceBox.SetPosition(Margin, sellPriceBoxY);

            // Botón para confirmar la venta (a la derecha de la caja de precio)
            mConfirmSellButton = new Button(mSellPanel, "ConfirmSellButton");
            mConfirmSellButton.SetText("✅ Poner en Venta");
            int confirmSellButtonX = Margin + SellPriceBoxWidth + Margin; // 10 + 150 + 10 = 170
            mConfirmSellButton.SetSize(ConfirmSellButtonWidth, ConfirmSellButtonHeight);
            mConfirmSellButton.SetPosition(confirmSellButtonX, sellPriceBoxY);
            mConfirmSellButton.Clicked += OnConfirmSellClicked;

            // Cargar los ítems del inventario en el contenedor
           InitSellItemContainer();
        }

        private void InitializeContextMenu()
        {
            mContextMenu = new Framework.Gwen.Control.Menu(mAuctionWindow, "ContextMenu");
            mContextMenu.IsHidden = true;
            mContextMenu.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }

        #endregion

        #region Eventos y Funciones de la UI

        private void OnTabClicked(Base sender, EventArgs args)
        {
            var tab = (AuctionHouseTab)sender.UserData;
            mCurrentTab = tab;
            // Habilitar todos los botones y deshabilitar el seleccionado
            foreach (var btn in mTabButtons.Values)
            {
                btn.Enable();
            }
            sender.Disable();

            // Mostrar el panel correspondiente según la pestaña seleccionada
            mBuyPanel.IsHidden = (tab != AuctionHouseTab.Buy);
            mSellPanel.IsHidden = (tab != AuctionHouseTab.Sell);
        }

        private void OnSearchClicked(Base sender, ClickedEventArgs args)
        {
            string searchText = mSearchBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                new MessageBox(mAuctionWindow, "Debes ingresar un nombre de ítem para buscar.", "Error").Show();
                return;
            }

            // Filtrar localmente la lista de órdenes de subasta
            var filteredOrders = allAuctionOrders
                .Where(order => SearchHelper.IsSearchable(ItemBase.Get(order.ItemId).Name, searchText))
                .ToList();

            UpdateAuctionItems(filteredOrders);
        }

        private void OnSortFilterChanged(Base sender, ItemSelectedEventArgs args)
        {
            int selectedSort = (int)mSortFilter.SelectedItem.UserData;
            ApplySorting(selectedSort);
        }

        private void ApplySorting(int selectedSort)
        {
            List<AuctionHouseOrderInfo> sortedOrders = selectedSort switch
            {
                0 => allAuctionOrders.OrderBy(order => order.Price).ToList(),
                1 => allAuctionOrders.OrderByDescending(order => order.Price).ToList(),
                2 => allAuctionOrders.OrderBy(order => order.Quantity).ToList(),
                3 => allAuctionOrders.OrderByDescending(order => order.Quantity).ToList(),
                4 => allAuctionOrders.OrderBy(order => ItemBase.Get(order.ItemId).Type).ToList(),
                _ => allAuctionOrders
            };

            UpdateAuctionItems(sortedOrders);
        }

        /// <summary>
        /// Actualiza la lista de órdenes de la Auction House.
        /// Se crea un objeto AuctionItem por cada orden, se configura y se agrega al ListBox.
        /// </summary>
        public void UpdateAuctionItems(List<AuctionHouseOrderInfo> orders)
        {
            mItemList.Clear();
            foreach (var order in orders)
            {
                // Crear el AuctionItem para la orden actual
                var auctionItem = new AuctionItem(this, order);
                auctionItem.Setup();
                // Agregar el contenedor (fila) al ListBox
                var row = mItemList.AddRow("");
                row.SetCellContents(0, auctionItem.Container);
            }
        }

        public void OpenContextMenu(Guid orderId)
        {
            mContextMenu.Children.Clear();

            var buyOption = mContextMenu.AddItem("🛒 Comprar");
            buyOption.Clicked += (sender, args) =>
                PacketSender.SendBuyAuctionItem(Globals.Me.Id, orderId, 1);

            var cancelOption = mContextMenu.AddItem("❌ Cancelar");
            // Acción para cancelar la orden (descomentar cuando se implemente)
            // cancelOption.Clicked += (sender, args) => PacketSender.SendCancelAuctionItem(Globals.Me.Id, orderId);

            mContextMenu.SizeToChildren();
            mContextMenu.Open(Framework.Gwen.Pos.None);
        }

        private void InitSellItemContainer()
        {
            // Limpiar los ítems ya cargados en el contenedor
            SellItems.Clear();
            mInventoryContainer.DeleteAll();

            int itemsPerRow = Math.Max(3, mInventoryContainer.Width / (InventoryItemSize + InventoryItemMargin)); // Mínimo 3 columnas
            int row = 0, col = 0;

            for (var i = 0; i < Globals.Me.Inventory.Length; i++)
            {
                var itemSlot = Globals.Me.Inventory[i];
                if (itemSlot == null || itemSlot.ItemId == Guid.Empty)
                    continue; // Saltar slots vacíos

                // Crear el SellAuctionItem
                var sellItem = new SellAuctionItem(this, i);
                SellItems.Add(sellItem);

                // Asignar el contenedor visual
                sellItem.Container = new ImagePanel(mInventoryContainer, "SellAuctionItem");
                sellItem.Setup();
                sellItem.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                // Calcular posición en la grilla
                int posX = col * (InventoryItemSize + InventoryItemMargin);
                int posY = row * (InventoryItemSize + InventoryItemMargin);
                sellItem.Container.SetPosition(posX, posY);

                // Incrementar columnas y filas
                col++;
                if (col >= itemsPerRow)
                {
                    col = 0;
                    row++;
                }
            }
        }

        public void Update()
        {
            if (!mInitializedSellItems)
            {
                mInitializedSellItems = true;
                InitSellItemContainer();
            }

            if (mSellPanel.IsHidden)
                return;

            if (SellItems.Count != Globals.Me.Inventory.Length) // Verificar con el inventario real
            {
                InitSellItemContainer(); // Re-sincronizar listas
            }

            for (var i = 0; i < Globals.Me.Inventory.Length; i++)
            {
                if (i >= SellItems.Count) // Prevenir acceso fuera de rango
                    break;

                var inventorySlot = Globals.Me.Inventory[i];
                if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
                {
                    SellItems[i].Container.IsHidden = true;
                    continue;
                }

                var item = ItemBase.Get(inventorySlot.ItemId);
                if (item != null)
                {
                    SellItems[i].Container.IsHidden = false;

                    // Aplicar color de rareza
                    SellItems[i].Container.RenderColor = CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor)
                        ? rarityColor
                        : Color.White;

                    // Actualizar visualización interna del ítem
                    SellItems[i].Update();
                }
                else
                {
                    SellItems[i].Container.IsHidden = true;
                }
            }
        }

        public void SelectItemForSale(int slot, Guid itemId)
        {
            if (itemId == Guid.Empty)
                return;

            mSelectedItemId = itemId;

            // Resaltar el ítem seleccionado cambiando su color
            foreach (var item in SellItems)
            {
                item.Container.RenderColor = Color.White; // Resetear color de todos los ítems
            }

            SellItems[slot].Container.RenderColor = new Color(255, 200, 50); // Resaltar el ítem seleccionado

            new MessageBox(mAuctionWindow, $"Seleccionaste {ItemBase.Get(itemId).Name} para la venta.", "Información").Show();
        }

        // Método que se llama al hacer clic en el botón "Poner en Venta"
        private void OnConfirmSellClicked(Base sender, EventArgs args)
        {
            if (mSelectedItemId == Guid.Empty)
            {
                new MessageBox(mAuctionWindow, "Debes seleccionar un ítem para vender.", "Error").Show();
                return;
            }

            int price = (int)mSellPriceBox.Value;
            if (price <= 0)
            {
                new MessageBox(mAuctionWindow, "El precio debe ser mayor a 0.", "Error").Show();
                return;
            }

            // Enviar la orden de venta al servidor
            PacketSender.SendCreateSellOrder(Guid.NewGuid(), mSelectedItemId, 1, price);
        }


        public void Show()
        {
            mAuctionWindow.IsHidden = false;
        }

        public void Hide()
        {
            mAuctionWindow.IsHidden = true;
        }

   

        #endregion
    }

    #region Enumeraciones

    public enum AuctionHouseTab
    {
        Buy,  // 🛒 Comprar
        Sell, // 💰 Vender
        Count
    }

    public enum AuctionCategory
    {
        Armors,      // 🏰 Armaduras
        Helmets,     // ⛑️ Cascos
        Boots,       // 🥾 Botas
        Weapons,     // ⚔️ Armas
        Consumables, // 🍖 Consumibles
        Accessories, // 💍 Accesorios
        Karmas,      // 💠 Karmas
        Misc,        // 🎲 Misceláneos
        Count
    }

    #endregion
}
