using DarkUI.Controls;

namespace Intersect.Editor.Forms.Editors
{
    partial class FrmClass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var dataGridViewCellStyle7 = new DataGridViewCellStyle();
            var dataGridViewCellStyle8 = new DataGridViewCellStyle();
            var dataGridViewCellStyle9 = new DataGridViewCellStyle();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClass));
            grpClasses = new DarkGroupBox();
            btnClearSearch = new DarkButton();
            txtSearch = new DarkTextBox();
            lstGameObjects = new Controls.GameObjectList();
            grpBaseStats = new DarkGroupBox();
            nudBaseMana = new DarkNumericUpDown();
            nudBaseHP = new DarkNumericUpDown();
            nudPoints = new DarkNumericUpDown();
            nudSpd = new DarkNumericUpDown();
            nudMR = new DarkNumericUpDown();
            nudDef = new DarkNumericUpDown();
            nudMag = new DarkNumericUpDown();
            nudAttack = new DarkNumericUpDown();
            nudARP = new DarkNumericUpDown();
            nudVit = new DarkNumericUpDown();
            nudWis = new DarkNumericUpDown();
            lblPoints = new Label();
            lblMana = new Label();
            lblHP = new Label();
            lblSpd = new Label();
            lblMR = new Label();
            lblDef = new Label();
            lblMag = new Label();
            lblAttack = new Label();
            lblARP = new Label();
            lblVit = new Label();
            lblWis = new Label();
            grpGeneral = new DarkGroupBox();
            btnAddFolder = new DarkButton();
            lblFolder = new Label();
            cmbFolder = new DarkComboBox();
            chkLocked = new DarkCheckBox();
            lblName = new Label();
            txtName = new DarkTextBox();
            btnRemove = new DarkButton();
            btnAdd = new DarkButton();
            rbFemale = new DarkRadioButton();
            rbMale = new DarkRadioButton();
            lstSprites = new ListBox();
            cmbSprite = new DarkComboBox();
            lblSprite = new Label();
            picSprite = new PictureBox();
            grpSpells = new DarkGroupBox();
            nudLevel = new DarkNumericUpDown();
            cmbSpell = new DarkComboBox();
            lblLevel = new Label();
            lblSpellNum = new Label();
            btnRemoveSpell = new DarkButton();
            btnAddSpell = new DarkButton();
            lstSpells = new ListBox();
            grpSpawnPoint = new DarkGroupBox();
            nudY = new DarkNumericUpDown();
            nudX = new DarkNumericUpDown();
            btnVisualMapSelector = new DarkButton();
            cmbWarpMap = new DarkComboBox();
            cmbDirection = new DarkComboBox();
            lblDir = new Label();
            lblY = new Label();
            lblX = new Label();
            lblMap = new Label();
            pnlContainer = new Panel();
            grpSpawnItems = new DarkGroupBox();
            btnSpawnItemRemove = new DarkButton();
            btnSpawnItemAdd = new DarkButton();
            lstSpawnItems = new ListBox();
            nudSpawnItemAmount = new DarkNumericUpDown();
            cmbSpawnItem = new DarkComboBox();
            lblSpawnItemAmount = new Label();
            lblSpawnItem = new Label();
            grpCombat = new DarkGroupBox();
            cmbAttackSprite = new DarkComboBox();
            lblSpriteAttack = new Label();
            grpAttackSpeed = new DarkGroupBox();
            nudAttackSpeedValue = new DarkNumericUpDown();
            lblAttackSpeedValue = new Label();
            cmbAttackSpeedModifier = new DarkComboBox();
            lblAttackSpeedModifier = new Label();
            nudCritMultiplier = new DarkNumericUpDown();
            lblCritMultiplier = new Label();
            nudScaling = new DarkNumericUpDown();
            nudCritChance = new DarkNumericUpDown();
            nudDamage = new DarkNumericUpDown();
            cmbScalingStat = new DarkComboBox();
            lblScalingStat = new Label();
            lblScalingAmount = new Label();
            cmbDamageType = new DarkComboBox();
            lblDamageType = new Label();
            lblCritChance = new Label();
            cmbAttackAnimation = new DarkComboBox();
            lblAttackAnimation = new Label();
            lblDamage = new Label();
            grpRegen = new DarkGroupBox();
            nudMpRegen = new DarkNumericUpDown();
            nudHPRegen = new DarkNumericUpDown();
            lblHpRegen = new Label();
            lblManaRegen = new Label();
            lblRegenHint = new Label();
            grpSprite = new DarkGroupBox();
            grpSpriteOptions = new DarkGroupBox();
            lblFace = new Label();
            picFace = new PictureBox();
            cmbFace = new DarkComboBox();
            grpGender = new DarkGroupBox();
            grpLeveling = new DarkGroupBox();
            btnExpGrid = new DarkButton();
            nudBaseExp = new DarkNumericUpDown();
            nudExpIncrease = new DarkNumericUpDown();
            lblExpIncrease = new Label();
            lblBaseExp = new Label();
            grpLevelBoosts = new DarkGroupBox();
            nudHpIncrease = new DarkNumericUpDown();
            nudMpIncrease = new DarkNumericUpDown();
            nudPointsIncrease = new DarkNumericUpDown();
            nudMagicResistIncrease = new DarkNumericUpDown();
            nudSpeedIncrease = new DarkNumericUpDown();
            nudMagicIncrease = new DarkNumericUpDown();
            nudArmorIncrease = new DarkNumericUpDown();
            nudStrengthIncrease = new DarkNumericUpDown();
            nudArmorPenIncrease = new DarkNumericUpDown();
            nudVitalityIncrease = new DarkNumericUpDown();
            nudWisdomIncrease = new DarkNumericUpDown();
            rdoPercentageIncrease = new DarkRadioButton();
            rdoStaticIncrease = new DarkRadioButton();
            lblPointsIncrease = new Label();
            lblHpIncrease = new Label();
            lblMpIncrease = new Label();
            lblSpeedIncrease = new Label();
            lblStrengthIncrease = new Label();
            lblMagicResistIncrease = new Label();
            lblArmorIncrease = new Label();
            lblArmorPenIncrease = new Label();
            lblVitalityIncrease = new Label();
            lblWisdomIncrease = new Label();
            lblMagicIncrease = new Label();
            grpExpGrid = new DarkGroupBox();
            btnResetExpGrid = new DarkButton();
            btnCloseExpGrid = new DarkButton();
            expGrid = new DataGridView();
            btnCancel = new DarkButton();
            btnSave = new DarkButton();
            toolStrip = new DarkToolStrip();
            toolStripItemNew = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripItemDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnAlphabetical = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripItemCopy = new ToolStripButton();
            toolStripItemPaste = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripItemUndo = new ToolStripButton();
            mnuExpGrid = new ContextMenuStrip(components);
            btnExpPaste = new ToolStripMenuItem();
            grpClasses.SuspendLayout();
            grpBaseStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseMana).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBaseHP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDef).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMag).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAttack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudARP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWis).BeginInit();
            grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSprite).BeginInit();
            grpSpells.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
            grpSpawnPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
            pnlContainer.SuspendLayout();
            grpSpawnItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpawnItemAmount).BeginInit();
            grpCombat.SuspendLayout();
            grpAttackSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAttackSpeedValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCritMultiplier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudScaling).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCritChance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).BeginInit();
            grpRegen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMpRegen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHPRegen).BeginInit();
            grpSprite.SuspendLayout();
            grpSpriteOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFace).BeginInit();
            grpGender.SuspendLayout();
            grpLeveling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseExp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudExpIncrease).BeginInit();
            grpLevelBoosts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHpIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMpIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPointsIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMagicResistIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMagicIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudArmorIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStrengthIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudArmorPenIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVitalityIncrease).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWisdomIncrease).BeginInit();
            grpExpGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)expGrid).BeginInit();
            toolStrip.SuspendLayout();
            mnuExpGrid.SuspendLayout();
            SuspendLayout();
            // 
            // grpClasses
            // 
            grpClasses.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpClasses.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpClasses.Controls.Add(btnClearSearch);
            grpClasses.Controls.Add(txtSearch);
            grpClasses.Controls.Add(lstGameObjects);
            grpClasses.ForeColor = System.Drawing.Color.Gainsboro;
            grpClasses.Location = new System.Drawing.Point(4, 32);
            grpClasses.Margin = new Padding(2);
            grpClasses.Name = "grpClasses";
            grpClasses.Padding = new Padding(2);
            grpClasses.Size = new Size(233, 625);
            grpClasses.TabIndex = 15;
            grpClasses.TabStop = false;
            grpClasses.Text = "Classes";
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new System.Drawing.Point(204, 22);
            btnClearSearch.Margin = new Padding(4, 3, 4, 3);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Padding = new Padding(6, 6, 6, 6);
            btnClearSearch.Size = new Size(21, 23);
            btnClearSearch.TabIndex = 22;
            btnClearSearch.Text = "X";
            btnClearSearch.Click += btnClearSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtSearch.Location = new System.Drawing.Point(6, 22);
            txtSearch.Margin = new Padding(4, 3, 4, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(192, 23);
            txtSearch.TabIndex = 21;
            txtSearch.Text = "Search...";
            txtSearch.Click += txtSearch_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
            // 
            // lstGameObjects
            // 
            lstGameObjects.AllowDrop = true;
            lstGameObjects.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstGameObjects.BorderStyle = BorderStyle.None;
            lstGameObjects.ForeColor = System.Drawing.Color.Gainsboro;
            lstGameObjects.HideSelection = false;
            lstGameObjects.ImageIndex = 0;
            lstGameObjects.LineColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lstGameObjects.Location = new System.Drawing.Point(6, 52);
            lstGameObjects.Margin = new Padding(4, 3, 4, 3);
            lstGameObjects.Name = "lstGameObjects";
            lstGameObjects.SelectedImageIndex = 0;
            lstGameObjects.Size = new Size(222, 568);
            lstGameObjects.TabIndex = 20;
            // 
            // grpBaseStats
            // 
            grpBaseStats.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpBaseStats.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpBaseStats.Controls.Add(nudBaseMana);
            grpBaseStats.Controls.Add(nudBaseHP);
            grpBaseStats.Controls.Add(nudPoints);
            grpBaseStats.Controls.Add(nudSpd);
            grpBaseStats.Controls.Add(nudMR);
            grpBaseStats.Controls.Add(nudDef);
            grpBaseStats.Controls.Add(nudMag);
            grpBaseStats.Controls.Add(nudAttack);
            grpBaseStats.Controls.Add(nudARP);
            grpBaseStats.Controls.Add(nudVit);
            grpBaseStats.Controls.Add(nudWis);
            grpBaseStats.Controls.Add(lblPoints);
            grpBaseStats.Controls.Add(lblMana);
            grpBaseStats.Controls.Add(lblHP);
            grpBaseStats.Controls.Add(lblSpd);
            grpBaseStats.Controls.Add(lblMR);
            grpBaseStats.Controls.Add(lblDef);
            grpBaseStats.Controls.Add(lblMag);
            grpBaseStats.Controls.Add(lblAttack);
            grpBaseStats.Controls.Add(lblARP);
            grpBaseStats.Controls.Add(lblVit);
            grpBaseStats.Controls.Add(lblWis);
            grpBaseStats.ForeColor = System.Drawing.Color.Gainsboro;
            grpBaseStats.Location = new System.Drawing.Point(9, 493);
            grpBaseStats.Margin = new Padding(2);
            grpBaseStats.Name = "grpBaseStats";
            grpBaseStats.Padding = new Padding(2);
            grpBaseStats.Size = new Size(532, 220);
            grpBaseStats.TabIndex = 17;
            grpBaseStats.TabStop = false;
            grpBaseStats.Text = "Base Stats:";
            // 
            // nudBaseMana
            // 
            nudBaseMana.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudBaseMana.ForeColor = System.Drawing.Color.Gainsboro;
            nudBaseMana.Location = new System.Drawing.Point(369, 21);
            nudBaseMana.Margin = new Padding(4, 3, 4, 3);
            nudBaseMana.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudBaseMana.Name = "nudBaseMana";
            nudBaseMana.Size = new Size(117, 23);
            nudBaseMana.TabIndex = 35;
            nudBaseMana.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudBaseMana.ValueChanged += nudBaseMana_ValueChanged;
            // 
            // nudBaseHP
            // 
            nudBaseHP.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudBaseHP.ForeColor = System.Drawing.Color.Gainsboro;
            nudBaseHP.Location = new System.Drawing.Point(122, 21);
            nudBaseHP.Margin = new Padding(4, 3, 4, 3);
            nudBaseHP.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudBaseHP.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudBaseHP.Name = "nudBaseHP";
            nudBaseHP.Size = new Size(117, 23);
            nudBaseHP.TabIndex = 34;
            nudBaseHP.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudBaseHP.ValueChanged += nudBaseHP_ValueChanged;
            // 
            // nudPoints
            // 
            nudPoints.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudPoints.ForeColor = System.Drawing.Color.Gainsboro;
            nudPoints.Location = new System.Drawing.Point(369, 117);
            nudPoints.Margin = new Padding(4, 3, 4, 3);
            nudPoints.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudPoints.Name = "nudPoints";
            nudPoints.Size = new Size(117, 23);
            nudPoints.TabIndex = 33;
            nudPoints.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudPoints.ValueChanged += nudPoints_ValueChanged;
            // 
            // nudSpd
            // 
            nudSpd.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSpd.ForeColor = System.Drawing.Color.Gainsboro;
            nudSpd.Location = new System.Drawing.Point(122, 117);
            nudSpd.Margin = new Padding(4, 3, 4, 3);
            nudSpd.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSpd.Name = "nudSpd";
            nudSpd.Size = new Size(117, 23);
            nudSpd.TabIndex = 32;
            nudSpd.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudSpd.ValueChanged += nudSpd_ValueChanged;
            // 
            // nudMR
            // 
            nudMR.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMR.ForeColor = System.Drawing.Color.Gainsboro;
            nudMR.Location = new System.Drawing.Point(369, 85);
            nudMR.Margin = new Padding(4, 3, 4, 3);
            nudMR.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMR.Name = "nudMR";
            nudMR.Size = new Size(117, 23);
            nudMR.TabIndex = 31;
            nudMR.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMR.ValueChanged += nudMR_ValueChanged;
            // 
            // nudDef
            // 
            nudDef.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudDef.ForeColor = System.Drawing.Color.Gainsboro;
            nudDef.Location = new System.Drawing.Point(369, 54);
            nudDef.Margin = new Padding(4, 3, 4, 3);
            nudDef.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudDef.Name = "nudDef";
            nudDef.Size = new Size(117, 23);
            nudDef.TabIndex = 30;
            nudDef.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudDef.ValueChanged += nudDef_ValueChanged;
            // 
            // nudMag
            // 
            nudMag.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMag.ForeColor = System.Drawing.Color.Gainsboro;
            nudMag.Location = new System.Drawing.Point(122, 85);
            nudMag.Margin = new Padding(4, 3, 4, 3);
            nudMag.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMag.Name = "nudMag";
            nudMag.Size = new Size(117, 23);
            nudMag.TabIndex = 29;
            nudMag.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMag.ValueChanged += nudMag_ValueChanged;
            // 
            // nudAttack
            // 
            nudAttack.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudAttack.ForeColor = System.Drawing.Color.Gainsboro;
            nudAttack.Location = new System.Drawing.Point(122, 54);
            nudAttack.Margin = new Padding(4, 3, 4, 3);
            nudAttack.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudAttack.Name = "nudAttack";
            nudAttack.Size = new Size(117, 23);
            nudAttack.TabIndex = 28;
            nudAttack.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudAttack.ValueChanged += nudStr_ValueChanged;
            // 
            // nudARP
            // 
            nudARP.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudARP.ForeColor = System.Drawing.Color.Gainsboro;
            nudARP.Location = new System.Drawing.Point(122, 152);
            nudARP.Margin = new Padding(4, 3, 4, 3);
            nudARP.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudARP.Name = "nudARP";
            nudARP.Size = new Size(117, 23);
            nudARP.TabIndex = 69;
            nudARP.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudARP.ValueChanged += nudARP_ValueChanged;
            // 
            // nudVit
            // 
            nudVit.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudVit.ForeColor = System.Drawing.Color.Gainsboro;
            nudVit.Location = new System.Drawing.Point(369, 154);
            nudVit.Margin = new Padding(4, 3, 4, 3);
            nudVit.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudVit.Name = "nudVit";
            nudVit.Size = new Size(117, 23);
            nudVit.TabIndex = 70;
            nudVit.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudVit.ValueChanged += nudVit_ValueChanged;
            // 
            // nudWis
            // 
            nudWis.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudWis.ForeColor = System.Drawing.Color.Gainsboro;
            nudWis.Location = new System.Drawing.Point(369, 192);
            nudWis.Margin = new Padding(4, 3, 4, 3);
            nudWis.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudWis.Name = "nudWis";
            nudWis.Size = new Size(117, 23);
            nudWis.TabIndex = 71;
            nudWis.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudWis.ValueChanged += nudWis_ValueChanged;
            // 
            // lblPoints
            // 
            lblPoints.AutoSize = true;
            lblPoints.Location = new System.Drawing.Point(272, 119);
            lblPoints.Margin = new Padding(2, 0, 2, 0);
            lblPoints.Name = "lblPoints";
            lblPoints.Size = new Size(43, 15);
            lblPoints.TabIndex = 18;
            lblPoints.Text = "Points:";
            // 
            // lblMana
            // 
            lblMana.AutoSize = true;
            lblMana.Location = new System.Drawing.Point(272, 23);
            lblMana.Margin = new Padding(2, 0, 2, 0);
            lblMana.Name = "lblMana";
            lblMana.Size = new Size(40, 15);
            lblMana.TabIndex = 15;
            lblMana.Text = "Mana:";
            // 
            // lblHP
            // 
            lblHP.AutoSize = true;
            lblHP.Location = new System.Drawing.Point(20, 23);
            lblHP.Margin = new Padding(2, 0, 2, 0);
            lblHP.Name = "lblHP";
            lblHP.Size = new Size(26, 15);
            lblHP.TabIndex = 14;
            lblHP.Text = "HP:";
            // 
            // lblSpd
            // 
            lblSpd.AutoSize = true;
            lblSpd.Location = new System.Drawing.Point(20, 119);
            lblSpd.Margin = new Padding(2, 0, 2, 0);
            lblSpd.Name = "lblSpd";
            lblSpd.Size = new Size(75, 15);
            lblSpd.TabIndex = 9;
            lblSpd.Text = "Move Speed:";
            // 
            // lblMR
            // 
            lblMR.AutoSize = true;
            lblMR.Location = new System.Drawing.Point(272, 88);
            lblMR.Margin = new Padding(2, 0, 2, 0);
            lblMR.Name = "lblMR";
            lblMR.Size = new Size(76, 15);
            lblMR.TabIndex = 8;
            lblMR.Text = "Magic Resist:";
            // 
            // lblDef
            // 
            lblDef.AutoSize = true;
            lblDef.Location = new System.Drawing.Point(272, 57);
            lblDef.Margin = new Padding(2, 0, 2, 0);
            lblDef.Name = "lblDef";
            lblDef.Size = new Size(44, 15);
            lblDef.TabIndex = 7;
            lblDef.Text = "Armor:";
            // 
            // lblMag
            // 
            lblMag.AutoSize = true;
            lblMag.Location = new System.Drawing.Point(20, 88);
            lblMag.Margin = new Padding(2, 0, 2, 0);
            lblMag.Name = "lblMag";
            lblMag.Size = new Size(43, 15);
            lblMag.TabIndex = 6;
            lblMag.Text = "Magic:";
            // 
            // lblAttack
            // 
            lblAttack.AutoSize = true;
            lblAttack.Location = new System.Drawing.Point(20, 54);
            lblAttack.Margin = new Padding(2, 0, 2, 0);
            lblAttack.Name = "lblAttack";
            lblAttack.Size = new Size(44, 15);
            lblAttack.TabIndex = 5;
            lblAttack.Text = "Attack:";
            // 
            // lblARP
            // 
            lblARP.AutoSize = true;
            lblARP.Location = new System.Drawing.Point(7, 156);
            lblARP.Margin = new Padding(2, 0, 2, 0);
            lblARP.Name = "lblARP";
            lblARP.Size = new Size(108, 15);
            lblARP.TabIndex = 72;
            lblARP.Text = "Armor Penetration:";
            // 
            // lblVit
            // 
            lblVit.AutoSize = true;
            lblVit.Location = new System.Drawing.Point(272, 156);
            lblVit.Margin = new Padding(2, 0, 2, 0);
            lblVit.Name = "lblVit";
            lblVit.Size = new Size(46, 15);
            lblVit.TabIndex = 73;
            lblVit.Text = "Vitality:";
            // 
            // lblWis
            // 
            lblWis.AutoSize = true;
            lblWis.Location = new System.Drawing.Point(272, 194);
            lblWis.Margin = new Padding(2, 0, 2, 0);
            lblWis.Name = "lblWis";
            lblWis.Size = new Size(54, 15);
            lblWis.TabIndex = 74;
            lblWis.Text = "Wisdom:";
            // 
            // grpGeneral
            // 
            grpGeneral.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpGeneral.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpGeneral.Controls.Add(btnAddFolder);
            grpGeneral.Controls.Add(lblFolder);
            grpGeneral.Controls.Add(cmbFolder);
            grpGeneral.Controls.Add(chkLocked);
            grpGeneral.Controls.Add(lblName);
            grpGeneral.Controls.Add(txtName);
            grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            grpGeneral.Location = new System.Drawing.Point(9, 6);
            grpGeneral.Margin = new Padding(2);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Padding = new Padding(2);
            grpGeneral.Size = new Size(240, 188);
            grpGeneral.TabIndex = 19;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "General";
            // 
            // btnAddFolder
            // 
            btnAddFolder.Location = new System.Drawing.Point(206, 57);
            btnAddFolder.Margin = new Padding(4, 3, 4, 3);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Padding = new Padding(6, 6, 6, 6);
            btnAddFolder.Size = new Size(21, 24);
            btnAddFolder.TabIndex = 20;
            btnAddFolder.Text = "+";
            btnAddFolder.Click += btnAddFolder_Click;
            // 
            // lblFolder
            // 
            lblFolder.AutoSize = true;
            lblFolder.Location = new System.Drawing.Point(5, 61);
            lblFolder.Margin = new Padding(4, 0, 4, 0);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new Size(43, 15);
            lblFolder.TabIndex = 19;
            lblFolder.Text = "Folder:";
            // 
            // cmbFolder
            // 
            cmbFolder.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbFolder.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbFolder.BorderStyle = ButtonBorderStyle.Solid;
            cmbFolder.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbFolder.DrawDropdownHoverOutline = false;
            cmbFolder.DrawFocusRectangle = false;
            cmbFolder.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFolder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFolder.FlatStyle = FlatStyle.Flat;
            cmbFolder.ForeColor = System.Drawing.Color.Gainsboro;
            cmbFolder.FormattingEnabled = true;
            cmbFolder.Location = new System.Drawing.Point(58, 58);
            cmbFolder.Margin = new Padding(4, 3, 4, 3);
            cmbFolder.Name = "cmbFolder";
            cmbFolder.Size = new Size(140, 24);
            cmbFolder.TabIndex = 18;
            cmbFolder.Text = null;
            cmbFolder.TextPadding = new Padding(2);
            cmbFolder.SelectedIndexChanged += cmbFolder_SelectedIndexChanged;
            // 
            // chkLocked
            // 
            chkLocked.AutoSize = true;
            chkLocked.Location = new System.Drawing.Point(9, 89);
            chkLocked.Margin = new Padding(4, 3, 4, 3);
            chkLocked.Name = "chkLocked";
            chkLocked.Size = new Size(94, 19);
            chkLocked.TabIndex = 14;
            chkLocked.Text = "Class Locked";
            chkLocked.CheckedChanged += chkLocked_CheckedChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(6, 24);
            lblName.Margin = new Padding(2, 0, 2, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 13;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtName.Location = new System.Drawing.Point(59, 22);
            txtName.Margin = new Padding(2);
            txtName.Name = "txtName";
            txtName.Size = new Size(168, 23);
            txtName.TabIndex = 12;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(12, 53);
            btnRemove.Margin = new Padding(2);
            btnRemove.Name = "btnRemove";
            btnRemove.Padding = new Padding(6, 6, 6, 6);
            btnRemove.Size = new Size(140, 28);
            btnRemove.TabIndex = 21;
            btnRemove.Text = "Remove Sprite";
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(12, 21);
            btnAdd.Margin = new Padding(2);
            btnAdd.Name = "btnAdd";
            btnAdd.Padding = new Padding(6, 6, 6, 6);
            btnAdd.Size = new Size(140, 28);
            btnAdd.TabIndex = 20;
            btnAdd.Text = "Add Sprite";
            btnAdd.Click += btnAdd_Click;
            // 
            // rbFemale
            // 
            rbFemale.AutoSize = true;
            rbFemale.Location = new System.Drawing.Point(79, 20);
            rbFemale.Margin = new Padding(2);
            rbFemale.Name = "rbFemale";
            rbFemale.Size = new Size(63, 19);
            rbFemale.TabIndex = 19;
            rbFemale.Text = "Female";
            rbFemale.Click += rbFemale_Click;
            // 
            // rbMale
            // 
            rbMale.AutoSize = true;
            rbMale.Checked = true;
            rbMale.Location = new System.Drawing.Point(13, 21);
            rbMale.Margin = new Padding(2);
            rbMale.Name = "rbMale";
            rbMale.Size = new Size(51, 19);
            rbMale.TabIndex = 18;
            rbMale.TabStop = true;
            rbMale.Text = "Male";
            rbMale.Click += rbMale_Click;
            // 
            // lstSprites
            // 
            lstSprites.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstSprites.BorderStyle = BorderStyle.FixedSingle;
            lstSprites.ForeColor = System.Drawing.Color.Gainsboro;
            lstSprites.FormattingEnabled = true;
            lstSprites.ItemHeight = 15;
            lstSprites.Location = new System.Drawing.Point(7, 24);
            lstSprites.Margin = new Padding(4, 3, 4, 3);
            lstSprites.Name = "lstSprites";
            lstSprites.Size = new Size(117, 152);
            lstSprites.TabIndex = 17;
            lstSprites.Click += lstSprites_Click;
            // 
            // cmbSprite
            // 
            cmbSprite.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbSprite.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbSprite.BorderStyle = ButtonBorderStyle.Solid;
            cmbSprite.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbSprite.DrawDropdownHoverOutline = false;
            cmbSprite.DrawFocusRectangle = false;
            cmbSprite.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSprite.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSprite.FlatStyle = FlatStyle.Flat;
            cmbSprite.ForeColor = System.Drawing.Color.Gainsboro;
            cmbSprite.FormattingEnabled = true;
            cmbSprite.Items.AddRange(new object[] { "None" });
            cmbSprite.Location = new System.Drawing.Point(304, 38);
            cmbSprite.Margin = new Padding(2);
            cmbSprite.Name = "cmbSprite";
            cmbSprite.Size = new Size(111, 24);
            cmbSprite.TabIndex = 16;
            cmbSprite.Text = "None";
            cmbSprite.TextPadding = new Padding(2);
            cmbSprite.SelectedIndexChanged += cmbSprite_SelectedIndexChanged;
            // 
            // lblSprite
            // 
            lblSprite.AutoSize = true;
            lblSprite.Location = new System.Drawing.Point(301, 17);
            lblSprite.Margin = new Padding(2, 0, 2, 0);
            lblSprite.Name = "lblSprite";
            lblSprite.Size = new Size(40, 15);
            lblSprite.TabIndex = 15;
            lblSprite.Text = "Sprite:";
            // 
            // picSprite
            // 
            picSprite.BackColor = System.Drawing.Color.Black;
            picSprite.Location = new System.Drawing.Point(304, 72);
            picSprite.Margin = new Padding(2);
            picSprite.Name = "picSprite";
            picSprite.Size = new Size(112, 111);
            picSprite.TabIndex = 14;
            picSprite.TabStop = false;
            // 
            // grpSpells
            // 
            grpSpells.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSpells.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSpells.Controls.Add(nudLevel);
            grpSpells.Controls.Add(cmbSpell);
            grpSpells.Controls.Add(lblLevel);
            grpSpells.Controls.Add(lblSpellNum);
            grpSpells.Controls.Add(btnRemoveSpell);
            grpSpells.Controls.Add(btnAddSpell);
            grpSpells.Controls.Add(lstSpells);
            grpSpells.ForeColor = System.Drawing.Color.Gainsboro;
            grpSpells.Location = new System.Drawing.Point(553, 390);
            grpSpells.Margin = new Padding(4, 3, 4, 3);
            grpSpells.Name = "grpSpells";
            grpSpells.Padding = new Padding(2);
            grpSpells.Size = new Size(268, 173);
            grpSpells.TabIndex = 21;
            grpSpells.TabStop = false;
            grpSpells.Text = "Spells";
            // 
            // nudLevel
            // 
            nudLevel.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudLevel.ForeColor = System.Drawing.Color.Gainsboro;
            nudLevel.Location = new System.Drawing.Point(176, 67);
            nudLevel.Margin = new Padding(4, 3, 4, 3);
            nudLevel.Name = "nudLevel";
            nudLevel.Size = new Size(82, 23);
            nudLevel.TabIndex = 27;
            nudLevel.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudLevel.ValueChanged += nudLevel_ValueChanged;
            // 
            // cmbSpell
            // 
            cmbSpell.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbSpell.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbSpell.BorderStyle = ButtonBorderStyle.Solid;
            cmbSpell.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbSpell.DrawDropdownHoverOutline = false;
            cmbSpell.DrawFocusRectangle = false;
            cmbSpell.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSpell.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpell.FlatStyle = FlatStyle.Flat;
            cmbSpell.ForeColor = System.Drawing.Color.Gainsboro;
            cmbSpell.FormattingEnabled = true;
            cmbSpell.Location = new System.Drawing.Point(132, 32);
            cmbSpell.Margin = new Padding(4, 3, 4, 3);
            cmbSpell.Name = "cmbSpell";
            cmbSpell.Size = new Size(125, 24);
            cmbSpell.TabIndex = 26;
            cmbSpell.Text = null;
            cmbSpell.TextPadding = new Padding(2);
            cmbSpell.SelectedIndexChanged += cmbSpell_SelectedIndexChanged;
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.Location = new System.Drawing.Point(128, 67);
            lblLevel.Margin = new Padding(2, 0, 2, 0);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(37, 15);
            lblLevel.TabIndex = 25;
            lblLevel.Text = "Level:";
            // 
            // lblSpellNum
            // 
            lblSpellNum.AutoSize = true;
            lblSpellNum.Location = new System.Drawing.Point(128, 14);
            lblSpellNum.Margin = new Padding(2, 0, 2, 0);
            lblSpellNum.Name = "lblSpellNum";
            lblSpellNum.Size = new Size(35, 15);
            lblSpellNum.TabIndex = 23;
            lblSpellNum.Text = "Spell:";
            // 
            // btnRemoveSpell
            // 
            btnRemoveSpell.Location = new System.Drawing.Point(128, 132);
            btnRemoveSpell.Margin = new Padding(2);
            btnRemoveSpell.Name = "btnRemoveSpell";
            btnRemoveSpell.Padding = new Padding(6, 6, 6, 6);
            btnRemoveSpell.Size = new Size(130, 24);
            btnRemoveSpell.TabIndex = 21;
            btnRemoveSpell.Text = "Remove Spell";
            btnRemoveSpell.Click += btnRemoveSpell_Click;
            // 
            // btnAddSpell
            // 
            btnAddSpell.Location = new System.Drawing.Point(128, 103);
            btnAddSpell.Margin = new Padding(2);
            btnAddSpell.Name = "btnAddSpell";
            btnAddSpell.Padding = new Padding(6, 6, 6, 6);
            btnAddSpell.Size = new Size(130, 24);
            btnAddSpell.TabIndex = 20;
            btnAddSpell.Text = "Add Spell";
            btnAddSpell.Click += btnAddSpell_Click;
            // 
            // lstSpells
            // 
            lstSpells.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstSpells.BorderStyle = BorderStyle.FixedSingle;
            lstSpells.ForeColor = System.Drawing.Color.Gainsboro;
            lstSpells.FormattingEnabled = true;
            lstSpells.ItemHeight = 15;
            lstSpells.Location = new System.Drawing.Point(8, 20);
            lstSpells.Margin = new Padding(2);
            lstSpells.Name = "lstSpells";
            lstSpells.Size = new Size(108, 137);
            lstSpells.TabIndex = 17;
            lstSpells.SelectedIndexChanged += lstSpells_SelectedIndexChanged;
            // 
            // grpSpawnPoint
            // 
            grpSpawnPoint.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSpawnPoint.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSpawnPoint.Controls.Add(nudY);
            grpSpawnPoint.Controls.Add(nudX);
            grpSpawnPoint.Controls.Add(btnVisualMapSelector);
            grpSpawnPoint.Controls.Add(cmbWarpMap);
            grpSpawnPoint.Controls.Add(cmbDirection);
            grpSpawnPoint.Controls.Add(lblDir);
            grpSpawnPoint.Controls.Add(lblY);
            grpSpawnPoint.Controls.Add(lblX);
            grpSpawnPoint.Controls.Add(lblMap);
            grpSpawnPoint.ForeColor = System.Drawing.Color.Gainsboro;
            grpSpawnPoint.Location = new System.Drawing.Point(553, 202);
            grpSpawnPoint.Margin = new Padding(4, 3, 4, 3);
            grpSpawnPoint.Name = "grpSpawnPoint";
            grpSpawnPoint.Padding = new Padding(4, 3, 4, 3);
            grpSpawnPoint.Size = new Size(270, 185);
            grpSpawnPoint.TabIndex = 27;
            grpSpawnPoint.TabStop = false;
            grpSpawnPoint.Text = "Spawn Point";
            // 
            // nudY
            // 
            nudY.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudY.ForeColor = System.Drawing.Color.Gainsboro;
            nudY.Location = new System.Drawing.Point(174, 61);
            nudY.Margin = new Padding(4, 3, 4, 3);
            nudY.Name = "nudY";
            nudY.Size = new Size(75, 23);
            nudY.TabIndex = 26;
            nudY.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudY.ValueChanged += nudY_ValueChanged;
            // 
            // nudX
            // 
            nudX.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudX.ForeColor = System.Drawing.Color.Gainsboro;
            nudX.Location = new System.Drawing.Point(51, 61);
            nudX.Margin = new Padding(4, 3, 4, 3);
            nudX.Name = "nudX";
            nudX.Size = new Size(75, 23);
            nudX.TabIndex = 25;
            nudX.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudX.ValueChanged += nudX_ValueChanged;
            // 
            // btnVisualMapSelector
            // 
            btnVisualMapSelector.Location = new System.Drawing.Point(19, 144);
            btnVisualMapSelector.Margin = new Padding(2);
            btnVisualMapSelector.Name = "btnVisualMapSelector";
            btnVisualMapSelector.Padding = new Padding(6, 6, 6, 6);
            btnVisualMapSelector.Size = new Size(241, 28);
            btnVisualMapSelector.TabIndex = 24;
            btnVisualMapSelector.Text = "Open Visual Interface";
            btnVisualMapSelector.Click += btnVisualMapSelector_Click;
            // 
            // cmbWarpMap
            // 
            cmbWarpMap.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbWarpMap.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbWarpMap.BorderStyle = ButtonBorderStyle.Solid;
            cmbWarpMap.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbWarpMap.DrawDropdownHoverOutline = false;
            cmbWarpMap.DrawFocusRectangle = false;
            cmbWarpMap.DrawMode = DrawMode.OwnerDrawFixed;
            cmbWarpMap.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarpMap.FlatStyle = FlatStyle.Flat;
            cmbWarpMap.ForeColor = System.Drawing.Color.Gainsboro;
            cmbWarpMap.FormattingEnabled = true;
            cmbWarpMap.Location = new System.Drawing.Point(51, 27);
            cmbWarpMap.Margin = new Padding(2);
            cmbWarpMap.Name = "cmbWarpMap";
            cmbWarpMap.Size = new Size(196, 24);
            cmbWarpMap.TabIndex = 12;
            cmbWarpMap.Text = null;
            cmbWarpMap.TextPadding = new Padding(2);
            cmbWarpMap.SelectedIndexChanged += cmbWarpMap_SelectedIndexChanged;
            // 
            // cmbDirection
            // 
            cmbDirection.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbDirection.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbDirection.BorderStyle = ButtonBorderStyle.Solid;
            cmbDirection.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbDirection.DrawDropdownHoverOutline = false;
            cmbDirection.DrawFocusRectangle = false;
            cmbDirection.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDirection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDirection.FlatStyle = FlatStyle.Flat;
            cmbDirection.ForeColor = System.Drawing.Color.Gainsboro;
            cmbDirection.FormattingEnabled = true;
            cmbDirection.Items.AddRange(new object[] { "Up", "Down", "Left", "Right" });
            cmbDirection.Location = new System.Drawing.Point(75, 93);
            cmbDirection.Margin = new Padding(2);
            cmbDirection.Name = "cmbDirection";
            cmbDirection.Size = new Size(173, 24);
            cmbDirection.TabIndex = 23;
            cmbDirection.Text = "Up";
            cmbDirection.TextPadding = new Padding(2);
            cmbDirection.SelectedIndexChanged += cmbDirection_SelectedIndexChanged;
            // 
            // lblDir
            // 
            lblDir.AutoSize = true;
            lblDir.Location = new System.Drawing.Point(9, 97);
            lblDir.Margin = new Padding(2, 0, 2, 0);
            lblDir.Name = "lblDir";
            lblDir.Size = new Size(58, 15);
            lblDir.TabIndex = 22;
            lblDir.Text = "Direction:";
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new System.Drawing.Point(148, 63);
            lblY.Margin = new Padding(2, 0, 2, 0);
            lblY.Name = "lblY";
            lblY.Size = new Size(17, 15);
            lblY.TabIndex = 11;
            lblY.Text = "Y:";
            // 
            // lblX
            // 
            lblX.AutoSize = true;
            lblX.Location = new System.Drawing.Point(9, 63);
            lblX.Margin = new Padding(2, 0, 2, 0);
            lblX.Name = "lblX";
            lblX.Size = new Size(17, 15);
            lblX.TabIndex = 10;
            lblX.Text = "X:";
            // 
            // lblMap
            // 
            lblMap.AutoSize = true;
            lblMap.Location = new System.Drawing.Point(9, 30);
            lblMap.Margin = new Padding(2, 0, 2, 0);
            lblMap.Name = "lblMap";
            lblMap.Size = new Size(34, 15);
            lblMap.TabIndex = 9;
            lblMap.Text = "Map:";
            // 
            // pnlContainer
            // 
            pnlContainer.AutoScroll = true;
            pnlContainer.Controls.Add(grpSpawnItems);
            pnlContainer.Controls.Add(grpCombat);
            pnlContainer.Controls.Add(grpRegen);
            pnlContainer.Controls.Add(grpSprite);
            pnlContainer.Controls.Add(grpSpawnPoint);
            pnlContainer.Controls.Add(grpGeneral);
            pnlContainer.Controls.Add(grpSpells);
            pnlContainer.Controls.Add(grpBaseStats);
            pnlContainer.Controls.Add(nudArmorPenIncrease);
            pnlContainer.Controls.Add(grpLeveling);
            pnlContainer.Controls.Add(nudVitalityIncrease);
            pnlContainer.Controls.Add(grpExpGrid);
            pnlContainer.Controls.Add(nudWisdomIncrease);
            pnlContainer.Location = new System.Drawing.Point(243, 39);
            pnlContainer.Margin = new Padding(2);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(1126, 618);
            pnlContainer.TabIndex = 28;
            pnlContainer.Visible = false;
            // 
            // grpSpawnItems
            // 
            grpSpawnItems.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSpawnItems.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSpawnItems.Controls.Add(btnSpawnItemRemove);
            grpSpawnItems.Controls.Add(btnSpawnItemAdd);
            grpSpawnItems.Controls.Add(lstSpawnItems);
            grpSpawnItems.Controls.Add(nudSpawnItemAmount);
            grpSpawnItems.Controls.Add(cmbSpawnItem);
            grpSpawnItems.Controls.Add(lblSpawnItemAmount);
            grpSpawnItems.Controls.Add(lblSpawnItem);
            grpSpawnItems.ForeColor = System.Drawing.Color.Gainsboro;
            grpSpawnItems.Location = new System.Drawing.Point(553, 567);
            grpSpawnItems.Margin = new Padding(4, 3, 4, 3);
            grpSpawnItems.Name = "grpSpawnItems";
            grpSpawnItems.Padding = new Padding(4, 3, 4, 3);
            grpSpawnItems.Size = new Size(268, 173);
            grpSpawnItems.TabIndex = 32;
            grpSpawnItems.TabStop = false;
            grpSpawnItems.Text = "Spawn Items";
            // 
            // btnSpawnItemRemove
            // 
            btnSpawnItemRemove.Location = new System.Drawing.Point(128, 132);
            btnSpawnItemRemove.Margin = new Padding(4, 3, 4, 3);
            btnSpawnItemRemove.Name = "btnSpawnItemRemove";
            btnSpawnItemRemove.Padding = new Padding(6, 6, 6, 6);
            btnSpawnItemRemove.Size = new Size(130, 24);
            btnSpawnItemRemove.TabIndex = 64;
            btnSpawnItemRemove.Text = "Remove Item";
            btnSpawnItemRemove.Click += btnSpawnItemRemove_Click;
            // 
            // btnSpawnItemAdd
            // 
            btnSpawnItemAdd.Location = new System.Drawing.Point(128, 103);
            btnSpawnItemAdd.Margin = new Padding(4, 3, 4, 3);
            btnSpawnItemAdd.Name = "btnSpawnItemAdd";
            btnSpawnItemAdd.Padding = new Padding(6, 6, 6, 6);
            btnSpawnItemAdd.Size = new Size(130, 24);
            btnSpawnItemAdd.TabIndex = 63;
            btnSpawnItemAdd.Text = "Add Item";
            btnSpawnItemAdd.Click += btnSpawnItemAdd_Click;
            // 
            // lstSpawnItems
            // 
            lstSpawnItems.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstSpawnItems.BorderStyle = BorderStyle.FixedSingle;
            lstSpawnItems.ForeColor = System.Drawing.Color.Gainsboro;
            lstSpawnItems.FormattingEnabled = true;
            lstSpawnItems.ItemHeight = 15;
            lstSpawnItems.Location = new System.Drawing.Point(8, 20);
            lstSpawnItems.Margin = new Padding(4, 3, 4, 3);
            lstSpawnItems.Name = "lstSpawnItems";
            lstSpawnItems.Size = new Size(108, 137);
            lstSpawnItems.TabIndex = 62;
            lstSpawnItems.SelectedIndexChanged += lstSpawnItems_SelectedIndexChanged;
            // 
            // nudSpawnItemAmount
            // 
            nudSpawnItemAmount.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSpawnItemAmount.ForeColor = System.Drawing.Color.Gainsboro;
            nudSpawnItemAmount.Location = new System.Drawing.Point(188, 67);
            nudSpawnItemAmount.Margin = new Padding(4, 3, 4, 3);
            nudSpawnItemAmount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nudSpawnItemAmount.Name = "nudSpawnItemAmount";
            nudSpawnItemAmount.Size = new Size(70, 23);
            nudSpawnItemAmount.TabIndex = 61;
            nudSpawnItemAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpawnItemAmount.ValueChanged += nudSpawnItemAmount_ValueChanged;
            // 
            // cmbSpawnItem
            // 
            cmbSpawnItem.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbSpawnItem.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbSpawnItem.BorderStyle = ButtonBorderStyle.Solid;
            cmbSpawnItem.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbSpawnItem.DrawDropdownHoverOutline = false;
            cmbSpawnItem.DrawFocusRectangle = false;
            cmbSpawnItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSpawnItem.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpawnItem.FlatStyle = FlatStyle.Flat;
            cmbSpawnItem.ForeColor = System.Drawing.Color.Gainsboro;
            cmbSpawnItem.FormattingEnabled = true;
            cmbSpawnItem.Location = new System.Drawing.Point(132, 32);
            cmbSpawnItem.Margin = new Padding(4, 3, 4, 3);
            cmbSpawnItem.Name = "cmbSpawnItem";
            cmbSpawnItem.Size = new Size(125, 24);
            cmbSpawnItem.TabIndex = 17;
            cmbSpawnItem.Text = null;
            cmbSpawnItem.TextPadding = new Padding(2);
            cmbSpawnItem.SelectedIndexChanged += cmbSpawnItem_SelectedIndexChanged;
            // 
            // lblSpawnItemAmount
            // 
            lblSpawnItemAmount.AutoSize = true;
            lblSpawnItemAmount.Location = new System.Drawing.Point(128, 67);
            lblSpawnItemAmount.Margin = new Padding(4, 0, 4, 0);
            lblSpawnItemAmount.Name = "lblSpawnItemAmount";
            lblSpawnItemAmount.Size = new Size(54, 15);
            lblSpawnItemAmount.TabIndex = 15;
            lblSpawnItemAmount.Text = "Amount:";
            // 
            // lblSpawnItem
            // 
            lblSpawnItem.AutoSize = true;
            lblSpawnItem.Location = new System.Drawing.Point(128, 14);
            lblSpawnItem.Margin = new Padding(4, 0, 4, 0);
            lblSpawnItem.Name = "lblSpawnItem";
            lblSpawnItem.Size = new Size(34, 15);
            lblSpawnItem.TabIndex = 11;
            lblSpawnItem.Text = "Item:";
            // 
            // grpCombat
            // 
            grpCombat.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpCombat.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpCombat.Controls.Add(cmbAttackSprite);
            grpCombat.Controls.Add(lblSpriteAttack);
            grpCombat.Controls.Add(grpAttackSpeed);
            grpCombat.Controls.Add(nudCritMultiplier);
            grpCombat.Controls.Add(lblCritMultiplier);
            grpCombat.Controls.Add(nudScaling);
            grpCombat.Controls.Add(nudCritChance);
            grpCombat.Controls.Add(nudDamage);
            grpCombat.Controls.Add(cmbScalingStat);
            grpCombat.Controls.Add(lblScalingStat);
            grpCombat.Controls.Add(lblScalingAmount);
            grpCombat.Controls.Add(cmbDamageType);
            grpCombat.Controls.Add(lblDamageType);
            grpCombat.Controls.Add(lblCritChance);
            grpCombat.Controls.Add(cmbAttackAnimation);
            grpCombat.Controls.Add(lblAttackAnimation);
            grpCombat.Controls.Add(lblDamage);
            grpCombat.ForeColor = System.Drawing.Color.Gainsboro;
            grpCombat.Location = new System.Drawing.Point(833, 202);
            grpCombat.Margin = new Padding(4, 3, 4, 3);
            grpCombat.Name = "grpCombat";
            grpCombat.Padding = new Padding(4, 3, 4, 3);
            grpCombat.Size = new Size(264, 552);
            grpCombat.TabIndex = 30;
            grpCombat.TabStop = false;
            grpCombat.Text = "Combat (Unarmed)";
            // 
            // cmbAttackSprite
            // 
            cmbAttackSprite.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbAttackSprite.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbAttackSprite.BorderStyle = ButtonBorderStyle.Solid;
            cmbAttackSprite.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbAttackSprite.DrawDropdownHoverOutline = false;
            cmbAttackSprite.DrawFocusRectangle = false;
            cmbAttackSprite.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAttackSprite.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAttackSprite.FlatStyle = FlatStyle.Flat;
            cmbAttackSprite.ForeColor = System.Drawing.Color.Gainsboro;
            cmbAttackSprite.FormattingEnabled = true;
            cmbAttackSprite.Location = new System.Drawing.Point(14, 327);
            cmbAttackSprite.Margin = new Padding(4, 3, 4, 3);
            cmbAttackSprite.Name = "cmbAttackSprite";
            cmbAttackSprite.Size = new Size(223, 24);
            cmbAttackSprite.TabIndex = 68;
            cmbAttackSprite.Text = null;
            cmbAttackSprite.TextPadding = new Padding(2);
            cmbAttackSprite.SelectedIndexChanged += cmbAttackSprite_SelectedIndexChanged;
            // 
            // lblSpriteAttack
            // 
            lblSpriteAttack.AutoSize = true;
            lblSpriteAttack.Location = new System.Drawing.Point(12, 308);
            lblSpriteAttack.Margin = new Padding(4, 0, 4, 0);
            lblSpriteAttack.Name = "lblSpriteAttack";
            lblSpriteAttack.Size = new Size(136, 15);
            lblSpriteAttack.TabIndex = 67;
            lblSpriteAttack.Text = "Sprite Attack Animation:";
            // 
            // grpAttackSpeed
            // 
            grpAttackSpeed.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpAttackSpeed.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpAttackSpeed.Controls.Add(nudAttackSpeedValue);
            grpAttackSpeed.Controls.Add(lblAttackSpeedValue);
            grpAttackSpeed.Controls.Add(cmbAttackSpeedModifier);
            grpAttackSpeed.Controls.Add(lblAttackSpeedModifier);
            grpAttackSpeed.ForeColor = System.Drawing.Color.Gainsboro;
            grpAttackSpeed.Location = new System.Drawing.Point(15, 413);
            grpAttackSpeed.Margin = new Padding(4, 3, 4, 3);
            grpAttackSpeed.Name = "grpAttackSpeed";
            grpAttackSpeed.Padding = new Padding(4, 3, 4, 3);
            grpAttackSpeed.Size = new Size(224, 99);
            grpAttackSpeed.TabIndex = 66;
            grpAttackSpeed.TabStop = false;
            grpAttackSpeed.Text = "Attack Speed";
            // 
            // nudAttackSpeedValue
            // 
            nudAttackSpeedValue.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudAttackSpeedValue.ForeColor = System.Drawing.Color.Gainsboro;
            nudAttackSpeedValue.Location = new System.Drawing.Point(70, 67);
            nudAttackSpeedValue.Margin = new Padding(4, 3, 4, 3);
            nudAttackSpeedValue.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudAttackSpeedValue.Name = "nudAttackSpeedValue";
            nudAttackSpeedValue.Size = new Size(133, 23);
            nudAttackSpeedValue.TabIndex = 56;
            nudAttackSpeedValue.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudAttackSpeedValue.ValueChanged += nudAttackSpeedValue_ValueChanged;
            // 
            // lblAttackSpeedValue
            // 
            lblAttackSpeedValue.AutoSize = true;
            lblAttackSpeedValue.Location = new System.Drawing.Point(10, 69);
            lblAttackSpeedValue.Margin = new Padding(4, 0, 4, 0);
            lblAttackSpeedValue.Name = "lblAttackSpeedValue";
            lblAttackSpeedValue.Size = new Size(38, 15);
            lblAttackSpeedValue.TabIndex = 29;
            lblAttackSpeedValue.Text = "Value:";
            // 
            // cmbAttackSpeedModifier
            // 
            cmbAttackSpeedModifier.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbAttackSpeedModifier.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbAttackSpeedModifier.BorderStyle = ButtonBorderStyle.Solid;
            cmbAttackSpeedModifier.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbAttackSpeedModifier.DrawDropdownHoverOutline = false;
            cmbAttackSpeedModifier.DrawFocusRectangle = false;
            cmbAttackSpeedModifier.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAttackSpeedModifier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAttackSpeedModifier.FlatStyle = FlatStyle.Flat;
            cmbAttackSpeedModifier.ForeColor = System.Drawing.Color.Gainsboro;
            cmbAttackSpeedModifier.FormattingEnabled = true;
            cmbAttackSpeedModifier.Location = new System.Drawing.Point(70, 28);
            cmbAttackSpeedModifier.Margin = new Padding(4, 3, 4, 3);
            cmbAttackSpeedModifier.Name = "cmbAttackSpeedModifier";
            cmbAttackSpeedModifier.Size = new Size(132, 24);
            cmbAttackSpeedModifier.TabIndex = 28;
            cmbAttackSpeedModifier.Text = null;
            cmbAttackSpeedModifier.TextPadding = new Padding(2);
            cmbAttackSpeedModifier.SelectedIndexChanged += cmbAttackSpeedModifier_SelectedIndexChanged;
            // 
            // lblAttackSpeedModifier
            // 
            lblAttackSpeedModifier.AutoSize = true;
            lblAttackSpeedModifier.Location = new System.Drawing.Point(10, 31);
            lblAttackSpeedModifier.Margin = new Padding(4, 0, 4, 0);
            lblAttackSpeedModifier.Name = "lblAttackSpeedModifier";
            lblAttackSpeedModifier.Size = new Size(55, 15);
            lblAttackSpeedModifier.TabIndex = 0;
            lblAttackSpeedModifier.Text = "Modifier:";
            // 
            // nudCritMultiplier
            // 
            nudCritMultiplier.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCritMultiplier.DecimalPlaces = 2;
            nudCritMultiplier.ForeColor = System.Drawing.Color.Gainsboro;
            nudCritMultiplier.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudCritMultiplier.Location = new System.Drawing.Point(14, 129);
            nudCritMultiplier.Margin = new Padding(4, 3, 4, 3);
            nudCritMultiplier.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudCritMultiplier.Name = "nudCritMultiplier";
            nudCritMultiplier.Size = new Size(223, 23);
            nudCritMultiplier.TabIndex = 65;
            nudCritMultiplier.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudCritMultiplier.ValueChanged += nudCritMultiplier_ValueChanged;
            // 
            // lblCritMultiplier
            // 
            lblCritMultiplier.AutoSize = true;
            lblCritMultiplier.Location = new System.Drawing.Point(10, 113);
            lblCritMultiplier.Margin = new Padding(4, 0, 4, 0);
            lblCritMultiplier.Name = "lblCritMultiplier";
            lblCritMultiplier.Size = new Size(156, 15);
            lblCritMultiplier.TabIndex = 64;
            lblCritMultiplier.Text = "Crit Multiplier (Default 1.5x):";
            // 
            // nudScaling
            // 
            nudScaling.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudScaling.ForeColor = System.Drawing.Color.Gainsboro;
            nudScaling.Location = new System.Drawing.Point(15, 273);
            nudScaling.Margin = new Padding(4, 3, 4, 3);
            nudScaling.Name = "nudScaling";
            nudScaling.Size = new Size(224, 23);
            nudScaling.TabIndex = 61;
            nudScaling.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudScaling.ValueChanged += nudScaling_ValueChanged;
            // 
            // nudCritChance
            // 
            nudCritChance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCritChance.ForeColor = System.Drawing.Color.Gainsboro;
            nudCritChance.Location = new System.Drawing.Point(14, 82);
            nudCritChance.Margin = new Padding(4, 3, 4, 3);
            nudCritChance.Name = "nudCritChance";
            nudCritChance.Size = new Size(224, 23);
            nudCritChance.TabIndex = 60;
            nudCritChance.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudCritChance.ValueChanged += nudCritChance_ValueChanged;
            // 
            // nudDamage
            // 
            nudDamage.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudDamage.ForeColor = System.Drawing.Color.Gainsboro;
            nudDamage.Location = new System.Drawing.Point(14, 40);
            nudDamage.Margin = new Padding(4, 3, 4, 3);
            nudDamage.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(224, 23);
            nudDamage.TabIndex = 59;
            nudDamage.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudDamage.ValueChanged += nudDamage_ValueChanged;
            // 
            // cmbScalingStat
            // 
            cmbScalingStat.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbScalingStat.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbScalingStat.BorderStyle = ButtonBorderStyle.Solid;
            cmbScalingStat.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbScalingStat.DrawDropdownHoverOutline = false;
            cmbScalingStat.DrawFocusRectangle = false;
            cmbScalingStat.DrawMode = DrawMode.OwnerDrawFixed;
            cmbScalingStat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbScalingStat.FlatStyle = FlatStyle.Flat;
            cmbScalingStat.ForeColor = System.Drawing.Color.Gainsboro;
            cmbScalingStat.FormattingEnabled = true;
            cmbScalingStat.Location = new System.Drawing.Point(15, 224);
            cmbScalingStat.Margin = new Padding(4, 3, 4, 3);
            cmbScalingStat.Name = "cmbScalingStat";
            cmbScalingStat.Size = new Size(222, 24);
            cmbScalingStat.TabIndex = 58;
            cmbScalingStat.Text = null;
            cmbScalingStat.TextPadding = new Padding(2);
            cmbScalingStat.SelectedIndexChanged += cmbScalingStat_SelectedIndexChanged;
            // 
            // lblScalingStat
            // 
            lblScalingStat.AutoSize = true;
            lblScalingStat.Location = new System.Drawing.Point(12, 204);
            lblScalingStat.Margin = new Padding(4, 0, 4, 0);
            lblScalingStat.Name = "lblScalingStat";
            lblScalingStat.Size = new Size(71, 15);
            lblScalingStat.TabIndex = 57;
            lblScalingStat.Text = "Scaling Stat:";
            // 
            // lblScalingAmount
            // 
            lblScalingAmount.AutoSize = true;
            lblScalingAmount.Location = new System.Drawing.Point(10, 254);
            lblScalingAmount.Margin = new Padding(4, 0, 4, 0);
            lblScalingAmount.Name = "lblScalingAmount";
            lblScalingAmount.Size = new Size(95, 15);
            lblScalingAmount.TabIndex = 56;
            lblScalingAmount.Text = "Scaling Amount:";
            // 
            // cmbDamageType
            // 
            cmbDamageType.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbDamageType.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbDamageType.BorderStyle = ButtonBorderStyle.Solid;
            cmbDamageType.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbDamageType.DrawDropdownHoverOutline = false;
            cmbDamageType.DrawFocusRectangle = false;
            cmbDamageType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDamageType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDamageType.FlatStyle = FlatStyle.Flat;
            cmbDamageType.ForeColor = System.Drawing.Color.Gainsboro;
            cmbDamageType.FormattingEnabled = true;
            cmbDamageType.Items.AddRange(new object[] { "Physical", "Magic", "True" });
            cmbDamageType.Location = new System.Drawing.Point(15, 177);
            cmbDamageType.Margin = new Padding(4, 3, 4, 3);
            cmbDamageType.Name = "cmbDamageType";
            cmbDamageType.Size = new Size(222, 24);
            cmbDamageType.TabIndex = 54;
            cmbDamageType.Text = "Physical";
            cmbDamageType.TextPadding = new Padding(2);
            cmbDamageType.SelectedIndexChanged += cmbDamageType_SelectedIndexChanged;
            // 
            // lblDamageType
            // 
            lblDamageType.AutoSize = true;
            lblDamageType.Location = new System.Drawing.Point(12, 157);
            lblDamageType.Margin = new Padding(4, 0, 4, 0);
            lblDamageType.Name = "lblDamageType";
            lblDamageType.Size = new Size(81, 15);
            lblDamageType.TabIndex = 53;
            lblDamageType.Text = "Damage Type:";
            // 
            // lblCritChance
            // 
            lblCritChance.AutoSize = true;
            lblCritChance.Location = new System.Drawing.Point(10, 67);
            lblCritChance.Margin = new Padding(4, 0, 4, 0);
            lblCritChance.Name = "lblCritChance";
            lblCritChance.Size = new Size(93, 15);
            lblCritChance.TabIndex = 52;
            lblCritChance.Text = "Crit Chance: (%)";
            // 
            // cmbAttackAnimation
            // 
            cmbAttackAnimation.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbAttackAnimation.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbAttackAnimation.BorderStyle = ButtonBorderStyle.Solid;
            cmbAttackAnimation.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbAttackAnimation.DrawDropdownHoverOutline = false;
            cmbAttackAnimation.DrawFocusRectangle = false;
            cmbAttackAnimation.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAttackAnimation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAttackAnimation.FlatStyle = FlatStyle.Flat;
            cmbAttackAnimation.ForeColor = System.Drawing.Color.Gainsboro;
            cmbAttackAnimation.FormattingEnabled = true;
            cmbAttackAnimation.Location = new System.Drawing.Point(14, 375);
            cmbAttackAnimation.Margin = new Padding(4, 3, 4, 3);
            cmbAttackAnimation.Name = "cmbAttackAnimation";
            cmbAttackAnimation.Size = new Size(223, 24);
            cmbAttackAnimation.TabIndex = 50;
            cmbAttackAnimation.Text = null;
            cmbAttackAnimation.TextPadding = new Padding(2);
            cmbAttackAnimation.SelectedIndexChanged += cmbAttackAnimation_SelectedIndexChanged;
            // 
            // lblAttackAnimation
            // 
            lblAttackAnimation.AutoSize = true;
            lblAttackAnimation.Location = new System.Drawing.Point(10, 358);
            lblAttackAnimation.Margin = new Padding(4, 0, 4, 0);
            lblAttackAnimation.Name = "lblAttackAnimation";
            lblAttackAnimation.Size = new Size(132, 15);
            lblAttackAnimation.TabIndex = 49;
            lblAttackAnimation.Text = "Extra Attack Animation:";
            // 
            // lblDamage
            // 
            lblDamage.AutoSize = true;
            lblDamage.Location = new System.Drawing.Point(10, 21);
            lblDamage.Margin = new Padding(4, 0, 4, 0);
            lblDamage.Name = "lblDamage";
            lblDamage.Size = new Size(81, 15);
            lblDamage.TabIndex = 48;
            lblDamage.Text = "Base Damage:";
            // 
            // grpRegen
            // 
            grpRegen.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpRegen.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpRegen.Controls.Add(nudMpRegen);
            grpRegen.Controls.Add(nudHPRegen);
            grpRegen.Controls.Add(lblHpRegen);
            grpRegen.Controls.Add(lblManaRegen);
            grpRegen.Controls.Add(lblRegenHint);
            grpRegen.ForeColor = System.Drawing.Color.Gainsboro;
            grpRegen.Location = new System.Drawing.Point(833, 6);
            grpRegen.Margin = new Padding(2);
            grpRegen.Name = "grpRegen";
            grpRegen.Padding = new Padding(2);
            grpRegen.Size = new Size(264, 188);
            grpRegen.TabIndex = 19;
            grpRegen.TabStop = false;
            grpRegen.Text = "Regen";
            // 
            // nudMpRegen
            // 
            nudMpRegen.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMpRegen.ForeColor = System.Drawing.Color.Gainsboro;
            nudMpRegen.Location = new System.Drawing.Point(15, 78);
            nudMpRegen.Margin = new Padding(4, 3, 4, 3);
            nudMpRegen.Name = "nudMpRegen";
            nudMpRegen.Size = new Size(224, 23);
            nudMpRegen.TabIndex = 31;
            nudMpRegen.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMpRegen.ValueChanged += nudMpRegen_ValueChanged;
            // 
            // nudHPRegen
            // 
            nudHPRegen.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudHPRegen.ForeColor = System.Drawing.Color.Gainsboro;
            nudHPRegen.Location = new System.Drawing.Point(15, 36);
            nudHPRegen.Margin = new Padding(4, 3, 4, 3);
            nudHPRegen.Name = "nudHPRegen";
            nudHPRegen.Size = new Size(224, 23);
            nudHPRegen.TabIndex = 30;
            nudHPRegen.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudHPRegen.ValueChanged += nudHPRegen_ValueChanged;
            // 
            // lblHpRegen
            // 
            lblHpRegen.AutoSize = true;
            lblHpRegen.Location = new System.Drawing.Point(6, 20);
            lblHpRegen.Margin = new Padding(2, 0, 2, 0);
            lblHpRegen.Name = "lblHpRegen";
            lblHpRegen.Size = new Size(47, 15);
            lblHpRegen.TabIndex = 26;
            lblHpRegen.Text = "HP: (%)";
            // 
            // lblManaRegen
            // 
            lblManaRegen.AutoSize = true;
            lblManaRegen.Location = new System.Drawing.Point(6, 62);
            lblManaRegen.Margin = new Padding(2, 0, 2, 0);
            lblManaRegen.Name = "lblManaRegen";
            lblManaRegen.Size = new Size(61, 15);
            lblManaRegen.TabIndex = 27;
            lblManaRegen.Text = "Mana: (%)";
            // 
            // lblRegenHint
            // 
            lblRegenHint.Location = new System.Drawing.Point(8, 113);
            lblRegenHint.Margin = new Padding(4, 0, 4, 0);
            lblRegenHint.Name = "lblRegenHint";
            lblRegenHint.Size = new Size(231, 57);
            lblRegenHint.TabIndex = 0;
            lblRegenHint.Text = "% of HP/Mana to restore per tick.\r\n\r\nTick timer saved in server config.json.";
            // 
            // grpSprite
            // 
            grpSprite.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSprite.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSprite.Controls.Add(grpSpriteOptions);
            grpSprite.Controls.Add(lblFace);
            grpSprite.Controls.Add(picFace);
            grpSprite.Controls.Add(cmbFace);
            grpSprite.Controls.Add(grpGender);
            grpSprite.Controls.Add(lstSprites);
            grpSprite.Controls.Add(lblSprite);
            grpSprite.Controls.Add(picSprite);
            grpSprite.Controls.Add(cmbSprite);
            grpSprite.ForeColor = System.Drawing.Color.Gainsboro;
            grpSprite.Location = new System.Drawing.Point(259, 6);
            grpSprite.Margin = new Padding(4, 3, 4, 3);
            grpSprite.Name = "grpSprite";
            grpSprite.Padding = new Padding(4, 3, 4, 3);
            grpSprite.Size = new Size(564, 188);
            grpSprite.TabIndex = 28;
            grpSprite.TabStop = false;
            grpSprite.Text = "Sprite and Face";
            // 
            // grpSpriteOptions
            // 
            grpSpriteOptions.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSpriteOptions.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSpriteOptions.Controls.Add(btnAdd);
            grpSpriteOptions.Controls.Add(btnRemove);
            grpSpriteOptions.ForeColor = System.Drawing.Color.Gainsboro;
            grpSpriteOptions.Location = new System.Drawing.Point(134, 83);
            grpSpriteOptions.Margin = new Padding(4, 3, 4, 3);
            grpSpriteOptions.Name = "grpSpriteOptions";
            grpSpriteOptions.Padding = new Padding(4, 3, 4, 3);
            grpSpriteOptions.Size = new Size(162, 93);
            grpSpriteOptions.TabIndex = 21;
            grpSpriteOptions.TabStop = false;
            grpSpriteOptions.Text = "Options";
            // 
            // lblFace
            // 
            lblFace.AutoSize = true;
            lblFace.Location = new System.Drawing.Point(438, 17);
            lblFace.Margin = new Padding(2, 0, 2, 0);
            lblFace.Name = "lblFace";
            lblFace.Size = new Size(34, 15);
            lblFace.TabIndex = 22;
            lblFace.Text = "Face:";
            // 
            // picFace
            // 
            picFace.BackColor = System.Drawing.Color.Black;
            picFace.Location = new System.Drawing.Point(441, 72);
            picFace.Margin = new Padding(2);
            picFace.Name = "picFace";
            picFace.Size = new Size(112, 111);
            picFace.TabIndex = 21;
            picFace.TabStop = false;
            // 
            // cmbFace
            // 
            cmbFace.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbFace.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbFace.BorderStyle = ButtonBorderStyle.Solid;
            cmbFace.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbFace.DrawDropdownHoverOutline = false;
            cmbFace.DrawFocusRectangle = false;
            cmbFace.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFace.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFace.FlatStyle = FlatStyle.Flat;
            cmbFace.ForeColor = System.Drawing.Color.Gainsboro;
            cmbFace.FormattingEnabled = true;
            cmbFace.Items.AddRange(new object[] { "None" });
            cmbFace.Location = new System.Drawing.Point(441, 39);
            cmbFace.Margin = new Padding(2);
            cmbFace.Name = "cmbFace";
            cmbFace.Size = new Size(111, 24);
            cmbFace.TabIndex = 23;
            cmbFace.Text = "None";
            cmbFace.TextPadding = new Padding(2);
            cmbFace.SelectedIndexChanged += cmbFace_SelectedIndexChanged;
            // 
            // grpGender
            // 
            grpGender.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpGender.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpGender.Controls.Add(rbMale);
            grpGender.Controls.Add(rbFemale);
            grpGender.ForeColor = System.Drawing.Color.Gainsboro;
            grpGender.Location = new System.Drawing.Point(134, 16);
            grpGender.Margin = new Padding(4, 3, 4, 3);
            grpGender.Name = "grpGender";
            grpGender.Padding = new Padding(4, 3, 4, 3);
            grpGender.Size = new Size(162, 60);
            grpGender.TabIndex = 20;
            grpGender.TabStop = false;
            grpGender.Text = "Gender";
            // 
            // grpLeveling
            // 
            grpLeveling.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpLeveling.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpLeveling.Controls.Add(btnExpGrid);
            grpLeveling.Controls.Add(nudBaseExp);
            grpLeveling.Controls.Add(nudExpIncrease);
            grpLeveling.Controls.Add(lblExpIncrease);
            grpLeveling.Controls.Add(lblBaseExp);
            grpLeveling.Controls.Add(grpLevelBoosts);
            grpLeveling.ForeColor = System.Drawing.Color.Gainsboro;
            grpLeveling.Location = new System.Drawing.Point(9, 202);
            grpLeveling.Margin = new Padding(2);
            grpLeveling.Name = "grpLeveling";
            grpLeveling.Padding = new Padding(2);
            grpLeveling.Size = new Size(532, 223);
            grpLeveling.TabIndex = 29;
            grpLeveling.TabStop = false;
            grpLeveling.Text = "Leveling Up";
            // 
            // btnExpGrid
            // 
            btnExpGrid.Location = new System.Drawing.Point(388, 35);
            btnExpGrid.Margin = new Padding(2);
            btnExpGrid.Name = "btnExpGrid";
            btnExpGrid.Padding = new Padding(6, 6, 6, 6);
            btnExpGrid.Size = new Size(130, 24);
            btnExpGrid.TabIndex = 37;
            btnExpGrid.Text = "Experience Grid";
            btnExpGrid.Click += btnExpGrid_Click;
            // 
            // nudBaseExp
            // 
            nudBaseExp.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudBaseExp.ForeColor = System.Drawing.Color.Gainsboro;
            nudBaseExp.Location = new System.Drawing.Point(8, 36);
            nudBaseExp.Margin = new Padding(4, 3, 4, 3);
            nudBaseExp.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudBaseExp.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudBaseExp.Name = "nudBaseExp";
            nudBaseExp.Size = new Size(115, 23);
            nudBaseExp.TabIndex = 36;
            nudBaseExp.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudBaseExp.ValueChanged += nudBaseExp_ValueChanged;
            // 
            // nudExpIncrease
            // 
            nudExpIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudExpIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudExpIncrease.Location = new System.Drawing.Point(134, 36);
            nudExpIncrease.Margin = new Padding(4, 3, 4, 3);
            nudExpIncrease.Name = "nudExpIncrease";
            nudExpIncrease.Size = new Size(142, 23);
            nudExpIncrease.TabIndex = 31;
            nudExpIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudExpIncrease.ValueChanged += nudExpIncrease_ValueChanged;
            // 
            // lblExpIncrease
            // 
            lblExpIncrease.AutoSize = true;
            lblExpIncrease.Location = new System.Drawing.Point(131, 17);
            lblExpIncrease.Margin = new Padding(4, 0, 4, 0);
            lblExpIncrease.Name = "lblExpIncrease";
            lblExpIncrease.Size = new Size(133, 15);
            lblExpIncrease.TabIndex = 21;
            lblExpIncrease.Text = "Exp Increase (Per Lvl %):";
            // 
            // lblBaseExp
            // 
            lblBaseExp.AutoSize = true;
            lblBaseExp.Location = new System.Drawing.Point(7, 18);
            lblBaseExp.Margin = new Padding(4, 0, 4, 0);
            lblBaseExp.Name = "lblBaseExp";
            lblBaseExp.Size = new Size(101, 15);
            lblBaseExp.TabIndex = 19;
            lblBaseExp.Text = "Base Exp To Level:";
            // 
            // grpLevelBoosts
            // 
            grpLevelBoosts.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpLevelBoosts.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpLevelBoosts.Controls.Add(nudHpIncrease);
            grpLevelBoosts.Controls.Add(nudMpIncrease);
            grpLevelBoosts.Controls.Add(nudPointsIncrease);
            grpLevelBoosts.Controls.Add(nudMagicResistIncrease);
            grpLevelBoosts.Controls.Add(nudSpeedIncrease);
            grpLevelBoosts.Controls.Add(nudMagicIncrease);
            grpLevelBoosts.Controls.Add(nudArmorIncrease);
            grpLevelBoosts.Controls.Add(nudStrengthIncrease);
            grpLevelBoosts.Controls.Add(rdoPercentageIncrease);
            grpLevelBoosts.Controls.Add(rdoStaticIncrease);
            grpLevelBoosts.Controls.Add(lblPointsIncrease);
            grpLevelBoosts.Controls.Add(lblHpIncrease);
            grpLevelBoosts.Controls.Add(lblMpIncrease);
            grpLevelBoosts.Controls.Add(lblSpeedIncrease);
            grpLevelBoosts.Controls.Add(lblStrengthIncrease);
            grpLevelBoosts.Controls.Add(lblMagicResistIncrease);
            grpLevelBoosts.Controls.Add(lblArmorIncrease);
            grpLevelBoosts.Controls.Add(lblArmorPenIncrease);
            grpLevelBoosts.Controls.Add(lblVitalityIncrease);
            grpLevelBoosts.Controls.Add(lblWisdomIncrease);
            grpLevelBoosts.Controls.Add(lblMagicIncrease);
            grpLevelBoosts.ForeColor = System.Drawing.Color.Gainsboro;
            grpLevelBoosts.Location = new System.Drawing.Point(10, 67);
            grpLevelBoosts.Margin = new Padding(4, 3, 4, 3);
            grpLevelBoosts.Name = "grpLevelBoosts";
            grpLevelBoosts.Padding = new Padding(4, 3, 4, 3);
            grpLevelBoosts.Size = new Size(507, 181);
            grpLevelBoosts.TabIndex = 23;
            grpLevelBoosts.TabStop = false;
            grpLevelBoosts.Text = "Level Up Boosts";
            // 
            // nudHpIncrease
            // 
            nudHpIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudHpIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudHpIncrease.Location = new System.Drawing.Point(13, 62);
            nudHpIncrease.Margin = new Padding(4, 3, 4, 3);
            nudHpIncrease.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudHpIncrease.Name = "nudHpIncrease";
            nudHpIncrease.Size = new Size(99, 23);
            nudHpIncrease.TabIndex = 36;
            nudHpIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudHpIncrease.ValueChanged += nudHpIncrease_ValueChanged;
            // 
            // nudMpIncrease
            // 
            nudMpIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMpIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudMpIncrease.Location = new System.Drawing.Point(138, 62);
            nudMpIncrease.Margin = new Padding(4, 3, 4, 3);
            nudMpIncrease.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudMpIncrease.Name = "nudMpIncrease";
            nudMpIncrease.Size = new Size(99, 23);
            nudMpIncrease.TabIndex = 35;
            nudMpIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMpIncrease.ValueChanged += nudMpIncrease_ValueChanged;
            // 
            // nudPointsIncrease
            // 
            nudPointsIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudPointsIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudPointsIncrease.Location = new System.Drawing.Point(396, 112);
            nudPointsIncrease.Margin = new Padding(4, 3, 4, 3);
            nudPointsIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudPointsIncrease.Name = "nudPointsIncrease";
            nudPointsIncrease.Size = new Size(99, 23);
            nudPointsIncrease.TabIndex = 34;
            nudPointsIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudPointsIncrease.ValueChanged += nudPointsIncrease_ValueChanged;
            // 
            // nudMagicResistIncrease
            // 
            nudMagicResistIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMagicResistIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudMagicResistIncrease.Location = new System.Drawing.Point(396, 62);
            nudMagicResistIncrease.Margin = new Padding(4, 3, 4, 3);
            nudMagicResistIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMagicResistIncrease.Name = "nudMagicResistIncrease";
            nudMagicResistIncrease.Size = new Size(99, 23);
            nudMagicResistIncrease.TabIndex = 33;
            nudMagicResistIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMagicResistIncrease.ValueChanged += nudMagicResistIncrease_ValueChanged;
            // 
            // nudSpeedIncrease
            // 
            nudSpeedIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSpeedIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudSpeedIncrease.Location = new System.Drawing.Point(265, 112);
            nudSpeedIncrease.Margin = new Padding(4, 3, 4, 3);
            nudSpeedIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSpeedIncrease.Name = "nudSpeedIncrease";
            nudSpeedIncrease.Size = new Size(99, 23);
            nudSpeedIncrease.TabIndex = 32;
            nudSpeedIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudSpeedIncrease.ValueChanged += nudSpeedIncrease_ValueChanged;
            // 
            // nudMagicIncrease
            // 
            nudMagicIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMagicIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudMagicIncrease.Location = new System.Drawing.Point(138, 112);
            nudMagicIncrease.Margin = new Padding(4, 3, 4, 3);
            nudMagicIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMagicIncrease.Name = "nudMagicIncrease";
            nudMagicIncrease.Size = new Size(99, 23);
            nudMagicIncrease.TabIndex = 31;
            nudMagicIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMagicIncrease.ValueChanged += nudMagicIncrease_ValueChanged;
            // 
            // nudArmorIncrease
            // 
            nudArmorIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudArmorIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudArmorIncrease.Location = new System.Drawing.Point(265, 62);
            nudArmorIncrease.Margin = new Padding(4, 3, 4, 3);
            nudArmorIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudArmorIncrease.Name = "nudArmorIncrease";
            nudArmorIncrease.Size = new Size(99, 23);
            nudArmorIncrease.TabIndex = 30;
            nudArmorIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudArmorIncrease.ValueChanged += nudArmorIncrease_ValueChanged;
            // 
            // nudStrengthIncrease
            // 
            nudStrengthIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudStrengthIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudStrengthIncrease.Location = new System.Drawing.Point(13, 112);
            nudStrengthIncrease.Margin = new Padding(4, 3, 4, 3);
            nudStrengthIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudStrengthIncrease.Name = "nudStrengthIncrease";
            nudStrengthIncrease.Size = new Size(99, 23);
            nudStrengthIncrease.TabIndex = 29;
            nudStrengthIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudStrengthIncrease.ValueChanged += nudStrengthIncrease_ValueChanged;
            // 
            // nudArmorPenIncrease
            // 
            nudArmorPenIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudArmorPenIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudArmorPenIncrease.Location = new System.Drawing.Point(157, 426);
            nudArmorPenIncrease.Margin = new Padding(4, 3, 4, 3);
            nudArmorPenIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudArmorPenIncrease.Name = "nudArmorPenIncrease";
            nudArmorPenIncrease.Size = new Size(99, 23);
            nudArmorPenIncrease.TabIndex = 75;
            nudArmorPenIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudArmorPenIncrease.ValueChanged += nudArmorPenIncrease_ValueChanged;
            // 
            // nudVitalityIncrease
            // 
            nudVitalityIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudVitalityIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudVitalityIncrease.Location = new System.Drawing.Point(284, 427);
            nudVitalityIncrease.Margin = new Padding(4, 3, 4, 3);
            nudVitalityIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudVitalityIncrease.Name = "nudVitalityIncrease";
            nudVitalityIncrease.Size = new Size(99, 23);
            nudVitalityIncrease.TabIndex = 76;
            nudVitalityIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudVitalityIncrease.ValueChanged += nudVitalityIncrease_ValueChanged;
            // 
            // nudWisdomIncrease
            // 
            nudWisdomIncrease.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudWisdomIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            nudWisdomIncrease.Location = new System.Drawing.Point(415, 427);
            nudWisdomIncrease.Margin = new Padding(4, 3, 4, 3);
            nudWisdomIncrease.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudWisdomIncrease.Name = "nudWisdomIncrease";
            nudWisdomIncrease.Size = new Size(99, 23);
            nudWisdomIncrease.TabIndex = 77;
            nudWisdomIncrease.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudWisdomIncrease.ValueChanged += nudWisdomIncrease_ValueChanged;
            // 
            // rdoPercentageIncrease
            // 
            rdoPercentageIncrease.AutoSize = true;
            rdoPercentageIncrease.Location = new System.Drawing.Point(77, 17);
            rdoPercentageIncrease.Margin = new Padding(4, 3, 4, 3);
            rdoPercentageIncrease.Name = "rdoPercentageIncrease";
            rdoPercentageIncrease.Size = new Size(84, 19);
            rdoPercentageIncrease.TabIndex = 1;
            rdoPercentageIncrease.Text = "Percentage";
            rdoPercentageIncrease.CheckedChanged += rdoPercentageIncrease_CheckedChanged;
            // 
            // rdoStaticIncrease
            // 
            rdoStaticIncrease.AutoSize = true;
            rdoStaticIncrease.Checked = true;
            rdoStaticIncrease.Location = new System.Drawing.Point(8, 17);
            rdoStaticIncrease.Margin = new Padding(4, 3, 4, 3);
            rdoStaticIncrease.Name = "rdoStaticIncrease";
            rdoStaticIncrease.Size = new Size(54, 19);
            rdoStaticIncrease.TabIndex = 0;
            rdoStaticIncrease.TabStop = true;
            rdoStaticIncrease.Text = "Static";
            rdoStaticIncrease.CheckedChanged += rdoStaticIncrease_CheckedChanged;
            // 
            // lblPointsIncrease
            // 
            lblPointsIncrease.AutoSize = true;
            lblPointsIncrease.Location = new System.Drawing.Point(392, 97);
            lblPointsIncrease.Margin = new Padding(2, 0, 2, 0);
            lblPointsIncrease.Name = "lblPointsIncrease";
            lblPointsIncrease.Size = new Size(43, 15);
            lblPointsIncrease.TabIndex = 18;
            lblPointsIncrease.Text = "Points:";
            // 
            // lblHpIncrease
            // 
            lblHpIncrease.AutoSize = true;
            lblHpIncrease.Location = new System.Drawing.Point(9, 44);
            lblHpIncrease.Margin = new Padding(2, 0, 2, 0);
            lblHpIncrease.Name = "lblHpIncrease";
            lblHpIncrease.Size = new Size(52, 15);
            lblHpIncrease.TabIndex = 14;
            lblHpIncrease.Text = "Max HP:";
            // 
            // lblMpIncrease
            // 
            lblMpIncrease.AutoSize = true;
            lblMpIncrease.Location = new System.Drawing.Point(138, 44);
            lblMpIncrease.Margin = new Padding(2, 0, 2, 0);
            lblMpIncrease.Name = "lblMpIncrease";
            lblMpIncrease.Size = new Size(54, 15);
            lblMpIncrease.TabIndex = 15;
            lblMpIncrease.Text = "Max MP:";
            // 
            // lblSpeedIncrease
            // 
            lblSpeedIncrease.AutoSize = true;
            lblSpeedIncrease.Location = new System.Drawing.Point(261, 97);
            lblSpeedIncrease.Margin = new Padding(2, 0, 2, 0);
            lblSpeedIncrease.Name = "lblSpeedIncrease";
            lblSpeedIncrease.Size = new Size(75, 15);
            lblSpeedIncrease.TabIndex = 9;
            lblSpeedIncrease.Text = "Move Speed:";
            // 
            // lblStrengthIncrease
            // 
            lblStrengthIncrease.AutoSize = true;
            lblStrengthIncrease.Location = new System.Drawing.Point(9, 97);
            lblStrengthIncrease.Margin = new Padding(2, 0, 2, 0);
            lblStrengthIncrease.Name = "lblStrengthIncrease";
            lblStrengthIncrease.Size = new Size(55, 15);
            lblStrengthIncrease.TabIndex = 5;
            lblStrengthIncrease.Text = "Strength:";
            // 
            // lblMagicResistIncrease
            // 
            lblMagicResistIncrease.AutoSize = true;
            lblMagicResistIncrease.Location = new System.Drawing.Point(392, 44);
            lblMagicResistIncrease.Margin = new Padding(2, 0, 2, 0);
            lblMagicResistIncrease.Name = "lblMagicResistIncrease";
            lblMagicResistIncrease.Size = new Size(76, 15);
            lblMagicResistIncrease.TabIndex = 8;
            lblMagicResistIncrease.Text = "Magic Resist:";
            // 
            // lblArmorIncrease
            // 
            lblArmorIncrease.AutoSize = true;
            lblArmorIncrease.Location = new System.Drawing.Point(261, 44);
            lblArmorIncrease.Margin = new Padding(2, 0, 2, 0);
            lblArmorIncrease.Name = "lblArmorIncrease";
            lblArmorIncrease.Size = new Size(44, 15);
            lblArmorIncrease.TabIndex = 7;
            lblArmorIncrease.Text = "Armor:";
            // 
            // lblArmorPenIncrease
            // 
            lblArmorPenIncrease.AutoSize = true;
            lblArmorPenIncrease.Location = new System.Drawing.Point(134, 141);
            lblArmorPenIncrease.Margin = new Padding(2, 0, 2, 0);
            lblArmorPenIncrease.Name = "lblArmorPenIncrease";
            lblArmorPenIncrease.Size = new Size(108, 15);
            lblArmorPenIncrease.TabIndex = 78;
            lblArmorPenIncrease.Text = "Armor Penetration:";
            // 
            // lblVitalityIncrease
            // 
            lblVitalityIncrease.AutoSize = true;
            lblVitalityIncrease.Location = new System.Drawing.Point(265, 141);
            lblVitalityIncrease.Margin = new Padding(2, 0, 2, 0);
            lblVitalityIncrease.Name = "lblVitalityIncrease";
            lblVitalityIncrease.Size = new Size(46, 15);
            lblVitalityIncrease.TabIndex = 79;
            lblVitalityIncrease.Text = "Vitality:";
            // 
            // lblWisdomIncrease
            // 
            lblWisdomIncrease.AutoSize = true;
            lblWisdomIncrease.Location = new System.Drawing.Point(396, 141);
            lblWisdomIncrease.Margin = new Padding(2, 0, 2, 0);
            lblWisdomIncrease.Name = "lblWisdomIncrease";
            lblWisdomIncrease.Size = new Size(54, 15);
            lblWisdomIncrease.TabIndex = 80;
            lblWisdomIncrease.Text = "Wisdom:";
            // 
            // lblMagicIncrease
            // 
            lblMagicIncrease.AutoSize = true;
            lblMagicIncrease.Location = new System.Drawing.Point(134, 97);
            lblMagicIncrease.Margin = new Padding(2, 0, 2, 0);
            lblMagicIncrease.Name = "lblMagicIncrease";
            lblMagicIncrease.Size = new Size(43, 15);
            lblMagicIncrease.TabIndex = 6;
            lblMagicIncrease.Text = "Magic:";
            // 
            // grpExpGrid
            // 
            grpExpGrid.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpExpGrid.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpExpGrid.Controls.Add(btnResetExpGrid);
            grpExpGrid.Controls.Add(btnCloseExpGrid);
            grpExpGrid.Controls.Add(expGrid);
            grpExpGrid.ForeColor = System.Drawing.Color.Gainsboro;
            grpExpGrid.Location = new System.Drawing.Point(9, 202);
            grpExpGrid.Margin = new Padding(2);
            grpExpGrid.Name = "grpExpGrid";
            grpExpGrid.Padding = new Padding(2);
            grpExpGrid.Size = new Size(532, 223);
            grpExpGrid.TabIndex = 37;
            grpExpGrid.TabStop = false;
            grpExpGrid.Text = "Experience Overrides";
            // 
            // btnResetExpGrid
            // 
            btnResetExpGrid.Location = new System.Drawing.Point(8, 193);
            btnResetExpGrid.Margin = new Padding(2);
            btnResetExpGrid.Name = "btnResetExpGrid";
            btnResetExpGrid.Padding = new Padding(6, 6, 6, 6);
            btnResetExpGrid.Size = new Size(97, 24);
            btnResetExpGrid.TabIndex = 39;
            btnResetExpGrid.Text = "Reset Grid";
            btnResetExpGrid.Click += btnResetExpGrid_Click;
            // 
            // btnCloseExpGrid
            // 
            btnCloseExpGrid.Location = new System.Drawing.Point(388, 193);
            btnCloseExpGrid.Margin = new Padding(2);
            btnCloseExpGrid.Name = "btnCloseExpGrid";
            btnCloseExpGrid.Padding = new Padding(6, 6, 6, 6);
            btnCloseExpGrid.Size = new Size(130, 24);
            btnCloseExpGrid.TabIndex = 38;
            btnCloseExpGrid.Text = "Close";
            btnCloseExpGrid.Click += btnCloseExpGrid_Click;
            // 
            // expGrid
            // 
            expGrid.AllowUserToAddRows = false;
            expGrid.AllowUserToDeleteRows = false;
            expGrid.AllowUserToResizeColumns = false;
            expGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(50, 53, 55);
            expGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            expGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            expGrid.BackgroundColor = System.Drawing.Color.FromArgb(45, 45, 48);
            expGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            expGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            expGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            expGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            expGrid.ColumnHeadersHeight = 24;
            expGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            expGrid.EnableHeadersVisualStyles = false;
            expGrid.Location = new System.Drawing.Point(8, 21);
            expGrid.Margin = new Padding(4, 3, 4, 3);
            expGrid.MultiSelect = false;
            expGrid.Name = "expGrid";
            expGrid.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            expGrid.RowsDefaultCellStyle = dataGridViewCellStyle9;
            expGrid.Size = new Size(510, 164);
            expGrid.TabIndex = 0;
            expGrid.CellEndEdit += expGrid_CellEndEdit;
            expGrid.CellMouseDown += expGrid_CellMouseDown;
            expGrid.EditingControlShowing += expGrid_EditingControlShowing;
            expGrid.SelectionChanged += expGrid_SelectionChanged;
            expGrid.KeyDown += expGrid_KeyDown;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(1222, 674);
            btnCancel.Margin = new Padding(2);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6, 6, 6, 6);
            btnCancel.Size = new Size(148, 37);
            btnCancel.TabIndex = 32;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(1065, 674);
            btnSave.Margin = new Padding(2);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6, 6, 6, 6);
            btnSave.Size = new Size(148, 37);
            btnSave.TabIndex = 29;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // toolStrip
            // 
            toolStrip.AutoSize = false;
            toolStrip.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            toolStrip.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripItemNew, toolStripSeparator1, toolStripItemDelete, toolStripSeparator2, btnAlphabetical, toolStripSeparator4, toolStripItemCopy, toolStripItemPaste, toolStripSeparator3, toolStripItemUndo });
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(6, 0, 1, 0);
            toolStrip.Size = new Size(1381, 29);
            toolStrip.TabIndex = 42;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripItemNew
            // 
            toolStripItemNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemNew.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemNew.Image = (Image)resources.GetObject("toolStripItemNew.Image");
            toolStripItemNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemNew.Name = "toolStripItemNew";
            toolStripItemNew.Size = new Size(23, 26);
            toolStripItemNew.Text = "New";
            toolStripItemNew.Click += toolStripItemNew_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator1.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 29);
            // 
            // toolStripItemDelete
            // 
            toolStripItemDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemDelete.Enabled = false;
            toolStripItemDelete.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemDelete.Image = (Image)resources.GetObject("toolStripItemDelete.Image");
            toolStripItemDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemDelete.Name = "toolStripItemDelete";
            toolStripItemDelete.Size = new Size(23, 26);
            toolStripItemDelete.Text = "Delete";
            toolStripItemDelete.Click += toolStripItemDelete_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator2.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 29);
            // 
            // btnAlphabetical
            // 
            btnAlphabetical.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAlphabetical.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btnAlphabetical.Image = (Image)resources.GetObject("btnAlphabetical.Image");
            btnAlphabetical.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAlphabetical.Name = "btnAlphabetical";
            btnAlphabetical.Size = new Size(23, 26);
            btnAlphabetical.Text = "Order Chronologically";
            btnAlphabetical.Click += btnAlphabetical_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator4.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 29);
            // 
            // toolStripItemCopy
            // 
            toolStripItemCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemCopy.Enabled = false;
            toolStripItemCopy.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemCopy.Image = (Image)resources.GetObject("toolStripItemCopy.Image");
            toolStripItemCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemCopy.Name = "toolStripItemCopy";
            toolStripItemCopy.Size = new Size(23, 26);
            toolStripItemCopy.Text = "Copy";
            toolStripItemCopy.Click += toolStripItemCopy_Click;
            // 
            // toolStripItemPaste
            // 
            toolStripItemPaste.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            toolStripItemPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemPaste.Enabled = false;
            toolStripItemPaste.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemPaste.Image = (Image)resources.GetObject("toolStripItemPaste.Image");
            toolStripItemPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemPaste.Name = "toolStripItemPaste";
            toolStripItemPaste.Size = new Size(23, 26);
            toolStripItemPaste.Text = "Paste";
            toolStripItemPaste.Click += toolStripItemPaste_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator3.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 29);
            // 
            // toolStripItemUndo
            // 
            toolStripItemUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemUndo.Enabled = false;
            toolStripItemUndo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemUndo.Image = (Image)resources.GetObject("toolStripItemUndo.Image");
            toolStripItemUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemUndo.Name = "toolStripItemUndo";
            toolStripItemUndo.Size = new Size(23, 26);
            toolStripItemUndo.Text = "Undo";
            toolStripItemUndo.Click += toolStripItemUndo_Click;
            // 
            // mnuExpGrid
            // 
            mnuExpGrid.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            mnuExpGrid.ImageScalingSize = new Size(24, 24);
            mnuExpGrid.Items.AddRange(new ToolStripItem[] { btnExpPaste });
            mnuExpGrid.Name = "commandMenu";
            mnuExpGrid.RenderMode = ToolStripRenderMode.System;
            mnuExpGrid.Size = new Size(103, 26);
            // 
            // btnExpPaste
            // 
            btnExpPaste.ForeColor = System.Drawing.Color.Gainsboro;
            btnExpPaste.Name = "btnExpPaste";
            btnExpPaste.Size = new Size(102, 22);
            btnExpPaste.Text = "Paste";
            btnExpPaste.Click += btnPaste_Click;
            // 
            // FrmClass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1381, 728);
            ControlBox = false;
            Controls.Add(toolStrip);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpClasses);
            Controls.Add(pnlContainer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new Padding(2);
            Name = "FrmClass";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Class Editor";
            Load += frmClass_Load;
            KeyDown += form_KeyDown;
            grpClasses.ResumeLayout(false);
            grpClasses.PerformLayout();
            grpBaseStats.ResumeLayout(false);
            grpBaseStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseMana).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBaseHP).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMR).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDef).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMag).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAttack).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudARP).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWis).EndInit();
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picSprite).EndInit();
            grpSpells.ResumeLayout(false);
            grpSpells.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
            grpSpawnPoint.ResumeLayout(false);
            grpSpawnPoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
            pnlContainer.ResumeLayout(false);
            grpSpawnItems.ResumeLayout(false);
            grpSpawnItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpawnItemAmount).EndInit();
            grpCombat.ResumeLayout(false);
            grpCombat.PerformLayout();
            grpAttackSpeed.ResumeLayout(false);
            grpAttackSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAttackSpeedValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCritMultiplier).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudScaling).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCritChance).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).EndInit();
            grpRegen.ResumeLayout(false);
            grpRegen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMpRegen).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHPRegen).EndInit();
            grpSprite.ResumeLayout(false);
            grpSprite.PerformLayout();
            grpSpriteOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picFace).EndInit();
            grpGender.ResumeLayout(false);
            grpGender.PerformLayout();
            grpLeveling.ResumeLayout(false);
            grpLeveling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaseExp).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudExpIncrease).EndInit();
            grpLevelBoosts.ResumeLayout(false);
            grpLevelBoosts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHpIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMpIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPointsIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMagicResistIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMagicIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudArmorIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStrengthIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudArmorPenIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVitalityIncrease).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWisdomIncrease).EndInit();
            grpExpGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)expGrid).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            mnuExpGrid.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private DarkGroupBox grpClasses;
        private DarkGroupBox grpBaseStats;
        private System.Windows.Forms.Label lblMana;
        private System.Windows.Forms.Label lblHP;
        private System.Windows.Forms.Label lblSpd;
        private System.Windows.Forms.Label lblMR;
        private System.Windows.Forms.Label lblDef;
        private System.Windows.Forms.Label lblMag;
        private System.Windows.Forms.Label lblAttack;
        private System.Windows.Forms.Label lblARP;
        private System.Windows.Forms.Label lblVit;
        private System.Windows.Forms.Label lblWis;
        private DarkGroupBox grpGeneral;
        private System.Windows.Forms.ListBox lstSprites;
        private DarkComboBox cmbSprite;
        private System.Windows.Forms.Label lblSprite;
        private System.Windows.Forms.PictureBox picSprite;
        private System.Windows.Forms.Label lblName;
        private DarkTextBox txtName;
        private DarkButton btnRemove;
        private DarkButton btnAdd;
        private DarkRadioButton rbFemale;
        private DarkRadioButton rbMale;
        private DarkGroupBox grpSpells;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblSpellNum;
        private DarkButton btnRemoveSpell;
        private DarkButton btnAddSpell;
        private System.Windows.Forms.ListBox lstSpells;
        private System.Windows.Forms.Label lblPoints;
        private DarkGroupBox grpSpawnPoint;
        private DarkButton btnVisualMapSelector;
        private DarkComboBox cmbWarpMap;
        private DarkComboBox cmbDirection;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.Panel pnlContainer;
        private DarkButton btnSave;
        private DarkButton btnCancel;
        private DarkGroupBox grpSprite;
        private System.Windows.Forms.Label lblFace;
        private System.Windows.Forms.PictureBox picFace;
        private DarkComboBox cmbFace;
        private DarkGroupBox grpGender;
        private DarkCheckBox chkLocked;
        private DarkGroupBox grpRegen;
        private System.Windows.Forms.Label lblHpRegen;
        private System.Windows.Forms.Label lblManaRegen;
        private System.Windows.Forms.Label lblRegenHint;
        private DarkGroupBox grpLeveling;
        private System.Windows.Forms.Label lblExpIncrease;
        private System.Windows.Forms.Label lblBaseExp;
        private DarkGroupBox grpLevelBoosts;
        private DarkRadioButton rdoPercentageIncrease;
        private DarkRadioButton rdoStaticIncrease;
        private System.Windows.Forms.Label lblPointsIncrease;
        private System.Windows.Forms.Label lblHpIncrease;
        private System.Windows.Forms.Label lblMpIncrease;
        private System.Windows.Forms.Label lblSpeedIncrease;
        private System.Windows.Forms.Label lblStrengthIncrease;
        private System.Windows.Forms.Label lblMagicResistIncrease;
        private System.Windows.Forms.Label lblArmorIncrease;
        private System.Windows.Forms.Label lblMagicIncrease;
        private System.Windows.Forms.Label lblArmorPenIncrease;
        private System.Windows.Forms.Label lblVitalityIncrease;
        private System.Windows.Forms.Label lblWisdomIncrease;
        private DarkToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripItemNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton toolStripItemCopy;
        public System.Windows.Forms.ToolStripButton toolStripItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton toolStripItemUndo;
        private DarkGroupBox grpCombat;
        private DarkComboBox cmbScalingStat;
        private System.Windows.Forms.Label lblScalingStat;
        private System.Windows.Forms.Label lblScalingAmount;
        private DarkComboBox cmbDamageType;
        private System.Windows.Forms.Label lblDamageType;
        private System.Windows.Forms.Label lblCritChance;
        private DarkComboBox cmbAttackAnimation;
        private System.Windows.Forms.Label lblAttackAnimation;
        private System.Windows.Forms.Label lblDamage;
        private DarkComboBox cmbSpell;
        private DarkNumericUpDown nudLevel;
        private DarkNumericUpDown nudY;
        private DarkNumericUpDown nudX;
        private DarkNumericUpDown nudScaling;
        private DarkNumericUpDown nudCritChance;
        private DarkNumericUpDown nudDamage;
        private DarkNumericUpDown nudMpRegen;
        private DarkNumericUpDown nudHPRegen;
        private DarkNumericUpDown nudPoints;
        private DarkNumericUpDown nudSpd;
        private DarkNumericUpDown nudMR;
        private DarkNumericUpDown nudDef;
        private DarkNumericUpDown nudMag;
        private DarkNumericUpDown nudAttack;
        private DarkNumericUpDown nudARP;
        private DarkNumericUpDown nudVit;
        private DarkNumericUpDown nudWis;
        private DarkNumericUpDown nudExpIncrease;
        private DarkNumericUpDown nudHpIncrease;
        private DarkNumericUpDown nudMpIncrease;
        private DarkNumericUpDown nudPointsIncrease;
        private DarkNumericUpDown nudMagicResistIncrease;
        private DarkNumericUpDown nudSpeedIncrease;
        private DarkNumericUpDown nudMagicIncrease;
        private DarkNumericUpDown nudArmorIncrease;
        private DarkNumericUpDown nudStrengthIncrease;
        private DarkNumericUpDown nudArmorPenIncrease;
        private DarkNumericUpDown nudVitalityIncrease;
        private DarkNumericUpDown nudWisdomIncrease;
        private DarkNumericUpDown nudBaseMana;
        private DarkNumericUpDown nudBaseHP;
        private DarkNumericUpDown nudBaseExp;
        private DarkGroupBox grpSpawnItems;
        private DarkButton btnSpawnItemRemove;
        private DarkButton btnSpawnItemAdd;
        private System.Windows.Forms.ListBox lstSpawnItems;
        private DarkNumericUpDown nudSpawnItemAmount;
        private DarkComboBox cmbSpawnItem;
        private System.Windows.Forms.Label lblSpawnItemAmount;
        private System.Windows.Forms.Label lblSpawnItem;
        private DarkNumericUpDown nudCritMultiplier;
        private System.Windows.Forms.Label lblCritMultiplier;
        private DarkButton btnExpGrid;
        private DarkGroupBox grpExpGrid;
        private DarkButton btnResetExpGrid;
        private DarkButton btnCloseExpGrid;
        private System.Windows.Forms.DataGridView expGrid;
        private System.Windows.Forms.ContextMenuStrip mnuExpGrid;
        private System.Windows.Forms.ToolStripMenuItem btnExpPaste;
        private System.Windows.Forms.ToolStripButton btnAlphabetical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private DarkButton btnClearSearch;
        private DarkTextBox txtSearch;
        private DarkButton btnAddFolder;
        private System.Windows.Forms.Label lblFolder;
        private DarkComboBox cmbFolder;
        private DarkGroupBox grpAttackSpeed;
        private DarkNumericUpDown nudAttackSpeedValue;
        private System.Windows.Forms.Label lblAttackSpeedValue;
        private DarkComboBox cmbAttackSpeedModifier;
        private System.Windows.Forms.Label lblAttackSpeedModifier;
        private Controls.GameObjectList lstGameObjects;
        private System.Windows.Forms.Label lblSpriteAttack;
        private DarkComboBox cmbAttackSprite;
        private DarkGroupBox grpSpriteOptions;
    }
}
