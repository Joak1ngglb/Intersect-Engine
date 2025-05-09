using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Intersect.Client.Core;
using Intersect.Client.Framework.Content;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Graphics;
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

public partial class QuestsWindow : IQuestWindow
{
    public int X => mQuestsWindow.X;
    public int Y => mQuestsWindow.Y;

// ---------- FLAGS & CACHE ----------
private static readonly GameTexture TexCheckFull =
    Globals.ContentManager.GetTexture(TextureType.Gui, "checkboxfull.png");
private static readonly GameTexture TexCheckEmpty =
    Globals.ContentManager.GetTexture(TextureType.Gui, "checkboxempty.png");


// ---------- GUI refs ----------
private readonly WindowControl mQuestsWindow;
    private Button mQuitButton;

    private ScrollControl mQuestDescArea;
    private RichLabel mQuestDescLabel;
    private Label mQuestDescTemplateLabel;

    private ListBox mQuestList;
    private Label mQuestStatus;
    private Label mQuestTitle;

    // Recompensas
    public ScrollControl mRewardContainer;
public ScrollControl mRewardItemsContainer;
    public ScrollControl mRewardExpContainer;
    private readonly List<QuestRewardItem> RewardItems = new();
    private readonly List<Label> mRewardValues = new();
    private readonly List<QuestRewardExp> RewardExp = new();

    // Objetivos
    private ScrollControl mQuestTasksContainer;
    private Label mQuestTasksTitle;
    private ListBox mQuestTasksList;

    // Lista de misiones
    private ScrollControl mQuestListContainer;

    // Estado
    private QuestBase mSelectedQuest;

    // ---------------------------------------------------- INIT ----------------------------------------------------
    public QuestsWindow(Canvas gameCanvas)
    {
        mQuestsWindow = new WindowControl(gameCanvas, Strings.QuestLog.Title, false, "QuestsWindow");
        mQuestsWindow.DisableResizing();

        InitializeQuestListPanel();
        InitializeQuestDetailPanel();
        InitializeRewardPanel();

        mQuestsWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
    }

    // ----------------------------------------------- PANELS -------------------------------------------------------
    private void InitializeQuestListPanel()
    {
        mQuestListContainer = new ScrollControl(mQuestsWindow, "QuestListContainer");
        mQuestListContainer.SetSize(250, 400);
        mQuestListContainer.SetPosition(10, 40);

        mQuestList = new ListBox(mQuestListContainer, "QuestList");
        mQuestList.EnableScroll(false, true);
        mQuestList.SetSize(250, 370);
        mQuestList.SetPosition(0, 30);
    }

    private void InitializeQuestDetailPanel()
    {
        mQuestTitle = new Label(mQuestsWindow, "QuestTitle");
        mQuestTitle.SetSize(330, 20);
        mQuestTitle.SetPosition(270, 40);
        mQuestTitle.TextColor = Color.Yellow;

        mQuestStatus = new Label(mQuestsWindow, "QuestStatus");
        mQuestStatus.SetSize(330, 20);
        mQuestStatus.SetPosition(270, 65);

        mQuestDescArea = new ScrollControl(mQuestsWindow, "QuestDescription");
        mQuestDescArea.SetSize(400, 200);
        mQuestDescArea.SetPosition(270, 90);

        mQuestDescTemplateLabel = new Label(mQuestDescArea, "QuestDescriptionTemplate");
        mQuestDescLabel = new RichLabel(mQuestDescArea);

        InitializeQuestTasksPanel();

        mQuitButton = new Button(mQuestsWindow, "AbandonQuestButton");
        mQuitButton.SetText(Strings.QuestLog.Abandon);
        mQuitButton.SetPosition(400, 470);
        mQuitButton.Clicked += _quitButton_Clicked;
    }

    private void InitializeQuestTasksPanel()
    {
        mQuestTasksContainer = new ScrollControl(mQuestsWindow, "QuestTasksContainer");
        mQuestTasksContainer.SetSize(400, 150);
        mQuestTasksContainer.SetPosition(270, 300);
        mQuestTasksContainer.EnableScroll(false, true);
        mQuestTasksContainer.IsHidden = true;

        mQuestTasksTitle = new Label(mQuestTasksContainer, "QuestTasksTitle");
        mQuestTasksTitle.SetSize(380, 20);
        mQuestTasksTitle.SetPosition(10, 5);
        mQuestTasksTitle.SetText("Objetivos:");
        mQuestTasksTitle.TextColor = Color.White;

        mQuestTasksList = new ListBox(mQuestTasksContainer, "QuestTasksList");
        mQuestTasksList.SetSize(380, 120);
        mQuestTasksList.SetPosition(10, 30);
        mQuestTasksList.EnableScroll(false, true);
    }

