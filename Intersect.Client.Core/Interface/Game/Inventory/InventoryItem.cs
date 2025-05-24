using System;
using System.Reflection;
using Intersect.Client.Core;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Interface.Game.Enchanting;
using Intersect.Client.Interface.Game.Mail;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Configuration;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Client.Interface.Game.Inventory;


public partial class InventoryItem
{

    public ImagePanel Container;

    public Label EquipLabel;

    public ImagePanel EquipPanel;

    public bool IsDragging;

    //Dragging
    private bool mCanDrag;

    private long mClickTime;

    private Label mCooldownLabel;

    private int mCurrentAmt = 0;

    private Guid mCurrentItemId;

    private ItemDescriptionWindow mDescWindow;

    private Draggable mDragIcon;

    private bool mIconCd;

    //Drag/Drop References
    private InventoryWindow mInventoryWindow;

    private bool mIsEquipped;

    //Mouse Event Variables
    private bool mMouseOver;

    private int mMouseX = -1;

    private int mMouseY = -1;
    public int DisplaySlot { get; set; } // Slot real dinámico

    private string mTexLoaded = "";

    public ImagePanel Pnl;
    private SendMailBoxWindow mSendMailBoxWindow;


   

    public InventoryItem(InventoryWindow inventoryWindow, int index)
    {
        mInventoryWindow = inventoryWindow;
        DisplaySlot = index;
    }

    public InventoryItem(SendMailBoxWindow sendMailBoxWindow, int index)
    {
       mSendMailBoxWindow = sendMailBoxWindow;
        DisplaySlot = index;
    }

    public void Setup()
    {
        Pnl = new ImagePanel(Container, "InventoryItemIcon");
        Pnl.HoverEnter += pnl_HoverEnter;
        Pnl.HoverLeave += pnl_HoverLeave;
        Pnl.RightClicked += pnl_RightClicked;
        Pnl.Clicked += pnl_Clicked;
        Pnl.DoubleClicked += Pnl_DoubleClicked;
        EquipPanel = new ImagePanel(Pnl, "InventoryItemEquippedIcon");
        EquipPanel.Texture = Graphics.Renderer.GetWhiteTexture();
        EquipLabel = new Label(Pnl, "InventoryItemEquippedLabel");
        EquipLabel.IsHidden = true;
        EquipLabel.Text = Strings.Inventory.EquippedSymbol;
        EquipLabel.TextColor = new Color(0, 255, 255, 255);
        mCooldownLabel = new Label(Pnl, "InventoryItemCooldownLabel");
        mCooldownLabel.IsHidden = true;
        mCooldownLabel.TextColor = new Color(0, 255, 255, 255);
    }

    private void Pnl_DoubleClicked(Base sender, ClickedEventArgs arguments)
    {
        if (mSendMailBoxWindow != null)
        {
            mSendMailBoxWindow.SelectItem(mSendMailBoxWindow.Items[DisplaySlot], DisplaySlot);
        }
     
      
        else if (Globals.GameShop != null)
        {
            Globals.Me.TrySellItem(DisplaySlot);
        }
        else if (Globals.InBank)
        {
            if (Globals.InputManager.KeyDown(Keys.Shift))
            {
                Globals.Me.TryDepositItem(
                    DisplaySlot,
                    skipPrompt: true
                );
            }
            else
            {
                var slot = Globals.Me.Inventory[DisplaySlot];
                Globals.Me.TryDepositItem(
                    DisplaySlot,
                    slot,
                    quantityHint: slot.Quantity,
                    skipPrompt: false
                );
            }
        }
        else if (Globals.InBag)
        {
            Globals.Me.TryStoreBagItem(DisplaySlot, -1);
        }
        else if (Globals.InTrade)
        {
            Globals.Me.TryTradeItem(DisplaySlot);
        }
        else
        {
            Globals.Me.TryUseItem(DisplaySlot);
        }
    }

    void pnl_Clicked(Base sender, ClickedEventArgs arguments)
    {       
        mClickTime = Timing.Global.MillisecondsUtc + 500;
    }

