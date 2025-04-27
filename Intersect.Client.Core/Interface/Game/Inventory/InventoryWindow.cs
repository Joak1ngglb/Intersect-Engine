using System.ComponentModel;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Items;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Bank;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game.Inventory;


public partial class InventoryWindow
{

    //Item List
    public List<InventoryItem> Items = new List<InventoryItem>();

    //Initialized Items?
    private bool mInitializedItems = false;

    //Controls
    private WindowControl mInventoryWindow;

    private ScrollControl mItemContainer;

    private List<Label> mValues = new List<Label>();

    // Context menu
    private Framework.Gwen.Control.Menu mContextMenu;

    private MenuItem mUseItemContextItem;

    private MenuItem mActionItemContextItem;

    private MenuItem mDropItemContextItem;

    // Filtros y búsqueda
    private TextBox mSearchBox;
    private ComboBox mTypeFilterBox;
    private ComboBox mSubTypeFilterBox;
    private Button mClearFiltersButton;
    private bool mIsSortedView = false; // FALSE = vista normal, TRUE = vista ordenada
    private Button mSortButton; // Nuevo botón
    private string mCurrentSearch = "";
    private ItemType? mCurrentTypeFilter = null;
    private string mCurrentSubTypeFilter = null;
    //Init
    public InventoryWindow(Canvas gameCanvas)
    {
        mInventoryWindow = new WindowControl(gameCanvas, Strings.Inventory.Title, false, "InventoryWindow");
        mInventoryWindow.SetSize(400, 500);
        mInventoryWindow.DisableResizing();

        // 🔍 Cuadro de búsqueda
        mSearchBox = new TextBox(mInventoryWindow, "InventorySearchBox");
        mSearchBox.SetBounds(10, 10, 250, 30);
        mSearchBox.TextChanged += (sender, args) =>
        {
            mCurrentSearch = mSearchBox.Text;
            Update(); // O llama directamente a Update si lo prefieres
        };
        mSearchBox.Focus();
        Interface.FocusElements.Add(mSearchBox);
        // 🧹 Botón Limpiar Filtros
        mClearFiltersButton = new Button(mInventoryWindow, "ClearFiltersButton");
        mClearFiltersButton.SetBounds(250, 10, 50, 30);
        mClearFiltersButton.Text = "Limpiar";
        mClearFiltersButton.Clicked += (sender, args) =>
        {
            mSearchBox.Text = "";
            mTypeFilterBox.SelectByUserData(null);
            mSubTypeFilterBox.SelectByUserData(null);

            mCurrentSearch = "";
            mCurrentTypeFilter = null;
            mCurrentSubTypeFilter = null;
            mIsSortedView = false;

            Update(); // 👈 Esto ya lo haces
        };

        // Dentro del constructor
        mSortButton = new Button(mInventoryWindow, "SortButton");
        mSortButton.SetBounds(300, 10, 50, 30);
        mSortButton.Text = "Sort";
        mSortButton.Clicked += (sender, args) =>
        {
            //  mIsSortedView = true; // Activa la vista ordenada
            SortAndApplyInventory();
        };
        // 📂 Filtro por Tipo
        mTypeFilterBox = new ComboBox(mInventoryWindow, "InventoryTypeFilter");
        mTypeFilterBox.SetBounds(10, 50, 180, 30);
        mTypeFilterBox.AddItem("Todos", null);
        mTypeFilterBox.ItemSelected += (sender, args) =>
        {
            mCurrentTypeFilter = (ItemType?)args.SelectedItem.UserData;
            Update();
        };

        // 📂 Filtro por Subtipo
        mSubTypeFilterBox = new ComboBox(mInventoryWindow, "InventorySubTypeFilter");
        mSubTypeFilterBox.SetBounds(200, 50, 170, 30);
        mSubTypeFilterBox.AddItem("Todos", null);
        mSubTypeFilterBox.ItemSelected += (sender, args) =>
        {
            mCurrentSubTypeFilter = args.SelectedItem.UserData?.ToString();
            Update();
        };

        // 🎒 Contenedor de ítems con scroll
        mItemContainer = new ScrollControl(mInventoryWindow, "ItemsContainer");
        mItemContainer.SetBounds(10, 90, 380, 390);
        mItemContainer.EnableScroll(false, true);

        mInventoryWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

        // Generate our context menu with basic options.
        mContextMenu = new Framework.Gwen.Control.Menu(gameCanvas, "InventoryContextMenu");
        mContextMenu.IsHidden = true;
        mContextMenu.IconMarginDisabled = true;
        //TODO: Is this a memory leak?
        mContextMenu.Children.Clear();
        mUseItemContextItem = mContextMenu.AddItem(Strings.ItemContextMenu.Use);
        mUseItemContextItem.Clicked += MUseItemContextItem_Clicked;
        mDropItemContextItem = mContextMenu.AddItem(Strings.ItemContextMenu.Drop);
        mDropItemContextItem.Clicked += MDropItemContextItem_Clicked;
        mActionItemContextItem = mContextMenu.AddItem(Strings.ItemContextMenu.Bank);
        mActionItemContextItem.Clicked += MActionItemContextItem_Clicked;
        mContextMenu.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

    }