    private void InitializeRewardPanel()
    {
        mRewardContainer = new ScrollControl(mQuestsWindow, "RewardContainer");
        mRewardContainer.SetPosition(270, 350);
        mRewardContainer.SetSize(400, 120);
        mRewardContainer.IsHidden = true;

        mRewardExpContainer = new ScrollControl(mRewardContainer, "RewardExpContainer");
        mRewardExpContainer.SetSize(380, 40);
        mRewardExpContainer.SetPosition(10, 10);
        mRewardExpContainer.IsHidden = true;

        mRewardItemsContainer = new ScrollControl(mRewardContainer, "RewardItemsContainer");
        mRewardItemsContainer.SetSize(380, 50);
        mRewardItemsContainer.SetPosition(10, 60);
        mRewardItemsContainer.IsHidden = true;
    }

    // ------------------------------------------------ BOTONES -----------------------------------------------------
    private void _quitButton_Clicked(Base sender, ClickedEventArgs arguments)
    {
        if (mSelectedQuest == null) return;

       _ = new InputBox(
           Strings.QuestLog.AbandonTitle.ToString(mSelectedQuest.Name),
           Strings.QuestLog.AbandonPrompt.ToString(mSelectedQuest.Name),
           InputBox.InputType.YesNo,
           null, // Change the fourth argument from 'mSelectedQuest.Id' to 'null'
           (s, e) =>
           {
               if (s is InputBox ib && ib.UserData is Guid qid)
               {
                   PacketSender.SendAbandonQuest(qid);
                   mSelectedQuest = null;
                   ClearSelectedRewardItems();
               }
           }
       );
           
    }

    private void ClearSelectedRewardItems()
    {
        RewardItems.Clear();
        RewardExp.Clear();
        mRewardValues.Clear();
        mRewardItemsContainer.DeleteAll();
        mRewardExpContainer.DeleteAll();
        mRewardItemsContainer.IsHidden = true;
        mRewardExpContainer.IsHidden = true;
        mRewardContainer.IsHidden = true;
    }

    // ------------------------------------------------ UPDATE ------------------------------------------------------
    public void Update(bool shouldUpdateList)
    {
        if (shouldUpdateList || Globals.QuestDirty)
        {
            Globals.QuestDirty = false;
            UpdateQuestList();
            UpdateSelectedQuest();
            UpdateQuestTasks();
        }

        if (mQuestsWindow.IsHidden) return;

        if (mSelectedQuest != null)
        {
            if (!Globals.Me.QuestProgress.ContainsKey(mSelectedQuest.Id))
            {
                mSelectedQuest = null;
                UpdateSelectedQuest();
            }
        }
    }

    // --------------------------------------------- LISTA DE QUESTS ------------------------------------------------
    private void UpdateQuestList()
    {
        mQuestList.RemoveAllRows();
        if (Globals.Me == null) return;

        var dict = new Dictionary<string, List<Tuple<QuestBase, int, Color>>>();

        foreach (var quest in QuestBase.Lookup.Values)
            if (quest != null)
                AddQuestToDict(dict, (QuestBase)quest);

        foreach (var category in Options.Instance.Quest.Categories)
        {
            if (!dict.ContainsKey(category)) continue;

            AddCategoryToList(category, Color.White);
            foreach (var q in dict[category].OrderBy(t => t.Item2).ThenBy(t => t.Item1.OrderValue))
                AddQuestToList(q.Item1.Name, q.Item3, q.Item1.Id, false);
        }

        if (dict.ContainsKey(""))
            foreach (var q in dict[""].OrderBy(t => t.Item2).ThenBy(t => t.Item1.OrderValue))
                AddQuestToList(q.Item1.Name, q.Item3, q.Item1.Id, false);
    }

