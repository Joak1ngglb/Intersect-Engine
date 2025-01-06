using System;
using System.Collections.Generic;

using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
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

        public Canvas GameCanvas { get; private set; }

        public MailBoxWindow(Canvas gameCanvas)
        {
            mMailBoxWindow = new WindowControl(gameCanvas, Strings.MailBox.title, false, "MailBoxWindow");
            mMailBoxWindow.SetSize(500, 500); // Tamaño de la ventana
            Interface.InputBlockingElements.Add(mMailBoxWindow);

            // "Mail" Label
            mMail = new Label(mMailBoxWindow, "Mail") { Text = Strings.MailBox.mails };
            mMail.SetBounds(20, 10, 100, 20); // Posición y tamaño del texto "Mail"

            // Mail ListBox
            mMailListBox = new ListBox(mMailBoxWindow, "MailListBox");
            mMailListBox.SetBounds(20, 40, 460, 150); // Posición y tamaño del ListBox
            mMailListBox.EnableScroll(false, true);
            mMailListBox.RowSelected += Selected_MailListBox;
            mMailListBox.AllowMultiSelect = false;

            // Sender Label
            mSender = new Label(mMailBoxWindow, "Sender");
            mSender.SetBounds(20, 200, 460, 20); // Posición y tamaño del texto "Sender"
            mSender.Hide();

            // Title Label
            mTitle = new Label(mMailBoxWindow, "Title");
            mTitle.SetBounds(20, 230, 460, 20); // Posición y tamaño del texto "Title"
            mTitle.Hide();

            // Message Label
            mMessage = new RichLabel(mMailBoxWindow, "Message");
            mMessage.SetBounds(20, 260, 460, 100); // Posición y tamaño del área de mensaje
            mMessage.Hide();

            // Attachments Label
            mAttachments = new Label(mMailBoxWindow, "Attachments");
            mAttachments.SetBounds(20, 370, 460, 20); // Posición y tamaño del texto "Attachments"
            mAttachments.Hide();

            // Take Button
            mTakeButton = new Button(mMailBoxWindow, "TakeButton");
            mTakeButton.SetText(Strings.MailBox.take);
            mTakeButton.SetBounds(20, 400, 120, 30); // Posición y tamaño del botón "Take"
            mTakeButton.Clicked += Take_Clicked;
            mTakeButton.Hide();

            // Send Mail Button
            mSendMail = new Button(mMailBoxWindow, "SendMailButton");
            mSendMail.SetText("Send Mail");
            mSendMail.SetBounds(150, 400, 120, 30); // Posición y tamaño del botón "Send Mail"
            mSendMail.Clicked += SendMail_Clicked;

            // Close Button
            mCloseButton = new Button(mMailBoxWindow, "CloseButton");
            mCloseButton.SetText(Strings.MailBox.close);
            mCloseButton.SetBounds(380, 400, 100, 30); // Posición y tamaño del botón "Close"
            mCloseButton.Clicked += CloseButton_Clicked;
            mCloseButton.Hide();

            mMailBoxWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            // Centrar la ventana
            mMailBoxWindow.SetPosition(
                Graphics.Renderer.GetScreenWidth() / 2 - mMailBoxWindow.Width / 2,
                Graphics.Renderer.GetScreenHeight() / 2 - mMailBoxWindow.Height / 2
            );

            mMailBoxWindow.DisableResizing();

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
                PacketSender.SendTakeMail(mail.MailID);
            }
        }

        private void Selected_MailListBox(Base sender, ItemSelectedEventArgs e)
        {
            var selected = mMailListBox.SelectedRow;
            if (selected?.UserData is Client.Mail mail)
            {
                mSender.Text = $"{Strings.MailBox.sender}: {mail.SenderName}";
                mTitle.Text = $"{Strings.MailBox.mailtitle}: {mail.Name}";
                mMessage.ClearText();
                mMessage.AddText(mail.Message, Color.White);

                if (mail.Attachments != null && mail.Attachments.Count > 0)
                {
                    var attachmentDetails = string.Join("\n", mail.Attachments.Select(a =>
                    {
                        var item = ItemBase.Get(a.ItemId);
                        return item != null ? $"{item.Name} x{a.Quantity}" : "Unknown Item";
                    }));
                    mAttachments.Text = $"{Strings.MailBox.attachments}:\n{attachmentDetails}";
                    mAttachments.Show();
                }
                else
                {
                    mAttachments.Hide();
                }

                mSender.Show();
                mTitle.Show();
                mMessage.Show();
                mTakeButton.Show();
            }
            else
            {
                mSender.Hide();
                mTitle.Hide();
                mMessage.Hide();
                mAttachments.Hide();
                mTakeButton.Hide();
            }
        }

        void CloseButton_Clicked(Base sender, ClickedEventArgs e)
        {
            PacketSender.SendCloseMail();
        }

        public void UpdateMail()
        {
            mMailListBox.RemoveAllRows();
            mMailListBox.ScrollToTop();

            foreach (var mail in Globals.Mails)
            {
                var row = mMailListBox.AddRow(mail.Name.Trim(), "", mail);
                row.SetTextColor(Color.White);
            }

            if (Globals.Mails.Count > 0)
            {
                mMailListBox.SelectByUserData(Globals.Mails[0]);
            }
            else
            {
                mSender.Hide();
                mTitle.Hide();
                mMessage.Hide();
                mAttachments.Hide();
                mTakeButton.Hide();
            }
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
