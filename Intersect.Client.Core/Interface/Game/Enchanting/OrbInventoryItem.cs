using Intersect.Client.Core;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Interface.Game.Inventory;

using Intersect.Client.Items;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Client.Interface.Game.Enchanting;

    public class OrbInventoryItem
    {
        public ImagePanel Container;
        public ImagePanel ItemIcon;

        private OrbItemWindow mOrbWindow;
        private int mMySlot;
        private ItemDescriptionWindow mDescWindow;

        public bool IsDragging;

        public ImagePanel Pnl;
    private Label mCooldownLabel;

    public OrbInventoryItem(OrbItemWindow orbWindow, int slot)
        {
            mOrbWindow = orbWindow;
            mMySlot = slot;
        }

        public void Setup()
        {
            Pnl = new ImagePanel(Container, "OrbInventoryItemIcon");
            Pnl.HoverEnter += OnHoverEnter;
            Pnl.HoverLeave += OnHoverLeave;
            Pnl.Clicked += OnClicked;
        Pnl.RightClicked += Onrightclick;
        Pnl.DoubleClicked += Pnl_DoubleClicked;
    }

    private void OnHoverLeave(Base sender, EventArgs arguments)
    {
        // Cerrar la ventana de descripción cuando el mouse salga del ítem
        mDescWindow?.Dispose();
        mDescWindow = null;
    }

    private void Pnl_DoubleClicked(Base sender, ClickedEventArgs arguments)
    {
        throw new NotImplementedException();
    }

    private void Onrightclick(Base sender, ClickedEventArgs arguments)
    {
        if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
        {
            return; // Salir si el inventario no está configurado o el índice es inválido
        }
        var item = Globals.Me.Inventory[mMySlot];
        if (item?.Base != null)
        {
            mOrbWindow.SelectOrbItem((Items.Item)item); // Notificar a la ventana que se ha seleccionado un ítem
        }
    }

    private void OnHoverEnter(Base sender, EventArgs arguments)
    {
        if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
        {
            return; // Salir si el inventario no está configurado o el índice es inválido
        }

        var item = Globals.Me.Inventory[mMySlot];
        if (item?.Base == null)
        {
            return; // Salir si el ítem es nulo o no tiene una base válida
        }

        // Mostrar la descripción del ítem
        mDescWindow?.Dispose();
        mDescWindow = new ItemDescriptionWindow(
            item.Base,
            item.Quantity,
           mOrbWindow.X,
           mOrbWindow.Y,
            item.ItemProperties
        );
    }
    private void OnClicked(Base sender, ClickedEventArgs arguments)
    {
        if (Globals.Me == null || Globals.Me.Inventory == null || mMySlot < 0 || mMySlot >= Globals.Me.Inventory.Length)
        {
            return; // Salir si el inventario no está configurado o el índice es inválido
        }

        var item = Globals.Me.Inventory[mMySlot];
        if (item?.Base != null)
        {
            mOrbWindow.SelectItem((Items.Item)item); // Notificar a la ventana que se ha seleccionado un ítem
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

