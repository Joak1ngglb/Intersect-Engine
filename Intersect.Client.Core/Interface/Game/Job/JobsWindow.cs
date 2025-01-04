using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Crafting;
using Intersect.Client.Localization;
using Intersect.Config;
using Intersect.GameObjects.Crafting;
using Intersect.GameObjects;
using Newtonsoft.Json.Linq;
using Intersect.Client.Framework.Gwen;
using Intersect.Utilities;
using static Intersect.Client.Localization.Strings;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Networking;

namespace Intersect.Client.Interface.Game.Job
{
    public class JobsWindow
    {
        private WindowControl mJobsWindow;
        private ImagePanel InfoPanel;
        private ImagePanel JobsPanel;

        private Label JobNameLabel;
        public ImagePanel ExpBackground;
        public ImagePanel JobIcon;
        public ImagePanel ExpBar;
        private Label ExpLabel;
        private Label ExpTitle;
        private RichLabel JobDescriptionLabel;
        private Label JobLevelLabel;

        private ScrollControl mRecipePanel;
        private Label mJobtDescTemplateLabel;
        private List<RecipeItem> mItems = new List<RecipeItem>();
        private RecipeItem mCombinedItem;

        private Dictionary<JobType, JobUI> JobUIElements = new Dictionary<JobType, JobUI>();
        private JobType SelectedJob = JobType.None;
        private float CurExpWidth = 0;
        // Panels

        public int X;
        public int Y;
        public float CurExpSize = -1;
        private long mLastUpdateTime;
        public float xtnl;
        public float Jobexp;
        private List<Label> mValues = new List<Label>();
        private Label mNameLabel;
        private ScrollControl ingredientsPanel;
        private ImagePanel recipeContainer;

        public Label mXpLabel;

        public JobsWindow(Canvas gameCanvas)
        {
            mJobsWindow = new WindowControl(gameCanvas, Strings.Job.job, false, "JobsWindow");
            mJobsWindow.DisableResizing();
          
            InfoPanel = new ImagePanel(mJobsWindow, "InfoPanel");

          
            // Initialize the InfoPanel
            InitializeInfoPanel(SelectedJob);

            JobsPanel = new ImagePanel(mJobsWindow, "JobsPanel");
    
            JobsPanel.SetPosition(0, 0);
            mJobsWindow.SetSize(600, 400); // Tamaño general reducido

            JobsPanel.SetSize(200, 400); // Panel izquierdo más angosto
            JobsPanel.SetPosition(0, 0);

            InfoPanel.SetSize(400, 400); // Panel derecho para la información del trabajo
            InfoPanel.SetPosition(200, 0);

            var jobContainerHeight = 50; // Altura de cada botón
            var jobContainerWidth = 190; // Más compacto
            var jobIconSize = 40; // Íconos más pequeños

            InitializeJobsPanel();
            mJobsWindow.AddChild(JobsPanel);
            mJobsWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }

