using System;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Network.Packets.Client;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Interface.Game.AuctionHouse
{
    public partial class AuctionItem
    {
        // Contenedor principal de la fila para la orden de subasta.
        public ImagePanel Container;
        // Panel para mostrar el ícono del ítem (similar a "Pnl" en InventoryItem).
        public ImagePanel Pnl;

        // Controles adicionales para mostrar datos del ítem.
        private Label mNameLabel;
        private Label mQuantityLabel;
        private Label mPriceLabel;
        private Button mBuyButton;

        // Referencias a la ventana de Auction House y la orden actual.
        private AuctionHouseWindow mAuctionWindow;
        private AuctionHouseOrderInfo mOrder;
        private ItemDescriptionWindow mDescWindow;

        /// <summary>
        /// Crea una instancia de AuctionItem con la información de la orden.
        /// </summary>
        /// <param name="auctionWindow">Referencia a la ventana de Auction House.</param>
        /// <param name="order">Información de la orden de subasta.</param>
        public AuctionItem(AuctionHouseWindow auctionWindow, AuctionHouseOrderInfo order)
        {
            mAuctionWindow = auctionWindow;
            mOrder = order;
        }

        /// <summary>
        /// Configura el control creando el contenedor (Container) y sus controles hijos,
        /// asignando tamaños, posiciones y eventos.
        /// </summary>
        public void Setup()
        {
            // Crear el contenedor principal de la fila.
            // Se asume un tamaño fijo de 760x40 (ajústalo según necesites).

            Container.SetSize(760, 40);

            // Crear el panel para el ícono del ítem (Pnl) como hijo del contenedor.
            Pnl = new ImagePanel(Container, "AuctionItemIcon");
            Pnl.SetSize(32, 32);
            Pnl.SetPosition(5, 4);
            // Asignar eventos al panel del ícono.
            Pnl.HoverEnter += Pnl_HoverEnter;
            Pnl.HoverLeave += Pnl_HoverLeave;
            Pnl.RightClicked += Pnl_RightClicked;
            Pnl.DoubleClicked += Pnl_DoubleClicked;

            // Cargar la textura del ítem usando la información obtenida de ItemBase.
            var itemData = ItemBase.Get(mOrder.ItemId);
            Pnl.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, itemData.Icon);

            // Crear la etiqueta para el nombre del ítem.
            mNameLabel = new Label(Container, "AuctionItemName");
            mNameLabel.Text = itemData.Name;
            mNameLabel.SetPosition(42, 5);  // 32px (ícono) + ~10px de margen
            mNameLabel.SetSize(200, 30);

            // Crear la etiqueta para mostrar la cantidad.
            mQuantityLabel = new Label(Container, "AuctionItemQuantity");
            mQuantityLabel.Text = $"x{mOrder.Quantity}";
            mQuantityLabel.SetPosition(250, 5);
            mQuantityLabel.SetSize(50, 30);

            // Crear la etiqueta para el precio.
            mPriceLabel = new Label(Container, "AuctionItemPrice");
            mPriceLabel.Text = $"{mOrder.Price} 🪙";
            mPriceLabel.SetPosition(310, 5);
            mPriceLabel.SetSize(100, 30);

            // Crear el botón de "Comprar".
            mBuyButton = new Button(Container, "BuyButton");
            mBuyButton.SetText("🛒 Comprar");
            mBuyButton.SetSize(100, 30);
            mBuyButton.SetPosition(620, 5);
            mBuyButton.Clicked += OnBuyClicked;
        }

        /// <summary>
        /// Evento: al pasar el ratón sobre el ícono, se puede mostrar la descripción del ítem.
        /// </summary>
        private void Pnl_HoverEnter(Base sender, EventArgs args)
        {
            var itemData = ItemBase.Get(mOrder.ItemId);
            // Opcional: crea y muestra una ventana de descripción cerca del cursor.
            // Aquí se muestra un ejemplo básico.
            if (mDescWindow == null)
            {
                mDescWindow = new ItemDescriptionWindow(itemData, mOrder.Quantity, 0, 0, null);
            }
        }

        /// <summary>
        /// Evento: al salir con el ratón del ícono, se cierra la descripción.
        /// </summary>
        private void Pnl_HoverLeave(Base sender, EventArgs args)
        {
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
        }

        /// <summary>
        /// Evento: al hacer clic derecho sobre el ícono, se abre el menú contextual.
        /// </summary>
        private void Pnl_RightClicked(Base sender, ClickedEventArgs args)
        {
            // Se asume que AuctionHouseWindow tiene un método OpenContextMenu que recibe el OrderId.
            mAuctionWindow.OpenContextMenu(mOrder.OrderId);
        }

        /// <summary>
        /// Evento: al hacer doble clic sobre el ícono, se intenta comprar el ítem.
        /// </summary>
        private void Pnl_DoubleClicked(Base sender, ClickedEventArgs args)
        {
            PacketSender.SendBuyAuctionItem(Globals.Me.Id, mOrder.OrderId, 1);
        }

        /// <summary>
        /// Evento: al hacer clic en el botón "Comprar", se muestra un mensaje de confirmación y se
        /// envía el paquete de compra si se confirma.
        /// </summary>
        private void OnBuyClicked(Base sender, ClickedEventArgs args)
        {
            var itemData = ItemBase.Get(mOrder.ItemId);
            // Usamos mAuctionWindow.RootWindow en lugar de mAuctionWindow.
            var confirmation = new MessageBox(mAuctionWindow.RootWindow,
                $"¿Comprar {itemData.Name} por {mOrder.Price} 🪙?", "Confirmar Compra");
            confirmation.Dismissed += (s, e) =>
            {
                PacketSender.SendBuyAuctionItem(Globals.Me.Id, mOrder.OrderId, 1);
            };
            confirmation.Show();
        }


        /// <summary>
        /// Actualiza los datos mostrados en el control con una nueva orden.
        /// </summary>
        /// <param name="order">La orden actualizada.</param>
        public void UpdateItem(AuctionHouseOrderInfo order)
        {
            mOrder = order;
            var itemData = ItemBase.Get(mOrder.ItemId);
            mNameLabel.Text = itemData.Name;
            mQuantityLabel.Text = $"x{mOrder.Quantity}";
            mPriceLabel.Text = $"{mOrder.Price} 🪙";
            Pnl.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, itemData.Icon);
        }
    }
}
