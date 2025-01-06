using System;
using System.Collections.Generic;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Job;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Interface.Game.Mail
{
    public class SendMailBoxWindow
    {
        private WindowControl mSendMailBoxWindow;

        private Label mTo;
        private TextBox mToTextbox;
        private Label mTitle;
        private TextBox mTitleTextbox;
        private Label mMessage;
        private TextBox mMsgTextbox;

        private Label mItem;
        private ComboBox mItemComboBox;
        private Label mQuantity;
        private TextBoxNumeric mQuantityTextBoxNumeric;

        private Button mAddItemButton;
        private Button mSendButton;
        private Button mCloseButton;

        private List<MailAttachmentPacket> mAttachments;

        public int X { get; internal set; }
        public int Y { get; internal set; }

        public SendMailBoxWindow(Canvas gameCanvas)
        {
            mSendMailBoxWindow = new WindowControl(gameCanvas, Strings.MailBox.sendtitle, false, "SendMailBoxWindow");
            Interface.InputBlockingElements.Add(mSendMailBoxWindow);

            // "To" Label and TextBox
            mTo = new Label(mSendMailBoxWindow, "To") { Text = Strings.MailBox.mailto };
            mTo.SetBounds(20, 20, 100, 20);

            mToTextbox = new TextBox(mSendMailBoxWindow, "ToTextbox");
            mToTextbox.SetBounds(130, 20, 200, 25);
            Interface.FocusElements.Add(mToTextbox);

            // Title Label and TextBox
            mTitle = new Label(mSendMailBoxWindow, "Title") { Text = Strings.MailBox.mailtitle };
            mTitle.SetBounds(20, 60, 100, 20);

            mTitleTextbox = new TextBox(mSendMailBoxWindow, "TitleTextbox");
            mTitleTextbox.SetBounds(130, 60, 200, 25);
            mTitleTextbox.SetMaxLength(20);
            Interface.FocusElements.Add(mTitleTextbox);

            // Message Label and TextBox
            mMessage = new Label(mSendMailBoxWindow, "Message") { Text = Strings.MailBox.mailmsg };
            mMessage.SetBounds(20, 100, 100, 20);

            mMsgTextbox = new TextBox(mSendMailBoxWindow, "MsgTextbox");
            mMsgTextbox.SetBounds(130, 100, 200, 60);
            mMsgTextbox.SetMaxLength(255);
            Interface.FocusElements.Add(mMsgTextbox);

            // Item Label and ComboBox
            mItem = new Label(mSendMailBoxWindow, "Item") { Text = Strings.MailBox.mailitem };
            mItem.SetBounds(20, 180, 100, 20);

            mItemComboBox = new ComboBox(mSendMailBoxWindow, "ItemComboBox");
            mItemComboBox.SetBounds(130, 180, 200, 25);
            Interface.FocusElements.Add(mItemComboBox);

            // Quantity Label and Numeric TextBox
            mQuantity = new Label(mSendMailBoxWindow, "Quantity") { Text = Strings.MailBox.mailquantity };
            mQuantity.SetBounds(20, 220, 100, 20);

            mQuantityTextBoxNumeric = new TextBoxNumeric(mSendMailBoxWindow, "QuantityTextBoxNumeric");
            mQuantityTextBoxNumeric.SetBounds(130, 220, 200, 25);

            // Add Item Button
            mAddItemButton = new Button(mSendMailBoxWindow, "AddItemButton");
            mAddItemButton.SetText(Strings.MailBox.additem);
            mAddItemButton.SetBounds(20, 260, 150, 30);
            mAddItemButton.Clicked += AddItemButton_Clicked;

            // Send Button
            mSendButton = new Button(mSendMailBoxWindow, "SendButton");
            mSendButton.SetText(Strings.MailBox.send);
            mSendButton.SetBounds(180, 260, 100, 30);
            mSendButton.Clicked += SendButton_Clicked;

            // Close Button
            mCloseButton = new Button(mSendMailBoxWindow, "CloseButton");
            mCloseButton.SetText(Strings.MailBox.close);
            mCloseButton.SetBounds(290, 260, 100, 30);
            mCloseButton.Clicked += CloseButton_Clicked;

            // Attachments List
            mAttachments = new List<MailAttachmentPacket>();

            mSendMailBoxWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            mSendMailBoxWindow.SetBounds(300, 200, 420, 320);
            mSendMailBoxWindow.DisableResizing();
        }


        private void AddItemButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            var selectedItem = mItemComboBox.SelectedItem;
            int invSlot = (int)selectedItem?.UserData;
            if (invSlot < 0) return;

            var item = Globals.Me.Inventory[invSlot];
            if (item == null || item.ItemId == Guid.Empty) return;

            int quantity = (int)mQuantityTextBoxNumeric.Value;
            if (quantity <= 0 || quantity > item.Quantity) return;

            mAttachments.Add(new MailAttachmentPacket
            {
                ItemId = item.ItemId,
                Quantity = quantity,
                Properties = item.ItemProperties
            });

            // Optional: Update UI to show added item
        }

        void SendButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (string.IsNullOrWhiteSpace(mToTextbox.Text) || string.IsNullOrWhiteSpace(mTitleTextbox.Text)) return;

            PacketSender.SendMail(mToTextbox.Text, mTitleTextbox.Text, mMsgTextbox.Text, mAttachments);
            Close();
        }

        void CloseButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            PacketSender.SendCloseMail();
            Close();
        }

        public void UpdateItemList()
        {
            mItemComboBox.DeleteAll();
            mItemComboBox.AddItem(Strings.MailBox.itemnone, "", -1);
            for (int i = 0; i < Globals.Me.Inventory.Length; i++)
            {
                var item = Globals.Me.Inventory[i];
                if (item?.ItemId != Guid.Empty)
                {
                    mItemComboBox.AddItem(item.Base.Name, "", i);
                }
            }
        }
        private void RestoreItemsToInventory()
        {
            foreach (var attachment in mAttachments)
            {
                var itemId = attachment.ItemId;
                var quantity = attachment.Quantity;
                var properties = attachment.Properties;

                bool added = false;

                // Iterar sobre el inventario e intentar agregar el ítem
                for (int i = 0; i < Globals.Me.Inventory.Length; i++)
                {
                    var slot = Globals.Me.Inventory[i];

                    if (slot.ItemId == Guid.Empty)
                    {
                        // Agregar el ítem en una ranura vacía
                        Globals.Me.Inventory[i] = new Item
                        {
                            ItemId = itemId,
                            Quantity = quantity,
                            ItemProperties = properties
                        };
                        added = true;
                        break;
                    }

                    // Verificar si el ítem puede apilarse
                    var itemBase = ItemBase.Get(itemId);
                    if (slot.ItemId == itemId && slot.Quantity < itemBase.MaxInventoryStack)
                    {
                        var maxStack = itemBase.MaxInventoryStack;
                        var spaceLeft = maxStack - slot.Quantity;

                        var toAdd = Math.Min(spaceLeft, quantity);
                        Globals.Me.Inventory[i].Quantity += toAdd;
                        quantity -= toAdd;

                        if (quantity <= 0)
                        {
                            added = true;
                            break;
                        }
                    }
                }

                // Si no se pudo agregar el ítem, enviar un mensaje de error
                if (!added)
                {
                    PacketSender.SendChatMsg(Strings.Inventory.InventoryFull, 4);
                }
            }

            mAttachments.Clear(); // Limpia los adjuntos después de procesarlos
        }

        public void Close()
        {
            RestoreItemsToInventory(); // Devolver los ítems al cerrar
            mSendMailBoxWindow.Close();
        }

        public bool IsVisible()
        {
           return !mSendMailBoxWindow.IsHidden;
        }

        public void Show()
        {
            mSendMailBoxWindow.IsHidden = false;
        }
    }
}