    public void OpenContextMenu(int slot)
    {
        // Clear out the old options.
        mContextMenu.RemoveChild(mUseItemContextItem, false);
        mContextMenu.RemoveChild(mActionItemContextItem, false);
        mContextMenu.RemoveChild(mDropItemContextItem, false);
        mContextMenu.Children.Clear();

        var item = ItemBase.Get(Globals.Me.Inventory[slot].ItemId);

        // No point showing a menu for blank space.
        if (item == null)
        {
            return;
        }

        // Add our use Item prompt, assuming we have a valid usecase.
        switch (item.ItemType)
        {
            case Enums.ItemType.Spell:
                mContextMenu.AddChild(mUseItemContextItem);
                mUseItemContextItem.SetText(item.QuickCast
                    ? Strings.ItemContextMenu.Cast.ToString(item.Name)
                    : Strings.ItemContextMenu.Learn.ToString(item.Name));
                break;

            case Enums.ItemType.Event:
            case Enums.ItemType.Consumable:
                mContextMenu.AddChild(mUseItemContextItem);
                mUseItemContextItem.SetText(Strings.ItemContextMenu.Use.ToString(item.Name));
                break;

            case Enums.ItemType.Bag:
                mContextMenu.AddChild(mUseItemContextItem);
                mUseItemContextItem.SetText(Strings.ItemContextMenu.Open.ToString(item.Name));
                break;

            case Enums.ItemType.Equipment:
                mContextMenu.AddChild(mUseItemContextItem);
                // Show the correct equip/unequip prompts.
                if (Globals.Me.MyEquipment.Contains(slot))
                {
                    mUseItemContextItem.SetText(Strings.ItemContextMenu.Unequip.ToString(item.Name));
                }
                else
                {
                    mUseItemContextItem.SetText(Strings.ItemContextMenu.Equip.ToString(item.Name));
                }

                break;
        }

        // Set up the correct contextual additional action.
        if (Globals.InBag && item.CanBag)
        {
            mContextMenu.AddChild(mActionItemContextItem);
            mActionItemContextItem.SetText(Strings.ItemContextMenu.Bag.ToString(item.Name));
        }
        else if (Globals.InBank && (item.CanBank || item.CanGuildBank))
        {
            mContextMenu.AddChild(mActionItemContextItem);
            mActionItemContextItem.SetText(Strings.ItemContextMenu.Bank.ToString(item.Name));
        }
        else if (Globals.InTrade && item.CanTrade)
        {
            mContextMenu.AddChild(mActionItemContextItem);
            mActionItemContextItem.SetText(Strings.ItemContextMenu.Trade.ToString(item.Name));
        }
        else if (Globals.GameShop != null && item.CanSell)
        {
            mContextMenu.AddChild(mActionItemContextItem);
            mActionItemContextItem.SetText(Strings.ItemContextMenu.Sell.ToString(item.Name));
        }

        // Can we drop this item? if so show the user!
        if (item.CanDrop)
        {
            mContextMenu.AddChild(mDropItemContextItem);
            mDropItemContextItem.SetText(Strings.ItemContextMenu.Drop.ToString(item.Name));
        }

        // Set our Inventory slot as userdata for future reference.
        mContextMenu.UserData = slot;

        // Display our menu.. If we have anything to display.
        if (mContextMenu.Children.Count > 0)
        {
            mContextMenu.SizeToChildren();
            mContextMenu.Open(Framework.Gwen.Pos.None);
        }
    }

    private void MUseItemContextItem_Clicked(Base sender, Framework.Gwen.Control.EventArguments.ClickedEventArgs arguments)
    {
        var slot = (int)sender.Parent.UserData;
        Globals.Me.TryUseItem(slot);
    }

    private void MActionItemContextItem_Clicked(Base sender, Framework.Gwen.Control.EventArguments.ClickedEventArgs arguments)
    {
        var slot = (int)sender.Parent.UserData;
        if (Globals.GameShop != null)
        {
            Globals.Me.TrySellItem(slot);
        }
        else if (Globals.InBank)
        {
            Globals.Me.TryDepositItem(slot);
        }
        else if (Globals.InBag)
        {
            Globals.Me.TryStoreBagItem(slot, -1);
        }
        else if (Globals.InTrade)
        {
            Globals.Me.TryTradeItem(slot);
        }
    }

