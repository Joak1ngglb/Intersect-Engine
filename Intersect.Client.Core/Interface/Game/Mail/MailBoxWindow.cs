using System;
using System.Collections.Generic;

using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game.Mail
{
    public class MailBoxWindow
    {
        private WindowControl mMailBoxWindow;

        private Label mMail;
        private ListBox mMailListBox;

        private Label mSender;
        private Label mTitle;
        private RichLabel mMessage;
        private Label mAttachments;
        private Button mSendMail;
        private Button mCloseButton;

        private Button mTakeButton;
        private ScrollControl mAttachmentContainer;
        private List<MailItem> mAttachmentSlots;
        public Canvas GameCanvas { get; private set; }

        public MailBoxWindow(Canvas gameCanvas)
        {
            mMailBoxWindow = new WindowControl(gameCanvas, Strings.MailBox.title, false, "MailBoxWindow");
            mMailBoxWindow.SetSize(600, 600); // Tamaño ajustado
            Interface.InputBlockingElements.Add(mMailBoxWindow);

            // "Mail" Label
            mMail = new Label(mMailBoxWindow, "Mail") { Text = Strings.MailBox.mails };
            mMail.SetBounds(20, 10, 100, 20);

            // Mail ListBox
            mMailListBox = new ListBox(mMailBoxWindow, "MailListBox");
            mMailListBox.SetBounds(20, 40, 460, 150);
            mMailListBox.EnableScroll(false, true);
            mMailListBox.RowSelected += Selected_MailListBox;
            mMailListBox.AllowMultiSelect = false;


            // Sender Label
            mSender = new Label(mMailBoxWindow, "Sender");
            mSender.SetBounds(20, 200, 560, 20);
            mSender.Hide();

            // Title Label
            mTitle = new Label(mMailBoxWindow, "Title");
            mTitle.SetBounds(20, 230, 560, 20);
            mTitle.Hide();

            // Message Label
            mMessage = new RichLabel(mMailBoxWindow, "Message");
            mMessage.SetBounds(20, 260, 560, 100);
            mMessage.Hide();

            // Attachments Container
            InitAttachmentSlots();

            // Buttons
            InitButtons();

            // Cargar diseño
            mMailBoxWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            // Centrar la ventana
            mMailBoxWindow.SetPosition(
                Graphics.Renderer.GetScreenWidth() / 2 - mMailBoxWindow.Width / 2,
                Graphics.Renderer.GetScreenHeight() / 2 - mMailBoxWindow.Height / 2
            );

            mMailBoxWindow.DisableResizing();
        

        }
        private void InitAttachmentSlots()
        {
            if (mAttachmentSlots == null)
            {
                mAttachmentSlots = new List<MailItem>();
            }

            if (mAttachmentContainer == null)
            {
                mAttachmentContainer = new ScrollControl(mMailBoxWindow, "AttachmentContainer");
                mAttachmentContainer.SetBounds(20, 370, 560, 80); // Posición ajustada
                mAttachmentContainer.EnableScroll(false, true);
            }

            for (int i = 0; i < 5; i++) // Máximo de 5 slots
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

        private void InitButtons()
        {
            // Take Button
            mTakeButton = new Button(mMailBoxWindow, "TakeButton");
            mTakeButton.SetText(Strings.MailBox.take);
            mTakeButton.SetBounds(20, 470, 120, 30);
            mTakeButton.Clicked += Take_Clicked;
            mTakeButton.Hide();

            // Send Mail Button
            mSendMail = new Button(mMailBoxWindow, "SendMailButton");
            mSendMail.SetText("Send Mail");
            mSendMail.SetBounds(150, 470, 120, 30);
            mSendMail.Clicked += SendMail_Clicked;

            // Close Button
            mCloseButton = new Button(mMailBoxWindow, "CloseButton");
            mCloseButton.SetText(Strings.MailBox.close);
            mCloseButton.SetBounds(490, 470, 100, 30);
            mCloseButton.Clicked += CloseButton_Clicked;
        }

        private void SendMail_Clicked(Base sender, ClickedEventArgs arguments)
        {
            
            var sendMailWindow = new SendMailBoxWindow(GameCanvas);
            sendMailWindow.Show();
        }

        private void Take_Clicked(Base sender, ClickedEventArgs e)
        {
            var selected = mMailListBox.SelectedRow;
            if (selected?.UserData is Client.Mail mail)
            {
                if (mail.Attachments == null || mail.Attachments.Count == 0)
                {
                    PacketSender.SendChatMsg(Strings.MailBox.noAttachments.ToString(), 4);
                    return;
                }

                PacketSender.SendTakeMail(mail.MailID);
            }
            else
            {
                PacketSender.SendChatMsg("No mail selected.", 4);
            }
        }

        private void Selected_MailListBox(Base sender, ItemSelectedEventArgs e)
        {
            var selected = mMailListBox.SelectedRow;
            if (selected?.UserData is Client.Mail mail)
            {
                // Actualizar detalles del correo
                mSender.Text = $"{Strings.MailBox.sender}: {mail.SenderName}";
                mTitle.Text = $"{Strings.MailBox.mailtitle}: {mail.Name}";
                mMessage.ClearText();
                mMessage.AddText(mail.Message, Color.White);

                // Mostrar adjuntos
                for (int i = 0; i < mAttachmentSlots.Count; i++)
                {
                    if (i < mail.Attachments.Count)
                    {
                        var attachment = mail.Attachments[i];
                        var item = new Item
                        {
                            ItemId = attachment.ItemId,
                            Quantity = attachment.Quantity,
                            ItemProperties = attachment.Properties
                        };

                        mAttachmentSlots[i].SetItem(item);
                        mAttachmentSlots[i].SlotPanel.IsHidden = false;
                    }
                    else
                    {
                        mAttachmentSlots[i].ClearItem();
                        mAttachmentSlots[i].SlotPanel.IsHidden = true;
                    }
                }

                mSender.Show();
                mTitle.Show();
                mMessage.Show();
                mTakeButton.Show();
            }
            else
            {
                // Ocultar elementos si no hay correo seleccionado
                mSender.Hide();
                mTitle.Hide();
                mMessage.Hide();
                mAttachments.Hide();
                mTakeButton.Hide();

                foreach (var slot in mAttachmentSlots)
                {
                    slot.ClearItem();
                    slot.SlotPanel.IsHidden = true;
                }
            }
        }

        void CloseButton_Clicked(Base sender, ClickedEventArgs e)
        {
            PacketSender.SendCloseMail();
        }

        public void UpdateMail()
        {
            // Validar inicialización
            if (mMailListBox == null || mAttachmentSlots == null)
            {
                Console.WriteLine("Componentes de MailBoxWindow no están inicializados.");
                return;
            }

            // Limpiar lista de correos y contenedor de adjuntos
            mMailListBox.RemoveAllRows();
            mMailListBox.ScrollToTop();

            foreach (var slot in mAttachmentSlots)
            {
                slot.ClearItem();
                slot.SlotPanel.IsHidden = true;
            }

            // Validar y actualizar correos
            if (Globals.Mails == null || Globals.Mails.Count == 0)
            {
                mSender?.Hide();
                mTitle?.Hide();
                mMessage?.Hide();
                mAttachments?.Hide();
                mTakeButton?.Hide();
                return;
            }

            foreach (var mail in Globals.Mails)
            {
                if (mail == null)
                {
                    Console.WriteLine("Se encontró un correo nulo en Globals.Mails.");
                    continue;
                }

                var row = mMailListBox.AddRow(mail.Name?.Trim() ?? "Unknown Mail", "", mail);
                row.SetTextColor(Color.White);
            }

            // Seleccionar el primer correo automáticamente
            mMailListBox.SelectByUserData(Globals.Mails[0]);
        }

        public void Close()
        {
            mMailBoxWindow.Close();
        }

        public bool IsVisible()
        {
            return !mMailBoxWindow.IsHidden;
        }

        public void Hide()
        {
            mMailBoxWindow.IsHidden = true;
        }

        public void Show()
        {
            mMailBoxWindow.Show();
        }
    }
}
