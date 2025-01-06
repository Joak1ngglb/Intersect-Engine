using Intersect.Client.Core;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.GameObjects;
using Intersect.Utilities;
using System;

namespace Intersect.Client.Interface.Game.Mail
{
    public class MailItem
    {
        private SendMailBoxWindow mMailWindow;
        private ItemDescriptionWindow mDescWindow;

        public ImagePanel Container;
        public ImagePanel Pnl;

        // Propiedades adicionales tomadas de InventoryItem
        public Label EquipLabel; // Indica si el ítem está equipado
        public ImagePanel EquipPanel; // Panel que muestra el estado de equipado
        private Label mCooldownLabel; // Etiqueta para mostrar el cooldown del ítem
        private string mTexLoaded = ""; // Textura actual cargada
        private bool mIconCd; // Indica si el ítem está en cooldown

        private int mMySlot;

        public MailItem(SendMailBoxWindow mailWindow, int index, ImagePanel container)
        {
            mMailWindow = mailWindow;
            mMySlot = index;
            Container = container;
            Pnl = new ImagePanel(Container, "MailItemIcon");

            // Eventos para manejar interacción
            Pnl.HoverEnter += OnHoverEnter;
            Pnl.HoverLeave += OnHoverLeave;
            Pnl.RightClicked += OnRemove;
            Pnl.DoubleClicked += OnRemove;

            // Inicialización de propiedades adicionales
            EquipPanel = new ImagePanel(Pnl, "MailItemEquippedIcon");
            EquipPanel.Texture = Graphics.Renderer.GetWhiteTexture();
            EquipLabel = new Label(Pnl, "MailItemEquippedLabel")
            {
                IsHidden = true,
                Text = "",
                TextColor = new Color(0, 255, 255, 255)
            };
            mCooldownLabel = new Label(Pnl, "MailItemCooldownLabel")
            {
                IsHidden = true,
                TextColor = new Color(0, 255, 255, 255)
            };
        }

        private void OnHoverEnter(Base sender, EventArgs arguments)
        {
            if (InputHandler.MouseFocus != null)
            {
                return;
            }

            DisposeDescriptionWindow();

            if (mMySlot >= 0 && Globals.Me.Inventory[mMySlot]?.Base != null)
            {
                mDescWindow = new ItemDescriptionWindow(
                    Globals.Me.Inventory[mMySlot].Base,
                    Globals.Me.Inventory[mMySlot].Quantity,
                    mMailWindow.X,
                    mMailWindow.Y,
                    Globals.Me.Inventory[mMySlot].ItemProperties
                );
            }
        }

        private void OnHoverLeave(Base sender, EventArgs arguments)
        {
            DisposeDescriptionWindow();
        }

        private void OnRemove(Base sender, ClickedEventArgs arguments)
        {
            SetSlot(-1);
            DisposeDescriptionWindow();
        }

        public int GetSlot()
        {
            return mMySlot;
        }

        public void SetSlot(int index)
        {
            mMySlot = index;

            if (mMySlot >= 0)
            {
                var item = ItemBase.Get((Guid)(Globals.Me.Inventory[mMySlot]?.ItemId));
                if (item != null)
                {
                    var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
                    Pnl.Texture = itemTex ?? null;

                    // Manejo de cooldown
                    mIconCd = Globals.Me.IsItemOnCooldown(mMySlot);
                    if (mIconCd)
                    {
                        var itemCooldownRemaining = Globals.Me.GetItemRemainingCooldown(mMySlot);
                        mCooldownLabel.IsHidden = false;
                        mCooldownLabel.Text = TimeSpan.FromMilliseconds(itemCooldownRemaining).WithSuffix("0.0");
                    }
                    else
                    {
                        mCooldownLabel.IsHidden = true;
                    }

                    mTexLoaded = item.Icon;
                }
                else
                {
                    ClearTexture();
                }
            }
            else
            {
                ClearTexture();
            }
        }

        private void ClearTexture()
        {
            if (Pnl.Texture != null)
            {
                Pnl.Texture = null;
            }

            mCooldownLabel.IsHidden = true;
            mTexLoaded = "";
        }

        private void DisposeDescriptionWindow()
        {
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
        }
    }
}