        private void InitializeJobsPanel()
        {
            var jobTypes = Enum.GetValues(typeof(JobType)).Cast<JobType>();

            int yOffset = 10; // Added spacing for better alignment

            foreach (var jobType in jobTypes)
            {
                if (jobType == JobType.None )
                {
                    continue;
                }
                if (jobType == JobType.JobCount)
                {
                    continue;
                }
                var container = new Button(JobsPanel, $"{jobType}Container");
                container.SetPosition(5, yOffset);
                container.SetSize(190, 50); // Botón más compacto
                container.Clicked += (sender, args) => JobButtonClicked(sender, args, jobType);

                var icon = new ImagePanel(container, $"{jobType}Icon");
                icon.SetPosition(5, 5);
                icon.SetSize(40, 40); // Adjusted size for better visibility

                var label = new Label(container, $"{jobType}NameLbl");
                label.SetPosition(50, 15);
                label.SetText(Strings.Job.GetJobName(jobType));                              
                            
                JobsPanel.AddChild(container);

                yOffset += 60; // Adjusted spacing between job buttons
            }
        }
        private void InitializeInfoPanel(JobType jobType)
        {
            // Icono del trabajo
            JobIcon = new ImagePanel(InfoPanel, "JobIcon");
            JobIcon.SetPosition(10, 10);
            JobIcon.SetSize(50, 50);

            // Nombre del trabajo
            JobNameLabel = new Label(InfoPanel, "JobNameLabel");
            JobNameLabel.SetPosition(70, 10);
            JobNameLabel.SetText(Strings.Job.GetJobName(jobType));

            // Nivel del trabajo
            JobLevelLabel = new Label(InfoPanel, "JobLevelLabel");
            JobLevelLabel.SetPosition(200, 10);
            JobLevelLabel.SetText(string.Format(Strings.Job.Level, Globals.Me.JobLevel)); // Ejemplo con nivel inicial
   
            // Título de experiencia
            ExpTitle = new Label(InfoPanel, "ExpTitle");
            ExpTitle.SetText(Strings.Job.Exp); // Referencia localizada
            ExpTitle.SetPosition(10, 200);
            ExpTitle.RenderColor = Color.FromArgb(255, 255, 255, 255);

            // Fondo de la barra de experiencia
            ExpBackground = new ImagePanel(InfoPanel, "ExpBackground");
         
            ExpBackground.RenderColor = Color.FromArgb(255, 100, 100, 100);

            // Barra de experiencia
            ExpBar = new ImagePanel(InfoPanel, "ExpBar");
           
            ExpBar.RenderColor = Color.FromArgb(255, 50, 150, 50);

            // Etiqueta de experiencia
            ExpLabel = new Label(InfoPanel, "ExpLabel");
    
            ExpLabel.SetText(string.Format(Strings.Job.ExpValue, Globals.Me.JobExp, Globals.Me.JobExpToNextLevel)); // Ejemplo inicial
            ExpLabel.RenderColor = Color.FromArgb(255, 255, 255, 255);
            mJobtDescTemplateLabel = new Label(InfoPanel, "JobDescriptionTemplate");
            // Descripción del trabajo
            JobDescriptionLabel = new RichLabel(InfoPanel, "Jobdesc");
         
            JobDescriptionLabel.ClearText();
            JobIcon.SetSize(40, 40); // Ícono del trabajo más pequeño
            JobIcon.SetPosition(10, 10);

            JobNameLabel.SetPosition(60, 10); // Nombre del trabajo
           

            JobLevelLabel.SetPosition(300, 10); // Nivel del trabajo alineado
          
            ExpBar.SetSize(380, 20); // Barra de experiencia más angosta
            ExpBar.SetPosition(10, 60);

            ExpLabel.SetPosition(10, 90); // Etiqueta de experiencia
  

            JobDescriptionLabel.SetPosition(10, 120); // Descripción del trabajo
            JobDescriptionLabel.SetSize(380, 100); // Tamaño compacto
     
          
            if (mJobtDescTemplateLabel != null)
            {
                JobDescriptionLabel.AddText(Strings.Job.GetJobDescription(JobType.None), mJobtDescTemplateLabel);
            }
            else
            {
                PacketSender.SendChatMsg("Error: No se pudo crear el Label para el template.", 5);
            }
        }

