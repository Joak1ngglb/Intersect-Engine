using DarkUI.Controls;

namespace Intersect.Editor.Forms.Editors
{
    partial class FrmCrafts
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCrafts));
            btnCancel = new DarkButton();
            btnSave = new DarkButton();
            grpCrafts = new DarkGroupBox();
            btnClearSearch = new DarkButton();
            txtSearch = new DarkTextBox();
            lstGameObjects = new Controls.GameObjectList();
            pnlContainer = new Panel();
            grpGeneral = new DarkGroupBox();
            btnCraftRequirements = new DarkButton();
            nudItemLossChance = new DarkNumericUpDown();
            lblItemLossChance = new Label();
            nudFailureChance = new DarkNumericUpDown();
            lblFailureChance = new Label();
            btnAddFolder = new DarkButton();
            lblFolder = new Label();
            cmbFolder = new DarkComboBox();
            nudCraftQuantity = new DarkNumericUpDown();
            lblCraftQuantity = new Label();
            nudSpeed = new DarkNumericUpDown();
            cmbResult = new DarkComboBox();
            lblItem = new Label();
            lblName = new Label();
            txtName = new DarkTextBox();
            lblSpeed = new Label();
            grpIngredients = new DarkGroupBox();
            GrpExp = new DarkGroupBox();
            cmbJobType = new DarkComboBox();
            label2 = new Label();
            NudExpAmount = new DarkNumericUpDown();
            label1 = new Label();
            cmbEvent = new DarkComboBox();
            lblCommonEvent = new Label();
            nudQuantity = new DarkNumericUpDown();
            cmbIngredient = new DarkComboBox();
            btnDupIngredient = new DarkButton();
            btnRemove = new DarkButton();
            btnAdd = new DarkButton();
            lblIngredient = new Label();
            lstIngredients = new ListBox();
            lblQuantity = new Label();
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
            grpCrafts.SuspendLayout();
            pnlContainer.SuspendLayout();
            grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudItemLossChance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFailureChance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCraftQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            grpIngredients.SuspendLayout();
            GrpExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NudExpAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantity).BeginInit();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(719, 487);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(201, 31);
            btnCancel.TabIndex = 24;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(514, 487);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6);
            btnSave.Size = new Size(197, 31);
            btnSave.TabIndex = 23;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // grpCrafts
            // 
            grpCrafts.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpCrafts.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpCrafts.Controls.Add(btnClearSearch);
            grpCrafts.Controls.Add(txtSearch);
            grpCrafts.Controls.Add(lstGameObjects);
            grpCrafts.ForeColor = System.Drawing.Color.Gainsboro;
            grpCrafts.Location = new System.Drawing.Point(14, 42);
            grpCrafts.Margin = new Padding(4, 3, 4, 3);
            grpCrafts.Name = "grpCrafts";
            grpCrafts.Padding = new Padding(4, 3, 4, 3);
            grpCrafts.Size = new Size(237, 435);
            grpCrafts.TabIndex = 22;
            grpCrafts.TabStop = false;
            grpCrafts.Text = "Crafts";
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new System.Drawing.Point(209, 15);
            btnClearSearch.Margin = new Padding(4, 3, 4, 3);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Padding = new Padding(6);
            btnClearSearch.Size = new Size(21, 23);
            btnClearSearch.TabIndex = 28;
            btnClearSearch.Text = "X";
            btnClearSearch.Click += btnClearSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtSearch.Location = new System.Drawing.Point(7, 15);
            txtSearch.Margin = new Padding(4, 3, 4, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(194, 23);
            txtSearch.TabIndex = 27;
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
            lstGameObjects.Location = new System.Drawing.Point(7, 45);
            lstGameObjects.Margin = new Padding(4, 3, 4, 3);
            lstGameObjects.Name = "lstGameObjects";
            lstGameObjects.SelectedImageIndex = 0;
            lstGameObjects.Size = new Size(223, 377);
            lstGameObjects.TabIndex = 26;
            // 
            // pnlContainer
            // 
            pnlContainer.Controls.Add(grpGeneral);
            pnlContainer.Controls.Add(grpIngredients);
            pnlContainer.Location = new System.Drawing.Point(258, 42);
            pnlContainer.Margin = new Padding(4, 3, 4, 3);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(662, 435);
            pnlContainer.TabIndex = 31;
            pnlContainer.Visible = false;
            // 
            // grpGeneral
            // 
            grpGeneral.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpGeneral.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpGeneral.Controls.Add(GrpExp);
            grpGeneral.Controls.Add(btnCraftRequirements);
            grpGeneral.Controls.Add(nudItemLossChance);
            grpGeneral.Controls.Add(lblItemLossChance);
            grpGeneral.Controls.Add(nudFailureChance);
            grpGeneral.Controls.Add(lblFailureChance);
            grpGeneral.Controls.Add(btnAddFolder);
            grpGeneral.Controls.Add(lblFolder);
            grpGeneral.Controls.Add(cmbFolder);
            grpGeneral.Controls.Add(nudCraftQuantity);
            grpGeneral.Controls.Add(lblCraftQuantity);
            grpGeneral.Controls.Add(nudSpeed);
            grpGeneral.Controls.Add(cmbResult);
            grpGeneral.Controls.Add(lblItem);
            grpGeneral.Controls.Add(lblName);
            grpGeneral.Controls.Add(txtName);
            grpGeneral.Controls.Add(lblSpeed);
            grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            grpGeneral.Location = new System.Drawing.Point(6, 5);
            grpGeneral.Margin = new Padding(4, 3, 4, 3);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Padding = new Padding(4, 3, 4, 3);
            grpGeneral.Size = new Size(318, 418);
            grpGeneral.TabIndex = 31;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "General";
            // 
            // btnCraftRequirements
            // 
            btnCraftRequirements.Location = new System.Drawing.Point(10, 279);
            btnCraftRequirements.Margin = new Padding(4, 3, 4, 3);
            btnCraftRequirements.Name = "btnCraftRequirements";
            btnCraftRequirements.Padding = new Padding(6);
            btnCraftRequirements.Size = new Size(296, 27);
            btnCraftRequirements.TabIndex = 44;
            btnCraftRequirements.Text = "Craft Requirements";
            btnCraftRequirements.Click += btnCraftRequirements_Click;
            // 
            // nudItemLossChance
            // 
            nudItemLossChance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudItemLossChance.ForeColor = System.Drawing.Color.Gainsboro;
            nudItemLossChance.Location = new System.Drawing.Point(98, 242);
            nudItemLossChance.Margin = new Padding(4, 3, 4, 3);
            nudItemLossChance.Name = "nudItemLossChance";
            nudItemLossChance.Size = new Size(209, 23);
            nudItemLossChance.TabIndex = 50;
            nudItemLossChance.Value = new decimal(new int[] { 100, 0, 0, 0 });
            nudItemLossChance.ValueChanged += nudItemLossChance_ValueChanged;
            // 
            // lblItemLossChance
            // 
            lblItemLossChance.AutoSize = true;
            lblItemLossChance.Location = new System.Drawing.Point(7, 245);
            lblItemLossChance.Margin = new Padding(4, 0, 4, 0);
            lblItemLossChance.Name = "lblItemLossChance";
            lblItemLossChance.Size = new Size(81, 15);
            lblItemLossChance.TabIndex = 49;
            lblItemLossChance.Text = "Item Loss (%):";
            // 
            // nudFailureChance
            // 
            nudFailureChance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudFailureChance.ForeColor = System.Drawing.Color.Gainsboro;
            nudFailureChance.Location = new System.Drawing.Point(85, 204);
            nudFailureChance.Margin = new Padding(4, 3, 4, 3);
            nudFailureChance.Name = "nudFailureChance";
            nudFailureChance.Size = new Size(222, 23);
            nudFailureChance.TabIndex = 48;
            nudFailureChance.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudFailureChance.ValueChanged += nudFailureChance_ValueChanged;
            // 
            // lblFailureChance
            // 
            lblFailureChance.AutoSize = true;
            lblFailureChance.Location = new System.Drawing.Point(7, 207);
            lblFailureChance.Margin = new Padding(4, 0, 4, 0);
            lblFailureChance.Name = "lblFailureChance";
            lblFailureChance.Size = new Size(66, 15);
            lblFailureChance.TabIndex = 47;
            lblFailureChance.Text = "Failure (%):";
            // 
            // btnAddFolder
            // 
            btnAddFolder.Location = new System.Drawing.Point(286, 53);
            btnAddFolder.Margin = new Padding(4, 3, 4, 3);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Padding = new Padding(6);
            btnAddFolder.Size = new Size(21, 24);
            btnAddFolder.TabIndex = 46;
            btnAddFolder.Text = "+";
            btnAddFolder.Click += btnAddFolder_Click;
            // 
            // lblFolder
            // 
            lblFolder.AutoSize = true;
            lblFolder.Location = new System.Drawing.Point(7, 58);
            lblFolder.Margin = new Padding(4, 0, 4, 0);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new Size(43, 15);
            lblFolder.TabIndex = 45;
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
            cmbFolder.Location = new System.Drawing.Point(85, 53);
            cmbFolder.Margin = new Padding(4, 3, 4, 3);
            cmbFolder.Name = "cmbFolder";
            cmbFolder.Size = new Size(193, 24);
            cmbFolder.TabIndex = 44;
            cmbFolder.Text = null;
            cmbFolder.TextPadding = new Padding(2);
            cmbFolder.SelectedIndexChanged += cmbFolder_SelectedIndexChanged;
            // 
            // nudCraftQuantity
            // 
            nudCraftQuantity.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudCraftQuantity.ForeColor = System.Drawing.Color.Gainsboro;
            nudCraftQuantity.Location = new System.Drawing.Point(85, 128);
            nudCraftQuantity.Margin = new Padding(4, 3, 4, 3);
            nudCraftQuantity.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            nudCraftQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCraftQuantity.Name = "nudCraftQuantity";
            nudCraftQuantity.Size = new Size(222, 23);
            nudCraftQuantity.TabIndex = 43;
            nudCraftQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudCraftQuantity.ValueChanged += nudCraftQuantity_ValueChanged;
            // 
            // lblCraftQuantity
            // 
            lblCraftQuantity.AutoSize = true;
            lblCraftQuantity.Location = new System.Drawing.Point(7, 130);
            lblCraftQuantity.Margin = new Padding(4, 0, 4, 0);
            lblCraftQuantity.Name = "lblCraftQuantity";
            lblCraftQuantity.Size = new Size(56, 15);
            lblCraftQuantity.TabIndex = 42;
            lblCraftQuantity.Text = "Quantity:";
            // 
            // nudSpeed
            // 
            nudSpeed.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudSpeed.ForeColor = System.Drawing.Color.Gainsboro;
            nudSpeed.Location = new System.Drawing.Point(85, 166);
            nudSpeed.Margin = new Padding(4, 3, 4, 3);
            nudSpeed.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            nudSpeed.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(222, 23);
            nudSpeed.TabIndex = 35;
            nudSpeed.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeed.ValueChanged += nudSpeed_ValueChanged;
            // 
            // cmbResult
            // 
            cmbResult.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbResult.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbResult.BorderStyle = ButtonBorderStyle.Solid;
            cmbResult.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbResult.DrawDropdownHoverOutline = false;
            cmbResult.DrawFocusRectangle = false;
            cmbResult.DrawMode = DrawMode.OwnerDrawFixed;
            cmbResult.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbResult.FlatStyle = FlatStyle.Flat;
            cmbResult.ForeColor = System.Drawing.Color.Gainsboro;
            cmbResult.FormattingEnabled = true;
            cmbResult.Location = new System.Drawing.Point(85, 90);
            cmbResult.Margin = new Padding(4, 3, 4, 3);
            cmbResult.Name = "cmbResult";
            cmbResult.Size = new Size(221, 24);
            cmbResult.TabIndex = 34;
            cmbResult.Text = null;
            cmbResult.TextPadding = new Padding(2);
            cmbResult.SelectedIndexChanged += cmbResult_SelectedIndexChanged;
            // 
            // lblItem
            // 
            lblItem.AutoSize = true;
            lblItem.Location = new System.Drawing.Point(7, 93);
            lblItem.Margin = new Padding(4, 0, 4, 0);
            lblItem.Name = "lblItem";
            lblItem.Size = new Size(34, 15);
            lblItem.TabIndex = 33;
            lblItem.Text = "Item:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(7, 20);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 19;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtName.Location = new System.Drawing.Point(85, 17);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(221, 23);
            txtName.TabIndex = 18;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new System.Drawing.Point(7, 166);
            lblSpeed.Margin = new Padding(4, 0, 4, 0);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(63, 15);
            lblSpeed.TabIndex = 3;
            lblSpeed.Text = "Time (ms):";
            // 
            // grpIngredients
            // 
            grpIngredients.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpIngredients.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpIngredients.Controls.Add(cmbEvent);
            grpIngredients.Controls.Add(lblCommonEvent);
            grpIngredients.Controls.Add(nudQuantity);
            grpIngredients.Controls.Add(cmbIngredient);
            grpIngredients.Controls.Add(btnDupIngredient);
            grpIngredients.Controls.Add(btnRemove);
            grpIngredients.Controls.Add(btnAdd);
            grpIngredients.Controls.Add(lblIngredient);
            grpIngredients.Controls.Add(lstIngredients);
            grpIngredients.Controls.Add(lblQuantity);
            grpIngredients.ForeColor = System.Drawing.Color.Gainsboro;
            grpIngredients.Location = new System.Drawing.Point(331, 3);
            grpIngredients.Margin = new Padding(4, 3, 4, 3);
            grpIngredients.Name = "grpIngredients";
            grpIngredients.Padding = new Padding(4, 3, 4, 3);
            grpIngredients.Size = new Size(318, 419);
            grpIngredients.TabIndex = 30;
            grpIngredients.TabStop = false;
            grpIngredients.Text = "Ingredients";
            // 
            // GrpExp
            // 
            GrpExp.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            GrpExp.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            GrpExp.Controls.Add(cmbJobType);
            GrpExp.Controls.Add(label2);
            GrpExp.Controls.Add(NudExpAmount);
            GrpExp.Controls.Add(label1);
            GrpExp.ForeColor = System.Drawing.Color.Gainsboro;
            GrpExp.Location = new System.Drawing.Point(10, 311);
            GrpExp.Margin = new Padding(2);
            GrpExp.Name = "GrpExp";
            GrpExp.Padding = new Padding(2);
            GrpExp.Size = new Size(296, 68);
            GrpExp.TabIndex = 51;
            GrpExp.TabStop = false;
            GrpExp.Text = "Exp";
            // 
            // cmbJobType
            // 
            cmbJobType.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbJobType.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbJobType.BorderStyle = ButtonBorderStyle.Solid;
            cmbJobType.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbJobType.DrawDropdownHoverOutline = false;
            cmbJobType.DrawFocusRectangle = false;
            cmbJobType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbJobType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJobType.FlatStyle = FlatStyle.Flat;
            cmbJobType.ForeColor = System.Drawing.Color.Gainsboro;
            cmbJobType.FormattingEnabled = true;
            cmbJobType.Location = new System.Drawing.Point(9, 37);
            cmbJobType.Margin = new Padding(4, 3, 4, 3);
            cmbJobType.Name = "cmbJobType";
            cmbJobType.Size = new Size(116, 24);
            cmbJobType.TabIndex = 32;
            cmbJobType.Text = null;
            cmbJobType.TextPadding = new Padding(2);
            cmbJobType.SelectedIndexChanged += cmbJobType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(155, 18);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 31;
            label2.Text = "EXP Amount:";
            // 
            // NudExpAmount
            // 
            NudExpAmount.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            NudExpAmount.ForeColor = System.Drawing.Color.Gainsboro;
            NudExpAmount.Location = new System.Drawing.Point(157, 37);
            NudExpAmount.Margin = new Padding(4, 3, 4, 3);
            NudExpAmount.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            NudExpAmount.Name = "NudExpAmount";
            NudExpAmount.Size = new Size(114, 23);
            NudExpAmount.TabIndex = 30;
            NudExpAmount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            NudExpAmount.ValueChanged += NudExpAmount_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 18);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 26;
            label1.Text = "Job";
            // 
            // cmbEvent
            // 
            cmbEvent.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbEvent.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbEvent.BorderStyle = ButtonBorderStyle.Solid;
            cmbEvent.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbEvent.DrawDropdownHoverOutline = false;
            cmbEvent.DrawFocusRectangle = false;
            cmbEvent.DrawMode = DrawMode.OwnerDrawFixed;
            cmbEvent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEvent.FlatStyle = FlatStyle.Flat;
            cmbEvent.ForeColor = System.Drawing.Color.Gainsboro;
            cmbEvent.FormattingEnabled = true;
            cmbEvent.Location = new System.Drawing.Point(14, 254);
            cmbEvent.Margin = new Padding(4, 3, 4, 3);
            cmbEvent.Name = "cmbEvent";
            cmbEvent.Size = new Size(291, 24);
            cmbEvent.TabIndex = 43;
            cmbEvent.Text = null;
            cmbEvent.TextPadding = new Padding(2);
            cmbEvent.SelectedIndexChanged += cmbEvent_SelectedIndexChanged;
            // 
            // lblCommonEvent
            // 
            lblCommonEvent.AutoSize = true;
            lblCommonEvent.Location = new System.Drawing.Point(9, 234);
            lblCommonEvent.Margin = new Padding(4, 0, 4, 0);
            lblCommonEvent.Name = "lblCommonEvent";
            lblCommonEvent.Size = new Size(93, 15);
            lblCommonEvent.TabIndex = 42;
            lblCommonEvent.Text = "Common Event:";
            // 
            // nudQuantity
            // 
            nudQuantity.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            nudQuantity.ForeColor = System.Drawing.Color.Gainsboro;
            nudQuantity.Location = new System.Drawing.Point(13, 350);
            nudQuantity.Margin = new Padding(4, 3, 4, 3);
            nudQuantity.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            nudQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudQuantity.Name = "nudQuantity";
            nudQuantity.Size = new Size(293, 23);
            nudQuantity.TabIndex = 41;
            nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudQuantity.ValueChanged += nudQuantity_ValueChanged;
            // 
            // cmbIngredient
            // 
            cmbIngredient.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbIngredient.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbIngredient.BorderStyle = ButtonBorderStyle.Solid;
            cmbIngredient.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbIngredient.DrawDropdownHoverOutline = false;
            cmbIngredient.DrawFocusRectangle = false;
            cmbIngredient.DrawMode = DrawMode.OwnerDrawFixed;
            cmbIngredient.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIngredient.FlatStyle = FlatStyle.Flat;
            cmbIngredient.ForeColor = System.Drawing.Color.Gainsboro;
            cmbIngredient.FormattingEnabled = true;
            cmbIngredient.Location = new System.Drawing.Point(14, 301);
            cmbIngredient.Margin = new Padding(4, 3, 4, 3);
            cmbIngredient.Name = "cmbIngredient";
            cmbIngredient.Size = new Size(291, 24);
            cmbIngredient.TabIndex = 40;
            cmbIngredient.Text = null;
            cmbIngredient.TextPadding = new Padding(2);
            cmbIngredient.SelectedIndexChanged += cmbIngredient_SelectedIndexChanged;
            // 
            // btnDupIngredient
            // 
            btnDupIngredient.Location = new System.Drawing.Point(218, 380);
            btnDupIngredient.Margin = new Padding(4, 3, 4, 3);
            btnDupIngredient.Name = "btnDupIngredient";
            btnDupIngredient.Padding = new Padding(6);
            btnDupIngredient.Size = new Size(88, 27);
            btnDupIngredient.TabIndex = 39;
            btnDupIngredient.Text = "Duplicate";
            btnDupIngredient.Click += btnDupIngredient_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(112, 380);
            btnRemove.Margin = new Padding(4, 3, 4, 3);
            btnRemove.Name = "btnRemove";
            btnRemove.Padding = new Padding(6);
            btnRemove.Size = new Size(92, 27);
            btnRemove.TabIndex = 38;
            btnRemove.Text = "Remove";
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(13, 380);
            btnAdd.Margin = new Padding(4, 3, 4, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Padding = new Padding(6);
            btnAdd.Size = new Size(88, 27);
            btnAdd.TabIndex = 37;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // lblIngredient
            // 
            lblIngredient.AutoSize = true;
            lblIngredient.Location = new System.Drawing.Point(9, 282);
            lblIngredient.Margin = new Padding(4, 0, 4, 0);
            lblIngredient.Name = "lblIngredient";
            lblIngredient.Size = new Size(34, 15);
            lblIngredient.TabIndex = 31;
            lblIngredient.Text = "Item:";
            // 
            // lstIngredients
            // 
            lstIngredients.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstIngredients.BorderStyle = BorderStyle.FixedSingle;
            lstIngredients.ForeColor = System.Drawing.Color.Gainsboro;
            lstIngredients.FormattingEnabled = true;
            lstIngredients.ItemHeight = 15;
            lstIngredients.Items.AddRange(new object[] { "Ingredient: None x1" });
            lstIngredients.Location = new System.Drawing.Point(14, 22);
            lstIngredients.Margin = new Padding(4, 3, 4, 3);
            lstIngredients.Name = "lstIngredients";
            lstIngredients.Size = new Size(297, 197);
            lstIngredients.TabIndex = 29;
            lstIngredients.SelectedIndexChanged += lstIngredients_SelectedIndexChanged;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(10, 331);
            lblQuantity.Margin = new Padding(4, 0, 4, 0);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(56, 15);
            lblQuantity.TabIndex = 28;
            lblQuantity.Text = "Quantity:";
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
            toolStrip.Size = new Size(932, 29);
            toolStrip.TabIndex = 43;
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
            // FrmCrafts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            ClientSize = new Size(932, 532);
            ControlBox = false;
            Controls.Add(toolStrip);
            Controls.Add(pnlContainer);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpCrafts);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCrafts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Crafts Editor";
            Load += frmCrafting_Load;
            KeyDown += form_KeyDown;
            grpCrafts.ResumeLayout(false);
            grpCrafts.PerformLayout();
            pnlContainer.ResumeLayout(false);
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudItemLossChance).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFailureChance).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCraftQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            grpIngredients.ResumeLayout(false);
            grpIngredients.PerformLayout();
            GrpExp.ResumeLayout(false);
            GrpExp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NudExpAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantity).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DarkButton btnCancel;
        private DarkButton btnSave;
        private DarkGroupBox grpCrafts;
        private System.Windows.Forms.Panel pnlContainer;
        private DarkGroupBox grpGeneral;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblName;
        private DarkTextBox txtName;
        private System.Windows.Forms.Label lblSpeed;
        private DarkGroupBox grpIngredients;
        private DarkButton btnRemove;
        private DarkButton btnAdd;
        private System.Windows.Forms.Label lblIngredient;
        private System.Windows.Forms.ListBox lstIngredients;
        private System.Windows.Forms.Label lblQuantity;
        private DarkToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripItemNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton toolStripItemCopy;
        public System.Windows.Forms.ToolStripButton toolStripItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton toolStripItemUndo;
        private DarkButton btnDupIngredient;
        private DarkComboBox cmbResult;
        private DarkComboBox cmbIngredient;
        private DarkNumericUpDown nudSpeed;
        private DarkNumericUpDown nudQuantity;
        private DarkNumericUpDown nudCraftQuantity;
        private System.Windows.Forms.Label lblCraftQuantity;
        private DarkButton btnClearSearch;
        private DarkTextBox txtSearch;
        private System.Windows.Forms.ToolStripButton btnAlphabetical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private DarkButton btnAddFolder;
        private System.Windows.Forms.Label lblFolder;
        private DarkComboBox cmbFolder;
        private Controls.GameObjectList lstGameObjects;
        private System.Windows.Forms.Label lblCommonEvent;
        private DarkComboBox cmbEvent;
        private DarkNumericUpDown nudItemLossChance;
        private System.Windows.Forms.Label lblItemLossChance;
        private DarkNumericUpDown nudFailureChance;
        private System.Windows.Forms.Label lblFailureChance;
        private DarkButton btnCraftRequirements;
        private DarkGroupBox GrpExp;
        private DarkComboBox cmbJobType;
        private Label label2;
        private DarkNumericUpDown NudExpAmount;
        private Label label1;
    }
}
