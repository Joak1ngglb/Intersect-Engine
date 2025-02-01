using System.Xml.Linq;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Quest;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.Framework.Core.Config;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Client.Interface.Game;


public partial class QuestsWindow:IQuestWindow
{
    public int X => mQuestsWindow.X;
    public int Y => mQuestsWindow.Y;
    private Button mBackButton;

    private ScrollControl mQuestDescArea;

    private RichLabel mQuestDescLabel;

    private Label mQuestDescTemplateLabel;

    private ListBox mQuestList;

    private Label mQuestStatus;

    //Controls
    private WindowControl mQuestsWindow;

    private Label mQuestTitle;

    private Button mQuitButton;

    private QuestBase mSelectedQuest;
    private List<QuestRewardItem> RewardItems = new List<QuestRewardItem>();
    private List<Label> mRewardValues = new List<Label>();
    private ScrollControl mRewardItemsContainer;
    private List<QuestRewardExp> RewardExp = new List<QuestRewardExp>();

    private ScrollControl mRewardContainer;
   public ScrollControl mRewardExpContainer;

    //Init
    public QuestsWindow(Canvas gameCanvas)
    {
        mQuestsWindow = new WindowControl(gameCanvas, Strings.QuestLog.Title, false, "QuestsWindow");
        mQuestsWindow.DisableResizing();

        mQuestList = new ListBox(mQuestsWindow, "QuestList");
        mQuestList.EnableScroll(false, true);

        mQuestTitle = new Label(mQuestsWindow, "QuestTitle");
        mQuestTitle.SetText("");

        mQuestStatus = new Label(mQuestsWindow, "QuestStatus");
        mQuestStatus.SetText("");

        mQuestDescArea = new ScrollControl(mQuestsWindow, "QuestDescription");

        mQuestDescTemplateLabel = new Label(mQuestsWindow, "QuestDescriptionTemplate");

        mQuestDescLabel = new RichLabel(mQuestDescArea);

        mBackButton = new Button(mQuestsWindow, "BackButton");
        mBackButton.Text = Strings.QuestLog.Back;
        mBackButton.Clicked += _backButton_Clicked;

        mQuitButton = new Button(mQuestsWindow, "AbandonQuestButton");
        mQuitButton.SetText(Strings.QuestLog.Abandon);
        mQuitButton.Clicked += _quitButton_Clicked;
        // Contenedor principal de recompensas (engloba EXP y objetos)
        mRewardContainer = new ScrollControl(mQuestsWindow, "RewardContainer");
        mRewardContainer.EnableScroll(false, true);
        mRewardContainer.SetSize(200, 120); // Ajustar tamaño general de recompensas
        mRewardContainer.SetPosition(10, 260); // Posición debajo de la descripción de la misión
        mRewardContainer.IsHidden = true;

        // Contenedor de recompensas de experiencia (se ubica dentro del contenedor principal)
        mRewardExpContainer = new ScrollControl(mRewardContainer, "RewardExpContainer");
        mRewardExpContainer.EnableScroll(false, true);
        mRewardExpContainer.SetSize(200, 40); // Espacio suficiente para múltiples recompensas de experiencia
        mRewardExpContainer.SetPosition(0, 0); // Se sitúa arriba dentro del contenedor principal
        mRewardExpContainer.IsHidden = true;

        // Contenedor de recompensas de ítems (se ubica debajo de la EXP dentro del contenedor principal)
        mRewardItemsContainer = new ScrollControl(mRewardContainer, "RewardItemsContainer");
        mRewardItemsContainer.EnableScroll(false, true);
        mRewardItemsContainer.SetSize(200, 80); // Espacio suficiente para varios ítems
        mRewardItemsContainer.SetPosition(0, 45); // Se sitúa debajo del contenedor de EXP
        mRewardItemsContainer.IsHidden = true;


        mQuestsWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
    }

    private void _quitButton_Clicked(Base sender, ClickedEventArgs arguments)
    {
        if (mSelectedQuest != null)
        {
            _ = new InputBox(
                title: Strings.QuestLog.AbandonTitle.ToString(mSelectedQuest.Name),
                prompt: Strings.QuestLog.AbandonPrompt.ToString(mSelectedQuest.Name),
                inputType: InputBox.InputType.YesNo,
                userData: mSelectedQuest.Id,
                onSuccess: (s, e) =>
                {
                    if (s is InputBox inputBox && inputBox.UserData is Guid questId)
                    {
                        PacketSender.SendAbandonQuest(questId);
                        mSelectedQuest = null;
                        ClearSelectedRewardItems();
                    }
                }
            );
        }
    }

