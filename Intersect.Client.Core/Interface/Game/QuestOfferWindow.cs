using System;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Quest;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game;


public partial class QuestOfferWindow : IQuestWindow
{

    private Button mAcceptButton;

    private Button mDeclineButton;

    private string mQuestOfferText = "";

    //Controls
    private WindowControl mQuestOfferWindow;

    private ScrollControl mQuestPromptArea;

    private RichLabel mQuestPromptLabel;

    private Label mQuestPromptTemplate;

    private Label mQuestTitle;
    private ScrollControl mRewardItemsContainer;

    private bool mRewardItemsLoaded = false;

    private List<QuestRewardItem> RewardItems = new List<QuestRewardItem>();
    private List<Label> mRewardValues = new List<Label>();
    private Guid mQuestId = Guid.Empty;

    public int X => mQuestOfferWindow.X;
    public int Y => mQuestOfferWindow.Y;
    public QuestOfferWindow(Canvas gameCanvas)
    {
        mQuestOfferWindow = new WindowControl(gameCanvas, Strings.QuestOffer.Title, false, "QuestOfferWindow");
        mQuestOfferWindow.DisableResizing();
        mQuestOfferWindow.IsClosable = false;

        //Menu Header
        mQuestTitle = new Label(mQuestOfferWindow, "QuestTitle");

        mQuestPromptArea = new ScrollControl(mQuestOfferWindow, "QuestOfferArea");

        mQuestPromptTemplate = new Label(mQuestPromptArea, "QuestOfferTemplate");

        mQuestPromptLabel = new RichLabel(mQuestPromptArea);

        //Accept Button
        mAcceptButton = new Button(mQuestOfferWindow, "AcceptButton");
        mAcceptButton.SetText(Strings.QuestOffer.Accept);
        mAcceptButton.Clicked += _acceptButton_Clicked;
        mRewardItemsContainer = new ScrollControl(mQuestOfferWindow, "RewardItemsContainer");
        mRewardItemsContainer.EnableScroll(true, false);
        mRewardItemsContainer.IsHidden = true;
        //Decline Button
        mDeclineButton = new Button(mQuestOfferWindow, "DeclineButton");
        mDeclineButton.SetText(Strings.QuestOffer.Decline);
        mDeclineButton.Clicked += _declineButton_Clicked;

        mQuestOfferWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        Interface.InputBlockingElements.Add(mQuestOfferWindow);
    }

    private void _declineButton_Clicked(Base sender, ClickedEventArgs arguments)
    {
        if (Globals.QuestOffers.Count > 0)
        {
            Globals.QuestRewards.Remove(Globals.QuestOffers[0]);
            Globals.QuestOffers.RemoveAt(0);
            RewardItems.Clear();
            mRewardItemsContainer.DeleteAll();
        }
    }

    private void _acceptButton_Clicked(Base sender, ClickedEventArgs arguments)
    {
        if (Globals.QuestOffers.Count > 0)
        {
            PacketSender.SendAcceptQuest(Globals.QuestOffers[0]);
            Globals.QuestOffers.RemoveAt(0);
            RewardItems.Clear();
            mRewardItemsContainer.DeleteAll();
        }
    }

    public void Update(QuestBase quest)
    {
        if (quest == null)
        {
            Hide();
        }
        else
        {
            Show();
            mQuestTitle.Text = quest.Name;
            if (mQuestOfferText != quest.StartDescription)
            {
                mQuestPromptLabel.ClearText();
                mQuestPromptLabel.Width = mQuestPromptArea.Width - mQuestPromptArea.GetVerticalScrollBar().Width;
                mQuestPromptLabel.AddText(quest.StartDescription, mQuestPromptTemplate);

                mQuestPromptLabel.SizeToChildren(false, true);
                mQuestOfferText = quest.StartDescription;
            }
            if (mQuestId != quest.Id || !mRewardItemsLoaded || RewardsChanged(quest.Id))
            {
                mQuestId = quest.Id;
                LoadRewardItems();
            }

            foreach (var rewardItem in RewardItems)
            {
                rewardItem.Update();
            }
        }
    }

    private bool RewardsChanged(Guid questId)
    {
        if (!Globals.QuestRewards.ContainsKey(questId))
        {
            return false;
        }

        var currentRewards = Globals.QuestRewards[questId];
        if (RewardItems.Count != currentRewards.Count)
        {
            return true;
        }

        foreach (var rewardItem in RewardItems)
        {
            if (!currentRewards.ContainsKey(rewardItem.mCurrentItemId))
            {
                return true;
            }
        }

        return false;
    }

    private void LoadRewardItems()
    {
        RewardItems.Clear();
        mRewardItemsContainer.DeleteAll();

        if (Globals.QuestRewards.ContainsKey(mQuestId) && Globals.QuestRewards[mQuestId].Count > 0)
        {
            var rewardItems = Globals.QuestRewards[mQuestId];
            int index = 0;
            foreach (var rewardItem in rewardItems)
            {
                var itemId = rewardItem.Key;
                var quantity = rewardItem.Value;

                RewardItems.Add(new QuestRewardItem(this, itemId, quantity));
                RewardItems[index].Container = new ImagePanel(mRewardItemsContainer, "RewardItem");
                RewardItems[index].Setup();

                // Add and configure the label for the item quantity
                var quantityLabel = new Label(RewardItems[index].Container, "RewardItemValue");
                quantityLabel.Text = Strings.FormatQuantityAbbreviated(quantity);
                mRewardValues.Add(quantityLabel);

                RewardItems[index].Container.LoadJsonUi(
                GameContentManager.UI.InGame,
                Graphics.Renderer.GetResolutionString()
                                );

                var xPadding = RewardItems[index].Container.Margin.Left + RewardItems[index].Container.Margin.Right;
                var yPadding = RewardItems[index].Container.Margin.Top + RewardItems[index].Container.Margin.Bottom;
                RewardItems[index].Container.SetPosition(
                index % (mRewardItemsContainer.Width / (RewardItems[index].Container.Width + xPadding)) *
                (RewardItems[index].Container.Width + xPadding) + xPadding,
                index / (mRewardItemsContainer.Width / (RewardItems[index].Container.Width + xPadding)) *
                (RewardItems[index].Container.Height + yPadding) + yPadding
                               );

                index++;
            }

            mRewardItemsLoaded = true;
        }
    }


    public void Show()
    {
        mQuestOfferWindow.IsHidden = false;
    }

    public void Close()
    {
        mQuestOfferWindow.Close();
    }

    public bool IsVisible()
    {
        return !mQuestOfferWindow.IsHidden;
    }

    public void Hide()
    {
        mQuestOfferWindow.IsHidden = true;
    }

}