    private void MDropItemContextItem_Clicked(Base sender, Framework.Gwen.Control.EventArguments.ClickedEventArgs arguments)
    {
        var slot = (int)sender.Parent.UserData;
        Globals.Me.TryDropItem(slot);
    }

    //Location
    public int X => mInventoryWindow.X;

    public int Y => mInventoryWindow.Y;

    //Methods
    public void Update()
    {
        if (!mInitializedItems)
        {
            mInitializedItems = true;
            InitItemContainer();
        }

        if (mInventoryWindow.IsHidden)
        {
            return;
        }

        if (Items.Count != Options.MaxInvItems || mValues.Count != Options.MaxInvItems)
        {
            InitItemContainer();
        }

        mInventoryWindow.IsClosable = Globals.CanCloseInventory;

        var isFiltering = !string.IsNullOrEmpty(mCurrentSearch) || mCurrentTypeFilter != null || !string.IsNullOrEmpty(mCurrentSubTypeFilter);

        if (!isFiltering)
        {
            // 🔄 Modo FÍSICO como antes, y reconstruir TODO
            for (var i = 0; i < Options.MaxInvItems; i++)
            {
                var inventorySlot = Globals.Me.Inventory[i];
                if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
                {
                    Items[i].Pnl.IsHidden = true;
                    mValues[i].IsHidden = true;
                    continue;
                }

                var item = ItemBase.Get(inventorySlot.ItemId);
                if (item != null)
                {
                    Items[i].Pnl.IsHidden = false;
                    Items[i].DisplaySlot = i; // 👈 Reafirma el slot físico real
                    RenderItem(i, item, inventorySlot);
                }
                else
                {
                    Items[i].Pnl.IsHidden = true;
                    mValues[i].IsHidden = true;
                }
            }
        }

        else
        {
            // 🔄 Modo visual ORDENADO al buscar/filtrar
            var visibleItems = Globals.Me.Inventory
                .Select((slot, index) => new { Slot = slot, Index = index, ItemBase = ItemBase.Get(slot.ItemId) })
                .Where(x => x.Slot != null && x.ItemBase != null)
                .Where(x => SearchHelper.IsSearchable(x.ItemBase, mCurrentSearch, mCurrentTypeFilter, mCurrentSubTypeFilter))
                .OrderByDescending(x => GetItemRelevance(x.ItemBase, mCurrentSearch))
                .ThenByDescending(x => x.ItemBase.Rarity)
                .ThenByDescending(x => x.Slot.Quantity)
                .ToList();

            for (int visualIndex = 0; visualIndex < Items.Count; visualIndex++)
            {
                if (visualIndex < visibleItems.Count)
                {
                    var entry = visibleItems[visualIndex];
                    var item = entry.ItemBase;
                    var inventorySlot = entry.Slot;

                    var visualItem = Items[visualIndex];
                    visualItem.Pnl.IsHidden = false;
                    visualItem.DisplaySlot = entry.Index;

                    RenderItem(visualIndex, item, inventorySlot);
                }
                else
                {
                    Items[visualIndex].Pnl.IsHidden = true;
                    mValues[visualIndex].IsHidden = true;
                }
            }
        }
    }

    // 📦 Render común
    private void RenderItem(int index, ItemBase item, IItem inventorySlot)
    {
        if (CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor))
        {
            Items[index].Container.RenderColor = rarityColor;
        }
        else
        {
            Items[index].Container.RenderColor = Color.White;
        }

        if (item.IsStackable)
        {
            mValues[index].IsHidden = inventorySlot.Quantity <= 1;
            mValues[index].Text = Strings.FormatQuantityAbbreviated(inventorySlot.Quantity);
        }
        else
        {
            mValues[index].IsHidden = true;
        }

