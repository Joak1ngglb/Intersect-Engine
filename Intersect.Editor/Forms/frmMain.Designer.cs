using System.ComponentModel;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Renderers;
using WeifenLuo.WinFormsUI.Docking;

namespace Intersect.Editor.Forms
{
	partial class FrmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

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
            var resources = new ComponentResourceManager(typeof(FrmMain));
            statusStrip1 = new DarkStatusStrip();
            toolStripLabelCoords = new ToolStripStatusLabel();
            toolStripSeparator7 = new ToolStripSeparator();
            toolStripLabelRevision = new ToolStripStatusLabel();
            toolStripSeparator8 = new ToolStripSeparator();
            toolStripLabelFPS = new ToolStripStatusLabel();
            toolStripSeparator9 = new ToolStripSeparator();
            toolStripLabelDebug = new ToolStripStatusLabel();
            dockLeft = new DockPanel();
            toolStripBtnNewMap = new ToolStripButton();
            toolStripBtnSaveMap = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripBtnCut = new ToolStripButton();
            toolStripBtnCopy = new ToolStripButton();
            toolStripBtnPaste = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripBtnUndo = new ToolStripButton();
            toolStripBtnRedo = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripBtnBrush = new ToolStripButton();
            toolStripBtnSelect = new ToolStripButton();
            toolStripBtnRect = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripBtnFill = new ToolStripButton();
            toolStripBtnErase = new ToolStripButton();
            toolStripBtnDropper = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            toolStripTimeButton = new ToolStripDropDownButton();
            toolStripSeparator6 = new ToolStripSeparator();
            toolStripBtnScreenshot = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            toolStripBtnRun = new ToolStripButton();
            toolStripButtonBug = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            toolStripBtnFlipVertical = new ToolStripButton();
            toolStripBtnFlipHorizontal = new ToolStripButton();
            toolStripSeparator13 = new ToolStripSeparator();
            toolStripButtonQuestion = new ToolStripButton();
            toolStrip1 = new DarkToolStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveMapToolStripMenuItem = new ToolStripMenuItem();
            newMapToolStripMenuItem = new ToolStripMenuItem();
            importMapToolStripMenuItem = new ToolStripMenuItem();
            exportMapToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            fillToolStripMenuItem = new ToolStripMenuItem();
            eraseLayerToolStripMenuItem = new ToolStripMenuItem();
            selectToolStripMenuItem = new ToolStripMenuItem();
            allLayersToolStripMenuItem = new ToolStripMenuItem();
            currentLayerOnlyToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            hideDarknessToolStripMenuItem = new ToolStripMenuItem();
            hideFogToolStripMenuItem = new ToolStripMenuItem();
            hideOverlayToolStripMenuItem = new ToolStripMenuItem();
            hideTilePreviewToolStripMenuItem = new ToolStripMenuItem();
            hideResourcesToolStripMenuItem = new ToolStripMenuItem();
            mapGridToolStripMenuItem = new ToolStripMenuItem();
            hideEventsToolStripMenuItem = new ToolStripMenuItem();
            layersToolStripMenuItem = new ToolStripMenuItem();
            contentEditorsToolStripMenuItem = new ToolStripMenuItem();
            animationEditorToolStripMenuItem = new ToolStripMenuItem();
            classEditorToolStripMenuItem = new ToolStripMenuItem();
            commonEventEditorToolStripMenuItem = new ToolStripMenuItem();
            craftsEditorToolStripMenuItem = new ToolStripMenuItem();
            craftingTableEditorToolStripMenuItem = new ToolStripMenuItem();
            itemEditorToolStripMenuItem = new ToolStripMenuItem();
            npcEditorToolStripMenuItem = new ToolStripMenuItem();
            projectileEditorToolStripMenuItem = new ToolStripMenuItem();
            questEditorToolStripMenuItem = new ToolStripMenuItem();
            resourceEditorToolStripMenuItem = new ToolStripMenuItem();
            shopEditorToolStripMenuItem = new ToolStripMenuItem();
            spellEditorToolStripMenuItem = new ToolStripMenuItem();
            variableEditorToolStripMenuItem = new ToolStripMenuItem();
            timeEditorToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            postQuestionToolStripMenuItem = new ToolStripMenuItem();
            reportBugToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            menuStrip = new DarkMenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            packageUpdateToolStripMenuItem = new ToolStripMenuItem();
            setEditorToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.AutoSize = false;
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            statusStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabelCoords, toolStripSeparator7, toolStripLabelRevision, toolStripSeparator8, toolStripLabelFPS, toolStripSeparator9, toolStripLabelDebug });
            statusStrip1.Location = new System.Drawing.Point(0, 645);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(0, 5, 0, 3);
            statusStrip1.Size = new Size(1186, 30);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripLabelCoords
            // 
            toolStripLabelCoords.ForeColor = SystemColors.Control;
            toolStripLabelCoords.Name = "toolStripLabelCoords";
            toolStripLabelCoords.Size = new Size(118, 17);
            toolStripLabelCoords.Text = "toolStripStatusLabel1";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 22);
            // 
            // toolStripLabelRevision
            // 
            toolStripLabelRevision.ForeColor = SystemColors.Control;
            toolStripLabelRevision.Name = "toolStripLabelRevision";
            toolStripLabelRevision.Size = new Size(63, 17);
            toolStripLabelRevision.Text = "Revision: 0";
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(6, 22);
            // 
            // toolStripLabelFPS
            // 
            toolStripLabelFPS.ForeColor = SystemColors.Control;
            toolStripLabelFPS.Name = "toolStripLabelFPS";
            toolStripLabelFPS.Size = new Size(44, 17);
            toolStripLabelFPS.Text = "FPS: 64";
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 22);
            // 
            // toolStripLabelDebug
            // 
            toolStripLabelDebug.Name = "toolStripLabelDebug";
            toolStripLabelDebug.Size = new Size(0, 17);
            // 
            // dockLeft
            // 
            dockLeft.BorderStyle = BorderStyle.FixedSingle;
            dockLeft.Dock = DockStyle.Fill;
            dockLeft.DockBackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            dockLeft.DockLeftPortion = 316D;
            dockLeft.DockRightPortion = 0.15D;
            dockLeft.Location = new System.Drawing.Point(0, 49);
            dockLeft.Name = "dockLeft";
            dockLeft.ShowAutoHideContentOnHover = false;
            dockLeft.Size = new Size(1186, 596);
            dockLeft.SupportDeeplyNestedContent = true;
            dockLeft.TabIndex = 7;
            // 
            // toolStripBtnNewMap
            // 
            toolStripBtnNewMap.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnNewMap.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnNewMap.Image = (Image)resources.GetObject("toolStripBtnNewMap.Image");
            toolStripBtnNewMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnNewMap.Margin = new Padding(12, 1, 0, 2);
            toolStripBtnNewMap.Name = "toolStripBtnNewMap";
            toolStripBtnNewMap.Size = new Size(23, 22);
            toolStripBtnNewMap.Text = "New Unconnected Map";
            toolStripBtnNewMap.Click += toolStripBtnNewMap_Click;
            // 
            // toolStripBtnSaveMap
            // 
            toolStripBtnSaveMap.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnSaveMap.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnSaveMap.Image = (Image)resources.GetObject("toolStripBtnSaveMap.Image");
            toolStripBtnSaveMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnSaveMap.Name = "toolStripBtnSaveMap";
            toolStripBtnSaveMap.Size = new Size(23, 22);
            toolStripBtnSaveMap.Text = "Save Map";
            toolStripBtnSaveMap.Click += toolStripBtnSaveMap_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator1.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripBtnCut
            // 
            toolStripBtnCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnCut.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnCut.Image = (Image)resources.GetObject("toolStripBtnCut.Image");
            toolStripBtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnCut.Name = "toolStripBtnCut";
            toolStripBtnCut.Size = new Size(23, 22);
            toolStripBtnCut.Text = "Cut Selection";
            toolStripBtnCut.Click += toolStripBtnCut_Click;
            // 
            // toolStripBtnCopy
            // 
            toolStripBtnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnCopy.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnCopy.Image = (Image)resources.GetObject("toolStripBtnCopy.Image");
            toolStripBtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnCopy.Name = "toolStripBtnCopy";
            toolStripBtnCopy.Size = new Size(23, 22);
            toolStripBtnCopy.Text = "Copy Selection";
            toolStripBtnCopy.Click += toolStripBtnCopy_Click;
            // 
            // toolStripBtnPaste
            // 
            toolStripBtnPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnPaste.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnPaste.Image = (Image)resources.GetObject("toolStripBtnPaste.Image");
            toolStripBtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnPaste.Name = "toolStripBtnPaste";
            toolStripBtnPaste.Size = new Size(23, 22);
            toolStripBtnPaste.Text = "Paste";
            toolStripBtnPaste.Click += toolStripBtnPaste_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator2.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripBtnUndo
            // 
            toolStripBtnUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnUndo.Enabled = false;
            toolStripBtnUndo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnUndo.Image = (Image)resources.GetObject("toolStripBtnUndo.Image");
            toolStripBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnUndo.Name = "toolStripBtnUndo";
            toolStripBtnUndo.Size = new Size(23, 22);
            toolStripBtnUndo.Text = "Undo";
            toolStripBtnUndo.Click += toolStripBtnUndo_Click;
            // 
            // toolStripBtnRedo
            // 
            toolStripBtnRedo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnRedo.Enabled = false;
            toolStripBtnRedo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnRedo.Image = (Image)resources.GetObject("toolStripBtnRedo.Image");
            toolStripBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnRedo.Name = "toolStripBtnRedo";
            toolStripBtnRedo.Size = new Size(23, 22);
            toolStripBtnRedo.Text = "Redo";
            toolStripBtnRedo.Click += toolStripBtnRedo_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator3.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // toolStripBtnBrush
            // 
            toolStripBtnBrush.Checked = true;
            toolStripBtnBrush.CheckState = CheckState.Checked;
            toolStripBtnBrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnBrush.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnBrush.Image = (Image)resources.GetObject("toolStripBtnBrush.Image");
            toolStripBtnBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnBrush.Name = "toolStripBtnBrush";
            toolStripBtnBrush.Size = new Size(23, 22);
            toolStripBtnBrush.Text = "Brush Tool";
            toolStripBtnBrush.Click += toolStripBtnBrush_Click;
            // 
            // toolStripBtnSelect
            // 
            toolStripBtnSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnSelect.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnSelect.Image = (Image)resources.GetObject("toolStripBtnSelect.Image");
            toolStripBtnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnSelect.Name = "toolStripBtnSelect";
            toolStripBtnSelect.Size = new Size(23, 22);
            toolStripBtnSelect.Text = "Selection";
            toolStripBtnSelect.Click += toolStripBtnSelect_Click;
            // 
            // toolStripBtnRect
            // 
            toolStripBtnRect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnRect.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnRect.Image = (Image)resources.GetObject("toolStripBtnRect.Image");
            toolStripBtnRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnRect.Name = "toolStripBtnRect";
            toolStripBtnRect.Size = new Size(23, 22);
            toolStripBtnRect.Text = "Rectangle Fill";
            toolStripBtnRect.Click += toolStripBtnRect_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator4.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 25);
            // 
            // toolStripBtnFill
            // 
            toolStripBtnFill.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnFill.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnFill.Image = (Image)resources.GetObject("toolStripBtnFill.Image");
            toolStripBtnFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnFill.Name = "toolStripBtnFill";
            toolStripBtnFill.Size = new Size(23, 22);
            toolStripBtnFill.Text = "Fill Layer";
            toolStripBtnFill.Click += toolStripBtnFill_Click;
            // 
            // toolStripBtnErase
            // 
            toolStripBtnErase.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnErase.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnErase.Image = (Image)resources.GetObject("toolStripBtnErase.Image");
            toolStripBtnErase.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnErase.Name = "toolStripBtnErase";
            toolStripBtnErase.Size = new Size(23, 22);
            toolStripBtnErase.Text = "Clear Layer";
            toolStripBtnErase.Click += toolStripBtnErase_Click;
            // 
            // toolStripBtnDropper
            // 
            toolStripBtnDropper.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnDropper.Enabled = false;
            toolStripBtnDropper.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnDropper.Image = (Image)resources.GetObject("toolStripBtnDropper.Image");
            toolStripBtnDropper.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnDropper.Name = "toolStripBtnDropper";
            toolStripBtnDropper.Size = new Size(23, 22);
            toolStripBtnDropper.Text = "Dropper";
            toolStripBtnDropper.Click += toolStripBtnDropper_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator5.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 25);
            // 
            // toolStripTimeButton
            // 
            toolStripTimeButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripTimeButton.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripTimeButton.Image = (Image)resources.GetObject("toolStripTimeButton.Image");
            toolStripTimeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripTimeButton.Name = "toolStripTimeButton";
            toolStripTimeButton.Size = new Size(29, 22);
            toolStripTimeButton.Text = "Toggle On/Off TOD Simulation";
            toolStripTimeButton.Click += toolStripTimeButton_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator6.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 25);
            // 
            // toolStripBtnScreenshot
            // 
            toolStripBtnScreenshot.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnScreenshot.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnScreenshot.Image = (Image)resources.GetObject("toolStripBtnScreenshot.Image");
            toolStripBtnScreenshot.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnScreenshot.Name = "toolStripBtnScreenshot";
            toolStripBtnScreenshot.Size = new Size(23, 22);
            toolStripBtnScreenshot.Text = "Screenshot Map";
            toolStripBtnScreenshot.Click += toolStripBtnScreenshot_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator10.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 25);
            // 
            // toolStripBtnRun
            // 
            toolStripBtnRun.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnRun.Enabled = false;
            toolStripBtnRun.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnRun.Image = (Image)resources.GetObject("toolStripBtnRun.Image");
            toolStripBtnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnRun.Name = "toolStripBtnRun";
            toolStripBtnRun.Size = new Size(23, 22);
            toolStripBtnRun.Text = "Run Client";
            toolStripBtnRun.Click += toolStripBtnRun_Click;
            // 
            // toolStripButtonBug
            // 
            toolStripButtonBug.Alignment = ToolStripItemAlignment.Right;
            toolStripButtonBug.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonBug.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripButtonBug.Image = (Image)resources.GetObject("toolStripButtonBug.Image");
            toolStripButtonBug.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBug.Name = "toolStripButtonBug";
            toolStripButtonBug.Size = new Size(23, 22);
            toolStripButtonBug.Text = "Report a Bug";
            toolStripButtonBug.Click += toolStripButtonBug_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator11.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator11.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 25);
            // 
            // toolStripBtnFlipVertical
            // 
            toolStripBtnFlipVertical.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnFlipVertical.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnFlipVertical.Image = (Image)resources.GetObject("toolStripBtnFlipVertical.Image");
            toolStripBtnFlipVertical.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnFlipVertical.Name = "toolStripBtnFlipVertical";
            toolStripBtnFlipVertical.Size = new Size(23, 22);
            toolStripBtnFlipVertical.Text = "toolStripBtnFlipVertical";
            toolStripBtnFlipVertical.Click += toolStripBtnFlipVertical_Click;
            // 
            // toolStripBtnFlipHorizontal
            // 
            toolStripBtnFlipHorizontal.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripBtnFlipHorizontal.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripBtnFlipHorizontal.Image = (Image)resources.GetObject("toolStripBtnFlipHorizontal.Image");
            toolStripBtnFlipHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripBtnFlipHorizontal.Name = "toolStripBtnFlipHorizontal";
            toolStripBtnFlipHorizontal.Size = new Size(23, 22);
            toolStripBtnFlipHorizontal.Text = "toolStripBtnFlipHorizontal";
            toolStripBtnFlipHorizontal.Click += toolStripBtnFlipHorizontal_Click;
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator13.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(6, 25);
            // 
            // toolStripButtonQuestion
            // 
            toolStripButtonQuestion.Alignment = ToolStripItemAlignment.Right;
            toolStripButtonQuestion.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonQuestion.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripButtonQuestion.Image = (Image)resources.GetObject("toolStripButtonQuestion.Image");
            toolStripButtonQuestion.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonQuestion.Name = "toolStripButtonQuestion";
            toolStripButtonQuestion.Size = new Size(23, 22);
            toolStripButtonQuestion.Text = "Ask a Question";
            toolStripButtonQuestion.Click += toolStripButtonQuestion_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            toolStrip1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripBtnNewMap, toolStripBtnSaveMap, toolStripSeparator1, toolStripBtnCut, toolStripBtnCopy, toolStripBtnPaste, toolStripSeparator2, toolStripBtnUndo, toolStripBtnRedo, toolStripSeparator3, toolStripBtnBrush, toolStripBtnSelect, toolStripBtnRect, toolStripSeparator4, toolStripBtnFlipVertical, toolStripBtnFlipHorizontal, toolStripSeparator5, toolStripBtnFill, toolStripBtnErase, toolStripBtnDropper, toolStripSeparator6, toolStripTimeButton, toolStripSeparator13, toolStripBtnScreenshot, toolStripSeparator10, toolStripBtnRun, toolStripButtonBug, toolStripSeparator11, toolStripButtonQuestion });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0);
            toolStrip1.Size = new Size(1186, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveMapToolStripMenuItem, newMapToolStripMenuItem, importMapToolStripMenuItem, exportMapToolStripMenuItem, optionsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveMapToolStripMenuItem
            // 
            saveMapToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            saveMapToolStripMenuItem.Name = "saveMapToolStripMenuItem";
            saveMapToolStripMenuItem.Size = new Size(137, 22);
            saveMapToolStripMenuItem.Text = "Save Map";
            saveMapToolStripMenuItem.Click += saveMapToolStripMenuItem_Click;
            // 
            // newMapToolStripMenuItem
            // 
            newMapToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
            newMapToolStripMenuItem.Size = new Size(137, 22);
            newMapToolStripMenuItem.Text = "New Map";
            newMapToolStripMenuItem.ToolTipText = "Create a new, unconnected map.";
            newMapToolStripMenuItem.Click += NewMapToolStripMenuItem_Click;
            // 
            // importMapToolStripMenuItem
            // 
            importMapToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            importMapToolStripMenuItem.Name = "importMapToolStripMenuItem";
            importMapToolStripMenuItem.Size = new Size(137, 22);
            importMapToolStripMenuItem.Text = "Import Map";
            importMapToolStripMenuItem.Visible = false;
            importMapToolStripMenuItem.Click += importMapToolStripMenuItem_Click;
            // 
            // exportMapToolStripMenuItem
            // 
            exportMapToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            exportMapToolStripMenuItem.Name = "exportMapToolStripMenuItem";
            exportMapToolStripMenuItem.Size = new Size(137, 22);
            exportMapToolStripMenuItem.Text = "Export Map";
            exportMapToolStripMenuItem.Visible = false;
            exportMapToolStripMenuItem.Click += exportMapToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(137, 22);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(137, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, fillToolStripMenuItem, eraseLayerToolStripMenuItem, selectToolStripMenuItem });
            editToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.Size = new Size(132, 22);
            undoToolStripMenuItem.Text = "Undo";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.Size = new Size(132, 22);
            redoToolStripMenuItem.Text = "Redo";
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new Size(132, 22);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(132, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(132, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // fillToolStripMenuItem
            // 
            fillToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            fillToolStripMenuItem.Name = "fillToolStripMenuItem";
            fillToolStripMenuItem.Size = new Size(132, 22);
            fillToolStripMenuItem.Text = "Fill";
            fillToolStripMenuItem.Click += fillToolStripMenuItem_Click;
            // 
            // eraseLayerToolStripMenuItem
            // 
            eraseLayerToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            eraseLayerToolStripMenuItem.Name = "eraseLayerToolStripMenuItem";
            eraseLayerToolStripMenuItem.Size = new Size(132, 22);
            eraseLayerToolStripMenuItem.Text = "Erase Layer";
            eraseLayerToolStripMenuItem.Click += eraseLayerToolStripMenuItem_Click;
            // 
            // selectToolStripMenuItem
            // 
            selectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { allLayersToolStripMenuItem, currentLayerOnlyToolStripMenuItem });
            selectToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            selectToolStripMenuItem.Size = new Size(132, 22);
            selectToolStripMenuItem.Text = "Select....";
            // 
            // allLayersToolStripMenuItem
            // 
            allLayersToolStripMenuItem.Checked = true;
            allLayersToolStripMenuItem.CheckState = CheckState.Checked;
            allLayersToolStripMenuItem.Name = "allLayersToolStripMenuItem";
            allLayersToolStripMenuItem.Size = new Size(173, 22);
            allLayersToolStripMenuItem.Text = "All Layers";
            allLayersToolStripMenuItem.Click += allLayersToolStripMenuItem_Click;
            // 
            // currentLayerOnlyToolStripMenuItem
            // 
            currentLayerOnlyToolStripMenuItem.Name = "currentLayerOnlyToolStripMenuItem";
            currentLayerOnlyToolStripMenuItem.Size = new Size(173, 22);
            currentLayerOnlyToolStripMenuItem.Text = "Current Layer Only";
            currentLayerOnlyToolStripMenuItem.Click += currentLayerOnlyToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hideDarknessToolStripMenuItem, hideFogToolStripMenuItem, hideOverlayToolStripMenuItem, hideTilePreviewToolStripMenuItem, hideResourcesToolStripMenuItem, mapGridToolStripMenuItem, hideEventsToolStripMenuItem, layersToolStripMenuItem });
            viewToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // hideDarknessToolStripMenuItem
            // 
            hideDarknessToolStripMenuItem.Checked = true;
            hideDarknessToolStripMenuItem.CheckState = CheckState.Checked;
            hideDarknessToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideDarknessToolStripMenuItem.Name = "hideDarknessToolStripMenuItem";
            hideDarknessToolStripMenuItem.Size = new Size(136, 22);
            hideDarknessToolStripMenuItem.Text = "Darkness";
            hideDarknessToolStripMenuItem.Click += hideDarknessToolStripMenuItem_Click;
            // 
            // hideFogToolStripMenuItem
            // 
            hideFogToolStripMenuItem.Checked = true;
            hideFogToolStripMenuItem.CheckState = CheckState.Checked;
            hideFogToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideFogToolStripMenuItem.Name = "hideFogToolStripMenuItem";
            hideFogToolStripMenuItem.Size = new Size(136, 22);
            hideFogToolStripMenuItem.Text = "Fog";
            hideFogToolStripMenuItem.Click += hideFogToolStripMenuItem_Click;
            // 
            // hideOverlayToolStripMenuItem
            // 
            hideOverlayToolStripMenuItem.Checked = true;
            hideOverlayToolStripMenuItem.CheckState = CheckState.Checked;
            hideOverlayToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideOverlayToolStripMenuItem.Name = "hideOverlayToolStripMenuItem";
            hideOverlayToolStripMenuItem.Size = new Size(136, 22);
            hideOverlayToolStripMenuItem.Text = "Overlay";
            hideOverlayToolStripMenuItem.Click += hideOverlayToolStripMenuItem_Click;
            // 
            // hideTilePreviewToolStripMenuItem
            // 
            hideTilePreviewToolStripMenuItem.Checked = true;
            hideTilePreviewToolStripMenuItem.CheckState = CheckState.Checked;
            hideTilePreviewToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideTilePreviewToolStripMenuItem.Name = "hideTilePreviewToolStripMenuItem";
            hideTilePreviewToolStripMenuItem.Size = new Size(136, 22);
            hideTilePreviewToolStripMenuItem.Text = "Tile Preview";
            hideTilePreviewToolStripMenuItem.Click += hideTilePreviewToolStripMenuItem_Click;
            // 
            // hideResourcesToolStripMenuItem
            // 
            hideResourcesToolStripMenuItem.Checked = true;
            hideResourcesToolStripMenuItem.CheckState = CheckState.Checked;
            hideResourcesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideResourcesToolStripMenuItem.Name = "hideResourcesToolStripMenuItem";
            hideResourcesToolStripMenuItem.Size = new Size(136, 22);
            hideResourcesToolStripMenuItem.Text = "Resources";
            hideResourcesToolStripMenuItem.Click += hideResourcesToolStripMenuItem_Click;
            // 
            // mapGridToolStripMenuItem
            // 
            mapGridToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            mapGridToolStripMenuItem.Name = "mapGridToolStripMenuItem";
            mapGridToolStripMenuItem.Size = new Size(136, 22);
            mapGridToolStripMenuItem.Text = "Map Grid";
            mapGridToolStripMenuItem.Click += mapGridToolStripMenuItem_Click;
            // 
            // hideEventsToolStripMenuItem
            // 
            hideEventsToolStripMenuItem.Checked = true;
            hideEventsToolStripMenuItem.CheckState = CheckState.Checked;
            hideEventsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            hideEventsToolStripMenuItem.Name = "hideEventsToolStripMenuItem";
            hideEventsToolStripMenuItem.Size = new Size(136, 22);
            hideEventsToolStripMenuItem.Text = "Events";
            hideEventsToolStripMenuItem.Click += hideEventsToolStripMenuItem_Click;
            // 
            // layersToolStripMenuItem
            // 
            layersToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            layersToolStripMenuItem.Name = "layersToolStripMenuItem";
            layersToolStripMenuItem.Size = new Size(136, 22);
            layersToolStripMenuItem.Text = "Layers";
            layersToolStripMenuItem.DropDownOpened += layersToolStripMenuItem_DropDownOpened;
            // 
            // contentEditorsToolStripMenuItem
            // 
            contentEditorsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { animationEditorToolStripMenuItem, classEditorToolStripMenuItem, commonEventEditorToolStripMenuItem, craftsEditorToolStripMenuItem, craftingTableEditorToolStripMenuItem, itemEditorToolStripMenuItem, npcEditorToolStripMenuItem, projectileEditorToolStripMenuItem, questEditorToolStripMenuItem, resourceEditorToolStripMenuItem, shopEditorToolStripMenuItem, spellEditorToolStripMenuItem, variableEditorToolStripMenuItem, timeEditorToolStripMenuItem, setEditorToolStripMenuItem });
            contentEditorsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            contentEditorsToolStripMenuItem.Name = "contentEditorsToolStripMenuItem";
            contentEditorsToolStripMenuItem.Size = new Size(101, 20);
            contentEditorsToolStripMenuItem.Text = "Content Editors";
            // 
            // animationEditorToolStripMenuItem
            // 
            animationEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            animationEditorToolStripMenuItem.Name = "animationEditorToolStripMenuItem";
            animationEditorToolStripMenuItem.Size = new Size(191, 22);
            animationEditorToolStripMenuItem.Text = "Animation Editor";
            animationEditorToolStripMenuItem.Click += animationEditorToolStripMenuItem_Click;
            // 
            // classEditorToolStripMenuItem
            // 
            classEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            classEditorToolStripMenuItem.Name = "classEditorToolStripMenuItem";
            classEditorToolStripMenuItem.Size = new Size(191, 22);
            classEditorToolStripMenuItem.Text = "Class Editor";
            classEditorToolStripMenuItem.Click += classEditorToolStripMenuItem_Click;
            // 
            // commonEventEditorToolStripMenuItem
            // 
            commonEventEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            commonEventEditorToolStripMenuItem.Name = "commonEventEditorToolStripMenuItem";
            commonEventEditorToolStripMenuItem.Size = new Size(191, 22);
            commonEventEditorToolStripMenuItem.Text = "Common Event Editor";
            commonEventEditorToolStripMenuItem.Click += commonEventEditorToolStripMenuItem_Click;
            // 
            // craftsEditorToolStripMenuItem
            // 
            craftsEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            craftsEditorToolStripMenuItem.Name = "craftsEditorToolStripMenuItem";
            craftsEditorToolStripMenuItem.Size = new Size(191, 22);
            craftsEditorToolStripMenuItem.Text = "Crafts Editor";
            craftsEditorToolStripMenuItem.Click += craftsEditorToolStripMenuItem_Click;
            // 
            // craftingTableEditorToolStripMenuItem
            // 
            craftingTableEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            craftingTableEditorToolStripMenuItem.Name = "craftingTableEditorToolStripMenuItem";
            craftingTableEditorToolStripMenuItem.Size = new Size(191, 22);
            craftingTableEditorToolStripMenuItem.Text = "Crafting Table Editor";
            craftingTableEditorToolStripMenuItem.Click += craftingTablesEditorToolStripMenuItem_Click;
            // 
            // itemEditorToolStripMenuItem
            // 
            itemEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            itemEditorToolStripMenuItem.Name = "itemEditorToolStripMenuItem";
            itemEditorToolStripMenuItem.Size = new Size(191, 22);
            itemEditorToolStripMenuItem.Text = "Item Editor";
            itemEditorToolStripMenuItem.Click += itemEditorToolStripMenuItem_Click;
            // 
            // npcEditorToolStripMenuItem
            // 
            npcEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            npcEditorToolStripMenuItem.Name = "npcEditorToolStripMenuItem";
            npcEditorToolStripMenuItem.Size = new Size(191, 22);
            npcEditorToolStripMenuItem.Text = "Npc Editor";
            npcEditorToolStripMenuItem.Click += npcEditorToolStripMenuItem_Click;
            // 
            // projectileEditorToolStripMenuItem
            // 
            projectileEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            projectileEditorToolStripMenuItem.Name = "projectileEditorToolStripMenuItem";
            projectileEditorToolStripMenuItem.Size = new Size(191, 22);
            projectileEditorToolStripMenuItem.Text = "Projectile Editor";
            projectileEditorToolStripMenuItem.Click += projectileEditorToolStripMenuItem_Click;
            // 
            // questEditorToolStripMenuItem
            // 
            questEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            questEditorToolStripMenuItem.Name = "questEditorToolStripMenuItem";
            questEditorToolStripMenuItem.Size = new Size(191, 22);
            questEditorToolStripMenuItem.Text = "Quest Editor";
            questEditorToolStripMenuItem.Click += questEditorToolStripMenuItem_Click;
            // 
            // resourceEditorToolStripMenuItem
            // 
            resourceEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resourceEditorToolStripMenuItem.Name = "resourceEditorToolStripMenuItem";
            resourceEditorToolStripMenuItem.Size = new Size(191, 22);
            resourceEditorToolStripMenuItem.Text = "Resource Editor";
            resourceEditorToolStripMenuItem.Click += resourceEditorToolStripMenuItem_Click;
            // 
            // shopEditorToolStripMenuItem
            // 
            shopEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            shopEditorToolStripMenuItem.Name = "shopEditorToolStripMenuItem";
            shopEditorToolStripMenuItem.Size = new Size(191, 22);
            shopEditorToolStripMenuItem.Text = "Shop Editor";
            shopEditorToolStripMenuItem.Click += shopEditorToolStripMenuItem_Click;
            // 
            // spellEditorToolStripMenuItem
            // 
            spellEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            spellEditorToolStripMenuItem.Name = "spellEditorToolStripMenuItem";
            spellEditorToolStripMenuItem.Size = new Size(191, 22);
            spellEditorToolStripMenuItem.Text = "Spell Editor";
            spellEditorToolStripMenuItem.Click += spellEditorToolStripMenuItem_Click;
            // 
            // variableEditorToolStripMenuItem
            // 
            variableEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            variableEditorToolStripMenuItem.Name = "variableEditorToolStripMenuItem";
            variableEditorToolStripMenuItem.Size = new Size(191, 22);
            variableEditorToolStripMenuItem.Text = "Variable Editor";
            variableEditorToolStripMenuItem.Click += variableEditorToolStripMenuItem_Click;
            // 
            // timeEditorToolStripMenuItem
            // 
            timeEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            timeEditorToolStripMenuItem.Name = "timeEditorToolStripMenuItem";
            timeEditorToolStripMenuItem.Size = new Size(191, 22);
            timeEditorToolStripMenuItem.Text = "Time Editor";
            timeEditorToolStripMenuItem.Click += timeEditorToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { postQuestionToolStripMenuItem, reportBugToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // postQuestionToolStripMenuItem
            // 
            postQuestionToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            postQuestionToolStripMenuItem.Name = "postQuestionToolStripMenuItem";
            postQuestionToolStripMenuItem.Size = new Size(148, 22);
            postQuestionToolStripMenuItem.Text = "Post Question";
            postQuestionToolStripMenuItem.Click += postQuestionToolStripMenuItem_Click;
            // 
            // reportBugToolStripMenuItem
            // 
            reportBugToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            reportBugToolStripMenuItem.Size = new Size(148, 22);
            reportBugToolStripMenuItem.Text = "Report Bug";
            reportBugToolStripMenuItem.Click += reportBugToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(148, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            menuStrip.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, contentEditorsToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(3, 2, 0, 2);
            menuStrip.Size = new Size(1186, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { packageUpdateToolStripMenuItem });
            toolsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // packageUpdateToolStripMenuItem
            // 
            packageUpdateToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            packageUpdateToolStripMenuItem.Name = "packageUpdateToolStripMenuItem";
            packageUpdateToolStripMenuItem.Size = new Size(159, 22);
            packageUpdateToolStripMenuItem.Text = "Package Update";
            packageUpdateToolStripMenuItem.Click += packageUpdateToolStripMenuItem_Click;
            // 
            // setEditorToolStripMenuItem
            // 
            setEditorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            setEditorToolStripMenuItem.Name = "setEditorToolStripMenuItem";
            setEditorToolStripMenuItem.Size = new Size(191, 22);
            setEditorToolStripMenuItem.Text = "Set Editor";
            setEditorToolStripMenuItem.Click += setEditorToolStripMenuItem_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1186, 675);
            Controls.Add(dockLeft);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Intersect Editor";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            KeyDown += FrmMain_KeyDown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private DarkStatusStrip statusStrip1;
		public ToolStripStatusLabel toolStripLabelCoords;
		private ToolStripSeparator toolStripSeparator7;
		public ToolStripStatusLabel toolStripLabelFPS;
		private ToolStripSeparator toolStripSeparator8;
		public ToolStripStatusLabel toolStripLabelDebug;
		public ToolStripStatusLabel toolStripLabelRevision;
		private ToolStripSeparator toolStripSeparator9;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockLeft;
		private ToolStripButton toolStripBtnNewMap;
		private ToolStripButton toolStripBtnSaveMap;
		private ToolStripSeparator toolStripSeparator1;
		public ToolStripButton toolStripBtnCut;
		public ToolStripButton toolStripBtnCopy;
		public ToolStripButton toolStripBtnPaste;
		private ToolStripSeparator toolStripSeparator2;
		public ToolStripButton toolStripBtnUndo;
		public ToolStripButton toolStripBtnRedo;
		private ToolStripSeparator toolStripSeparator3;
		public System.Windows.Forms.ToolStripButton toolStripBtnBrush;
		public ToolStripButton toolStripBtnSelect;
		public ToolStripButton toolStripBtnRect;
		private ToolStripSeparator toolStripSeparator4;
		public ToolStripButton toolStripBtnFill;
		public ToolStripButton toolStripBtnErase;
		private ToolStripButton toolStripBtnDropper;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripDropDownButton toolStripTimeButton;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripButton toolStripBtnScreenshot;
		private ToolStripSeparator toolStripSeparator10;
		private ToolStripButton toolStripBtnRun;
		private ToolStripButton toolStripButtonBug;
		private ToolStripSeparator toolStripSeparator11;
		private ToolStripButton toolStripButtonQuestion;
		private DarkToolStrip toolStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem saveMapToolStripMenuItem;
		private ToolStripMenuItem newMapToolStripMenuItem;
		private ToolStripMenuItem importMapToolStripMenuItem;
		private ToolStripMenuItem exportMapToolStripMenuItem;
		private ToolStripMenuItem optionsToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem undoToolStripMenuItem;
		private ToolStripMenuItem redoToolStripMenuItem;
		private ToolStripMenuItem cutToolStripMenuItem;
		private ToolStripMenuItem copyToolStripMenuItem;
		private ToolStripMenuItem pasteToolStripMenuItem;
		public ToolStripMenuItem fillToolStripMenuItem;
		public ToolStripMenuItem eraseLayerToolStripMenuItem;
		private ToolStripMenuItem selectToolStripMenuItem;
		private ToolStripMenuItem allLayersToolStripMenuItem;
		private ToolStripMenuItem currentLayerOnlyToolStripMenuItem;
		private ToolStripMenuItem viewToolStripMenuItem;
		private ToolStripMenuItem hideDarknessToolStripMenuItem;
		private ToolStripMenuItem hideFogToolStripMenuItem;
		private ToolStripMenuItem hideOverlayToolStripMenuItem;
		private ToolStripMenuItem hideTilePreviewToolStripMenuItem;
		private ToolStripMenuItem hideResourcesToolStripMenuItem;
		private ToolStripMenuItem mapGridToolStripMenuItem;
		private ToolStripMenuItem contentEditorsToolStripMenuItem;
		private ToolStripMenuItem animationEditorToolStripMenuItem;
		private ToolStripMenuItem classEditorToolStripMenuItem;
		private ToolStripMenuItem commonEventEditorToolStripMenuItem;
		private ToolStripMenuItem craftingTableEditorToolStripMenuItem;
		private ToolStripMenuItem itemEditorToolStripMenuItem;
		private ToolStripMenuItem npcEditorToolStripMenuItem;
		private ToolStripMenuItem projectileEditorToolStripMenuItem;
		private ToolStripMenuItem questEditorToolStripMenuItem;
		private ToolStripMenuItem resourceEditorToolStripMenuItem;
		private ToolStripMenuItem shopEditorToolStripMenuItem;
		private ToolStripMenuItem spellEditorToolStripMenuItem;
		private ToolStripMenuItem variableEditorToolStripMenuItem;
		private ToolStripMenuItem timeEditorToolStripMenuItem;
		private ToolStripMenuItem helpToolStripMenuItem;
		private ToolStripMenuItem postQuestionToolStripMenuItem;
		private ToolStripMenuItem reportBugToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem;
		private DarkMenuStrip menuStrip;
		private ToolStripMenuItem toolsToolStripMenuItem;
		private ToolStripButton toolStripBtnFlipHorizontal;
		private ToolStripButton toolStripBtnFlipVertical;
		private ToolStripSeparator toolStripSeparator13;
		private ToolStripMenuItem craftsEditorToolStripMenuItem;
        private ToolStripMenuItem packageUpdateToolStripMenuItem;
        private ToolStripMenuItem layersToolStripMenuItem;
        private ToolStripMenuItem hideEventsToolStripMenuItem;
        private ToolStripMenuItem setEditorToolStripMenuItem;
    }
}