    void AbandonQuest(object sender, EventArgs e)
    {
        PacketSender.SendAbandonQuest((Guid) ((InputBox) sender).UserData);
        ClearSelectedRewardItems();
    }

    private void _backButton_Clicked(Base sender, ClickedEventArgs arguments)
    {
        mSelectedQuest = null;
        ClearSelectedRewardItems();
        UpdateSelectedQuest();
      
    }
    private void ClearSelectedRewardItems()
    {
        RewardItems.Clear();
        mRewardValues.Clear();
        mRewardItemsContainer.DeleteAll();
        mRewardItemsContainer.IsHidden = true;
        mRewardContainer.IsHidden = true;
    }
    //Methods
    public void Update(bool shouldUpdateList)
    {
        if (shouldUpdateList)
        {
            UpdateQuestList();
            UpdateSelectedQuest();
        }

        if (mQuestsWindow.IsHidden)
        {
            return;
        }

        if (mSelectedQuest != null)
        {
            if (Globals.Me.QuestProgress.ContainsKey(mSelectedQuest.Id))
            {
                if (Globals.Me.QuestProgress[mSelectedQuest.Id].Completed &&
                    Globals.Me.QuestProgress[mSelectedQuest.Id].TaskId == Guid.Empty)
                {
                    //Completed
                    if (!mSelectedQuest.LogAfterComplete)
                    {
                        mSelectedQuest = null;
                        UpdateSelectedQuest();
                    }

                    return;
                }
                else
                {
                    if (Globals.Me.QuestProgress[mSelectedQuest.Id].TaskId == Guid.Empty)
                    {
                        //Not Started
                        if (!mSelectedQuest.LogBeforeOffer)
                        {
                            mSelectedQuest = null;
                            UpdateSelectedQuest();
                        }
                    }

                    return;
                }
            }

            if (!mSelectedQuest.LogBeforeOffer)
            {
                mSelectedQuest = null;
                UpdateSelectedQuest();
            }
        }
    }

    private void UpdateQuestList()
    {
        mQuestList.RemoveAllRows();
        if (Globals.Me != null)
        {
            var quests = QuestBase.Lookup.Values;

            var dict = new Dictionary<string, List<Tuple<QuestBase, int, Color>>>();

            foreach (QuestBase quest in quests)
            {
                if (quest != null)
                {
                    AddQuestToDict(dict, quest);
                }
            }


            foreach (var category in Options.Instance.Quest.Categories)
            {
                if (dict.ContainsKey(category))
                {
                    AddCategoryToList(category, Color.White);
                    var sortedList = dict[category].OrderBy(l => l.Item2).ThenBy(l => l.Item1.OrderValue).ToList();
                    foreach (var qst in sortedList)
                    {
                        AddQuestToList(qst.Item1.Name, qst.Item3, qst.Item1.Id, true);
                    }
                }
            }

            if (dict.ContainsKey(""))
            {
                var sortedList = dict[""].OrderBy(l => l.Item2).ThenBy(l => l.Item1.OrderValue).ToList();
                foreach (var qst in sortedList)
                {
                    AddQuestToList(qst.Item1.Name, qst.Item3, qst.Item1.Id, false);
                }
            }

        }
    }

    private void AddQuestToDict(Dictionary<string, List<Tuple<QuestBase, int, Color>>> dict, QuestBase quest)
    {
        var category = "";
        var add = false;
        var color = Color.White;
        var orderVal = -1;
        if (Globals.Me.QuestProgress.ContainsKey(quest.Id))
        {
            if (Globals.Me.QuestProgress[quest.Id].TaskId != Guid.Empty)
            {
                add = true;
                category = !TextUtils.IsNone(quest.InProgressCategory) ? quest.InProgressCategory : "";
                color = CustomColors.QuestWindow.InProgress;
                orderVal = 1;
            }
            else
            {
                if (Globals.Me.QuestProgress[quest.Id].Completed)
                {
                    if (quest.LogAfterComplete)
                    {
                        add = true;
                        category = !TextUtils.IsNone(quest.CompletedCategory) ? quest.CompletedCategory : "";
                        color = CustomColors.QuestWindow.Completed;
                        orderVal = 3;
                    }
                }
                else
                {
                    if (quest.LogBeforeOffer && !Globals.Me.HiddenQuests.Contains(quest.Id))
                    {
                        add = true;
                        category = !TextUtils.IsNone(quest.UnstartedCategory) ? quest.UnstartedCategory : "";
                        color = CustomColors.QuestWindow.NotStarted;
                        orderVal = 2;
                    }
                }
            }
        }
        else
        {
            if (quest.LogBeforeOffer && !Globals.Me.HiddenQuests.Contains(quest.Id))
            {
                add = true;
                category = !TextUtils.IsNone(quest.UnstartedCategory) ? quest.UnstartedCategory : "";
                color = CustomColors.QuestWindow.NotStarted;
                orderVal = 2;
            }
        }

        if (add)
        {
            if (!dict.ContainsKey(category))
            {
                dict.Add(category, new List<Tuple<QuestBase, int, Color>>());
            }

            dict[category].Add(new Tuple<QuestBase, int, Color>(quest, orderVal, color));
        }
    }

