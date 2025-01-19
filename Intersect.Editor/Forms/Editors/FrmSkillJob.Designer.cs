using System.Windows.Forms;

namespace Intersect.Editor.Forms.Editors;

partial class FrmSkillJob
{

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSkillJob));
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
        grpSkilJobs = new DarkUI.Controls.DarkGroupBox();
        btnClearSearch = new DarkUI.Controls.DarkButton();
        txtSearch = new DarkUI.Controls.DarkTextBox();
        lstSkills = new Controls.GameObjectList();
        grpGeneral = new DarkUI.Controls.DarkGroupBox();
        nudRequiredLevel = new DarkUI.Controls.DarkNumericUpDown();
        label1 = new Label();
        nudCost = new DarkUI.Controls.DarkNumericUpDown();
        lblCost = new Label();
        cmbJob = new DarkUI.Controls.DarkComboBox();
        label2 = new Label();
        btnAddFolder = new DarkUI.Controls.DarkButton();
        lblFolder = new Label();
        cmbFolder = new DarkUI.Controls.DarkComboBox();
        lblDesc = new Label();
        txtDescription = new DarkUI.Controls.DarkTextBox();
        picSpell = new PictureBox();
        cmbSprite = new DarkUI.Controls.DarkComboBox();
        lblIcon = new Label();
        lblType = new Label();
        cmbEffectType = new DarkUI.Controls.DarkComboBox();
        lblName = new Label();
        txtName = new DarkUI.Controls.DarkTextBox();
        btnCancel = new DarkUI.Controls.DarkButton();
        btnSave = new DarkUI.Controls.DarkButton();
        toolStrip.SuspendLayout();
        grpSkilJobs.SuspendLayout();
        grpGeneral.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudRequiredLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudCost).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picSpell).BeginInit();
        SuspendLayout();
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
        toolStrip.Size = new Size(685, 29);
        toolStrip.TabIndex = 48;
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
        // grpSkilJobs
        // 
        grpSkilJobs.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        grpSkilJobs.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
        grpSkilJobs.Controls.Add(btnClearSearch);
        grpSkilJobs.Controls.Add(txtSearch);
        grpSkilJobs.Controls.Add(lstSkills);
        grpSkilJobs.ForeColor = System.Drawing.Color.Gainsboro;
        grpSkilJobs.Location = new System.Drawing.Point(0, 32);
        grpSkilJobs.Margin = new Padding(4, 3, 4, 3);
        grpSkilJobs.Name = "grpSkilJobs";
        grpSkilJobs.Padding = new Padding(4, 3, 4, 3);
        grpSkilJobs.Size = new Size(237, 489);
        grpSkilJobs.TabIndex = 49;
        grpSkilJobs.TabStop = false;
        grpSkilJobs.Text = "Skills";
        // 
        // btnClearSearch
        // 
        btnClearSearch.Location = new System.Drawing.Point(209, 22);
        btnClearSearch.Margin = new Padding(4, 3, 4, 3);
        btnClearSearch.Name = "btnClearSearch";
        btnClearSearch.Padding = new Padding(6);
        btnClearSearch.Size = new Size(21, 23);
        btnClearSearch.TabIndex = 34;
        btnClearSearch.Text = "X";
        // 
        // txtSearch
        // 
        txtSearch.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
        txtSearch.Location = new System.Drawing.Point(7, 22);
        txtSearch.Margin = new Padding(4, 3, 4, 3);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(194, 23);
        txtSearch.TabIndex = 33;
        txtSearch.Text = "Search...";
        // 
        // lstSkills
        // 
        lstSkills.AllowDrop = true;
        lstSkills.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
        lstSkills.BorderStyle = BorderStyle.None;
        lstSkills.ForeColor = System.Drawing.Color.Gainsboro;
        lstSkills.HideSelection = false;
        lstSkills.ImageIndex = 0;
        lstSkills.LineColor = System.Drawing.Color.FromArgb(150, 150, 150);
        lstSkills.Location = new System.Drawing.Point(7, 52);
        lstSkills.Margin = new Padding(4, 3, 4, 3);
        lstSkills.Name = "lstSkills";
        lstSkills.SelectedImageIndex = 0;
        lstSkills.Size = new Size(223, 572);
        lstSkills.TabIndex = 32;
        // 
        // grpGeneral
        // 
        grpGeneral.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        grpGeneral.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
        grpGeneral.Controls.Add(nudRequiredLevel);
        grpGeneral.Controls.Add(label1);
        grpGeneral.Controls.Add(nudCost);
        grpGeneral.Controls.Add(lblCost);
        grpGeneral.Controls.Add(cmbJob);
        grpGeneral.Controls.Add(label2);
        grpGeneral.Controls.Add(btnAddFolder);
        grpGeneral.Controls.Add(lblFolder);
        grpGeneral.Controls.Add(cmbFolder);
        grpGeneral.Controls.Add(lblDesc);
        grpGeneral.Controls.Add(txtDescription);
        grpGeneral.Controls.Add(picSpell);
        grpGeneral.Controls.Add(cmbSprite);
        grpGeneral.Controls.Add(lblIcon);
        grpGeneral.Controls.Add(lblType);
        grpGeneral.Controls.Add(cmbEffectType);
        grpGeneral.Controls.Add(lblName);
        grpGeneral.Controls.Add(txtName);
        grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
        grpGeneral.Location = new System.Drawing.Point(245, 32);
        grpGeneral.Margin = new Padding(4, 3, 4, 3);
        grpGeneral.Name = "grpGeneral";
        grpGeneral.Padding = new Padding(4, 3, 4, 3);
        grpGeneral.Size = new Size(315, 448);
        grpGeneral.TabIndex = 50;
        grpGeneral.TabStop = false;
        grpGeneral.Text = "General";
        // 
        // nudRequiredLevel
        // 
        nudRequiredLevel.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        nudRequiredLevel.ForeColor = System.Drawing.Color.Gainsboro;
        nudRequiredLevel.Location = new System.Drawing.Point(70, 243);
        nudRequiredLevel.Margin = new Padding(4, 3, 4, 3);
        nudRequiredLevel.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudRequiredLevel.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
        nudRequiredLevel.Name = "nudRequiredLevel";
        nudRequiredLevel.Size = new Size(233, 23);
        nudRequiredLevel.TabIndex = 65;
        nudRequiredLevel.Value = new decimal(new int[] { 0, 0, 0, 0 });
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(10, 245);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(37, 15);
        label1.TabIndex = 64;
        label1.Text = "Level:";
        // 
        // nudCost
        // 
        nudCost.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        nudCost.ForeColor = System.Drawing.Color.Gainsboro;
        nudCost.Location = new System.Drawing.Point(70, 206);
        nudCost.Margin = new Padding(4, 3, 4, 3);
        nudCost.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudCost.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
        nudCost.Name = "nudCost";
        nudCost.Size = new Size(233, 23);
        nudCost.TabIndex = 63;
        nudCost.Value = new decimal(new int[] { 0, 0, 0, 0 });
        // 
        // lblCost
        // 
        lblCost.AutoSize = true;
        lblCost.Location = new System.Drawing.Point(10, 208);
        lblCost.Margin = new Padding(4, 0, 4, 0);
        lblCost.Name = "lblCost";
        lblCost.Size = new Size(34, 15);
        lblCost.TabIndex = 62;
        lblCost.Text = "Cost:";
        // 
        // cmbJob
        // 
        cmbJob.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        cmbJob.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
        cmbJob.BorderStyle = ButtonBorderStyle.Solid;
        cmbJob.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
        cmbJob.DrawDropdownHoverOutline = false;
        cmbJob.DrawFocusRectangle = false;
        cmbJob.DrawMode = DrawMode.OwnerDrawFixed;
        cmbJob.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbJob.FlatStyle = FlatStyle.Flat;
        cmbJob.ForeColor = System.Drawing.Color.Gainsboro;
        cmbJob.FormattingEnabled = true;
        cmbJob.Location = new System.Drawing.Point(70, 165);
        cmbJob.Margin = new Padding(4, 3, 4, 3);
        cmbJob.Name = "cmbJob";
        cmbJob.Size = new Size(231, 24);
        cmbJob.TabIndex = 61;
        cmbJob.Text = null;
        cmbJob.TextPadding = new Padding(2);
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(10, 168);
        label2.Margin = new Padding(2, 0, 2, 0);
        label2.Name = "label2";
        label2.Size = new Size(25, 15);
        label2.TabIndex = 60;
        label2.Text = "Job";
        // 
        // btnAddFolder
        // 
        btnAddFolder.Location = new System.Drawing.Point(281, 54);
        btnAddFolder.Margin = new Padding(4, 3, 4, 3);
        btnAddFolder.Name = "btnAddFolder";
        btnAddFolder.Padding = new Padding(6);
        btnAddFolder.Size = new Size(21, 24);
        btnAddFolder.TabIndex = 59;
        btnAddFolder.Text = "+";
        // 
        // lblFolder
        // 
        lblFolder.AutoSize = true;
        lblFolder.Location = new System.Drawing.Point(8, 59);
        lblFolder.Margin = new Padding(4, 0, 4, 0);
        lblFolder.Name = "lblFolder";
        lblFolder.Size = new Size(43, 15);
        lblFolder.TabIndex = 58;
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
        cmbFolder.Location = new System.Drawing.Point(70, 55);
        cmbFolder.Margin = new Padding(4, 3, 4, 3);
        cmbFolder.Name = "cmbFolder";
        cmbFolder.Size = new Size(204, 24);
        cmbFolder.TabIndex = 57;
        cmbFolder.Text = null;
        cmbFolder.TextPadding = new Padding(2);
        // 
        // lblDesc
        // 
        lblDesc.AutoSize = true;
        lblDesc.Location = new System.Drawing.Point(8, 304);
        lblDesc.Margin = new Padding(4, 0, 4, 0);
        lblDesc.Name = "lblDesc";
        lblDesc.Size = new Size(98, 15);
        lblDesc.TabIndex = 19;
        lblDesc.Text = "Spell Description:";
        // 
        // txtDescription
        // 
        txtDescription.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        txtDescription.BorderStyle = BorderStyle.FixedSingle;
        txtDescription.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
        txtDescription.Location = new System.Drawing.Point(10, 326);
        txtDescription.Margin = new Padding(4, 3, 4, 3);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(291, 102);
        txtDescription.TabIndex = 18;
        // 
        // picSpell
        // 
        picSpell.BackColor = System.Drawing.Color.Black;
        picSpell.Location = new System.Drawing.Point(265, 121);
        picSpell.Margin = new Padding(4, 3, 4, 3);
        picSpell.Name = "picSpell";
        picSpell.Size = new Size(37, 37);
        picSpell.TabIndex = 4;
        picSpell.TabStop = false;
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
        cmbSprite.Location = new System.Drawing.Point(70, 128);
        cmbSprite.Margin = new Padding(4, 3, 4, 3);
        cmbSprite.Name = "cmbSprite";
        cmbSprite.Size = new Size(187, 24);
        cmbSprite.TabIndex = 11;
        cmbSprite.Text = "None";
        cmbSprite.TextPadding = new Padding(2);
        // 
        // lblIcon
        // 
        lblIcon.AutoSize = true;
        lblIcon.Location = new System.Drawing.Point(8, 132);
        lblIcon.Margin = new Padding(4, 0, 4, 0);
        lblIcon.Name = "lblIcon";
        lblIcon.Size = new Size(33, 15);
        lblIcon.TabIndex = 6;
        lblIcon.Text = "Icon:";
        // 
        // lblType
        // 
        lblType.AutoSize = true;
        lblType.Location = new System.Drawing.Point(8, 93);
        lblType.Margin = new Padding(4, 0, 4, 0);
        lblType.Name = "lblType";
        lblType.Size = new Size(34, 15);
        lblType.TabIndex = 3;
        lblType.Text = "Type:";
        // 
        // cmbEffectType
        // 
        cmbEffectType.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        cmbEffectType.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
        cmbEffectType.BorderStyle = ButtonBorderStyle.Solid;
        cmbEffectType.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
        cmbEffectType.DrawDropdownHoverOutline = false;
        cmbEffectType.DrawFocusRectangle = false;
        cmbEffectType.DrawMode = DrawMode.OwnerDrawFixed;
        cmbEffectType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbEffectType.FlatStyle = FlatStyle.Flat;
        cmbEffectType.ForeColor = System.Drawing.Color.Gainsboro;
        cmbEffectType.FormattingEnabled = true;
        cmbEffectType.Location = new System.Drawing.Point(70, 90);
        cmbEffectType.Margin = new Padding(4, 3, 4, 3);
        cmbEffectType.Name = "cmbEffectType";
        cmbEffectType.Size = new Size(231, 24);
        cmbEffectType.TabIndex = 2;
        cmbEffectType.Text = "Combat Spell";
        cmbEffectType.TextPadding = new Padding(2);
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new System.Drawing.Point(8, 23);
        lblName.Margin = new Padding(4, 0, 4, 0);
        lblName.Name = "lblName";
        lblName.Size = new Size(42, 15);
        lblName.TabIndex = 1;
        lblName.Text = "Name:";
        // 
        // txtName
        // 
        txtName.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
        txtName.BorderStyle = BorderStyle.FixedSingle;
        txtName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
        txtName.Location = new System.Drawing.Point(70, 22);
        txtName.Margin = new Padding(4, 3, 4, 3);
        txtName.Name = "txtName";
        txtName.Size = new Size(232, 23);
        txtName.TabIndex = 0;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new System.Drawing.Point(412, 590);
        btnCancel.Margin = new Padding(4, 3, 4, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Padding = new Padding(6);
        btnCancel.Size = new Size(148, 31);
        btnCancel.TabIndex = 52;
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new System.Drawing.Point(245, 590);
        btnSave.Margin = new Padding(4, 3, 4, 3);
        btnSave.Name = "btnSave";
        btnSave.Padding = new Padding(6);
        btnSave.Size = new Size(148, 31);
        btnSave.TabIndex = 51;
        btnSave.Text = "Save";
        btnSave.Click += btnSave_Click;
        // 
        // FrmSkillJob
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
        ClientSize = new Size(685, 631);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(grpGeneral);
        Controls.Add(grpSkilJobs);
        Controls.Add(toolStrip);
        Margin = new Padding(4, 3, 4, 3);
        Name = "FrmSkillJob";
        Text = "Skill Job Editor";
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        grpSkilJobs.ResumeLayout(false);
        grpSkilJobs.PerformLayout();
        grpGeneral.ResumeLayout(false);
        grpGeneral.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudRequiredLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudCost).EndInit();
        ((System.ComponentModel.ISupportInitialize)picSpell).EndInit();
        ResumeLayout(false);
    }

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
    private DarkUI.Controls.DarkGroupBox grpSkilJobs;
    private DarkUI.Controls.DarkButton btnClearSearch;
    private DarkUI.Controls.DarkTextBox txtSearch;
    private Controls.GameObjectList lstSkills;
    private System.ComponentModel.IContainer components;
    private DarkUI.Controls.DarkGroupBox grpGeneral;
    private DarkUI.Controls.DarkButton btnAddFolder;
    private Label lblFolder;
    private DarkUI.Controls.DarkComboBox cmbFolder;
    private Label lblDesc;
    private DarkUI.Controls.DarkTextBox txtDescription;
    private PictureBox picSpell;
    private DarkUI.Controls.DarkComboBox cmbSprite;
    private Label lblIcon;
    private Label lblType;
    private DarkUI.Controls.DarkComboBox cmbEffectType;
    private Label lblName;
    private DarkUI.Controls.DarkTextBox txtName;
    private DarkUI.Controls.DarkComboBox cmbJob;
    private Label label2;
    private DarkUI.Controls.DarkNumericUpDown nudCost;
    private Label lblCost;
    private DarkUI.Controls.DarkButton btnCancel;

    private DarkUI.Controls.DarkNumericUpDown nudRequiredLevel;
    private Label label1;
    private DarkUI.Controls.DarkButton btnSave;
}
