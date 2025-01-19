using DarkUI.Forms;
using Intersect.Editor.Core;
using Intersect.Editor.General;
using Intersect.Editor.Networking;
using Intersect.Enums;
using Intersect.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmSkillJob : EditorForm
    {
        private List<JobBase.JobSkill> mSkills = new List<JobBase.JobSkill>();
        private JobBase.JobSkill mSelectedSkill;
        private string mCopiedItem;
        private List<JobBase.JobSkill> mChanged = new List<JobBase.JobSkill>();

        public FrmSkillJob()
        {
            InitializeComponent();
            Icon = Program.Icon;

            lstSkills.SelectedIndexChanged += LstSkills_SelectedIndexChanged;

            btnSave.Click += BtnSave_Click;
            cmbEffectType.DataSource = Enum.GetValues(typeof(JobSkillEffectType)).Cast<JobSkillEffectType>().ToList();
        }




        private void LstSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSkills.SelectedIndex >= 0)
            {
                mSelectedSkill = mSkills[lstSkills.SelectedIndex];
                UpdateEditor();
            }
        }

        private void UpdateEditor()
        {
            if (mSelectedSkill != null)
            {
                pnlEditor.Enabled = true;
                txtName.Text = mSelectedSkill.Name;
                txtDescription.Text = mSelectedSkill.Description;
                nudRequiredLevel.Value = mSelectedSkill.RequiredLevel;
                nudCost.Value = mSelectedSkill.Cost;
                cmbEffectType.SelectedItem = mSelectedSkill.EffectType;
                nudEffectValue.Value = (decimal)mSelectedSkill.EffectValue;
            }
            else
            {
                pnlEditor.Enabled = false;
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (var item in mChanged)
            {
                item.RestoreBackup();
                item.DeleteBackup();
            }

            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Send Changed items
            foreach (var item in mChanged)
            {
                PacketSender.SendSaveObject(item);
                item.DeleteBackup();
            }

            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }
    }
}
