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
using Newtonsoft.Json.Linq;

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
        private ScrollControl mAttachmentContainer;
        private List<MailAttachmentPacket> mAttachments;
        private List<MailItem> mAttachmentSlots;
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
            InitializeAttachmentSlots();
            mSendMailBoxWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            mSendMailBoxWindow.SetBounds(300, 200, 420, 320);
            mSendMailBoxWindow.DisableResizing();
        }

        private void InitializeAttachmentSlots()
        {
            mAttachmentSlots = new List<MailItem>();

            mAttachmentContainer = new ScrollControl(mSendMailBoxWindow, "AttachmentContainer");
            mAttachmentContainer.SetBounds(20, 180, 380, 100); // Ajustar según el diseño
            mAttachmentContainer.EnableScroll(false, true);

            for (int i = 0; i < 5; i++) // Ejemplo: 5 slots
            {
                var mailSlot = new MailItem(this, i, mAttachmentContainer);
                mAttachmentSlots.Add(mailSlot);
               
                var xPadding = 5;
                var yPadding = 5;

                mailSlot.SlotPanel.SetPosition(
                    i % (mAttachmentContainer.Width / (mailSlot.SlotPanel.Width + xPadding)) * (mailSlot.SlotPanel.Width + xPadding) + xPadding,
                    i / (mAttachmentContainer.Width / (mailSlot.SlotPanel.Width + xPadding)) * (mailSlot.SlotPanel.Height + yPadding) + yPadding
                );
            }
        }
        public void UpdateAttachmentSlots()
        {
            for (int i = 0; i < mAttachmentSlots.Count; i++)
            {
                if (i < mAttachments.Count)
                {
                    var attachment = mAttachments[i];
                    var item = new Item
                    {
                        ItemId = attachment.ItemId,
                        Quantity = attachment.Quantity,
                        ItemProperties = attachment.Properties
                    };

                    mAttachmentSlots[i].SetItem(item);
                }
                else
                {
                    mAttachmentSlots[i].ClearItem();
                }
            }
        }

        public void OnDropItem(Item item, int slotIndex)
        {
            var targetSlot = mAttachmentSlots[slotIndex];

            if (targetSlot.IsEmpty)
            {
                targetSlot.SetItem(item);
                for (int i = 0; i < Globals.Me.Inventory.Length; i++)
                {
                    var inventorySlot = Globals.Me.Inventory[i];
                    if (inventorySlot?.ItemId == item.ItemId)
                    {
                        var toRemove = Math.Min(item.Quantity, inventorySlot.Quantity);
                        inventorySlot.Quantity -= toRemove;

                        if (inventorySlot.Quantity <= 0)
                        {
                            Globals.Me.Inventory[i] = null;
                        }

                        break;
                    }
                }
            }
            else
            {
                PacketSender.SendChatMsg(Strings.MailBox.slotOccupied,4);
            }
        }

        private void AddItemButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            var selectedItem = mItemComboBox.SelectedItem;
            int invSlot = (int)selectedItem?.UserData;
            if (invSlot < 0) return;

            var item = Globals.Me.Inventory[invSlot];
            if (item == null || item.ItemId == Guid.Empty) return;

            int quantity = (int)mQuantityTextBoxNumeric.Value;

            // Manejar ítems no apilables
            if (!item.Base.IsStackable && quantity > 1)
            {
                PacketSender.SendChatMsg("Cannot send multiple non-stackable items in one slot.", 4);
                return;
            }

            // Verificar si hay slots disponibles
            foreach (var slot in mAttachmentSlots)
            {
                if (slot.IsEmpty)
                {
                    // Crear una instancia parcial del ítem para adjuntar
                    var partialItem = new Item
                    {
                        ItemId = item.ItemId,
                        Quantity = quantity,
                        ItemProperties = item.ItemProperties
                    };

                    slot.SetItem(partialItem);

                    mAttachments.Add(new MailAttachmentPacket
                    {
                        ItemId = item.ItemId,
                        Quantity = quantity,
                        Properties = item.ItemProperties
                    });

                    // Reducir la cantidad en el inventario
                    item.Quantity -= quantity;
                    if (item.Quantity <= 0)
                    {
                        Globals.Me.Inventory[invSlot] = null;
                    }

                    return;
                }
            }

            // Si no hay slots disponibles
            PacketSender.SendChatMsg("No free slots available to add the item.", 4);
        }


        void SendButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (string.IsNullOrWhiteSpace(mToTextbox.Text) || string.IsNullOrWhiteSpace(mTitleTextbox.Text))
            {
                PacketSender.SendChatMsg(Strings.MailBox.invalidInput.ToString(), 4);
                return;
            }

            if (mAttachments.Count == 0)
            {
                PacketSender.SendChatMsg(Strings.MailBox.noAttachments.ToString(), 4);
                return;
            }

            // Validar ítems no apilables
            foreach (var attachment in mAttachments)
            {
                var itemBase = ItemBase.Get(attachment.ItemId);
                if (!itemBase.IsStackable && attachment.Quantity > 1)
                {
                    PacketSender.SendChatMsg("Cannot send multiple non-stackable items in one attachment.", 4);
                    return;
                }
            }

            // Enviar correo
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
        public void RestoreItemsToInventory()
        {
            foreach (var slot in mAttachmentSlots)
            {
                if (!slot.IsEmpty)
                {
                    var item = slot.CurrentSlot;
                    if (item == null || item.ItemId == Guid.Empty)
                    {
                        continue;
                    }

                    bool added = false;
                    for (int i = 0; i < Globals.Me.Inventory.Length; i++)
                    {
                        var inventorySlot = Globals.Me.Inventory[i];

                        if (inventorySlot == null || inventorySlot.ItemId == Guid.Empty)
                        {
                            Globals.Me.Inventory[i] = new Item
                            {
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                ItemProperties = item.ItemProperties
                            };
                            added = true;
                            break;
                        }

                        if (inventorySlot.ItemId == item.ItemId && inventorySlot.Base.IsStackable)
                        {
                            var maxStack = inventorySlot.Base.MaxInventoryStack;
                            var spaceLeft = maxStack - inventorySlot.Quantity;

                            var toAdd = Math.Min(spaceLeft, item.Quantity);
                            inventorySlot.Quantity += toAdd;
                            item.Quantity -= toAdd;

                            if (item.Quantity <= 0)
                            {
                                added = true;
                                break;
                            }
                        }
                    }

                    if (!added)
                    {
                        PacketSender.SendChatMsg("Inventory full. Could not restore item.", 4);
                    }

                    slot.ClearItem();
                }
            }
        }


        public void Close()
        {
            if (mAttachmentSlots != null)
            {
                RestoreItemsToInventory();
            }

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