    private void AddQuestToList(string name, Color clr, Guid questId, bool indented = true)
    {
        var item = mQuestList.AddRow((indented ? "\t\t\t" : "") + name);
        item.UserData = questId;
        item.Clicked += QuestListItem_Clicked;
        item.Selected += Item_Selected;
        item.SetTextColor(clr);
        item.RenderColor = new Color(50, 255, 255, 255);
    }

    private void AddCategoryToList(string name, Color clr)
    {
        var item = mQuestList.AddRow(name);
        item.MouseInputEnabled = false;
        item.SetTextColor(clr);
        item.RenderColor = new Color(0, 255, 255, 255);
    }

    private void Item_Selected(Base sender, ItemSelectedEventArgs arguments)
    {
        mQuestList.UnselectAll();
    }

    private void QuestListItem_Clicked(Base sender, ClickedEventArgs arguments)
    {
        var questNum = (Guid) ((ListBoxRow) sender).UserData;
        var quest = QuestBase.Get(questNum);
        if (quest != null)
        {
            mSelectedQuest = quest;
            UpdateSelectedQuest();
        }

        mQuestList.UnselectAll();
    }

    private void UpdateSelectedQuest()
    {
        if (mSelectedQuest == null)
        {
            mQuestList.Show();
            mQuestTitle.Hide();
            mQuestDescArea.Hide();
            mQuestStatus.Hide();
            mBackButton.Hide();
            mQuitButton.Hide();
        }
        else
        {
            mQuestDescLabel.ClearText();
            mQuitButton.IsDisabled = true;
            ListBoxRow rw;
            string[] myText = null;
            var taskString = new List<string>();
            if (Globals.Me.QuestProgress.ContainsKey(mSelectedQuest.Id))
            {
                if (Globals.Me.QuestProgress[mSelectedQuest.Id].TaskId != Guid.Empty)
                {
                    //In Progress
                    mQuestStatus.SetText(Strings.QuestLog.InProgress);
                    mQuestStatus.SetTextColor(CustomColors.QuestWindow.InProgress, Label.ControlState.Normal);
                    mQuestDescTemplateLabel.SetTextColor(CustomColors.QuestWindow.QuestDesc, Label.ControlState.Normal);
                    
                    if (mSelectedQuest.InProgressDescription.Length > 0)
                    {    
                        mQuestDescLabel.AddText(mSelectedQuest.InProgressDescription, mQuestDescTemplateLabel);

                        mQuestDescLabel.AddLineBreak();
                        mQuestDescLabel.AddLineBreak();
                    }

                    mQuestDescLabel.AddText(Strings.QuestLog.CurrentTask, mQuestDescTemplateLabel);

                    mQuestDescLabel.AddLineBreak();
                    for (var i = 0; i < mSelectedQuest.Tasks.Count; i++)
                    {
                        if (mSelectedQuest.Tasks[i].Id == Globals.Me.QuestProgress[mSelectedQuest.Id].TaskId)
                        {
                            if (mSelectedQuest.Tasks[i].Description.Length > 0)
                            {
                                mQuestDescLabel.AddText(mSelectedQuest.Tasks[i].Description, mQuestDescTemplateLabel);

                                mQuestDescLabel.AddLineBreak();
                                mQuestDescLabel.AddLineBreak();
                            }

                            if (mSelectedQuest.Tasks[i].Objective == QuestObjective.GatherItems) //Gather Items
                            {
                                mQuestDescLabel.AddText(
                                    Strings.QuestLog.TaskItem.ToString(
                                        Globals.Me.QuestProgress[mSelectedQuest.Id].TaskProgress,
                                        mSelectedQuest.Tasks[i].Quantity,
                                        ItemBase.GetName(mSelectedQuest.Tasks[i].TargetId)
                                    ), mQuestDescTemplateLabel
                                );
                            }
                            else if (mSelectedQuest.Tasks[i].Objective == QuestObjective.KillNpcs) //Kill Npcs
                            {
                                mQuestDescLabel.AddText(
                                    Strings.QuestLog.TaskNpc.ToString(
                                        Globals.Me.QuestProgress[mSelectedQuest.Id].TaskProgress,
                                        mSelectedQuest.Tasks[i].Quantity,
                                        NpcBase.GetName(mSelectedQuest.Tasks[i].TargetId)
                                    ), mQuestDescTemplateLabel
                                );
                            }
                        }
                    }

                    mQuitButton.IsDisabled = !mSelectedQuest.Quitable;
                }
                else
                {
                    if (Globals.Me.QuestProgress[mSelectedQuest.Id].Completed)
                    {
                        //Completed
                        if (mSelectedQuest.LogAfterComplete)
                        {
                            mQuestStatus.SetText(Strings.QuestLog.Completed);
                            mQuestStatus.SetTextColor(CustomColors.QuestWindow.Completed, Label.ControlState.Normal);
                            mQuestDescLabel.AddText(mSelectedQuest.EndDescription, mQuestDescTemplateLabel);
                        }
                    }
                    else
                    {
                        //Not Started
                        if (mSelectedQuest.LogBeforeOffer)
                        {
                            mQuestStatus.SetText(Strings.QuestLog.NotStarted);
                            mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
                            mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);

                            mQuitButton?.Hide();
                        }
                    }
                }
            }
            else
            {
                //Not Started
                if (mSelectedQuest.LogBeforeOffer)
                {
                    mQuestStatus.SetText(Strings.QuestLog.NotStarted);
                    mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
                    mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);
                }
            }
            // Load and display reward items
            LoadRewardItems(mSelectedQuest.Id);

