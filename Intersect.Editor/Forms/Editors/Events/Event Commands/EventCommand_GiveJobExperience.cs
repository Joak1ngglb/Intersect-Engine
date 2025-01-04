using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Intersect.Editor.Localization;
using Intersect.GameObjects.Events.Commands;
using Intersect.Enums;
using Intersect.Config;

namespace Intersect.Editor.Forms.Editors.Events.Event_Commands
{
    public partial class EventCommandGiveJobExperience : UserControl
    {
        private readonly FrmEvent mEventEditor;
        private GiveJobExperienceCommand mMyCommand;

        private JobType selectedJob;
        private long selectedExperience;

        // Diccionario para manejar las experiencias de los trabajos
        private readonly Dictionary<JobType, long> JobExperience = new Dictionary<JobType, long>();

        // Diccionario para mapear los índices del ComboBox con los valores del JobType
        private readonly Dictionary<int, JobType> ComboBoxJobMapping = new Dictionary<int, JobType>();

        public EventCommandGiveJobExperience(GiveJobExperienceCommand refCommand, FrmEvent editor)
        {
            InitializeComponent();

            mMyCommand = refCommand;
            mEventEditor = editor;

            // Inicializar el combo box con los trabajos y mapear los índices
            InitializeComboBox();

            // Seleccionar valores iniciales
            if (selectedJob != JobType.None)
            {
                var selectedIndex = ComboBoxJobMapping.FirstOrDefault(x => x.Value == selectedJob).Key;
                cmbJob.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
            }

            nudExperience.Value = selectedExperience;

            // Cargar localización
            InitLocalization();

            // Inicializar experiencias
            InitializeJobExperiences();
        }

        private void InitLocalization()
        {
            grpGiveExperience.Text = Strings.EventGiveExperience.title;
            btnSave.Text = Strings.EventGiveExperience.okay;
            btnCancel.Text = Strings.EventGiveExperience.cancel;
        }

        private void InitializeComboBox()
        {
            cmbJob.Items.Clear();
            ComboBoxJobMapping.Clear();

            int comboIndex = 0;
            foreach (JobType job in Enum.GetValues(typeof(JobType)))
            {
                if (job != JobType.None && job != JobType.JobCount)
                {
                    ComboBoxJobMapping[comboIndex] = job; // Relaciona el índice con el JobType
                    cmbJob.Items.Add(General.Globals.GetJobName((int)job));
                    comboIndex++;
                }
            }
        }

        private void InitializeJobExperiences()
        {
            foreach (JobType job in Enum.GetValues(typeof(JobType)))
            {
                if (job != JobType.None && job != JobType.JobCount)
                {
                    JobExperience[job] = 0; // Inicializar todas las experiencias a 0
                }
            }
        }

        private void UpdateCommandPrinter()
        {
            string commandText = GetCommandText();
            // printerCommand.Text = commandText; // Actualizar el texto del comando en la interfaz
        }

        private string GetCommandText()
        {
            return $"Give {selectedExperience} EXP to {General.Globals.GetJobName((int)selectedJob)}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ResetExperience();
            if (selectedJob == JobType.None)
            {
                return;
            }

            JobExperience[selectedJob] = selectedExperience;

            // Asignar valores al comando
            mMyCommand.JobExp[selectedJob] = selectedExperience;

            // Actualizar la interfaz
            cmbJob.SelectedIndex = ComboBoxJobMapping.FirstOrDefault(x => x.Value == selectedJob).Key;
            nudExperience.Value = selectedExperience;
            UpdateCommandPrinter();

            mEventEditor.FinishCommandEdit();
        }

        private void ResetExperience()
        {
            foreach (var job in JobExperience.Keys.ToList())
            {
                JobExperience[job] = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mEventEditor.CancelCommandEdit();
        }

        private void nudExperience_ValueChanged(object sender, EventArgs e)
        {
            selectedExperience = (long)nudExperience.Value;
        }

        private void cmbJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxJobMapping.TryGetValue(cmbJob.SelectedIndex, out var job))
            {
                selectedJob = job;
            }
        }
    }
}
