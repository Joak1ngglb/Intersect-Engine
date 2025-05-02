using Intersect.Client.Core;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Items;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game.Breaking
{
    public class BreakInventoryItem
    {
        public ImagePanel Container;
        public ImagePanel Pnl;

        private BreakItemWindow mBreakWindow;
        private int mMySlot;
        private ItemDescriptionWindow mDescWindow;

        public bool IsDragging;

        public BreakInventoryItem(BreakItemWindow breakWindow, int slot)
        {
            mBreakWindow = breakWindow;
            mMySlot = slot;
        }

        public void Setup()
        {
            Pnl = new ImagePanel(Container, "BreakInventoryItemIcon");
            Pnl.HoverEnter += OnHoverEnter;
            Pnl.HoverLeave += OnHoverLeave;
            Pnl.Clicked += OnClicked;
            Pnl.RightClicked += OnRightClick;
            Pnl.DoubleClicked += OnDoubleClick;
        }

        private void OnHoverEnter(Base sender, EventArgs arguments)
        {
            if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
            {
                return;
            }

            var item = Globals.Me.Inventory[mMySlot];
            if (item?.Base == null)
            {
                return;
            }

            mDescWindow?.Dispose();
            mDescWindow = new ItemDescriptionWindow(
                item.Base,
                item.Quantity,
                mBreakWindow.X,
                mBreakWindow.Y,
                item.ItemProperties
            );
        }

        private void OnHoverLeave(Base sender, EventArgs arguments)
        {
            mDescWindow?.Dispose();
            mDescWindow = null;
        }

        private void OnClicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
            {
                return;
            }

            var item = Globals.Me.Inventory[mMySlot];
            if (item?.Base != null)
            {
                mBreakWindow.SelectItem((Item)item);
            }
        }

        private void OnRightClick(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
            {
                return;
            }

            var item = Globals.Me.Inventory[mMySlot];
            if (item?.Base != null)
            {
                mBreakWindow.SelectItem((Item)item);
            }
        }

        private void OnDoubleClick(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
            {
                return;
            }

            var item = Globals.Me.Inventory[mMySlot];
            if (item?.Base != null)
            {
                mBreakWindow.SelectItem((Item)item);
            
            }
        }

        public void Update()
        {
            // Validar que Globals.Me y el inventario estén inicializados
            if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
            {
                Container.IsHidden = true; // Ocultar el ítem si las referencias no son válidas
                return;
            }

            var inventoryItem = Globals.Me.Inventory[mMySlot];

            // Validar que el ítem existe en la ranura actual
            if (inventoryItem == null || inventoryItem.Base == null)
            {
                Container.IsHidden = true; // Ocultar si no hay ítem en esta ranura
                return;
            }

            // Mostrar el contenedor si el ítem es válido
            Container.IsHidden = false;

            // Actualizar el ícono del ítem
            var itemBase = inventoryItem.Base;
            var itemTexture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, itemBase.Icon);

            Pnl.Texture = itemTexture ?? Graphics.Renderer.GetWhiteTexture(); // Mostrar textura predeterminada si no hay ícono
            Pnl.RenderColor = itemBase.Color;


        }
    }
}
