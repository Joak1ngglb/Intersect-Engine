using System.ComponentModel;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Utilities;

namespace Intersect.Client.Interface.Game.Inventory;

public partial class InventoryWindow : Window
{
    public List<SlotItem> Items { get; set; } = [];
    private readonly ScrollControl _slotContainer;
    private readonly ContextMenu _contextMenu;

    public InventoryWindow(Canvas gameCanvas) : base(gameCanvas, Strings.Inventory.Title, false, nameof(InventoryWindow))
    {
        DisableResizing();

        Alignment = [Alignments.Bottom, Alignments.Right];
        MinimumSize = new Point(x: 225, y: 327);
        Margin = new Margin(0, 0, 15, 60);
        IsVisibleInTree = false;
        IsResizable = false;
        IsClosable = true;

        _slotContainer = new ScrollControl(this, "ItemsContainer")
        {
            Dock = Pos.Fill,
            OverflowX = OverflowBehavior.Auto,
            OverflowY = OverflowBehavior.Scroll,
        };

        _contextMenu = new ContextMenu(gameCanvas, "InventoryContextMenu")
        {
            IsVisibleInParent = false,
            IconMarginDisabled = true,
            ItemFont = GameContentManager.Current.GetFont(name: "sourcesansproblack"),
            ItemFontSize = 10,
        };
    }

    protected override void EnsureInitialized()
    {
        LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        InitItemContainer();
    }

    public void OpenContextMenu(int slot)
    {
        if (Items.Count <= slot)
        {
            return;
        }

        Items[slot].OpenContextMenu();
    }

    public void Update()
    {
        if (!IsVisibleInParent)
        {
            return;
        }
        if (Items.Count != Options.MaxInvItems || mValues.Count != Options.MaxInvItems)
        {
            InitItemContainer(); // Re-sincronizar listas
        }
        IsClosable = Globals.CanCloseInventory;

        if (Globals.Me?.Inventory == default)
        {
            var inventorySlot = Globals.Me.Inventory[i];
            if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
            {
                // Si el slot es nulo o no tiene un ítem, oculta el panel
                Items[i].Pnl.IsHidden = true;
                mValues[i].IsHidden = true;
                continue;
            }

            var item = ItemBase.Get(inventorySlot.ItemId);
            if (item != null)
            {
                Items[i].Pnl.IsHidden = false;

                // Aplicar color de rareza
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
                    mValues[i].IsHidden = inventorySlot.Quantity <= 1;
                    mValues[i].Text = Strings.FormatQuantityAbbreviated(inventorySlot.Quantity);
                }
                else
                {
                    mValues[i].IsHidden = true;
                }

        var slotCount = Math.Min(Options.Instance.Player.MaxInventory, Items.Count);
        for (var slotIndex = 0; slotIndex < slotCount; slotIndex++)
        {
            Items[slotIndex].Update();
        }

    }

    private void InitItemContainer()
    {
        for (var slotIndex = 0; slotIndex < Options.Instance.Player.MaxInventory; slotIndex++)
        {
            Items.Add(new InventoryItem(this, _slotContainer, slotIndex, _contextMenu));
            Items[i].Container.SetPosition(
                i % (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                i / (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
            );
        }

        PopulateSlotContainer.Populate(_slotContainer, Items);
    }

    public override void Hide()
    {
        if (!Globals.CanCloseInventory)
        {
            return;
        }

        _contextMenu?.Close();
        base.Hide();
    }
}
