
namespace Intersect.Editor.Forms.Editors
{
    partial class FrmPet
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private DarkUI.Controls.DarkButton btnClearSearch;
        private DarkUI.Controls.DarkTextBox txtSearch;
        private Intersect.Editor.Forms.Controls.GameObjectList lstGameObjects;

        // Panel contenedor de los detalles
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lblName;
        private DarkUI.Controls.DarkTextBox txtName;
        private System.Windows.Forms.Label lblFolder;
        private DarkUI.Controls.DarkComboBox cmbFolder;
        private System.Windows.Forms.Label lblPic;
        private DarkUI.Controls.DarkComboBox cmbSprite;
        private System.Windows.Forms.PictureBox picPet;

        // Propiedades específicas de mascotas (por ejemplo, especie, requisitos de reproducción)
        private System.Windows.Forms.Label lblSpecies;
        private DarkUI.Controls.DarkTextBox txtSpecies;
        private System.Windows.Forms.Label lblRequiredMaturity;
        private DarkUI.Controls.DarkNumericUpDown nudRequiredMaturity;
        private System.Windows.Forms.Label lblRequiredEnergy;
        private DarkUI.Controls.DarkNumericUpDown nudRequiredEnergy;
        private System.Windows.Forms.Label lblRequiredMood;
        private DarkUI.Controls.DarkNumericUpDown nudRequiredMood;
        private System.Windows.Forms.Label lblMaxBreedCount;
        private DarkUI.Controls.DarkNumericUpDown nudMaxBreedCount;
        private System.Windows.Forms.Label lblStr;
        private DarkUI.Controls.DarkNumericUpDown nudStr;
        private System.Windows.Forms.Label lblMag;
        private DarkUI.Controls.DarkNumericUpDown nudMag;
        private System.Windows.Forms.Label lblDef;
        private DarkUI.Controls.DarkNumericUpDown nudDef;
        private System.Windows.Forms.Label lblMR;
        private DarkUI.Controls.DarkNumericUpDown nudMR;
        private System.Windows.Forms.Label lblSpd;
        private DarkUI.Controls.DarkNumericUpDown nudSpd;
        private System.Windows.Forms.Label lblDamage;
        private DarkUI.Controls.DarkNumericUpDown nudDamage;
        private System.Windows.Forms.Label lblCritChance;
        private DarkUI.Controls.DarkNumericUpDown nudCritChance;
        private System.Windows.Forms.Label lblCritMultiplier;
        private DarkUI.Controls.DarkNumericUpDown nudCritMultiplier;
        private System.Windows.Forms.Label lblScaling;
        private DarkUI.Controls.DarkNumericUpDown nudScaling;
        private System.Windows.Forms.Label lblDamageType;
        private DarkUI.Controls.DarkComboBox cmbDamageType;
        private System.Windows.Forms.Label lblScalingStat;
        private DarkUI.Controls.DarkComboBox cmbScalingStat;
        private System.Windows.Forms.Label lblAttackAnimation;
        private DarkUI.Controls.DarkComboBox cmbAttackAnimation;
        private System.Windows.Forms.Label lblAttackSpeedModifier;
        private DarkUI.Controls.DarkComboBox cmbAttackSpeedModifier;
        private System.Windows.Forms.Label lblAttackSpeedValue;
        private DarkUI.Controls.DarkNumericUpDown nudAttackSpeedValue;
        private System.Windows.Forms.Label lblHpRegen;
        private DarkUI.Controls.DarkNumericUpDown nudHpRegen;
        private System.Windows.Forms.Label lblManaRegen;
        private DarkUI.Controls.DarkNumericUpDown nudMpRegen;
        private System.Windows.Forms.Label lblRegenHint;
        private System.Windows.Forms.ListBox lstSpells;
        private DarkUI.Controls.DarkComboBox cmbSpell;
        private DarkUI.Controls.DarkButton btnAdd;
        private DarkUI.Controls.DarkButton btnRemove;
        private System.Windows.Forms.Label lblDefaultBehavior;
        private DarkUI.Controls.DarkComboBox cmbDefaultBehavior;
        private System.Windows.Forms.Label lblCombatRange;
        private DarkUI.Controls.DarkNumericUpDown nudCombatRange;

