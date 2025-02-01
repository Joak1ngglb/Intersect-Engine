using System.Xml.Linq;
using Intersect.Client.Core;
using Intersect.Client.Framework.Content;
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
 
    private Button mShowCompletedButton;
    // Lista de misiones
    private ScrollControl mQuestListContainer;
    private ScrollControl mQuestTasksContainer;  
    private Label mQuestTasksTitle;             
    private ListBox mQuestTasksList;             


    //Init
    // Init
    public QuestsWindow(Canvas gameCanvas)
    {
        mQuestsWindow = new WindowControl(gameCanvas, Strings.QuestLog.Title, false, "QuestsWindow");
        mQuestsWindow.DisableResizing();

        // Inicializar cada panel por separado
        InitializeQuestListPanel();
        InitializeQuestDetailPanel();
        InitializeRewardPanel();

        // Cargar configuración UI desde JSON
        mQuestsWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
    }

    private void InitializeQuestListPanel()
    {
        // Contenedor de la lista de misiones
        mQuestListContainer = new ScrollControl(mQuestsWindow, "QuestListContainer");
        mQuestListContainer.SetSize(250, 400);
        mQuestListContainer.SetPosition(10, 40);

        // Lista de misiones
        mQuestList = new ListBox(mQuestListContainer, "QuestList");
        mQuestList.EnableScroll(false, true);
        mQuestList.SetSize(250, 370);
        mQuestList.SetPosition(0, 30);

        // Botón para mostrar/ocultar misiones completadas
        mShowCompletedButton = new Button(mQuestListContainer, "ShowCompletedButton");
        mShowCompletedButton.SetText("Mostrar Terminadas");
        mShowCompletedButton.SetSize(250, 25);
        mShowCompletedButton.SetPosition(0, 410);
        mShowCompletedButton.Clicked += ToggleShowCompletedQuests;
    }
    private void InitializeQuestDetailPanel()
    {
        // Título de la misión
        mQuestTitle = new Label(mQuestsWindow, "QuestTitle");
        mQuestTitle.SetSize(330, 20);
        mQuestTitle.SetPosition(270, 40);
        mQuestTitle.SetText("");
        mQuestTitle.TextColor = Color.Yellow;

        // Estado de la misión
        mQuestStatus = new Label(mQuestsWindow, "QuestStatus");
        mQuestStatus.SetSize(330, 20);
        mQuestStatus.SetPosition(270, 65);
        mQuestStatus.SetText("");

        // Descripción de la misión
        mQuestDescArea = new ScrollControl(mQuestsWindow, "QuestDescription");
        mQuestDescArea.SetSize(400, 200);
        mQuestDescArea.SetPosition(270, 90);

        mQuestDescTemplateLabel = new Label(mQuestDescArea, "QuestDescriptionTemplate");
        mQuestDescLabel = new RichLabel(mQuestDescArea);

        // Inicializar panel de tareas (objetivos)
        InitializeQuestTasksPanel();

        // Botón de abandonar misión
        mQuitButton = new Button(mQuestsWindow, "AbandonQuestButton");
        mQuitButton.SetText(Strings.QuestLog.Abandon);
        mQuitButton.SetPosition(400, 470);
        mQuitButton.Clicked += _quitButton_Clicked;
    }

    private void InitializeQuestTasksPanel()
    {
        // Contenedor de los objetivos
        mQuestTasksContainer = new ScrollControl(mQuestsWindow, "QuestTasksContainer");
        mQuestTasksContainer.SetSize(400, 150);
        mQuestTasksContainer.SetPosition(270, 300); // Justo debajo de la descripción de la misión
        mQuestTasksContainer.EnableScroll(false, true);
        mQuestTasksContainer.IsHidden = true;

        // Título "Objetivos de la Misión"
        mQuestTasksTitle = new Label(mQuestTasksContainer, "QuestTasksTitle");
        mQuestTasksTitle.SetSize(380, 20);
        mQuestTasksTitle.SetPosition(10, 5);
        mQuestTasksTitle.SetText("Objetivos:");
        mQuestTasksTitle.TextColor = Color.White;

        // Lista de tareas (se llenará dinámicamente)
        mQuestTasksList = new ListBox(mQuestTasksContainer, "QuestTasksList");
        mQuestTasksList.SetSize(380, 120);
        mQuestTasksList.SetPosition(10, 30);
        mQuestTasksList.EnableScroll(false, true);
    }


    private void InitializeRewardPanel()
    {
        // Contenedor Principal de Recompensas
        mRewardContainer = new ScrollControl(mQuestsWindow, "RewardContainer");
        mRewardContainer.SetPosition(270, 350);
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
    }

    private void ToggleShowCompletedQuests(Base sender, ClickedEventArgs arguments)
    {
        UpdateQuestList();
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
            UpdateQuestTasks();
      
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
                            UpdateQuestTasks();
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
                    var sortedList = dict[category]
                        .OrderBy(q => q.Item2)
                        .ThenBy(q => q.Item1.OrderValue)
                        .ToList();

                    foreach (var qst in sortedList)
                    {
                        AddQuestToList(qst.Item1.Name, qst.Item3, qst.Item1.Id, false); 
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
        mQuestList.Show();
        mQuestList.UnselectAll();
        UpdateQuestList();
        LoadRewardItems(questNum);
    }

    private void UpdateSelectedQuest()
    {
        if (mSelectedQuest == null)
        {
            // Mostrar solo la lista de misiones si no hay una seleccionada
            mQuestList.Show();
            mQuestTitle.Hide();
            mQuestDescArea.Hide();
            mQuestStatus.Hide();
            mQuestTasksContainer.Hide();
            mQuitButton.Hide();
            return;
        }

        mQuestDescLabel.ClearText();
        mQuitButton.IsDisabled = true;

        // Configurar título y estado de la misión
        mQuestTitle.Text = mSelectedQuest.Name;
        if (Globals.Me.QuestProgress.ContainsKey(mSelectedQuest.Id))
        {
            var questProgress = Globals.Me.QuestProgress[mSelectedQuest.Id];

            if (questProgress.TaskId != Guid.Empty)
            {
                // Misión en progreso
                mQuestStatus.SetText(Strings.QuestLog.InProgress);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.InProgress, Label.ControlState.Normal);

                if (!string.IsNullOrEmpty(mSelectedQuest.InProgressDescription))
                {
                    mQuestDescLabel.AddText(mSelectedQuest.InProgressDescription, mQuestDescTemplateLabel);
                    mQuestDescLabel.AddLineBreak();
                }

                // Mostrar Objetivos de la misión
                mQuestDescLabel.AddText(Strings.QuestLog.CurrentTask, mQuestDescTemplateLabel);
                UpdateQuestTasks();

                // Descripción de la tarea activa
                var activeTask = mSelectedQuest.Tasks.FirstOrDefault(t => t.Id == questProgress.TaskId);
                if (activeTask != null)
                {
                    if (!string.IsNullOrEmpty(activeTask.Description))
                    {
                        mQuestDescLabel.AddText(activeTask.Description, mQuestDescTemplateLabel);
                        mQuestDescLabel.AddLineBreak();
                    }

                    // Mostrar progreso en el formato adecuado
                    string taskProgressText = activeTask.Objective switch
                    {
                        QuestObjective.GatherItems => Strings.QuestLog.TaskItem.ToString(
                            questProgress.TaskProgress, activeTask.Quantity, ItemBase.GetName(activeTask.TargetId)
                        ),
                        QuestObjective.KillNpcs => Strings.QuestLog.TaskNpc.ToString(
                            questProgress.TaskProgress, activeTask.Quantity, NpcBase.GetName(activeTask.TargetId)
                        ),
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(taskProgressText))
                    {
                        mQuestDescLabel.AddText(taskProgressText, mQuestDescTemplateLabel);
                    }
                }

                // Verificar si la misión puede ser abandonada
                mQuitButton.IsDisabled = !mSelectedQuest.Quitable;
            }
            else if (questProgress.Completed)
            {
                // Misión completada
                mQuestStatus.SetText(Strings.QuestLog.Completed);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.Completed, Label.ControlState.Normal);
                mQuestDescLabel.AddText(mSelectedQuest.EndDescription, mQuestDescTemplateLabel);
            }
            else
            {
                // Misión aún no iniciada
                mQuestStatus.SetText(Strings.QuestLog.NotStarted);
                mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
                mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);
            }
        }
        else
        {
            // Misión aún no iniciada
            mQuestStatus.SetText(Strings.QuestLog.NotStarted);
            mQuestStatus.SetTextColor(CustomColors.QuestWindow.NotStarted, Label.ControlState.Normal);
            mQuestDescLabel.AddText(mSelectedQuest.BeforeDescription, mQuestDescTemplateLabel);
        }

        // Cargar recompensas y actualizar UI
        LoadRewardItems(mSelectedQuest.Id);
        foreach (var rewardItem in RewardItems) rewardItem.Update();
        foreach (var rewardExp in RewardExp) rewardExp.Update();
        mRewardContainer.IsHidden = RewardExp.Count == 0 && RewardItems.Count == 0;

        // Mostrar todos los elementos relevantes
        mQuestList.Show();
        mQuestTitle.Show();
        mQuestDescArea.Show();
        mQuestStatus.Show();
        mQuestTasksContainer.Show();
        mQuitButton.Show();
        if (mSelectedQuest != null && Globals.Me.QuestProgress.TryGetValue(mSelectedQuest.Id, out var progress))
        {
            foreach (var task in mSelectedQuest.Tasks)
            {
                            int taskProgress = GetTaskProgress(mSelectedQuest.Id, task.Id);
                bool isComplete = taskProgress >= task.Quantity;

                            UpdateTaskCheck(mSelectedQuest.Id, task.Id);
            }
        }

        // Ajustar ancho del texto de la misión
        mQuestDescLabel.Width = mQuestDescArea.Width - mQuestDescArea.GetVerticalScrollBar().Width;
        mQuestDescLabel.SizeToChildren(false, true);
    }
    private void UpdateTaskCheck(Guid questId, Guid taskId)
    {
        foreach (var row in mQuestTasksList.Children)
        {
            if (row is ImagePanel taskPanel && taskPanel.Name == $"TaskPanel_{taskId}")
            {
                var checkImage = taskPanel.FindChildByName($"TaskCheckImage_{taskId}", true) as ImagePanel;

                if (checkImage != null)
                {
                    bool questCompleted = Globals.Me.QuestProgress.ContainsKey(questId) && Globals.Me.QuestProgress[questId].Completed;
                    bool isComplete = questCompleted || IsTaskCompleted(questId, taskId);

                    // 📌 Mantener el check activo si la misión está completa
                    string texture = isComplete ? "checkboxfull.png" : "checkboxempty.png";
                    checkImage.Texture = Globals.ContentManager.GetTexture(TextureType.Gui, texture);
                }
            }
        }
    }

    private void UpdateQuestTasks()
    {
        mQuestTasksList.RemoveAllRows();

        if (mSelectedQuest == null || !Globals.Me.QuestProgress.TryGetValue(mSelectedQuest.Id, out var progress))
        {
            mQuestTasksContainer.IsHidden = true;
            return;
        }

        int taskIndex = 0;

        foreach (var task in mSelectedQuest.Tasks)
        {
            bool isComplete = IsTaskCompleted(mSelectedQuest.Id, task.Id);
            int taskProgress = GetTaskProgress(mSelectedQuest.Id, task.Id);

            string taskText = task.Objective switch
            {
                QuestObjective.GatherItems => $"{taskProgress}/{task.Quantity} {ItemBase.GetName(task.TargetId)}",
                QuestObjective.KillNpcs => $"{taskProgress}/{task.Quantity} {NpcBase.GetName(task.TargetId)}",
                _ => task.Description
            };

            var taskPanel = new ImagePanel(mQuestTasksList, $"TaskPanel_{task.Id}");
            taskPanel.SetSize(360, 25);
            taskPanel.SetPosition(10, taskIndex * 30);

            string checkTexture = isComplete ? "checkboxfull.png" : "checkboxempty.png";
            var taskCheckImage = new ImagePanel(taskPanel, $"TaskCheckImage_{task.Id}");
            taskCheckImage.SetSize(24, 25);
            taskCheckImage.SetPosition(0, 0);
            taskCheckImage.Texture = Globals.ContentManager.GetTexture(TextureType.Gui, checkTexture);

            var taskLabel = new Label(taskPanel, $"TaskLabel_{task.Id}");
            taskLabel.SetSize(330, 20);
            taskLabel.SetPosition(30, 3);
            taskLabel.SetText(taskText);

            taskIndex++;
        }

        int totalHeight = taskIndex * 30;
        mQuestTasksContainer.SetSize(mQuestTasksContainer.Width, totalHeight);
        mQuestTasksContainer.IsHidden = taskIndex == 0;
    }


    private int GetTaskProgress(Guid questId, Guid taskId)
    {
        if (!Globals.Me.QuestProgress.TryGetValue(questId, out var progress))
        {
            return 0; // No hay progreso si la misión no está activa.
        }

        // 🔍 Buscar la tarea en la misión
        var quest = QuestBase.Get(questId);
        var task = quest?.FindTask(taskId);

        if (task == null)
        {
            return 0; // Si la tarea no existe, retornar 0.
        }

        // 📌 Si la tarea actual en `progress.TaskId` coincide, devolver su progreso.
        if (progress.TaskId == taskId)
        {
            return progress.TaskProgress;
        }

        // 📌 Si `progress.TaskId` cambió a una nueva tarea, significa que esta tarea ya se completó.
        if (quest.Tasks.IndexOf(task) < quest.Tasks.IndexOf(quest.FindTask(progress.TaskId)))
        {
            return task.Quantity; // Consideramos la tarea como completada.
        }

        return 0;
    }
    private bool IsTaskCompleted(Guid questId, Guid taskId)
    {
        var taskProgress = GetTaskProgress(questId, taskId);
        var quest = QuestBase.Get(questId);
        var task = quest?.FindTask(taskId);

        if (task == null)
        {
            return false;
        }

        return taskProgress >= task.Quantity;
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
        // 🛑 Eliminar todas las recompensas previas antes de actualizar
        RewardExp.Clear();
        mRewardExpContainer.DeleteAll();

        int expIndex = 0; // Controla la posición horizontal de las recompensas

        // 🔍 Obtener experiencia de recompensa de la misión
        long rewardExp = Globals.QuestExperience.ContainsKey(questId) ? Globals.QuestExperience[questId] : 0;
        var rewardJobExp = Globals.QuestJobExperience.ContainsKey(questId)
                            ? new Dictionary<JobType, long>(Globals.QuestJobExperience[questId])
                            : new Dictionary<JobType, long>();

        int rewardWidth = 100; // Tamaño fijo de cada contenedor de experiencia
        int spacing = 5; // Espaciado entre cada recompensa

        // 🌟 Mostrar experiencia general si la misión la otorga
        if (rewardExp > 0)
        {
            var expReward = new QuestRewardExp(this, rewardExp);
            RewardExp.Add(expReward);
            expReward.Setup();
            expReward.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

            // 🔄 Ajustar la posición en la UI
            expReward.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
            expIndex++;
        }

        // 🌟 Mostrar experiencia de trabajo si la misión la otorga
        foreach (var jobExp in rewardJobExp)
        {
            if (jobExp.Value > 0)
            {
                // 🔄 Verificar si ya existe una entrada para este JobType en `RewardExp`
                var existingExp = RewardExp.FirstOrDefault(reward => reward.mIsJobExp && reward.mJobType == jobExp.Key);

                if (existingExp != null)
                {
                    // Si ya existe, actualizar el valor
                    existingExp.Update(jobExp.Value);
                }
                else
                {
                    // Si no existe, crear una nueva entrada
                    var jobExpReward = new QuestRewardExp(this, jobExp.Value, true, jobExp.Key);
                    RewardExp.Add(jobExpReward);
                    jobExpReward.Setup();
                    jobExpReward.Container.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());

                    // 🔄 Ajustar la posición en la UI
                    jobExpReward.Container.SetPosition(expIndex * (rewardWidth + spacing), 0);
                    expIndex++;
                }
            }
        }

        // 📌 Ajustar tamaño y visibilidad del contenedor de experiencia
        int totalWidth = expIndex * (rewardWidth + spacing) - spacing;
        mRewardExpContainer.SetSize(RewardExp.Count > 0 ? totalWidth : 0, mRewardExpContainer.Height);
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