    void pnl_RightClicked(Base sender, ClickedEventArgs arguments)
    {
       
        if (ClientConfiguration.Instance.EnableContextMenus)
        {
            mInventoryWindow.OpenContextMenu(DisplaySlot);
        }
        else
        {
            if (Globals.GameShop != null)
            {
                Globals.Me.TrySellItem(DisplaySlot);
            }
            else if (Globals.InBank)
            {
                Globals.Me.TryDepositItem(DisplaySlot);
            }
            else if (Globals.InBag)
            {
                Globals.Me.TryStoreBagItem(DisplaySlot, -1);
            }
            else if (Globals.InTrade)
            {
                Globals.Me.TryTradeItem(DisplaySlot);
            }
            else
            {
                Globals.Me.TryDropItem(DisplaySlot);
            }
        }
    }

    void pnl_HoverLeave(Base sender, EventArgs arguments)
    {
        mMouseOver = false;
        mMouseX = -1;
        mMouseY = -1;
        if (mDescWindow != null)
        {
            mDescWindow.Dispose();
            mDescWindow = null;
        }
    }

    void pnl_HoverEnter(Base sender, EventArgs arguments)
    {
        if (InputHandler.MouseFocus != null)
        {
            return;
        }

        mMouseOver = true;
        mCanDrag = true;

        if (Globals.InputManager.MouseButtonDown(MouseButtons.Left))
        {
            mCanDrag = false;
            return;
        }

        if (mDescWindow != null)
        {
            mDescWindow.Dispose();
            mDescWindow = null;
        }
             
        // 🔍 Nueva verificación: Si el ítem pertenece a SendMailBoxWindow
        if (mSendMailBoxWindow != null)
        {
            var inventorySlot = Globals.Me.Inventory[DisplaySlot];
            if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
            {
                return;
            }

            var itemBase = ItemBase.Get(inventorySlot.ItemId);
            if (itemBase == null)
            {
                return;
            }

            // Mostrar descripción en la ventana de envío de correo
            mDescWindow = new ItemDescriptionWindow(
                itemBase,
                inventorySlot.Quantity,
                mSendMailBoxWindow.X,
                mSendMailBoxWindow.Y,
                inventorySlot.ItemProperties
            );

            return; // No continuar con la lógica de GameShop
        }

        // 🛒 Lógica normal para la tienda o inventario
        if (Globals.GameShop == null)
        {
            if (Globals.Me.Inventory[DisplaySlot]?.Base != null)
            {
                mDescWindow = new ItemDescriptionWindow(
                    Globals.Me.Inventory[DisplaySlot].Base,
                    Globals.Me.Inventory[DisplaySlot].Quantity,
                    mInventoryWindow.X,
                    mInventoryWindow.Y,
                    Globals.Me.Inventory[DisplaySlot].ItemProperties
                );
            }
        }
        else
        {
            var invItem = Globals.Me.Inventory[DisplaySlot];
            ShopItem shopItem = null;
            for (var i = 0; i < Globals.GameShop.BuyingItems.Count; i++)
            {
                var tmpShop = Globals.GameShop.BuyingItems[i];

                if (invItem.ItemId == tmpShop.ItemId)
                {
                    shopItem = tmpShop;
                    break;
                }
            }

            if (Globals.GameShop.BuyingWhitelist && shopItem != null)
            {
                var hoveredItem = ItemBase.Get(shopItem.CostItemId);
                if (hoveredItem != null && Globals.Me.Inventory[DisplaySlot]?.Base != null)
                {
                    mDescWindow = new ItemDescriptionWindow(
                        Globals.Me.Inventory[DisplaySlot].Base,
                        Globals.Me.Inventory[DisplaySlot].Quantity,
                        mInventoryWindow.X,
                        mInventoryWindow.Y,
                        Globals.Me.Inventory[DisplaySlot].ItemProperties,
                        "",
                        Strings.Shop.SellsFor.ToString(shopItem.CostItemQuantity, hoveredItem.Name)
                    );
                }
            }
            else if (shopItem == null)
            {
                var costItem = Globals.GameShop.DefaultCurrency;
                if (invItem.Base != null && costItem != null && Globals.Me.Inventory[DisplaySlot]?.Base != null)
                {
                    mDescWindow = new ItemDescriptionWindow(
                        Globals.Me.Inventory[DisplaySlot].Base,
                        Globals.Me.Inventory[DisplaySlot].Quantity,
                        mInventoryWindow.X,
                        mInventoryWindow.Y,
                        Globals.Me.Inventory[DisplaySlot].ItemProperties,
                        "",
                        Strings.Shop.SellsFor.ToString(invItem.Base.Price.ToString(), costItem.Name)
                    );
                }
            }
            else
            {
                if (invItem?.Base != null)
                {
                    mDescWindow = new ItemDescriptionWindow(
                        invItem.Base,
                        invItem.Quantity,
                        mInventoryWindow.X,
                        mInventoryWindow.Y,
                        invItem.ItemProperties,
                        "",
                        Strings.Shop.WontBuy
                    );
                }
            }
        }
    }