    // ---- helpers de lista -----------------
    private void AddQuestToDict(Dictionary<string, List<Tuple<QuestBase, int, Color>>> dict, QuestBase quest)
    {
        string category = "";
        bool add = false;
        Color color = Color.White;
        int orderVal = -1;

        if (Globals.Me.QuestProgress.ContainsKey(quest.Id))
        {
            var prog = Globals.Me.QuestProgress[quest.Id];

            if (prog.TaskId != Guid.Empty)
            {
                add = true;
                category = !TextUtils.IsNone(quest.InProgressCategory) ? quest.InProgressCategory : "";
                color = CustomColors.QuestWindow.InProgress;
                orderVal = 1;
            }
            else
            {
                if (prog.Completed)
                {
                    if (quest.LogAfterComplete)
                    {
                        add = true;
                        category = !TextUtils.IsNone(quest.CompletedCategory) ? quest.CompletedCategory : "";
                        color = CustomColors.QuestWindow.Completed;
                        orderVal = 3;
                    }
                }
                else if (quest.LogBeforeOffer && !Globals.Me.HiddenQuests.Contains(quest.Id))
                {
                    add = true;
                    category = !TextUtils.IsNone(quest.UnstartedCategory) ? quest.UnstartedCategory : "";
                    color = CustomColors.QuestWindow.NotStarted;
                    orderVal = 2;
                }
            }
        }
        else if (quest.LogBeforeOffer && !Globals.Me.HiddenQuests.Contains(quest.Id))
        {
            add = true;
            category = !TextUtils.IsNone(quest.UnstartedCategory) ? quest.UnstartedCategory : "";
            color = CustomColors.QuestWindow.NotStarted;
            orderVal = 2;
        }

        if (!add) return;

        if (!dict.ContainsKey(category))
            dict[category] = new List<Tuple<QuestBase, int, Color>>();

        dict[category].Add(Tuple.Create(quest, orderVal, color));
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
        var questId = (Guid)((ListBoxRow)sender).UserData;
        mSelectedQuest = QuestBase.Get(questId);
        UpdateSelectedQuest();
        UpdateQuestTasks();
        LoadRewardItems(questId);
        mQuestList.UnselectAll();
    }
    private void ClearTaskPanels()
    {
        // Vaciamos absolutamente todo, no solo filas
        foreach (var child in mQuestTasksList.Children.ToList())
        {
            mQuestTasksList.RemoveChild(child, true);
        }
    }

