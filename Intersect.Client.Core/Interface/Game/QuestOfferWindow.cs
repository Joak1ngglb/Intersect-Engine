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
using Intersect.Framework.Core.Config;
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
    // Nuevo contenedor de experiencia
    private List<QuestRewardExp> RewardExp = new List<QuestRewardExp>();

    private ScrollControl mRewardContainer;
    public ScrollControl mRewardExpContainer;

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
        // Contenedor Principal de Recompensas
        mRewardContainer = new ScrollControl(mQuestOfferWindow, "RewardContainer");
        mRewardContainer.SetPosition(10, 250);
        mRewardContainer.SetSize(400, 120);
        mRewardContainer.IsHidden = true;

        // Contenedor de experiencia
        mRewardExpContainer = new ScrollControl(mRewardContainer, "RewardExpContainer");
        mRewardExpContainer.SetSize(380, 40);
        mRewardExpContainer.SetPosition(10, 10); // Ubicado en la parte superior
        mRewardExpContainer.IsHidden = true;

        // Contenedor de ítems de recompensa
        mRewardItemsContainer = new ScrollControl(mRewardContainer, "RewardItemsContainer");
        mRewardItemsContainer.SetSize(380, 50);
        mRewardItemsContainer.SetPosition(10, mRewardExpContainer.IsHidden ? 10 : mRewardExpContainer.Height + 15);
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
                LoadRewardItems(quest.Id);
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
    private void LoadRewardItems(Guid questId)
    {
        RewardItems.Clear();
        mRewardValues.Clear();
        mRewardItemsContainer.DeleteAll();
        mRewardExpContainer.DeleteAll();

        // Cargar EXP
        LoadRewardExperience(questId);

        // Cargar Ítems
        LoadRewardItemsList(questId, mRewardItemsContainer);

        // Ajustar el tamaño del contenedor de recompensas si es necesario
        mRewardExpContainer.IsHidden = RewardExp.Count == 0;
        mRewardItemsContainer.IsHidden = RewardItems.Count == 0;
        mRewardContainer.IsHidden = (RewardExp.Count == 0 && RewardItems.Count == 0);

        // Ajustar posición del contenedor de ítems según la cantidad de recompensas
        mRewardItemsContainer.SetPosition(10, mRewardExpContainer.IsHidden ? 10 : mRewardExpContainer.Height + 15);
    }

    private void LoadRewardExperience(Guid questId)
    {
        int expIndex = 0; // Controla la posición horizontal de las recompensas

        long rewardExp = Globals.QuestExperience.ContainsKey(questId) ? Globals.QuestExperience[questId] : 0;
        var rewardJobExp = Globals.QuestJobExperience.ContainsKey(questId) ? Globals.QuestJobExperience[questId] : new Dictionary<JobType, long>();

        RewardExp.Clear();
        mRewardExpContainer.DeleteAll(); // Limpiar antes de cargar nueva info

        int rewardWidth = 100; // Tamaño fijo de cada contenedor de experiencia
        int spacing = 5; // Espaciado entre cada recompensa

        if (rewardExp > 0)
        {
            var expReward = new QuestRewardExp(this, rewardExp);
            RewardExp.Add(expReward);
            expReward.Setup();
            expReward.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            // Colocar en fila
            expReward.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
            expIndex++;
        }

        foreach (var jobExp in rewardJobExp)
        {
            if (jobExp.Value > 0)
            {
                var jobExpReward = new QuestRewardExp(this, jobExp.Value, true, jobExp.Key);
                RewardExp.Add(jobExpReward);
                jobExpReward.Setup();
                jobExpReward.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                // Colocar en fila
                jobExpReward.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
                expIndex++;
            }
        }

        // Ajustar ancho del contenedor de experiencia en base a la cantidad de recompensas
        int totalWidth = expIndex * (rewardWidth + spacing) - spacing;
        mRewardExpContainer.SetSize(RewardExp.Count > 0 ? totalWidth : 0, mRewardExpContainer.Height);

        // Ocultar si no hay recompensas
        mRewardExpContainer.IsHidden = RewardExp.Count == 0;
    }

    private int LoadRewardItemsList(Guid questId, Base container)
    {
        if (!Globals.QuestRewards.ContainsKey(questId) || Globals.QuestRewards[questId].Count == 0)
        {
            return 0;
        }

        var rewardItems = Globals.QuestRewards[questId];
        int index = 0;

        foreach (var rewardItem in rewardItems)
        {
            var itemId = rewardItem.Key;
            var quantity = rewardItem.Value;

            var questRewardItem = new QuestRewardItem(this, itemId, quantity);
            RewardItems.Add(questRewardItem);

            questRewardItem.Container = new ImagePanel(container, "RewardItem");
            questRewardItem.Setup();

            // Agregar el label con la cantidad del ítem
            var quantityLabel = new Label(questRewardItem.Container, "RewardItemValue");
            quantityLabel.Text = Strings.FormatQuantityAbbreviated(quantity);
            mRewardValues.Add(quantityLabel);

            questRewardItem.Container.LoadJsonUi(
                GameContentManager.UI.InGame,
                Graphics.Renderer.GetResolutionString()
            );

            var xPadding = questRewardItem.Container.Margin.Left + questRewardItem.Container.Margin.Right;
            var yPadding = questRewardItem.Container.Margin.Top + questRewardItem.Container.Margin.Bottom;

            int containerWidth = Math.Max(container.Width, 1);
            int itemWidth = Math.Max(questRewardItem.Container.Width + xPadding, 1);
            int itemsPerRow = Math.Max(containerWidth / itemWidth, 1); // Evita división por 0

            questRewardItem.Container.SetPosition(
                (index % itemsPerRow) * itemWidth + xPadding,
                (index / itemsPerRow) * (questRewardItem.Container.Height + yPadding) + yPadding
            );

            index++;
        }

        return index * 40; // Devolver la altura ocupada
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
