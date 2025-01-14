using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Chat;
using Intersect.Client.Interface.Game.Inventory;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
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
        private ScrollControl mItemContainer;
        private Button mEnchantButton, mCloseButton;

        private bool mInitializedItems = false;
        private List<EnchantInventoryItem> Items = new List<EnchantInventoryItem>();
        private List<Label> mValues = new List<Label>();

        private Item mSelectedItem;
        private Item mSelectedCurrency;

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
            mItemContainer = new ScrollControl(mEnchantWindow, "ItemContainer");
            mItemContainer.SetPosition(300, 20);
            mItemContainer.SetSize(260, 200);
            mItemContainer.EnableScroll(false, true);

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
        }

        private void InitItemContainer()
        {
            for (var i = 0; i < Options.MaxInvItems; i++)
            {
                // Crear un nuevo objeto InventoryItem y agregarlo a la lista
                Items.Add(new EnchantInventoryItem(this, i));
                Items[i].Container = new ImagePanel(mItemContainer, "InventoryItem");
                Items[i].Setup();

                // Crear un Label para mostrar la cantidad del ítem y agregarlo a la lista
                mValues.Add(new Label(Items[i].Container, "InventoryItemValue"));
                mValues[i].Text = string.Empty;

                // Cargar la interfaz del contenedor desde el archivo JSON
                Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

               

                // Calcular el espaciado para posicionar los ítems en una cuadrícula
                var xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
                var yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;
                Items[i]
                    .Container.SetPosition(
                        i % (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                        i / (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
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
                return; // Salir si el ítem no es válido
            }

            mSelectedItem = item;
          
            // Actualizar la interfaz para reflejar el ítem seleccionado
            var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Base.Icon);
            mItemSlot.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture();
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


        public void Update()
        {
            if (!mInitializedItems)
            {
                mInitializedItems = true;
                InitItemContainer();
            }

            if (Globals.Me == null || Globals.Me.Inventory == null)
            {
                return; // Salir si el jugador o su inventario no están inicializados
            }

            for (var i = 0; i < Options.MaxInvItems; i++)
            {
                if (i >= Items.Count)
                {
                    continue; // Evitar índices fuera de rango en la lista Items
                }

                var inventoryItem = Globals.Me.Inventory[i];
                if (inventoryItem == null || Items[i] == null)
                {
                    Items[i].Container.IsHidden = true; // Ocultar ítems no válidos
                    continue;
                }

                var itemBase = ItemBase.Get(inventoryItem.ItemId);
                if (itemBase != null && itemBase.CanBeEnchanted && itemBase.ItemType == ItemType.None)
                {
                    Items[i].Container.IsHidden = false;
                    Items[i].Update();
                }
                else
                {
                    Items[i].Container.IsHidden = true;
                }
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

            // Obtener datos necesarios
            var itemId = mSelectedItem.ItemId;
            var targetLevel = mSelectedItem.EnchantmentLevel + 1;
            var currencyId = mSelectedCurrency.ItemId;
            var currencyAmount = mSelectedItem.Base.GetUpgradeCost(targetLevel);
            var useAmulet = mUseAmuletCheckbox.IsChecked;

            // Validar nivel máximo de encantamiento
            if (targetLevel > 10)
            {
                PacketSender.SendChatMsg("Item is already at max enchantment level.", 4);
                return;
            }

            // Enviar paquete al servidor
            PacketSender.SendEnchantItem(itemId, targetLevel, currencyId, currencyAmount, useAmulet);
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