        private void JobButtonClicked(Base sender, ClickedEventArgs arguments, JobType jobType)
        {
            JobDescriptionLabel.ClearText();
            ExpBackground.IsHidden = false;
            UpdateJobInfo(jobType);
        }
        private void UpdateJobInfo(JobType jobType)
        {
            // Validar que Globals.Me no sea null
            if (Globals.Me == null)
            {
                PacketSender.SendChatMsg("Error: El jugador no está inicializado.", 5);
                return;
            }

            // Validar que el trabajo es válido
            if (jobType == JobType.None || jobType == JobType.JobCount)
            {
                PacketSender.SendChatMsg($"Advertencia: Trabajo '{jobType}' no es válido para actualizar.", 5);
                return;
            }

            // Validar que los diccionarios de trabajos estén inicializados
            if (Globals.Me.JobLevel == null || Globals.Me.JobExp == null || Globals.Me.JobExpToNextLevel == null)
            {
                PacketSender.SendChatMsg("Error: Los datos de trabajos no están inicializados.", 5);
                return;
            }

            // Obtener valores de los diccionarios con valores predeterminados
            var level = Globals.Me.JobLevel.ContainsKey(jobType) ? Globals.Me.JobLevel[jobType] : 1;
            var exp = Globals.Me.JobExp.ContainsKey(jobType) ? Globals.Me.JobExp[jobType] : 0;
            var expToNextLevel = Globals.Me.JobExpToNextLevel.ContainsKey(jobType) ? Globals.Me.JobExpToNextLevel[jobType] : 100;

            // Validar que los datos son razonables
            if (level <= 0 || exp < 0 || expToNextLevel <= 0)
            {
                PacketSender.SendChatMsg($"Error: Datos inválidos para el trabajo '{jobType}'. Nivel: {level}, Exp: {exp}, Exp para siguiente nivel: {expToNextLevel}.", 5);
                return;
            }


            // Validar que los componentes de la interfaz están inicializados
            if (JobNameLabel == null || JobLevelLabel == null || ExpLabel == null || ExpBar == null || ExpBackground == null || JobDescriptionLabel == null)
            {
                PacketSender.SendChatMsg("Error: Los componentes de la interfaz no están inicializados.", 5);
                return;
            }

            // Actualizar la interfaz con los datos disponibles
            JobNameLabel.SetText(Strings.Job.GetJobName(jobType));
            JobLevelLabel.SetText(string.Format(Strings.Job.Level, level));
            ExpLabel.SetText(string.Format(Strings.Job.ExpValue, exp, expToNextLevel));
            ExpBar.Width = (int)(ExpBackground.Width * (exp / (float)expToNextLevel));
            ExpBar.SetTextureRect(0, 0, ExpBar.Width, ExpBar.Height);

            JobDescriptionLabel.ClearText();
            JobDescriptionLabel.AddText(Strings.Job.GetJobDescription(jobType), mJobtDescTemplateLabel);

            // Mensaje de depuración
            PacketSender.SendChatMsg($"Trabajo {jobType}: Nivel {level}, Exp {exp}/{expToNextLevel}", 1);
        }

        public void Show()
        {
            InfoPanel?.Show();
            ExpBackground?.Show();
            ExpBar?.Show();
            ExpLabel?.Show();
            ExpTitle?.Show();
            JobLevelLabel?.Show();
            JobNameLabel?.Show();
            JobDescriptionLabel?.Show();
            UpdateJobInfo(JobType.None);
           
            mJobsWindow.IsHidden = false;
        }

        public void Hide()
        {
            SelectedJob = JobType.None;
            InfoPanel.Hide();
            ExpBackground.Hide();
            ExpBar.Hide();
            ExpLabel.Hide();
            ExpTitle.Hide();
            JobLevelLabel.Hide();
            JobNameLabel.Hide();
            JobDescriptionLabel.Hide();
            JobDescriptionLabel.ClearText();

            mJobsWindow.IsHidden = true;
        }

        public bool IsVisible()
        {
            return !mJobsWindow.IsHidden;
        }
        public void Update()
        {
            if (SelectedJob == JobType.None)
            {
                return; // No actualices si no hay un trabajo seleccionado
            }

            UpdateJobInfo(SelectedJob);
        }
    }

    public class JobUI
    {
        public Button Container { get; set; }
        public ImagePanel Icon { get; set; }
        public Label Label { get; set; }
    }
}