        /// <summary>
        /// Método requerido para el Diseñador de Windows Forms.
        /// No se debe modificar el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPet));
            btnClearSearch = new DarkUI.Controls.DarkButton();
            txtSearch = new DarkUI.Controls.DarkTextBox();
            lstGameObjects = new Controls.GameObjectList();
            pnlContainer = new Panel();
            lblName = new Label();
            txtName = new DarkUI.Controls.DarkTextBox();
            lblFolder = new Label();
            cmbFolder = new DarkUI.Controls.DarkComboBox();
            lblPic = new Label();
            cmbSprite = new DarkUI.Controls.DarkComboBox();
            picPet = new PictureBox();
            lblSpecies = new Label();
            txtSpecies = new DarkUI.Controls.DarkTextBox();
            lblRequiredMaturity = new Label();
            nudRequiredMaturity = new DarkUI.Controls.DarkNumericUpDown();
            lblRequiredEnergy = new Label();
            nudRequiredEnergy = new DarkUI.Controls.DarkNumericUpDown();
            lblRequiredMood = new Label();
            nudRequiredMood = new DarkUI.Controls.DarkNumericUpDown();
            lblMaxBreedCount = new Label();
            nudMaxBreedCount = new DarkUI.Controls.DarkNumericUpDown();
            lblStr = new Label();
            nudStr = new DarkUI.Controls.DarkNumericUpDown();
            lblMag = new Label();
            nudMag = new DarkUI.Controls.DarkNumericUpDown();
            lblDef = new Label();
            nudDef = new DarkUI.Controls.DarkNumericUpDown();
            lblMR = new Label();
            nudMR = new DarkUI.Controls.DarkNumericUpDown();
            lblSpd = new Label();
            nudSpd = new DarkUI.Controls.DarkNumericUpDown();
            lblDamage = new Label();
            nudDamage = new DarkUI.Controls.DarkNumericUpDown();
            lblCritChance = new Label();
            nudCritChance = new DarkUI.Controls.DarkNumericUpDown();
            lblCritMultiplier = new Label();
            nudCritMultiplier = new DarkUI.Controls.DarkNumericUpDown();
            lblScaling = new Label();
            nudScaling = new DarkUI.Controls.DarkNumericUpDown();
            lblDamageType = new Label();
            cmbDamageType = new DarkUI.Controls.DarkComboBox();
            lblScalingStat = new Label();
            cmbScalingStat = new DarkUI.Controls.DarkComboBox();
            lblAttackAnimation = new Label();
            cmbAttackAnimation = new DarkUI.Controls.DarkComboBox();
            lblAttackSpeedModifier = new Label();
            cmbAttackSpeedModifier = new DarkUI.Controls.DarkComboBox();
            lblAttackSpeedValue = new Label();
            nudAttackSpeedValue = new DarkUI.Controls.DarkNumericUpDown();
            lblHpRegen = new Label();
            nudHpRegen = new DarkUI.Controls.DarkNumericUpDown();
            lblManaRegen = new Label();
            nudMpRegen = new DarkUI.Controls.DarkNumericUpDown();
            lblRegenHint = new Label();
            lstSpells = new ListBox();
            cmbSpell = new DarkUI.Controls.DarkComboBox();
            btnAdd = new DarkUI.Controls.DarkButton();
            btnRemove = new DarkUI.Controls.DarkButton();
            lblDefaultBehavior = new Label();
            cmbDefaultBehavior = new DarkUI.Controls.DarkComboBox();
            lblCombatRange = new Label();
            nudCombatRange = new DarkUI.Controls.DarkNumericUpDown();
            grpGeneral = new DarkUI.Controls.DarkGroupBox();
            lblAlpha = new Label();
            lblBlue = new Label();
            lblGreen = new Label();
            lblRed = new Label();
            nudRgbaA = new DarkUI.Controls.DarkNumericUpDown();
            nudRgbaB = new DarkUI.Controls.DarkNumericUpDown();
            nudRgbaG = new DarkUI.Controls.DarkNumericUpDown();
            nudRgbaR = new DarkUI.Controls.DarkNumericUpDown();
            btnAddFolder = new DarkUI.Controls.DarkButton();
            label1 = new Label();
            darkComboBox1 = new DarkUI.Controls.DarkComboBox();
            darkComboBox2 = new DarkUI.Controls.DarkComboBox();
            label2 = new Label();
            picNpc = new PictureBox();
            label3 = new Label();
            darkTextBox1 = new DarkUI.Controls.DarkTextBox();
            grpStats = new DarkUI.Controls.DarkGroupBox();
            nudMana = new DarkUI.Controls.DarkNumericUpDown();
            nudHp = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown1 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown2 = new DarkUI.Controls.DarkNumericUpDown();
            nudWis = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown3 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown4 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown5 = new DarkUI.Controls.DarkNumericUpDown();
            nudARP = new DarkUI.Controls.DarkNumericUpDown();
            nudVit = new DarkUI.Controls.DarkNumericUpDown();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            lblARP = new Label();
            lblVit = new Label();
            lblWis = new Label();
            lblMana = new Label();
            lblHP = new Label();
            grpCombat = new DarkUI.Controls.DarkGroupBox();
            grpAttackSpeed = new DarkUI.Controls.DarkGroupBox();
            darkNumericUpDown6 = new DarkUI.Controls.DarkNumericUpDown();
            label9 = new Label();
            darkComboBox3 = new DarkUI.Controls.DarkComboBox();
            label10 = new Label();
            darkNumericUpDown7 = new DarkUI.Controls.DarkNumericUpDown();
            label11 = new Label();
            darkNumericUpDown8 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown9 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown10 = new DarkUI.Controls.DarkNumericUpDown();
            darkComboBox4 = new DarkUI.Controls.DarkComboBox();
            label12 = new Label();
            label13 = new Label();
            darkComboBox5 = new DarkUI.Controls.DarkComboBox();
            label14 = new Label();
            label15 = new Label();
            darkComboBox6 = new DarkUI.Controls.DarkComboBox();
            label16 = new Label();
            label17 = new Label();
            grpRegen = new DarkUI.Controls.DarkGroupBox();
            darkNumericUpDown11 = new DarkUI.Controls.DarkNumericUpDown();
            darkNumericUpDown12 = new DarkUI.Controls.DarkNumericUpDown();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            grpSpells = new DarkUI.Controls.DarkGroupBox();
            darkComboBox7 = new DarkUI.Controls.DarkComboBox();
            cmbFreq = new DarkUI.Controls.DarkComboBox();
            lblFreq = new Label();
            lblSpell = new Label();
            darkButton1 = new DarkUI.Controls.DarkButton();
            darkButton2 = new DarkUI.Controls.DarkButton();
            listBox1 = new ListBox();
            grpPets = new DarkUI.Controls.DarkGroupBox();
            darkButton3 = new DarkUI.Controls.DarkButton();
            darkTextBox2 = new DarkUI.Controls.DarkTextBox();
            gameObjectList1 = new Controls.GameObjectList();
            grpBehavior = new DarkUI.Controls.DarkGroupBox();
            nudResetRadius = new DarkUI.Controls.DarkNumericUpDown();
            lblResetRadius = new Label();
            chkFocusDamageDealer = new DarkUI.Controls.DarkCheckBox();
            nudFlee = new DarkUI.Controls.DarkNumericUpDown();
            lblFlee = new Label();
            nudSightRange = new DarkUI.Controls.DarkNumericUpDown();
            lblSightRange = new Label();
            btnCancel = new DarkUI.Controls.DarkButton();
            btnSave = new DarkUI.Controls.DarkButton();
            toolStrip = new DarkUI.Controls.DarkToolStrip();
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
            pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredMaturity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredEnergy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredMood).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxBreedCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMag).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDef).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCritChance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCritMultiplier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudScaling).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAttackSpeedValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHpRegen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMpRegen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCombatRange).BeginInit();
            grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRgbaA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNpc).BeginInit();
            grpStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMana).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudARP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVit).BeginInit();
            grpCombat.SuspendLayout();
            grpAttackSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown10).BeginInit();
            grpRegen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown12).BeginInit();
            grpSpells.SuspendLayout();
            grpPets.SuspendLayout();
            grpBehavior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudResetRadius).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFlee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSightRange).BeginInit();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new System.Drawing.Point(180, 20);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Padding = new Padding(5);
            btnClearSearch.Size = new Size(30, 25);
            btnClearSearch.TabIndex = 0;
            btnClearSearch.Text = "X";
            btnClearSearch.Click += btnClearSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtSearch.Location = new System.Drawing.Point(12, 20);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(160, 23);
            txtSearch.TabIndex = 0;
            // 
            // lstGameObjects
            // 
            lstGameObjects.ImageIndex = 0;
            lstGameObjects.LineColor = System.Drawing.Color.Empty;
            lstGameObjects.Location = new System.Drawing.Point(12, 50);
            lstGameObjects.Name = "lstGameObjects";
            lstGameObjects.SelectedImageIndex = 0;
            lstGameObjects.Size = new Size(195, 540);
            lstGameObjects.TabIndex = 0;
            // 
            // pnlContainer
            // 
            pnlContainer.AutoScroll = true;
            pnlContainer.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            pnlContainer.Controls.Add(grpBehavior);
            pnlContainer.Controls.Add(grpRegen);
            pnlContainer.Controls.Add(grpSpells);
            pnlContainer.Controls.Add(grpCombat);
            pnlContainer.Controls.Add(grpStats);
            pnlContainer.Controls.Add(grpGeneral);
            pnlContainer.Location = new System.Drawing.Point(240, 32);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(844, 568);
            pnlContainer.TabIndex = 2;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(10, 25);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtName.Location = new System.Drawing.Point(120, 20);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 23);
            txtName.TabIndex = 0;
            // 
            // lblFolder
            // 
            lblFolder.AutoSize = true;
            lblFolder.Location = new System.Drawing.Point(10, 55);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new Size(43, 15);
            lblFolder.TabIndex = 0;
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
            cmbFolder.DrawMode = DrawMode.OwnerDrawVariable;
            cmbFolder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFolder.FlatStyle = FlatStyle.Flat;
            cmbFolder.ForeColor = System.Drawing.Color.Gainsboro;
            cmbFolder.Location = new System.Drawing.Point(120, 50);
            cmbFolder.Name = "cmbFolder";
            cmbFolder.Size = new Size(200, 23);
            cmbFolder.TabIndex = 0;
            cmbFolder.Text = null;
            cmbFolder.TextPadding = new Padding(2);
            // 
            // lblPic
            // 
            lblPic.AutoSize = true;
            lblPic.Location = new System.Drawing.Point(10, 85);
            lblPic.Name = "lblPic";
            lblPic.Size = new Size(40, 15);
            lblPic.TabIndex = 0;
            lblPic.Text = "Sprite:";
            // 
            // cmbSprite
            // 
            cmbSprite.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbSprite.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbSprite.BorderStyle = ButtonBorderStyle.Solid;
            cmbSprite.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbSprite.DrawDropdownHoverOutline = false;
            cmbSprite.DrawFocusRectangle = false;
            cmbSprite.DrawMode = DrawMode.OwnerDrawVariable;
            cmbSprite.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSprite.FlatStyle = FlatStyle.Flat;
            cmbSprite.ForeColor = System.Drawing.Color.Gainsboro;
            cmbSprite.Location = new System.Drawing.Point(120, 80);
            cmbSprite.Name = "cmbSprite";
            cmbSprite.Size = new Size(200, 23);
            cmbSprite.TabIndex = 0;
            cmbSprite.Text = null;
            cmbSprite.TextPadding = new Padding(2);
            // 
            // picPet
            // 
            picPet.BackColor = System.Drawing.Color.Black;
            picPet.Location = new System.Drawing.Point(10, 115);
            picPet.Name = "picPet";
            picPet.Size = new Size(100, 100);
            picPet.TabIndex = 0;
            picPet.TabStop = false;
            // 
            // lblSpecies
            // 
            lblSpecies.AutoSize = true;
            lblSpecies.Location = new System.Drawing.Point(120, 125);
            lblSpecies.Name = "lblSpecies";
            lblSpecies.Size = new Size(54, 15);
            lblSpecies.TabIndex = 0;
            lblSpecies.Text = "Species:";
            // 
            // txtSpecies
            // 
            txtSpecies.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtSpecies.BorderStyle = BorderStyle.FixedSingle;
            txtSpecies.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtSpecies.Location = new System.Drawing.Point(200, 120);
            txtSpecies.Name = "txtSpecies";
            txtSpecies.Size = new Size(120, 23);
            txtSpecies.TabIndex = 0;
            // 
            // lblRequiredMaturity
            // 
            lblRequiredMaturity.AutoSize = true;
            lblRequiredMaturity.Location = new System.Drawing.Point(10, 150);
            lblRequiredMaturity.Name = "lblRequiredMaturity";
            lblRequiredMaturity.Size = new Size(97, 15);
            lblRequiredMaturity.TabIndex = 0;
            lblRequiredMaturity.Text = "Req. Maturity:";
            // 
            // nudRequiredMaturity
            // 
            nudRequiredMaturity.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRequiredMaturity.ForeColor = System.Drawing.Color.Gainsboro;
            nudRequiredMaturity.Location = new System.Drawing.Point(120, 145);
            nudRequiredMaturity.Name = "nudRequiredMaturity";
            nudRequiredMaturity.Size = new Size(80, 23);
            nudRequiredMaturity.TabIndex = 0;
            nudRequiredMaturity.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblRequiredEnergy
            // 
            lblRequiredEnergy.AutoSize = true;
            lblRequiredEnergy.Location = new System.Drawing.Point(10, 180);
            lblRequiredEnergy.Name = "lblRequiredEnergy";
            lblRequiredEnergy.Size = new Size(92, 15);
            lblRequiredEnergy.TabIndex = 0;
            lblRequiredEnergy.Text = "Req. Energy:";
            // 
            // nudRequiredEnergy
            // 
            nudRequiredEnergy.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRequiredEnergy.ForeColor = System.Drawing.Color.Gainsboro;
            nudRequiredEnergy.Location = new System.Drawing.Point(120, 175);
            nudRequiredEnergy.Name = "nudRequiredEnergy";
            nudRequiredEnergy.Size = new Size(80, 23);
            nudRequiredEnergy.TabIndex = 0;
            nudRequiredEnergy.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblRequiredMood
            // 
            lblRequiredMood.AutoSize = true;
            lblRequiredMood.Location = new System.Drawing.Point(10, 210);
            lblRequiredMood.Name = "lblRequiredMood";
            lblRequiredMood.Size = new Size(80, 15);
            lblRequiredMood.TabIndex = 0;
            lblRequiredMood.Text = "Req. Mood:";
            // 
            // nudRequiredMood
            // 
            nudRequiredMood.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRequiredMood.ForeColor = System.Drawing.Color.Gainsboro;
            nudRequiredMood.Location = new System.Drawing.Point(120, 205);
            nudRequiredMood.Name = "nudRequiredMood";
            nudRequiredMood.Size = new Size(80, 23);
            nudRequiredMood.TabIndex = 0;
            nudRequiredMood.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblMaxBreedCount
            // 
            lblMaxBreedCount.AutoSize = true;
            lblMaxBreedCount.Location = new System.Drawing.Point(10, 240);
            lblMaxBreedCount.Name = "lblMaxBreedCount";
            lblMaxBreedCount.Size = new Size(104, 15);
            lblMaxBreedCount.TabIndex = 0;
            lblMaxBreedCount.Text = "Max Breed Count:";
            // 
            // nudMaxBreedCount
            // 
            nudMaxBreedCount.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMaxBreedCount.ForeColor = System.Drawing.Color.Gainsboro;
            nudMaxBreedCount.Location = new System.Drawing.Point(120, 235);
            nudMaxBreedCount.Name = "nudMaxBreedCount";
            nudMaxBreedCount.Size = new Size(80, 23);
            nudMaxBreedCount.TabIndex = 0;
            nudMaxBreedCount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblStr
            // 
            lblStr.AutoSize = true;
            lblStr.Location = new System.Drawing.Point(10, 25);
            lblStr.Name = "lblStr";
            lblStr.Size = new Size(55, 15);
            lblStr.TabIndex = 0;
            lblStr.Text = "Strength:";
            // 
            // nudStr
            // 
            nudStr.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudStr.ForeColor = System.Drawing.Color.Gainsboro;
            nudStr.Location = new System.Drawing.Point(120, 20);
            nudStr.Name = "nudStr";
            nudStr.Size = new Size(80, 23);
            nudStr.TabIndex = 0;
            nudStr.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblMag
            // 
            lblMag.AutoSize = true;
            lblMag.Location = new System.Drawing.Point(10, 55);
            lblMag.Name = "lblMag";
            lblMag.Size = new Size(43, 15);
            lblMag.TabIndex = 0;
            lblMag.Text = "Magic:";
            // 
            // nudMag
            // 
            nudMag.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMag.ForeColor = System.Drawing.Color.Gainsboro;
            nudMag.Location = new System.Drawing.Point(120, 50);
            nudMag.Name = "nudMag";
            nudMag.Size = new Size(80, 23);
            nudMag.TabIndex = 0;
            nudMag.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblDef
            // 
            lblDef.AutoSize = true;
            lblDef.Location = new System.Drawing.Point(10, 85);
            lblDef.Name = "lblDef";
            lblDef.Size = new Size(44, 15);
            lblDef.TabIndex = 0;
            lblDef.Text = "Defense:";
            // 
            // nudDef
            // 
            nudDef.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudDef.ForeColor = System.Drawing.Color.Gainsboro;
            nudDef.Location = new System.Drawing.Point(120, 80);
            nudDef.Name = "nudDef";
            nudDef.Size = new Size(80, 23);
            nudDef.TabIndex = 0;
            nudDef.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblMR
            // 
            lblMR.AutoSize = true;
            lblMR.Location = new System.Drawing.Point(10, 115);
            lblMR.Name = "lblMR";
            lblMR.Size = new Size(76, 15);
            lblMR.TabIndex = 0;
            lblMR.Text = "Magic Resist:";
            // 
            // nudMR
            // 
            nudMR.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMR.ForeColor = System.Drawing.Color.Gainsboro;
            nudMR.Location = new System.Drawing.Point(120, 110);
            nudMR.Name = "nudMR";
            nudMR.Size = new Size(80, 23);
            nudMR.TabIndex = 0;
            nudMR.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblSpd
            // 
            lblSpd.AutoSize = true;
            lblSpd.Location = new System.Drawing.Point(10, 145);
            lblSpd.Name = "lblSpd";
            lblSpd.Size = new Size(45, 15);
            lblSpd.TabIndex = 0;
            lblSpd.Text = "Speed:";
            // 
            // nudSpd
            // 
            nudSpd.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSpd.ForeColor = System.Drawing.Color.Gainsboro;
            nudSpd.Location = new System.Drawing.Point(120, 140);
            nudSpd.Name = "nudSpd";
            nudSpd.Size = new Size(80, 23);
            nudSpd.TabIndex = 0;
            nudSpd.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblDamage
            // 
            lblDamage.AutoSize = true;
            lblDamage.Location = new System.Drawing.Point(10, 25);
            lblDamage.Name = "lblDamage";
            lblDamage.Size = new Size(81, 15);
            lblDamage.TabIndex = 0;
            lblDamage.Text = "Base Damage:";
            // 
            // nudDamage
            // 
            nudDamage.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudDamage.ForeColor = System.Drawing.Color.Gainsboro;
            nudDamage.Location = new System.Drawing.Point(120, 20);
            nudDamage.Name = "nudDamage";
            nudDamage.Size = new Size(80, 23);
            nudDamage.TabIndex = 0;
            nudDamage.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblCritChance
            // 
            lblCritChance.AutoSize = true;
            lblCritChance.Location = new System.Drawing.Point(10, 55);
            lblCritChance.Name = "lblCritChance";
            lblCritChance.Size = new Size(93, 15);
            lblCritChance.TabIndex = 0;
            lblCritChance.Text = "Crit Chance (%):";
            // 
            // nudCritChance
            // 
            nudCritChance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCritChance.ForeColor = System.Drawing.Color.Gainsboro;
            nudCritChance.Location = new System.Drawing.Point(120, 50);
            nudCritChance.Name = "nudCritChance";
            nudCritChance.Size = new Size(80, 23);
            nudCritChance.TabIndex = 0;
            nudCritChance.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblCritMultiplier
            // 
            lblCritMultiplier.AutoSize = true;
            lblCritMultiplier.Location = new System.Drawing.Point(10, 85);
            lblCritMultiplier.Name = "lblCritMultiplier";
            lblCritMultiplier.Size = new Size(85, 15);
            lblCritMultiplier.TabIndex = 0;
            lblCritMultiplier.Text = "Crit Multiplier:";
            // 
            // nudCritMultiplier
            // 
            nudCritMultiplier.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCritMultiplier.ForeColor = System.Drawing.Color.Gainsboro;
            nudCritMultiplier.Location = new System.Drawing.Point(120, 80);
            nudCritMultiplier.Name = "nudCritMultiplier";
            nudCritMultiplier.Size = new Size(80, 23);
            nudCritMultiplier.TabIndex = 0;
            nudCritMultiplier.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblScaling
            // 
            lblScaling.AutoSize = true;
            lblScaling.Location = new System.Drawing.Point(10, 115);
            lblScaling.Name = "lblScaling";
            lblScaling.Size = new Size(95, 15);
            lblScaling.TabIndex = 0;
            lblScaling.Text = "Scaling Amount:";
            // 
            // nudScaling
            // 
            nudScaling.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudScaling.ForeColor = System.Drawing.Color.Gainsboro;
            nudScaling.Location = new System.Drawing.Point(120, 110);
            nudScaling.Name = "nudScaling";
            nudScaling.Size = new Size(80, 23);
            nudScaling.TabIndex = 0;
            nudScaling.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblDamageType
            // 
            lblDamageType.AutoSize = true;
            lblDamageType.Location = new System.Drawing.Point(10, 145);
            lblDamageType.Name = "lblDamageType";
            lblDamageType.Size = new Size(81, 15);
            lblDamageType.TabIndex = 0;
            lblDamageType.Text = "Damage Type:";
            // 
            // cmbDamageType
            // 
            cmbDamageType.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbDamageType.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbDamageType.BorderStyle = ButtonBorderStyle.Solid;
            cmbDamageType.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbDamageType.DrawDropdownHoverOutline = false;
            cmbDamageType.DrawFocusRectangle = false;
            cmbDamageType.DrawMode = DrawMode.OwnerDrawVariable;
            cmbDamageType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDamageType.FlatStyle = FlatStyle.Flat;
            cmbDamageType.ForeColor = System.Drawing.Color.Gainsboro;
            cmbDamageType.Location = new System.Drawing.Point(120, 140);
            cmbDamageType.Name = "cmbDamageType";
            cmbDamageType.Size = new Size(120, 23);
            cmbDamageType.TabIndex = 0;
            cmbDamageType.Text = null;
            cmbDamageType.TextPadding = new Padding(2);
            // 
            // lblScalingStat
            // 
            lblScalingStat.AutoSize = true;
            lblScalingStat.Location = new System.Drawing.Point(10, 175);
            lblScalingStat.Name = "lblScalingStat";
            lblScalingStat.Size = new Size(71, 15);
            lblScalingStat.TabIndex = 0;
            lblScalingStat.Text = "Scaling Stat:";
            // 
            // cmbScalingStat
            // 
            cmbScalingStat.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbScalingStat.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbScalingStat.BorderStyle = ButtonBorderStyle.Solid;
            cmbScalingStat.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbScalingStat.DrawDropdownHoverOutline = false;
            cmbScalingStat.DrawFocusRectangle = false;
            cmbScalingStat.DrawMode = DrawMode.OwnerDrawVariable;
            cmbScalingStat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbScalingStat.FlatStyle = FlatStyle.Flat;
            cmbScalingStat.ForeColor = System.Drawing.Color.Gainsboro;
            cmbScalingStat.Location = new System.Drawing.Point(120, 170);
            cmbScalingStat.Name = "cmbScalingStat";
            cmbScalingStat.Size = new Size(120, 23);
            cmbScalingStat.TabIndex = 0;
            cmbScalingStat.Text = null;
            cmbScalingStat.TextPadding = new Padding(2);
            // 
            // lblAttackAnimation
            // 
            lblAttackAnimation.AutoSize = true;
            lblAttackAnimation.Location = new System.Drawing.Point(10, 205);
            lblAttackAnimation.Name = "lblAttackAnimation";
            lblAttackAnimation.Size = new Size(103, 15);
            lblAttackAnimation.TabIndex = 0;
            lblAttackAnimation.Text = "Attack Animation:";
            // 
            // cmbAttackAnimation
            // 
            cmbAttackAnimation.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbAttackAnimation.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbAttackAnimation.BorderStyle = ButtonBorderStyle.Solid;
            cmbAttackAnimation.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbAttackAnimation.DrawDropdownHoverOutline = false;
            cmbAttackAnimation.DrawFocusRectangle = false;
            cmbAttackAnimation.DrawMode = DrawMode.OwnerDrawVariable;
            cmbAttackAnimation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAttackAnimation.FlatStyle = FlatStyle.Flat;
            cmbAttackAnimation.ForeColor = System.Drawing.Color.Gainsboro;
            cmbAttackAnimation.Location = new System.Drawing.Point(120, 200);
            cmbAttackAnimation.Name = "cmbAttackAnimation";
            cmbAttackAnimation.Size = new Size(120, 23);
            cmbAttackAnimation.TabIndex = 0;
            cmbAttackAnimation.Text = null;
            cmbAttackAnimation.TextPadding = new Padding(2);
            // 
            // lblAttackSpeedModifier
            // 
            lblAttackSpeedModifier.AutoSize = true;
            lblAttackSpeedModifier.Location = new System.Drawing.Point(10, 235);
            lblAttackSpeedModifier.Name = "lblAttackSpeedModifier";
            lblAttackSpeedModifier.Size = new Size(135, 15);
            lblAttackSpeedModifier.TabIndex = 0;
            lblAttackSpeedModifier.Text = "Attack Speed Modifier:";
            // 
            // cmbAttackSpeedModifier
            // 
            cmbAttackSpeedModifier.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbAttackSpeedModifier.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbAttackSpeedModifier.BorderStyle = ButtonBorderStyle.Solid;
            cmbAttackSpeedModifier.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbAttackSpeedModifier.DrawDropdownHoverOutline = false;
            cmbAttackSpeedModifier.DrawFocusRectangle = false;
            cmbAttackSpeedModifier.DrawMode = DrawMode.OwnerDrawVariable;
            cmbAttackSpeedModifier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAttackSpeedModifier.FlatStyle = FlatStyle.Flat;
            cmbAttackSpeedModifier.ForeColor = System.Drawing.Color.Gainsboro;
            cmbAttackSpeedModifier.Location = new System.Drawing.Point(150, 230);
            cmbAttackSpeedModifier.Name = "cmbAttackSpeedModifier";
            cmbAttackSpeedModifier.Size = new Size(100, 23);
            cmbAttackSpeedModifier.TabIndex = 0;
            cmbAttackSpeedModifier.Text = null;
            cmbAttackSpeedModifier.TextPadding = new Padding(2);
            // 
            // lblAttackSpeedValue
            // 
            lblAttackSpeedValue.AutoSize = true;
            lblAttackSpeedValue.Location = new System.Drawing.Point(10, 265);
            lblAttackSpeedValue.Name = "lblAttackSpeedValue";
            lblAttackSpeedValue.Size = new Size(103, 15);
            lblAttackSpeedValue.TabIndex = 0;
            lblAttackSpeedValue.Text = "Attack Speed Value:";
            // 
            // nudAttackSpeedValue
            // 
            nudAttackSpeedValue.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudAttackSpeedValue.ForeColor = System.Drawing.Color.Gainsboro;
            nudAttackSpeedValue.Location = new System.Drawing.Point(150, 260);
            nudAttackSpeedValue.Name = "nudAttackSpeedValue";
            nudAttackSpeedValue.Size = new Size(100, 23);
            nudAttackSpeedValue.TabIndex = 0;
            nudAttackSpeedValue.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblHpRegen
            // 
            lblHpRegen.AutoSize = true;
            lblHpRegen.Location = new System.Drawing.Point(10, 25);
            lblHpRegen.Name = "lblHpRegen";
            lblHpRegen.Size = new Size(47, 15);
            lblHpRegen.TabIndex = 0;
            lblHpRegen.Text = "HP (%):";
            // 
            // nudHpRegen
            // 
            nudHpRegen.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudHpRegen.ForeColor = System.Drawing.Color.Gainsboro;
            nudHpRegen.Location = new System.Drawing.Point(120, 20);
            nudHpRegen.Name = "nudHpRegen";
            nudHpRegen.Size = new Size(80, 23);
            nudHpRegen.TabIndex = 0;
            nudHpRegen.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblManaRegen
            // 
            lblManaRegen.AutoSize = true;
            lblManaRegen.Location = new System.Drawing.Point(10, 55);
            lblManaRegen.Name = "lblManaRegen";
            lblManaRegen.Size = new Size(61, 15);
            lblManaRegen.TabIndex = 0;
            lblManaRegen.Text = "Mana (%):";
            // 
            // nudMpRegen
            // 
            nudMpRegen.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMpRegen.ForeColor = System.Drawing.Color.Gainsboro;
            nudMpRegen.Location = new System.Drawing.Point(120, 50);
            nudMpRegen.Name = "nudMpRegen";
            nudMpRegen.Size = new Size(80, 23);
            nudMpRegen.TabIndex = 0;
            nudMpRegen.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblRegenHint
            // 
            lblRegenHint.Location = new System.Drawing.Point(210, 20);
            lblRegenHint.Name = "lblRegenHint";
            lblRegenHint.Size = new Size(120, 60);
            lblRegenHint.TabIndex = 0;
            lblRegenHint.Text = "% of HP/Mana to restore per tick.";
            // 
            // lstSpells
            // 
            lstSpells.Location = new System.Drawing.Point(10, 25);
            lstSpells.Name = "lstSpells";
            lstSpells.Size = new Size(150, 100);
            lstSpells.TabIndex = 0;
            // 
            // cmbSpell
            // 
            cmbSpell.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbSpell.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbSpell.BorderStyle = ButtonBorderStyle.Solid;
            cmbSpell.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbSpell.DrawDropdownHoverOutline = false;
            cmbSpell.DrawFocusRectangle = false;
            cmbSpell.DrawMode = DrawMode.OwnerDrawVariable;
            cmbSpell.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpell.FlatStyle = FlatStyle.Flat;
            cmbSpell.ForeColor = System.Drawing.Color.Gainsboro;
            cmbSpell.Location = new System.Drawing.Point(170, 25);
            cmbSpell.Name = "cmbSpell";
            cmbSpell.Size = new Size(150, 23);
            cmbSpell.TabIndex = 0;
            cmbSpell.Text = null;
            cmbSpell.TextPadding = new Padding(2);
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(10, 130);
            btnAdd.Name = "btnAdd";
            btnAdd.Padding = new Padding(5);
            btnAdd.Size = new Size(75, 30);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(100, 130);
            btnRemove.Name = "btnRemove";
            btnRemove.Padding = new Padding(5);
            btnRemove.Size = new Size(75, 30);
            btnRemove.TabIndex = 0;
            btnRemove.Text = "Remove";
            btnRemove.Click += btnRemove_Click;
            // 
            // lblDefaultBehavior
            // 
            lblDefaultBehavior.AutoSize = true;
            lblDefaultBehavior.Location = new System.Drawing.Point(10, 25);
            lblDefaultBehavior.Name = "lblDefaultBehavior";
            lblDefaultBehavior.Size = new Size(96, 15);
            lblDefaultBehavior.TabIndex = 0;
            lblDefaultBehavior.Text = "Default Behavior:";
            // 
            // cmbDefaultBehavior
            // 
            cmbDefaultBehavior.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbDefaultBehavior.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbDefaultBehavior.BorderStyle = ButtonBorderStyle.Solid;
            cmbDefaultBehavior.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbDefaultBehavior.DrawDropdownHoverOutline = false;
            cmbDefaultBehavior.DrawFocusRectangle = false;
            cmbDefaultBehavior.DrawMode = DrawMode.OwnerDrawVariable;
            cmbDefaultBehavior.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDefaultBehavior.FlatStyle = FlatStyle.Flat;
            cmbDefaultBehavior.ForeColor = System.Drawing.Color.Gainsboro;
            cmbDefaultBehavior.Location = new System.Drawing.Point(130, 20);
            cmbDefaultBehavior.Name = "cmbDefaultBehavior";
            cmbDefaultBehavior.Size = new Size(150, 23);
            cmbDefaultBehavior.TabIndex = 0;
            cmbDefaultBehavior.Text = null;
            cmbDefaultBehavior.TextPadding = new Padding(2);
            // 
            // lblCombatRange
            // 
            lblCombatRange.AutoSize = true;
            lblCombatRange.Location = new System.Drawing.Point(10, 60);
            lblCombatRange.Name = "lblCombatRange";
            lblCombatRange.Size = new Size(88, 15);
            lblCombatRange.TabIndex = 0;
            lblCombatRange.Text = "Combat Range:";
            // 
            // nudCombatRange
            // 
            nudCombatRange.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCombatRange.ForeColor = System.Drawing.Color.Gainsboro;
            nudCombatRange.Location = new System.Drawing.Point(130, 55);
            nudCombatRange.Name = "nudCombatRange";
            nudCombatRange.Size = new Size(80, 23);
            nudCombatRange.TabIndex = 0;
            nudCombatRange.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // grpGeneral
            // 
            grpGeneral.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpGeneral.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpGeneral.Controls.Add(lblAlpha);
            grpGeneral.Controls.Add(lblBlue);
            grpGeneral.Controls.Add(lblGreen);
            grpGeneral.Controls.Add(lblRed);
            grpGeneral.Controls.Add(nudRgbaA);
            grpGeneral.Controls.Add(nudRgbaB);
            grpGeneral.Controls.Add(nudRgbaG);
            grpGeneral.Controls.Add(nudRgbaR);
            grpGeneral.Controls.Add(btnAddFolder);
            grpGeneral.Controls.Add(label1);
            grpGeneral.Controls.Add(darkComboBox1);
            grpGeneral.Controls.Add(darkComboBox2);
            grpGeneral.Controls.Add(label2);
            grpGeneral.Controls.Add(picNpc);
            grpGeneral.Controls.Add(label3);
            grpGeneral.Controls.Add(darkTextBox1);
            grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            grpGeneral.Location = new System.Drawing.Point(4, 3);
            grpGeneral.Margin = new Padding(4, 3, 4, 3);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Padding = new Padding(4, 3, 4, 3);
            grpGeneral.Size = new Size(241, 305);
            grpGeneral.TabIndex = 15;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "General";
            // 
            // lblAlpha
            // 
            lblAlpha.AutoSize = true;
            lblAlpha.Location = new System.Drawing.Point(127, 269);
            lblAlpha.Margin = new Padding(4, 0, 4, 0);
            lblAlpha.Name = "lblAlpha";
            lblAlpha.Size = new Size(41, 15);
            lblAlpha.TabIndex = 78;
            lblAlpha.Text = "Alpha:";
            // 
            // lblBlue
            // 
            lblBlue.AutoSize = true;
            lblBlue.Location = new System.Drawing.Point(127, 239);
            lblBlue.Margin = new Padding(4, 0, 4, 0);
            lblBlue.Name = "lblBlue";
            lblBlue.Size = new Size(33, 15);
            lblBlue.TabIndex = 77;
            lblBlue.Text = "Blue:";
            // 
            // lblGreen
            // 
            lblGreen.AutoSize = true;
            lblGreen.Location = new System.Drawing.Point(11, 269);
            lblGreen.Margin = new Padding(4, 0, 4, 0);
            lblGreen.Name = "lblGreen";
            lblGreen.Size = new Size(41, 15);
            lblGreen.TabIndex = 76;
            lblGreen.Text = "Green:";
            // 
            // lblRed
            // 
            lblRed.AutoSize = true;
            lblRed.Location = new System.Drawing.Point(11, 239);
            lblRed.Margin = new Padding(4, 0, 4, 0);
            lblRed.Name = "lblRed";
            lblRed.Size = new Size(30, 15);
            lblRed.TabIndex = 75;
            lblRed.Text = "Red:";
            // 
            // nudRgbaA
            // 
            nudRgbaA.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRgbaA.ForeColor = System.Drawing.Color.Gainsboro;
            nudRgbaA.Location = new System.Drawing.Point(178, 267);
            nudRgbaA.Margin = new Padding(4, 3, 4, 3);
            nudRgbaA.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudRgbaA.Name = "nudRgbaA";
            nudRgbaA.Size = new Size(49, 23);
            nudRgbaA.TabIndex = 74;
            nudRgbaA.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // nudRgbaB
            // 
            nudRgbaB.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRgbaB.ForeColor = System.Drawing.Color.Gainsboro;
            nudRgbaB.Location = new System.Drawing.Point(178, 237);
            nudRgbaB.Margin = new Padding(4, 3, 4, 3);
            nudRgbaB.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudRgbaB.Name = "nudRgbaB";
            nudRgbaB.Size = new Size(49, 23);
            nudRgbaB.TabIndex = 73;
            nudRgbaB.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // nudRgbaG
            // 
            nudRgbaG.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRgbaG.ForeColor = System.Drawing.Color.Gainsboro;
            nudRgbaG.Location = new System.Drawing.Point(64, 267);
            nudRgbaG.Margin = new Padding(4, 3, 4, 3);
            nudRgbaG.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudRgbaG.Name = "nudRgbaG";
            nudRgbaG.Size = new Size(49, 23);
            nudRgbaG.TabIndex = 72;
            nudRgbaG.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // nudRgbaR
            // 
            nudRgbaR.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudRgbaR.ForeColor = System.Drawing.Color.Gainsboro;
            nudRgbaR.Location = new System.Drawing.Point(64, 237);
            nudRgbaR.Margin = new Padding(4, 3, 4, 3);
            nudRgbaR.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudRgbaR.Name = "nudRgbaR";
            nudRgbaR.Size = new Size(49, 23);
            nudRgbaR.TabIndex = 71;
            nudRgbaR.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // btnAddFolder
            // 
            btnAddFolder.Location = new System.Drawing.Point(206, 54);
            btnAddFolder.Margin = new Padding(4, 3, 4, 3);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Padding = new Padding(6);
            btnAddFolder.Size = new Size(21, 24);
            btnAddFolder.TabIndex = 67;
            btnAddFolder.Text = "+";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 59);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 66;
            label1.Text = "Folder:";
            // 
            // darkComboBox1
            // 
            darkComboBox1.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox1.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox1.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox1.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox1.DrawDropdownHoverOutline = false;
            darkComboBox1.DrawFocusRectangle = false;
            darkComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox1.FlatStyle = FlatStyle.Flat;
            darkComboBox1.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox1.FormattingEnabled = true;
            darkComboBox1.Location = new System.Drawing.Point(70, 54);
            darkComboBox1.Margin = new Padding(4, 3, 4, 3);
            darkComboBox1.Name = "darkComboBox1";
            darkComboBox1.Size = new Size(131, 24);
            darkComboBox1.TabIndex = 65;
            darkComboBox1.Text = null;
            darkComboBox1.TextPadding = new Padding(2);
            // 
            // darkComboBox2
            // 
            darkComboBox2.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox2.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox2.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox2.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox2.DrawDropdownHoverOutline = false;
            darkComboBox2.DrawFocusRectangle = false;
            darkComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox2.FlatStyle = FlatStyle.Flat;
            darkComboBox2.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox2.FormattingEnabled = true;
            darkComboBox2.Items.AddRange(new object[] { "None" });
            darkComboBox2.Location = new System.Drawing.Point(70, 89);
            darkComboBox2.Margin = new Padding(4, 3, 4, 3);
            darkComboBox2.Name = "darkComboBox2";
            darkComboBox2.Size = new Size(156, 24);
            darkComboBox2.TabIndex = 11;
            darkComboBox2.Text = "None";
            darkComboBox2.TextPadding = new Padding(2);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 93);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 6;
            label2.Text = "Sprite:";
            // 
            // picNpc
            // 
            picNpc.BackColor = System.Drawing.Color.Black;
            picNpc.Location = new System.Drawing.Point(64, 120);
            picNpc.Margin = new Padding(4, 3, 4, 3);
            picNpc.Name = "picNpc";
            picNpc.Size = new Size(112, 111);
            picNpc.TabIndex = 4;
            picNpc.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(11, 24);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(42, 15);
            label3.TabIndex = 1;
            label3.Text = "Name:";
            // 
            // darkTextBox1
            // 
            darkTextBox1.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkTextBox1.BorderStyle = BorderStyle.FixedSingle;
            darkTextBox1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkTextBox1.Location = new System.Drawing.Point(70, 22);
            darkTextBox1.Margin = new Padding(4, 3, 4, 3);
            darkTextBox1.Name = "darkTextBox1";
            darkTextBox1.Size = new Size(157, 23);
            darkTextBox1.TabIndex = 0;
            // 
            // grpStats
            // 
            grpStats.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpStats.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpStats.Controls.Add(nudMana);
            grpStats.Controls.Add(nudHp);
            grpStats.Controls.Add(darkNumericUpDown1);
            grpStats.Controls.Add(darkNumericUpDown2);
            grpStats.Controls.Add(nudWis);
            grpStats.Controls.Add(darkNumericUpDown3);
            grpStats.Controls.Add(darkNumericUpDown4);
            grpStats.Controls.Add(darkNumericUpDown5);
            grpStats.Controls.Add(nudARP);
            grpStats.Controls.Add(nudVit);
            grpStats.Controls.Add(label4);
            grpStats.Controls.Add(label5);
            grpStats.Controls.Add(label6);
            grpStats.Controls.Add(label7);
            grpStats.Controls.Add(label8);
            grpStats.Controls.Add(lblARP);
            grpStats.Controls.Add(lblVit);
            grpStats.Controls.Add(lblWis);
            grpStats.Controls.Add(lblMana);
            grpStats.Controls.Add(lblHP);
            grpStats.ForeColor = System.Drawing.Color.Gainsboro;
            grpStats.Location = new System.Drawing.Point(4, 311);
            grpStats.Margin = new Padding(4, 3, 4, 3);
            grpStats.Name = "grpStats";
            grpStats.Padding = new Padding(4, 3, 4, 3);
            grpStats.Size = new Size(241, 316);
            grpStats.TabIndex = 16;
            grpStats.TabStop = false;
            grpStats.Text = "Stats:";
            // 
            // nudMana
            // 
            nudMana.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudMana.ForeColor = System.Drawing.Color.Gainsboro;
            nudMana.Location = new System.Drawing.Point(122, 40);
            nudMana.Margin = new Padding(4, 3, 4, 3);
            nudMana.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudMana.Name = "nudMana";
            nudMana.Size = new Size(90, 23);
            nudMana.TabIndex = 44;
            nudMana.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // nudHp
            // 
            nudHp.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudHp.ForeColor = System.Drawing.Color.Gainsboro;
            nudHp.Location = new System.Drawing.Point(14, 40);
            nudHp.Margin = new Padding(4, 3, 4, 3);
            nudHp.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudHp.Name = "nudHp";
            nudHp.Size = new Size(90, 23);
            nudHp.TabIndex = 43;
            nudHp.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown1
            // 
            darkNumericUpDown1.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown1.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown1.Location = new System.Drawing.Point(15, 190);
            darkNumericUpDown1.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown1.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown1.Name = "darkNumericUpDown1";
            darkNumericUpDown1.Size = new Size(90, 23);
            darkNumericUpDown1.TabIndex = 42;
            darkNumericUpDown1.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown2
            // 
            darkNumericUpDown2.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown2.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown2.Location = new System.Drawing.Point(122, 142);
            darkNumericUpDown2.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown2.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown2.Name = "darkNumericUpDown2";
            darkNumericUpDown2.Size = new Size(92, 23);
            darkNumericUpDown2.TabIndex = 41;
            darkNumericUpDown2.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // nudWis
            // 
            nudWis.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudWis.ForeColor = System.Drawing.Color.Gainsboro;
            nudWis.Location = new System.Drawing.Point(122, 236);
            nudWis.Margin = new Padding(4, 3, 4, 3);
            nudWis.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudWis.Name = "nudWis";
            nudWis.Size = new Size(90, 23);
            nudWis.TabIndex = 92;
            nudWis.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown3
            // 
            darkNumericUpDown3.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown3.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown3.Location = new System.Drawing.Point(14, 142);
            darkNumericUpDown3.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown3.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown3.Name = "darkNumericUpDown3";
            darkNumericUpDown3.Size = new Size(92, 23);
            darkNumericUpDown3.TabIndex = 40;
            darkNumericUpDown3.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown4
            // 
            darkNumericUpDown4.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown4.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown4.Location = new System.Drawing.Point(122, 92);
            darkNumericUpDown4.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown4.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown4.Name = "darkNumericUpDown4";
            darkNumericUpDown4.Size = new Size(90, 23);
            darkNumericUpDown4.TabIndex = 39;
            darkNumericUpDown4.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown5
            // 
            darkNumericUpDown5.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown5.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown5.Location = new System.Drawing.Point(14, 92);
            darkNumericUpDown5.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown5.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown5.Name = "darkNumericUpDown5";
            darkNumericUpDown5.Size = new Size(90, 23);
            darkNumericUpDown5.TabIndex = 38;
            darkNumericUpDown5.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // nudARP
            // 
            nudARP.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudARP.ForeColor = System.Drawing.Color.Gainsboro;
            nudARP.Location = new System.Drawing.Point(15, 236);
            nudARP.Margin = new Padding(4, 3, 4, 3);
            nudARP.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudARP.Name = "nudARP";
            nudARP.Size = new Size(90, 23);
            nudARP.TabIndex = 90;
            nudARP.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // nudVit
            // 
            nudVit.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudVit.ForeColor = System.Drawing.Color.Gainsboro;
            nudVit.Location = new System.Drawing.Point(122, 191);
            nudVit.Margin = new Padding(4, 3, 4, 3);
            nudVit.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudVit.Name = "nudVit";
            nudVit.Size = new Size(90, 23);
            nudVit.TabIndex = 91;
            nudVit.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(13, 175);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(75, 15);
            label4.TabIndex = 37;
            label4.Text = "Move Speed:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(122, 123);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(76, 15);
            label5.TabIndex = 36;
            label5.Text = "Magic Resist:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(11, 123);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 35;
            label6.Text = "Armor:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(125, 73);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(43, 15);
            label7.TabIndex = 34;
            label7.Text = "Magic:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(16, 72);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(55, 15);
            label8.TabIndex = 33;
            label8.Text = "Strength:";
            // 
            // lblARP
            // 
            lblARP.AutoSize = true;
            lblARP.Location = new System.Drawing.Point(15, 218);
            lblARP.Margin = new Padding(2, 0, 2, 0);
            lblARP.Name = "lblARP";
            lblARP.Size = new Size(50, 15);
            lblARP.TabIndex = 87;
            lblARP.Text = "Dextery:";
            // 
            // lblVit
            // 
            lblVit.AutoSize = true;
            lblVit.Location = new System.Drawing.Point(127, 173);
            lblVit.Margin = new Padding(2, 0, 2, 0);
            lblVit.Name = "lblVit";
            lblVit.Size = new Size(46, 15);
            lblVit.TabIndex = 88;
            lblVit.Text = "Vitality:";
            // 
            // lblWis
            // 
            lblWis.AutoSize = true;
            lblWis.Location = new System.Drawing.Point(122, 218);
            lblWis.Margin = new Padding(2, 0, 2, 0);
            lblWis.Name = "lblWis";
            lblWis.Size = new Size(71, 15);
            lblWis.TabIndex = 89;
            lblWis.Text = "Intelligence:";
            // 
            // lblMana
            // 
            lblMana.AutoSize = true;
            lblMana.Location = new System.Drawing.Point(127, 21);
            lblMana.Margin = new Padding(4, 0, 4, 0);
            lblMana.Name = "lblMana";
            lblMana.Size = new Size(40, 15);
            lblMana.TabIndex = 15;
            lblMana.Text = "Mana:";
            // 
            // lblHP
            // 
            lblHP.AutoSize = true;
            lblHP.Location = new System.Drawing.Point(13, 22);
            lblHP.Margin = new Padding(4, 0, 4, 0);
            lblHP.Name = "lblHP";
            lblHP.Size = new Size(26, 15);
            lblHP.TabIndex = 14;
            lblHP.Text = "HP:";
            // 
            // grpCombat
            // 
            grpCombat.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpCombat.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpCombat.Controls.Add(grpAttackSpeed);
            grpCombat.Controls.Add(darkNumericUpDown7);
            grpCombat.Controls.Add(label11);
            grpCombat.Controls.Add(darkNumericUpDown8);
            grpCombat.Controls.Add(darkNumericUpDown9);
            grpCombat.Controls.Add(darkNumericUpDown10);
            grpCombat.Controls.Add(darkComboBox4);
            grpCombat.Controls.Add(label12);
            grpCombat.Controls.Add(label13);
            grpCombat.Controls.Add(darkComboBox5);
            grpCombat.Controls.Add(label14);
            grpCombat.Controls.Add(label15);
            grpCombat.Controls.Add(darkComboBox6);
            grpCombat.Controls.Add(label16);
            grpCombat.Controls.Add(label17);
            grpCombat.ForeColor = System.Drawing.Color.Gainsboro;
            grpCombat.Location = new System.Drawing.Point(253, 3);
            grpCombat.Margin = new Padding(4, 3, 4, 3);
            grpCombat.Name = "grpCombat";
            grpCombat.Padding = new Padding(4, 3, 4, 3);
            grpCombat.Size = new Size(298, 495);
            grpCombat.TabIndex = 18;
            grpCombat.TabStop = false;
            grpCombat.Text = "Combat";
            // 
            // grpAttackSpeed
            // 
            grpAttackSpeed.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpAttackSpeed.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpAttackSpeed.Controls.Add(darkNumericUpDown6);
            grpAttackSpeed.Controls.Add(label9);
            grpAttackSpeed.Controls.Add(darkComboBox3);
            grpAttackSpeed.Controls.Add(label10);
            grpAttackSpeed.ForeColor = System.Drawing.Color.Gainsboro;
            grpAttackSpeed.Location = new System.Drawing.Point(15, 383);
            grpAttackSpeed.Margin = new Padding(4, 3, 4, 3);
            grpAttackSpeed.Name = "grpAttackSpeed";
            grpAttackSpeed.Padding = new Padding(4, 3, 4, 3);
            grpAttackSpeed.Size = new Size(275, 104);
            grpAttackSpeed.TabIndex = 64;
            grpAttackSpeed.TabStop = false;
            grpAttackSpeed.Text = "Attack Speed";
            // 
            // darkNumericUpDown6
            // 
            darkNumericUpDown6.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown6.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown6.Location = new System.Drawing.Point(70, 67);
            darkNumericUpDown6.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown6.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            darkNumericUpDown6.Name = "darkNumericUpDown6";
            darkNumericUpDown6.Size = new Size(184, 23);
            darkNumericUpDown6.TabIndex = 56;
            darkNumericUpDown6.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(11, 69);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(38, 15);
            label9.TabIndex = 29;
            label9.Text = "Value:";
            // 
            // darkComboBox3
            // 
            darkComboBox3.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox3.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox3.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox3.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox3.DrawDropdownHoverOutline = false;
            darkComboBox3.DrawFocusRectangle = false;
            darkComboBox3.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox3.FlatStyle = FlatStyle.Flat;
            darkComboBox3.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox3.FormattingEnabled = true;
            darkComboBox3.Location = new System.Drawing.Point(70, 28);
            darkComboBox3.Margin = new Padding(4, 3, 4, 3);
            darkComboBox3.Name = "darkComboBox3";
            darkComboBox3.Size = new Size(184, 24);
            darkComboBox3.TabIndex = 28;
            darkComboBox3.Text = null;
            darkComboBox3.TextPadding = new Padding(2);
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(11, 31);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(55, 15);
            label10.TabIndex = 0;
            label10.Text = "Modifier:";
            // 
            // darkNumericUpDown7
            // 
            darkNumericUpDown7.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown7.DecimalPlaces = 2;
            darkNumericUpDown7.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown7.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            darkNumericUpDown7.Location = new System.Drawing.Point(15, 135);
            darkNumericUpDown7.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown7.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            darkNumericUpDown7.Name = "darkNumericUpDown7";
            darkNumericUpDown7.Size = new Size(275, 23);
            darkNumericUpDown7.TabIndex = 63;
            darkNumericUpDown7.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(13, 119);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(156, 15);
            label11.TabIndex = 62;
            label11.Text = "Crit Multiplier (Default 1.5x):";
            // 
            // darkNumericUpDown8
            // 
            darkNumericUpDown8.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown8.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown8.Location = new System.Drawing.Point(14, 297);
            darkNumericUpDown8.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown8.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown8.Name = "darkNumericUpDown8";
            darkNumericUpDown8.Size = new Size(276, 23);
            darkNumericUpDown8.TabIndex = 61;
            darkNumericUpDown8.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown9
            // 
            darkNumericUpDown9.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown9.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown9.Location = new System.Drawing.Point(14, 39);
            darkNumericUpDown9.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown9.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            darkNumericUpDown9.Name = "darkNumericUpDown9";
            darkNumericUpDown9.Size = new Size(276, 23);
            darkNumericUpDown9.TabIndex = 60;
            darkNumericUpDown9.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown10
            // 
            darkNumericUpDown10.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown10.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown10.Location = new System.Drawing.Point(15, 87);
            darkNumericUpDown10.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown10.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            darkNumericUpDown10.Name = "darkNumericUpDown10";
            darkNumericUpDown10.Size = new Size(275, 23);
            darkNumericUpDown10.TabIndex = 59;
            darkNumericUpDown10.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkComboBox4
            // 
            darkComboBox4.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox4.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox4.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox4.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox4.DrawDropdownHoverOutline = false;
            darkComboBox4.DrawFocusRectangle = false;
            darkComboBox4.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox4.FlatStyle = FlatStyle.Flat;
            darkComboBox4.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox4.FormattingEnabled = true;
            darkComboBox4.Location = new System.Drawing.Point(15, 240);
            darkComboBox4.Margin = new Padding(4, 3, 4, 3);
            darkComboBox4.Name = "darkComboBox4";
            darkComboBox4.Size = new Size(275, 24);
            darkComboBox4.TabIndex = 58;
            darkComboBox4.Text = null;
            darkComboBox4.TextPadding = new Padding(2);
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(13, 220);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(71, 15);
            label12.TabIndex = 57;
            label12.Text = "Scaling Stat:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(11, 273);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(95, 15);
            label13.TabIndex = 56;
            label13.Text = "Scaling Amount:";
            // 
            // darkComboBox5
            // 
            darkComboBox5.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox5.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox5.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox5.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox5.DrawDropdownHoverOutline = false;
            darkComboBox5.DrawFocusRectangle = false;
            darkComboBox5.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox5.FlatStyle = FlatStyle.Flat;
            darkComboBox5.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox5.FormattingEnabled = true;
            darkComboBox5.Items.AddRange(new object[] { "Physical", "Magic", "True" });
            darkComboBox5.Location = new System.Drawing.Point(14, 187);
            darkComboBox5.Margin = new Padding(4, 3, 4, 3);
            darkComboBox5.Name = "darkComboBox5";
            darkComboBox5.Size = new Size(275, 24);
            darkComboBox5.TabIndex = 54;
            darkComboBox5.Text = "Physical";
            darkComboBox5.TextPadding = new Padding(2);
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(11, 167);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(81, 15);
            label14.TabIndex = 53;
            label14.Text = "Damage Type:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(11, 72);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(93, 15);
            label15.TabIndex = 52;
            label15.Text = "Crit Chance (%):";
            // 
            // darkComboBox6
            // 
            darkComboBox6.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox6.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox6.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox6.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox6.DrawDropdownHoverOutline = false;
            darkComboBox6.DrawFocusRectangle = false;
            darkComboBox6.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox6.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox6.FlatStyle = FlatStyle.Flat;
            darkComboBox6.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox6.FormattingEnabled = true;
            darkComboBox6.Location = new System.Drawing.Point(14, 346);
            darkComboBox6.Margin = new Padding(4, 3, 4, 3);
            darkComboBox6.Name = "darkComboBox6";
            darkComboBox6.Size = new Size(276, 24);
            darkComboBox6.TabIndex = 50;
            darkComboBox6.Text = null;
            darkComboBox6.TextPadding = new Padding(2);
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(11, 329);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(103, 15);
            label16.TabIndex = 49;
            label16.Text = "Attack Animation:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(11, 21);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(81, 15);
            label17.TabIndex = 48;
            label17.Text = "Base Damage:";
            // 
            // grpRegen
            // 
            grpRegen.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpRegen.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpRegen.Controls.Add(darkNumericUpDown11);
            grpRegen.Controls.Add(darkNumericUpDown12);
            grpRegen.Controls.Add(label18);
            grpRegen.Controls.Add(label19);
            grpRegen.Controls.Add(label20);
            grpRegen.ForeColor = System.Drawing.Color.Gainsboro;
            grpRegen.Location = new System.Drawing.Point(253, 500);
            grpRegen.Margin = new Padding(2);
            grpRegen.Name = "grpRegen";
            grpRegen.Padding = new Padding(2);
            grpRegen.Size = new Size(298, 136);
            grpRegen.TabIndex = 33;
            grpRegen.TabStop = false;
            grpRegen.Text = "Regen";
            // 
            // darkNumericUpDown11
            // 
            darkNumericUpDown11.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown11.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown11.Location = new System.Drawing.Point(159, 44);
            darkNumericUpDown11.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown11.Name = "darkNumericUpDown11";
            darkNumericUpDown11.Size = new Size(117, 23);
            darkNumericUpDown11.TabIndex = 31;
            darkNumericUpDown11.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // darkNumericUpDown12
            // 
            darkNumericUpDown12.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkNumericUpDown12.ForeColor = System.Drawing.Color.Gainsboro;
            darkNumericUpDown12.Location = new System.Drawing.Point(13, 44);
            darkNumericUpDown12.Margin = new Padding(4, 3, 4, 3);
            darkNumericUpDown12.Name = "darkNumericUpDown12";
            darkNumericUpDown12.Size = new Size(124, 23);
            darkNumericUpDown12.TabIndex = 30;
            darkNumericUpDown12.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(8, 21);
            label18.Margin = new Padding(2, 0, 2, 0);
            label18.Name = "label18";
            label18.Size = new Size(47, 15);
            label18.TabIndex = 26;
            label18.Text = "HP: (%)";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(154, 21);
            label19.Margin = new Padding(2, 0, 2, 0);
            label19.Name = "label19";
            label19.Size = new Size(61, 15);
            label19.TabIndex = 27;
            label19.Text = "Mana: (%)";
            // 
            // label20
            // 
            label20.Location = new System.Drawing.Point(7, 77);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(280, 51);
            label20.TabIndex = 0;
            label20.Text = "% of HP/Mana to restore per tick.\r\n\r\nTick timer saved in server config.json.";
            // 
            // grpSpells
            // 
            grpSpells.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSpells.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSpells.Controls.Add(darkComboBox7);
            grpSpells.Controls.Add(cmbFreq);
            grpSpells.Controls.Add(lblFreq);
            grpSpells.Controls.Add(lblSpell);
            grpSpells.Controls.Add(darkButton1);
            grpSpells.Controls.Add(darkButton2);
            grpSpells.Controls.Add(listBox1);
            grpSpells.ForeColor = System.Drawing.Color.Gainsboro;
            grpSpells.Location = new System.Drawing.Point(559, 8);
            grpSpells.Margin = new Padding(4, 3, 4, 3);
            grpSpells.Name = "grpSpells";
            grpSpells.Padding = new Padding(4, 3, 4, 3);
            grpSpells.Size = new Size(266, 249);
            grpSpells.TabIndex = 32;
            grpSpells.TabStop = false;
            grpSpells.Text = "Spells";
            // 
            // darkComboBox7
            // 
            darkComboBox7.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkComboBox7.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            darkComboBox7.BorderStyle = ButtonBorderStyle.Solid;
            darkComboBox7.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            darkComboBox7.DrawDropdownHoverOutline = false;
            darkComboBox7.DrawFocusRectangle = false;
            darkComboBox7.DrawMode = DrawMode.OwnerDrawFixed;
            darkComboBox7.DropDownStyle = ComboBoxStyle.DropDownList;
            darkComboBox7.FlatStyle = FlatStyle.Flat;
            darkComboBox7.ForeColor = System.Drawing.Color.Gainsboro;
            darkComboBox7.FormattingEnabled = true;
            darkComboBox7.Location = new System.Drawing.Point(18, 144);
            darkComboBox7.Margin = new Padding(4, 3, 4, 3);
            darkComboBox7.Name = "darkComboBox7";
            darkComboBox7.Size = new Size(236, 24);
            darkComboBox7.TabIndex = 43;
            darkComboBox7.Text = null;
            darkComboBox7.TextPadding = new Padding(2);
            // 
            // cmbFreq
            // 
            cmbFreq.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbFreq.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbFreq.BorderStyle = ButtonBorderStyle.Solid;
            cmbFreq.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbFreq.DrawDropdownHoverOutline = false;
            cmbFreq.DrawFocusRectangle = false;
            cmbFreq.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFreq.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFreq.FlatStyle = FlatStyle.Flat;
            cmbFreq.ForeColor = System.Drawing.Color.Gainsboro;
            cmbFreq.FormattingEnabled = true;
            cmbFreq.Items.AddRange(new object[] { "Not Very Often", "Not Often", "Normal", "Often", "Very Often" });
            cmbFreq.Location = new System.Drawing.Point(92, 216);
            cmbFreq.Margin = new Padding(4, 3, 4, 3);
            cmbFreq.Name = "cmbFreq";
            cmbFreq.Size = new Size(162, 24);
            cmbFreq.TabIndex = 42;
            cmbFreq.Text = "Not Very Often";
            cmbFreq.TextPadding = new Padding(2);
            // 
            // lblFreq
            // 
            lblFreq.AutoSize = true;
            lblFreq.Location = new System.Drawing.Point(15, 219);
            lblFreq.Margin = new Padding(4, 0, 4, 0);
            lblFreq.Name = "lblFreq";
            lblFreq.Size = new Size(65, 15);
            lblFreq.TabIndex = 41;
            lblFreq.Text = "Frequence:";
            // 
            // lblSpell
            // 
            lblSpell.AutoSize = true;
            lblSpell.Location = new System.Drawing.Point(17, 123);
            lblSpell.Margin = new Padding(4, 0, 4, 0);
            lblSpell.Name = "lblSpell";
            lblSpell.Size = new Size(35, 15);
            lblSpell.TabIndex = 39;
            lblSpell.Text = "Spell:";
            // 
            // darkButton1
            // 
            darkButton1.Location = new System.Drawing.Point(167, 178);
            darkButton1.Margin = new Padding(4, 3, 4, 3);
            darkButton1.Name = "darkButton1";
            darkButton1.Padding = new Padding(6);
            darkButton1.Size = new Size(88, 27);
            darkButton1.TabIndex = 38;
            darkButton1.Text = "Remove";
            // 
            // darkButton2
            // 
            darkButton2.Location = new System.Drawing.Point(18, 178);
            darkButton2.Margin = new Padding(4, 3, 4, 3);
            darkButton2.Name = "darkButton2";
            darkButton2.Padding = new Padding(6);
            darkButton2.Size = new Size(88, 27);
            darkButton2.TabIndex = 37;
            darkButton2.Text = "Add";
            // 
            // listBox1
            // 
            listBox1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.ForeColor = System.Drawing.Color.Gainsboro;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(18, 22);
            listBox1.Margin = new Padding(4, 3, 4, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(236, 92);
            listBox1.TabIndex = 29;
            // 
            // grpPets
            // 
            grpPets.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpPets.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpPets.Controls.Add(darkButton3);
            grpPets.Controls.Add(darkTextBox2);
            grpPets.Controls.Add(gameObjectList1);
            grpPets.ForeColor = System.Drawing.Color.Gainsboro;
            grpPets.Location = new System.Drawing.Point(3, 33);
            grpPets.Margin = new Padding(4, 3, 4, 3);
            grpPets.Name = "grpPets";
            grpPets.Padding = new Padding(4, 3, 4, 3);
            grpPets.Size = new Size(233, 643);
            grpPets.TabIndex = 47;
            grpPets.TabStop = false;
            grpPets.Text = "PETs";
            // 
            // darkButton3
            // 
            darkButton3.Location = new System.Drawing.Point(204, 22);
            darkButton3.Margin = new Padding(4, 3, 4, 3);
            darkButton3.Name = "darkButton3";
            darkButton3.Padding = new Padding(6);
            darkButton3.Size = new Size(21, 23);
            darkButton3.TabIndex = 34;
            darkButton3.Text = "X";
            // 
            // darkTextBox2
            // 
            darkTextBox2.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            darkTextBox2.BorderStyle = BorderStyle.FixedSingle;
            darkTextBox2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkTextBox2.Location = new System.Drawing.Point(6, 22);
            darkTextBox2.Margin = new Padding(4, 3, 4, 3);
            darkTextBox2.Name = "darkTextBox2";
            darkTextBox2.Size = new Size(192, 23);
            darkTextBox2.TabIndex = 33;
            darkTextBox2.Text = "Search...";
            // 
            // gameObjectList1
            // 
            gameObjectList1.AllowDrop = true;
            gameObjectList1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            gameObjectList1.BorderStyle = BorderStyle.None;
            gameObjectList1.ForeColor = System.Drawing.Color.Gainsboro;
            gameObjectList1.HideSelection = false;
            gameObjectList1.ImageIndex = 0;
            gameObjectList1.LineColor = System.Drawing.Color.FromArgb(150, 150, 150);
            gameObjectList1.Location = new System.Drawing.Point(6, 53);
            gameObjectList1.Margin = new Padding(4, 3, 4, 3);
            gameObjectList1.Name = "gameObjectList1";
            gameObjectList1.SelectedImageIndex = 0;
            gameObjectList1.Size = new Size(222, 558);
            gameObjectList1.TabIndex = 32;
            // 
            // grpBehavior
            // 
            grpBehavior.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpBehavior.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpBehavior.Controls.Add(nudResetRadius);
            grpBehavior.Controls.Add(lblResetRadius);
            grpBehavior.Controls.Add(chkFocusDamageDealer);
            grpBehavior.Controls.Add(nudFlee);
            grpBehavior.Controls.Add(lblFlee);
            grpBehavior.Controls.Add(nudSightRange);
            grpBehavior.Controls.Add(lblSightRange);
            grpBehavior.ForeColor = System.Drawing.Color.Gainsboro;
            grpBehavior.Location = new System.Drawing.Point(559, 269);
            grpBehavior.Margin = new Padding(4, 3, 4, 3);
            grpBehavior.Name = "grpBehavior";
            grpBehavior.Padding = new Padding(4, 3, 4, 3);
            grpBehavior.Size = new Size(266, 367);
            grpBehavior.TabIndex = 34;
            grpBehavior.TabStop = false;
            grpBehavior.Text = "Behavior:";
            // 
            // nudResetRadius
            // 
            nudResetRadius.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudResetRadius.ForeColor = System.Drawing.Color.Gainsboro;
            nudResetRadius.Location = new System.Drawing.Point(119, 113);
            nudResetRadius.Margin = new Padding(4, 3, 4, 3);
            nudResetRadius.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudResetRadius.Name = "nudResetRadius";
            nudResetRadius.Size = new Size(135, 23);
            nudResetRadius.TabIndex = 76;
            nudResetRadius.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            // 
            // lblResetRadius
            // 
            lblResetRadius.AutoSize = true;
            lblResetRadius.Location = new System.Drawing.Point(13, 115);
            lblResetRadius.Margin = new Padding(4, 0, 4, 0);
            lblResetRadius.Name = "lblResetRadius";
            lblResetRadius.Size = new Size(76, 15);
            lblResetRadius.TabIndex = 75;
            lblResetRadius.Text = "Reset Radius:";
            // 
            // chkFocusDamageDealer
            // 
            chkFocusDamageDealer.AutoSize = true;
            chkFocusDamageDealer.Location = new System.Drawing.Point(16, 48);
            chkFocusDamageDealer.Margin = new Padding(4, 3, 4, 3);
            chkFocusDamageDealer.Name = "chkFocusDamageDealer";
            chkFocusDamageDealer.Size = new Size(187, 19);
            chkFocusDamageDealer.TabIndex = 71;
            chkFocusDamageDealer.Text = "Focus Highest Damage Dealer:";
            // 
            // nudFlee
            // 
            nudFlee.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudFlee.ForeColor = System.Drawing.Color.Gainsboro;
            nudFlee.Location = new System.Drawing.Point(119, 146);
            nudFlee.Margin = new Padding(4, 3, 4, 3);
            nudFlee.Name = "nudFlee";
            nudFlee.Size = new Size(135, 23);
            nudFlee.TabIndex = 70;
            nudFlee.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblFlee
            // 
            lblFlee.AutoSize = true;
            lblFlee.Location = new System.Drawing.Point(13, 148);
            lblFlee.Margin = new Padding(4, 0, 4, 0);
            lblFlee.Name = "lblFlee";
            lblFlee.Size = new Size(82, 15);
            lblFlee.TabIndex = 69;
            lblFlee.Text = "Flee Health %:";
            // 
            // nudSightRange
            // 
            nudSightRange.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSightRange.ForeColor = System.Drawing.Color.Gainsboro;
            nudSightRange.Location = new System.Drawing.Point(119, 80);
            nudSightRange.Margin = new Padding(4, 3, 4, 3);
            nudSightRange.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudSightRange.Name = "nudSightRange";
            nudSightRange.Size = new Size(135, 23);
            nudSightRange.TabIndex = 62;
            nudSightRange.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lblSightRange
            // 
            lblSightRange.AutoSize = true;
            lblSightRange.Location = new System.Drawing.Point(13, 82);
            lblSightRange.Margin = new Padding(4, 0, 4, 0);
            lblSightRange.Name = "lblSightRange";
            lblSightRange.Size = new Size(73, 15);
            lblSightRange.TabIndex = 12;
            lblSightRange.Text = "Sight Range:";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(831, 606);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(222, 31);
            btnCancel.TabIndex = 49;
            btnCancel.Text = "Cancel";
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(601, 606);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6);
            btnSave.Size = new Size(222, 31);
            btnSave.TabIndex = 48;
            btnSave.Text = "Save";
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
            toolStrip.Size = new Size(1088, 29);
            toolStrip.TabIndex = 50;
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
            // 
            // toolStripItemPaste
            // 
            toolStripItemPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemPaste.Enabled = false;
            toolStripItemPaste.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemPaste.Image = (Image)resources.GetObject("toolStripItemPaste.Image");
            toolStripItemPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemPaste.Name = "toolStripItemPaste";
            toolStripItemPaste.Size = new Size(23, 26);
            toolStripItemPaste.Text = "Paste";
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
            // 
            // FrmPet
            // 
            BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1088, 650);
            Controls.Add(toolStrip);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpPets);
            Controls.Add(pnlContainer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmPet";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pet Editor";
            pnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPet).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredMaturity).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredEnergy).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRequiredMood).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxBreedCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStr).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMag).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDef).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMR).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCritChance).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCritMultiplier).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudScaling).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAttackSpeedValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHpRegen).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMpRegen).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCombatRange).EndInit();
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRgbaA).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaB).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaG).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRgbaR).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNpc).EndInit();
            grpStats.ResumeLayout(false);
            grpStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMana).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHp).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWis).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown4).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown5).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudARP).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVit).EndInit();
            grpCombat.ResumeLayout(false);
            grpCombat.PerformLayout();
            grpAttackSpeed.ResumeLayout(false);
            grpAttackSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown6).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown7).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown8).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown9).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown10).EndInit();
            grpRegen.ResumeLayout(false);
            grpRegen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown11).EndInit();
            ((System.ComponentModel.ISupportInitialize)darkNumericUpDown12).EndInit();
            grpSpells.ResumeLayout(false);
            grpSpells.PerformLayout();
            grpPets.ResumeLayout(false);
            grpPets.PerformLayout();
            grpBehavior.ResumeLayout(false);
            grpBehavior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudResetRadius).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFlee).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSightRange).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private DarkUI.Controls.DarkGroupBox grpGeneral;
        private Label lblAlpha;
        private Label lblBlue;
        private Label lblGreen;
        private Label lblRed;
        private DarkUI.Controls.DarkNumericUpDown nudRgbaA;
        private DarkUI.Controls.DarkNumericUpDown nudRgbaB;
        private DarkUI.Controls.DarkNumericUpDown nudRgbaG;
        private DarkUI.Controls.DarkNumericUpDown nudRgbaR;
        private DarkUI.Controls.DarkButton btnAddFolder;
        private Label label1;
        private DarkUI.Controls.DarkComboBox darkComboBox1;
        private DarkUI.Controls.DarkComboBox darkComboBox2;
        private Label label2;
        private PictureBox picNpc;
        private Label label3;
        private DarkUI.Controls.DarkTextBox darkTextBox1;
        private DarkUI.Controls.DarkGroupBox grpStats;
        private DarkUI.Controls.DarkNumericUpDown nudMana;
        private DarkUI.Controls.DarkNumericUpDown nudHp;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown1;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown2;
        private DarkUI.Controls.DarkNumericUpDown nudWis;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown3;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown4;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown5;
        private DarkUI.Controls.DarkNumericUpDown nudARP;
        private DarkUI.Controls.DarkNumericUpDown nudVit;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label lblARP;
        private Label lblVit;
        private Label lblWis;
        private Label lblMana;
        private Label lblHP;
        private DarkUI.Controls.DarkGroupBox grpCombat;
        private DarkUI.Controls.DarkGroupBox grpAttackSpeed;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown6;
        private Label label9;
        private DarkUI.Controls.DarkComboBox darkComboBox3;
        private Label label10;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown7;
        private Label label11;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown8;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown9;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown10;
        private DarkUI.Controls.DarkComboBox darkComboBox4;
        private Label label12;
        private Label label13;
        private DarkUI.Controls.DarkComboBox darkComboBox5;
        private Label label14;
        private Label label15;
        private DarkUI.Controls.DarkComboBox darkComboBox6;
        private Label label16;
        private Label label17;
        private DarkUI.Controls.DarkGroupBox grpRegen;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown11;
        private DarkUI.Controls.DarkNumericUpDown darkNumericUpDown12;
        private Label label18;
        private Label label19;
        private Label label20;
        private DarkUI.Controls.DarkGroupBox grpSpells;
        private DarkUI.Controls.DarkComboBox darkComboBox7;
        private DarkUI.Controls.DarkComboBox cmbFreq;
        private Label lblFreq;
        private Label lblSpell;
        private DarkUI.Controls.DarkButton darkButton1;
        private DarkUI.Controls.DarkButton darkButton2;
        private ListBox listBox1;
        private DarkUI.Controls.DarkGroupBox grpPets;
        private DarkUI.Controls.DarkButton darkButton3;
        private DarkUI.Controls.DarkTextBox darkTextBox2;
        private Controls.GameObjectList gameObjectList1;
        private DarkUI.Controls.DarkGroupBox grpBehavior;
        private DarkUI.Controls.DarkNumericUpDown nudResetRadius;
        private Label lblResetRadius;
        private DarkUI.Controls.DarkCheckBox chkFocusDamageDealer;
        private DarkUI.Controls.DarkNumericUpDown nudFlee;
        private Label lblFlee;
        private DarkUI.Controls.DarkNumericUpDown nudSightRange;
        private Label lblSightRange;
        private DarkUI.Controls.DarkButton btnCancel;
        private DarkUI.Controls.DarkButton btnSave;
        private DarkUI.Controls.DarkToolStrip toolStrip;
        private ToolStripButton toolStripItemNew;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripItemDelete;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnAlphabetical;
        private ToolStripSeparator toolStripSeparator4;
        public ToolStripButton toolStripItemCopy;
        public ToolStripButton toolStripItemPaste;
        private ToolStripSeparator toolStripSeparator3;
        public ToolStripButton toolStripItemUndo;
    }
}