            foreach (var rewardItem in RewardItems)
            {
                rewardItem.Update();
            }
            foreach (var rewardExp in RewardExp)
            {
                rewardExp.Update();
            }
            // Mostrar u ocultar el contenedor principal de recompensas
            mRewardContainer.IsHidden = (RewardExp.Count == 0 && RewardItems.Count == 0);

            mQuestList.Hide();
            mQuestTitle.IsHidden = false;
            mQuestTitle.Text = mSelectedQuest.Name;
            mQuestDescArea.IsHidden = false;
            mQuestDescLabel.Width = mQuestDescArea.Width - mQuestDescArea.GetVerticalScrollBar().Width;
            mQuestDescLabel.SizeToChildren(false, true);
            mQuestStatus.Show();
            mBackButton.Show();
            mQuitButton.Show();
        
        }
    }
    private void LoadRewardItems(Guid questId)
    {
        RewardItems.Clear();
        RewardExp.Clear();
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
        mRewardItemsContainer.SetPosition(0, RewardExp.Count > 0 ? 50 : 0);
    }

    private void LoadRewardExperience(Guid questId)
    {
        int expIndex = 0; // Controla la posición vertical de las recompensas

        long rewardExp = Globals.QuestExperience.ContainsKey(questId) ? Globals.QuestExperience[questId] : 0;
        var rewardJobExp = Globals.QuestJobExperience.ContainsKey(questId) ? Globals.QuestJobExperience[questId] : new Dictionary<JobType, long>();

        // Limpiar recompensas anteriores
        foreach (var reward in RewardExp)
        {
            RewardExp.Clear();
        }
       
        if (rewardExp > 0)
        {
            var expReward = new QuestRewardExp(this, rewardExp);
            RewardExp.Add(expReward);
            expReward.Setup();
            expReward.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            // Colocar en columna
            expReward.Container.SetPosition(0, expIndex * 30);
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

                // Colocar en columna
                jobExpReward.Container.SetPosition(0, expIndex * 30);
                expIndex++;
            }
        }

        // Ajustar altura del contenedor de experiencia en base a la cantidad de recompensas
        int totalHeight = expIndex * 30;
        mRewardExpContainer.SetSize(mRewardExpContainer.Width, Math.Max(totalHeight, 30)); // Mínimo de 30px si hay al menos una recompensa

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
        mQuestsWindow.IsHidden = false;
    }

    public bool IsVisible()
    {
        return !mQuestsWindow.IsHidden;
    }

    public void Hide()
    {
        mQuestsWindow.IsHidden = true;
        mSelectedQuest = null;
    }

}