    // ------------------------------------------- SELECCIÓN --------------------------------------------------------
    private void UpdateSelectedQuest()
    {
        if (mSelectedQuest == null)
        {
            mQuestTitle.Hide();
            mQuestStatus.Hide();
            mQuestDescArea.Hide();
            mQuestTasksContainer.Hide();
            mQuitButton.Hide();
            ClearSelectedRewardItems();
            return;
        }

        mQuestDescLabel.ClearText();
        mQuestTasksList.RemoveAllRows();
        mQuitButton.IsDisabled = true;

        // ------ encabezado ------
        mQuestTitle.Text = mSelectedQuest.Name;

        // ------ estado & descripción ------
        if (Globals.Me.QuestProgress.TryGetValue(mSelectedQuest.Id, out var prog))
        {
            if (prog.TaskId != Guid.Empty)
            {
                mQuestStatus.SetText(Strings.QuestLog.InProgress);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.InProgress, Label.ControlState.Normal);

                if (!string.IsNullOrEmpty(mSelectedQuest.InProgressDescription))
                {
                    mQuestDescLabel.AddText(mSelectedQuest.InProgressDescription, mQuestDescTemplateLabel);
                    mQuestDescLabel.AddLineBreak();
                }

                // descripción de tarea actual
                var activeTask = mSelectedQuest.Tasks.FirstOrDefault(t => t.Id == prog.TaskId);
                if (activeTask != null)
                {
                    if (!string.IsNullOrEmpty(activeTask.Description))
                    {
                        mQuestDescLabel.AddText(activeTask.Description, mQuestDescTemplateLabel);
                        mQuestDescLabel.AddLineBreak();
                    }

                    string progressTxt = activeTask.Objective switch
                    {
                        QuestObjective.GatherItems => Strings.QuestLog.TaskItem.ToString(
                            prog.TaskProgress, activeTask.Quantity, ItemBase.GetName(activeTask.TargetId)),
                        QuestObjective.KillNpcs => Strings.QuestLog.TaskNpc.ToString(
                            prog.TaskProgress, activeTask.Quantity, NpcBase.GetName(activeTask.TargetId)),
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(progressTxt))
                        mQuestDescLabel.AddText(progressTxt, mQuestDescTemplateLabel);
                }

                mQuitButton.IsDisabled = !mSelectedQuest.Quitable;
            }
            else if (prog.Completed)
            {
                mQuestStatus.SetText(Strings.QuestLog.Completed);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.Completed, Label.ControlState.Normal);
                mQuestDescLabel.AddText(mSelectedQuest.EndDescription, mQuestDescTemplateLabel);
            }
            else
            {
                mQuestStatus.SetText(Strings.QuestLog.NotStarted);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
                mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);
            }
        }
        else
        {
            mQuestStatus.SetText(Strings.QuestLog.NotStarted);
            mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
            mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);
        }

        // ------ recompensas ------
        LoadRewardItems(mSelectedQuest.Id);
        foreach (var ri in RewardItems) ri.Update();
        foreach (var re in RewardExp) re.Update();
        mRewardContainer.IsHidden = RewardItems.Count == 0 && RewardExp.Count == 0;

        // ------ mostrar ------
        mQuestTitle.Show();
        mQuestStatus.Show();
        mQuestDescArea.Show();
        mQuestTasksContainer.Show();
        mQuitButton.Show();

        mQuestDescLabel.Width = mQuestDescArea.Width - mQuestDescArea.GetVerticalScrollBar().Width;
        mQuestDescLabel.SizeToChildren(false, true);
    }

    // ------------------------------------- TAREAS & CHECKBOXES ----------------------------------------------------
    private void UpdateQuestTasks()
    {
        mQuestTasksList.RemoveAllRows();
        ClearTaskPanels();   // ✅ usamos la nueva

        if (mSelectedQuest == null ||
            !Globals.Me.QuestProgress.TryGetValue(mSelectedQuest.Id, out var prog))
        {
            mQuestTasksContainer.IsHidden = true;
            return;
        }

        int index = 0;
        foreach (var task in mSelectedQuest.Tasks)
        {
            bool isComplete = IsTaskCompleted(mSelectedQuest.Id, task.Id);
            int taskProg = GetTaskProgress(mSelectedQuest.Id, task.Id);

            string txt = task.Objective switch
            {
                QuestObjective.GatherItems => $"{taskProg}/{task.Quantity} {ItemBase.GetName(task.TargetId)}",
                QuestObjective.KillNpcs => $"{taskProg}/{task.Quantity} {NpcBase.GetName(task.TargetId)}",
                _ => task.Description
            };

            var panel = new ImagePanel(mQuestTasksList, "TaskPanel");
            panel.SetSize(360, 25);
            panel.SetPosition(10, index * 30);

            var chk = new ImagePanel(panel, "TaskCheckImage");
            chk.SetSize(24, 25);
            chk.Texture = isComplete ? TexCheckFull : TexCheckEmpty;

            var lbl = new Label(panel, "TaskLabel");
            lbl.SetSize(330, 20);
            lbl.SetPosition(30, 3);
            lbl.SetText(txt);
            lbl.SetTextColor(Color.White,Label.ControlState.Normal);

            index++;
        }

        mQuestTasksContainer.IsHidden = index == 0;
    }

    private int GetTaskProgress(Guid questId, Guid taskId)
    {
        if (!Globals.Me.QuestProgress.TryGetValue(questId, out var prog))
            return 0;

        var quest = QuestBase.Get(questId);
        var task = quest?.FindTask(taskId);
        if (task == null) return 0;

        if (prog.TaskId == taskId) return prog.TaskProgress;

        // si el task quedó atrás, darlo por completo
        int idxTask = quest.Tasks.IndexOf(task);
        int idxCurrent = quest.Tasks.IndexOf(quest.FindTask(prog.TaskId));
        return idxTask < idxCurrent ? task.Quantity : 0;
    }

    private bool IsTaskCompleted(Guid questId, Guid taskId)
    {
        var quest = QuestBase.Get(questId);
        var task = quest?.FindTask(taskId);
        if (task == null) return false;

        return GetTaskProgress(questId, taskId) >= task.Quantity;
    }

    // ----------------------------------------- RECOMPENSAS --------------------------------------------------------
    private void LoadRewardItems(Guid questId)
    {
        RewardItems.Clear();
        RewardExp.Clear();
        mRewardItemsContainer.DeleteAll();
        mRewardExpContainer.DeleteAll();

        LoadRewardExperience(questId);
        LoadRewardItemsList(questId, mRewardItemsContainer);

        mRewardExpContainer.IsHidden = RewardExp.Count == 0;
        mRewardItemsContainer.IsHidden = RewardItems.Count == 0;
        mRewardContainer.IsHidden = mRewardExpContainer.IsHidden && mRewardItemsContainer.IsHidden;

        int yOffset = mRewardExpContainer.IsHidden ? 10 : 55;
        mRewardItemsContainer.SetPosition(10, yOffset);

        int totalHeight = 0;
        if (!mRewardExpContainer.IsHidden) totalHeight += 40 + 5;
        if (!mRewardItemsContainer.IsHidden) totalHeight += mRewardItemsContainer.Height;
        mRewardContainer.SetSize(mRewardContainer.Width, totalHeight);
    }

    private void LoadRewardExperience(Guid questId)
    {
        int expIndex = 0;
        int rewardWidth = 100;
        int spacing = 5;

        long baseExp = Globals.QuestExperience.TryGetValue(questId, out var exp) ? exp : 0;
        if (baseExp > 0)
        {
            var expPanel = new QuestRewardExp(this, baseExp);
            RewardExp.Add(expPanel);
            expPanel.Setup();
            expPanel.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
            expPanel.Container.Show();
            expPanel.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
            expIndex++;
        }

        if (Globals.QuestJobExperience.TryGetValue(questId, out var jobDict))
        {
            foreach (var kv in jobDict.Where(kv => kv.Value > 0))
            {
                var jobPanel = new QuestRewardExp(this, kv.Value, true, kv.Key);
                RewardExp.Add(jobPanel);
                jobPanel.Setup();
                jobPanel.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
                jobPanel.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
                jobPanel.Container.Show();
                expIndex++;
            }
        }

        int totalWidth = expIndex * (rewardWidth + spacing) - spacing;
        mRewardExpContainer.SetSize(Math.Max(totalWidth, 1), 40);
        mRewardExpContainer.IsHidden = RewardExp.Count == 0;
    }

    private int LoadRewardItemsList(Guid questId, Base container)
    {
        if (!Globals.QuestRewards.TryGetValue(questId, out var dict) || dict.Count == 0)
            return 0;

        int index = 0;
        int xPad = 10;
        int yPad = 10;
        int itemW = 40; // se ajusta luego con datos reales
        int itemH = 40;

        foreach (var (itemId, qty) in dict)
        {
            var rewardItem = new QuestRewardItem(this, itemId, qty);
            RewardItems.Add(rewardItem);

            rewardItem.Container = new ImagePanel(container, "RewardItem");
            rewardItem.Setup();
            rewardItem.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            itemW = rewardItem.Container.Width +
                    rewardItem.Container.Margin.Left + rewardItem.Container.Margin.Right;
            itemH = rewardItem.Container.Height +
                    rewardItem.Container.Margin.Top + rewardItem.Container.Margin.Bottom;

            var qtyLabel = new Label(rewardItem.Container, "RewardItemValue");
            qtyLabel.Text = Strings.FormatQuantityAbbreviated(qty);
            mRewardValues.Add(qtyLabel);

            int itemsPerRow = Math.Max(container.Width / itemW, 1);

            int posX = (index % itemsPerRow) * itemW + xPad;
            int posY = (index / itemsPerRow) * itemH + yPad;

            rewardItem.Container.SetPosition(posX, posY);
            index++;
        }

        int rows = (int)Math.Ceiling(RewardItems.Count / (double)Math.Max(container.Width / itemW, 1));
        container.SetSize(container.Width, rows * itemH + yPad);

        return index;
    }

    // -------------------------------- VISIBILIDAD GENERAL --------------------------------
    public void Show() => mQuestsWindow.IsHidden = false;
    public bool IsVisible() => !mQuestsWindow.IsHidden;
    public void Hide()
    {
        mQuestsWindow.IsHidden = true;
        mSelectedQuest = null;
    }
}