    public FloatRect RenderBounds()
    {
        var rect = new FloatRect()
        {
            X = Pnl.LocalPosToCanvas(new Point(0, 0)).X,
            Y = Pnl.LocalPosToCanvas(new Point(0, 0)).Y,
            Width = Pnl.Width,
            Height = Pnl.Height
        };

        return rect;
    }

    public void Update()
    {
        var equipped = false;
        for (var i = 0; i < Options.EquipmentSlots.Count; i++)
        {
            if (Globals.Me.MyEquipment[i] == DisplaySlot)
            {
                equipped = true;

                break;
            }
        }

        var item = ItemBase.Get(Globals.Me.Inventory[DisplaySlot].ItemId);

        if (item != null)
        {
            // Obtener el color de la rareza del ítem
            if (CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor))
            {
                Container.RenderColor = rarityColor; // Aplicar color al contenedor
            }
            else
            {
                Container.RenderColor = Color.White; // Color por defecto si no se encuentra la rareza
            }
        }
        if (Globals.Me.Inventory[DisplaySlot].ItemId != mCurrentItemId ||
            Globals.Me.Inventory[DisplaySlot].Quantity != mCurrentAmt ||
            equipped != mIsEquipped ||
            item == null && mTexLoaded != "" ||
            item != null && mTexLoaded != item.Icon ||
            mIconCd != Globals.Me.IsItemOnCooldown(DisplaySlot) ||
            Globals.Me.IsItemOnCooldown(DisplaySlot))
        {
            mCurrentItemId = Globals.Me.Inventory[DisplaySlot].ItemId;
            mCurrentAmt = Globals.Me.Inventory[DisplaySlot].Quantity;
            mIsEquipped = equipped;
            EquipPanel.IsHidden = !mIsEquipped;
            EquipLabel.IsHidden = !mIsEquipped;
            mCooldownLabel.IsHidden = true;
            if (item != null)
            {
                var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
                if (itemTex != null)
                {
                    Pnl.Texture = itemTex;
                    if (Globals.Me.IsItemOnCooldown(DisplaySlot))
                    {
                        Pnl.RenderColor = new Color(100, item.Color.R, item.Color.G, item.Color.B);
                    }
                    else
                    {
                        Pnl.RenderColor = item.Color;
                    }
                }
                else
                {
                    if (Pnl.Texture != null)
                    {
                        Pnl.Texture = null;
                    }
                }

                mTexLoaded = item.Icon;
                mIconCd = Globals.Me.IsItemOnCooldown(DisplaySlot);
                if (mIconCd)
                {
                    var itemCooldownRemaining = Globals.Me.GetItemRemainingCooldown(DisplaySlot);
                    mCooldownLabel.IsHidden = false;
                    mCooldownLabel.Text = TimeSpan.FromMilliseconds(itemCooldownRemaining).WithSuffix("0.0");
                }
            }
            else
            {
                if (Pnl.Texture != null)
                {
                    Pnl.Texture = null;
                }

                mTexLoaded = "";
            }

            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
                pnl_HoverEnter(null, null);
            }
        }

        if (!IsDragging)
        {
            if (mMouseOver)
            {
                if (!Globals.InputManager.MouseButtonDown(MouseButtons.Left))
                {
                    mCanDrag = true;
                    mMouseX = -1;
                    mMouseY = -1;
                    if (Timing.Global.MillisecondsUtc < mClickTime)
                    {
                        mClickTime = 0;
                    }
                }
                else
                {
                    if (mCanDrag && Draggable.Active == null)
                    {
                        if (mMouseX == -1 || mMouseY == -1)
                        {
                            mMouseX = InputHandler.MousePosition.X - Pnl.LocalPosToCanvas(new Point(0, 0)).X;
                            mMouseY = InputHandler.MousePosition.Y - Pnl.LocalPosToCanvas(new Point(0, 0)).Y;
                        }
                        else
                        {
                            var xdiff = mMouseX -
                                        (InputHandler.MousePosition.X - Pnl.LocalPosToCanvas(new Point(0, 0)).X);

                            var ydiff = mMouseY -
                                        (InputHandler.MousePosition.Y - Pnl.LocalPosToCanvas(new Point(0, 0)).Y);

                            if (Math.Sqrt(Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2)) > 5)
                            {
                                IsDragging = true;
                                mDragIcon = new Draggable(
                                    Pnl.LocalPosToCanvas(new Point(0, 0)).X + mMouseX,
                                    Pnl.LocalPosToCanvas(new Point(0, 0)).X + mMouseY, Pnl.Texture, Pnl.RenderColor
                                );
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (mDragIcon.Update())
            {
                //Drug the item and now we stopped
                IsDragging = false;
                var dragRect = new FloatRect(
                    mDragIcon.X - (Container.Padding.Left + Container.Padding.Right) / 2,
                    mDragIcon.Y - (Container.Padding.Top + Container.Padding.Bottom) / 2,
                    (Container.Padding.Left + Container.Padding.Right) / 2 + Pnl.Width,
                    (Container.Padding.Top + Container.Padding.Bottom) / 2 + Pnl.Height
                );

                float bestIntersect = 0;
                var bestIntersectIndex = -1;

                //So we picked up an item and then dropped it. Lets see where we dropped it to.
                //Check inventory first.
                if (mInventoryWindow.RenderBounds().IntersectsWith(dragRect))
                {
                    for (var i = 0; i < Options.MaxInvItems; i++)
                    {
                        if (mInventoryWindow.Items[i].RenderBounds().IntersectsWith(dragRect))
                        {
                            if (FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Width *
                                FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Height >
                                bestIntersect)
                            {
                                bestIntersect =
                                    FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Width *
                                    FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Height;

                                bestIntersectIndex = i;
                            }
                        }
                    }
                    if (DisplaySlot != bestIntersectIndex)
                    {
                        Globals.Me.SwapItems(DisplaySlot, bestIntersectIndex);

                        // 🔥 Refrescar color incluso si queda vacío
                        var item1 = mInventoryWindow.Items.FirstOrDefault(x => x.DisplaySlot == DisplaySlot);
                        item1?.RefreshContainerColor();

                        var item2 = mInventoryWindow.Items.FirstOrDefault(x => x.DisplaySlot == bestIntersectIndex);
                        item2?.RefreshContainerColor();
                        item1?.Update();
                        item2?.Update();

                    }

                }
                else if (Interface.GameUi.Hotbar.RenderBounds().IntersectsWith(dragRect))
                {
                    for (var i = 0; i < Options.Instance.PlayerOpts.HotbarSlotCount; i++)
                    {
                        if (Interface.GameUi.Hotbar.Items[i].RenderBounds().IntersectsWith(dragRect))
                        {
                            if (FloatRect.Intersect(
                                        Interface.GameUi.Hotbar.Items[i].RenderBounds(), dragRect
                                    )
                                    .Width *
                                FloatRect.Intersect(Interface.GameUi.Hotbar.Items[i].RenderBounds(), dragRect)
                                    .Height >
                                bestIntersect)
                            {
                                bestIntersect =
                                    FloatRect.Intersect(Interface.GameUi.Hotbar.Items[i].RenderBounds(), dragRect)
                                        .Width *
                                    FloatRect.Intersect(Interface.GameUi.Hotbar.Items[i].RenderBounds(), dragRect)
                                        .Height;

                                bestIntersectIndex = i;
                            }
                        }
                    }

                    if (bestIntersectIndex > -1)
                    {
                        Globals.Me.AddToHotbar((byte) bestIntersectIndex, 0, DisplaySlot);
                    }
                }
                else if (Globals.InBag)
                {
                    var bagWindow = Interface.GameUi.GetBagWindow();
                    if (bagWindow.RenderBounds().IntersectsWith(dragRect))
                    {
                        for (var i = 0; i < Globals.Bag.Length; i++)
                        {
                            if (bagWindow.Items[i].RenderBounds().IntersectsWith(dragRect))
                            {
                                if (FloatRect.Intersect(bagWindow.Items[i].RenderBounds(), dragRect).Width *
                                    FloatRect.Intersect(bagWindow.Items[i].RenderBounds(), dragRect).Height >
                                    bestIntersect)
                                {
                                    bestIntersect =
                                        FloatRect.Intersect(bagWindow.Items[i].RenderBounds(), dragRect).Width *
                                        FloatRect.Intersect(bagWindow.Items[i].RenderBounds(), dragRect).Height;

                                    bestIntersectIndex = i;
                                }
                            }
                        }

                        if (bestIntersectIndex > -1)
                        {
                            Globals.Me.TryStoreBagItem(DisplaySlot, bestIntersectIndex);
                        }
                    }
                }
                else if (Globals.InBank)
                {
                    var bankWindow = Interface.GameUi.GetBankWindow();
                    if (bankWindow.RenderBounds().IntersectsWith(dragRect))
                    {
                        for (var i = 0; i < Globals.Bank.Length; i++)
                        {
                            if (bankWindow.Items[i].RenderBounds().IntersectsWith(dragRect))
                            {
                                if (FloatRect.Intersect(bankWindow.Items[i].RenderBounds(), dragRect).Width *
                                    FloatRect.Intersect(bankWindow.Items[i].RenderBounds(), dragRect).Height >
                                    bestIntersect)
                                {
                                    bestIntersect =
                                        FloatRect.Intersect(bankWindow.Items[i].RenderBounds(), dragRect).Width *
                                        FloatRect.Intersect(bankWindow.Items[i].RenderBounds(), dragRect).Height;

                                    bestIntersectIndex = i;
                                }
                            }
                        }

                        if (bestIntersectIndex > -1)
                        {
                            var slot = Globals.Me.Inventory[DisplaySlot];
                            Globals.Me.TryDepositItem(
                                DisplaySlot,
                                bankSlotIndex: bestIntersectIndex,
                                quantityHint: slot.Quantity,
                                skipPrompt: true
                            );
                        }
                    }
                }
                else if (!Globals.Me.IsBusy)
                {
                    PacketSender.SendDropItem(DisplaySlot, Globals.Me.Inventory[DisplaySlot].Quantity);
                }

                mDragIcon.Dispose();
            }
        }
    }
    public void RefreshContainerColor()
    {
        var inventorySlot = Globals.Me.Inventory[DisplaySlot];

        if (inventorySlot != null && inventorySlot.ItemId != Guid.Empty)
        {
            var item = ItemBase.Get(inventorySlot.ItemId);

            if (item != null && CustomColors.Items.Rarities.TryGetValue(item.Rarity, out var rarityColor))
            {
                Container.RenderColor = rarityColor;
            }
            else
            {
                Container.RenderColor = Color.White;
            }
        }
        else
        {
            // Slot vacío = sin ítem
            Container.RenderColor = Color.White;
        }
    }


}