        if (Items[index].IsDragging)
        {
            Items[index].Pnl.IsHidden = true;
            mValues[index].IsHidden = true;
        }
        Items[index].RefreshContainerColor();
        Items[index].Update();
    }

    private void InitItemContainer()
    {
        Items.Clear();
        mValues.Clear();

        for (var i = 0; i < Options.MaxInvItems; i++)
        {
            Items.Add(new InventoryItem(this, i));
            Items[i].Container = new ImagePanel(mItemContainer, "InventoryItem");
            Items[i].Setup();

            mValues.Add(new Label(Items[i].Container, "InventoryItemValue"));
            mValues[i].Text = "";

            Items[i].Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            if (Items[i].EquipPanel.Texture == null)
            {
                Items[i].EquipPanel.Texture = Graphics.Renderer.GetWhiteTexture();
            }

            var xPadding = Items[i].Container.Margin.Left + Items[i].Container.Margin.Right;
            var yPadding = Items[i].Container.Margin.Top + Items[i].Container.Margin.Bottom;

            Items[i].Container.SetPosition(
                i % (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Width + xPadding) + xPadding,
                i / (mItemContainer.Width / (Items[i].Container.Width + xPadding)) * (Items[i].Container.Height + yPadding) + yPadding
            );
        }
    }


    public void Show()
    {
        RefreshFilters();
        mInventoryWindow.IsHidden = false;
    }

    public bool IsVisible()
    {
        return !mInventoryWindow.IsHidden;
    }

    public void Hide()
    {
        if (!Globals.CanCloseInventory)
        {
            return;
        }

        mContextMenu?.Close();
        mInventoryWindow.IsHidden = true;
    }

    public FloatRect RenderBounds()
    {
        if (Items.Count == 0)
        {
            return new FloatRect(mInventoryWindow.X, mInventoryWindow.Y, mInventoryWindow.Width, mInventoryWindow.Height);
        }

        var padding = Items[0].Container.Padding;

        return new FloatRect
        {
            X = mInventoryWindow.LocalPosToCanvas(new Point(0, 0)).X - (padding.Left + padding.Right) / 2,
            Y = mInventoryWindow.LocalPosToCanvas(new Point(0, 0)).Y - (padding.Top + padding.Bottom) / 2,
            Width = mInventoryWindow.Width + padding.Left + padding.Right,
            Height = mInventoryWindow.Height + padding.Top + padding.Bottom
        };
    }

    public void RefreshFilters()
    {
        // Filtrar subtipos en inventario
        var subtypesInInventory = Globals.Me.Inventory
            .Where(slot => slot != null && slot.ItemId != Guid.Empty)
            .Select(slot => ItemBase.Get(slot.ItemId))
            .Where(item => item != null && !string.IsNullOrEmpty(item.Subtype))
            .Select(item => item.Subtype)
            .Distinct()
            .ToList();

        mSubTypeFilterBox.DeleteAll();
        mSubTypeFilterBox.AddItem("Todos", null);
        foreach (var subtype in subtypesInInventory)
        {
            mSubTypeFilterBox.AddItem(subtype, subtype, subtype);
        }

        // Filtrar tipos en inventario
        var typesInInventory = Globals.Me.Inventory
            .Where(slot => slot != null && slot.ItemId != Guid.Empty)
            .Select(slot => ItemBase.Get(slot.ItemId))
            .Where(item => item != null)
            .Select(item => item.ItemType)
            .Distinct()
            .ToList();


        // With the corrected code:
        mTypeFilterBox.DeleteAll();
        mTypeFilterBox.AddItem("Todos", null);
        foreach (var itemType in typesInInventory)
        {

            // Fix for CS1503: The second argument of AddItem should be a string, not a method group.
            // Corrected the second argument to properly convert the ItemType to a string.

            mTypeFilterBox.AddItem(itemType.ToString(), itemType.ToString(), itemType);


        }
    }
    private int GetItemRelevance(ItemBase item, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return 0;

        var name = item.Name.ToLower();
        var search = searchTerm.ToLower();

        if (name.StartsWith(search)) return 3; // Muy relevante
        if (name.EndsWith(search)) return 2;   // Medio
        if (name.Contains(search)) return 1;   // Poco relevante

        return 0;
    }
    private int CompareInventoryItems(ItemBase x, ItemBase y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.ItemType == y.ItemType)
        {
            return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) * -1; // Descendente
        }
        else
        {
            // Orden por tipo (ejemplo básico, ajusta según tus preferencias)
            return x.ItemType.CompareTo(y.ItemType) * -1; // Descendente por tipo
        }
    }
    private void SortAndApplyInventory()
    {
        var maxSlots = Globals.Me.Inventory.Length;

        for (var slot = 0; slot < maxSlots - 1; slot++)
        {
            for (var compareSlot = 0; compareSlot < maxSlots - slot - 1; compareSlot++)
            {
                var item1 = ItemBase.Get(Globals.Me.Inventory[compareSlot].ItemId);
                var item2 = ItemBase.Get(Globals.Me.Inventory[compareSlot + 1].ItemId);

                if (CompareInventoryItems(item1, item2) < 0) // Si item1 < item2, los intercambiamos
                {
                    Globals.Me.SwapItems(compareSlot, compareSlot + 1);
                }
            }
        }

        //  Update(); // Refrescar visualmente
    }


}
