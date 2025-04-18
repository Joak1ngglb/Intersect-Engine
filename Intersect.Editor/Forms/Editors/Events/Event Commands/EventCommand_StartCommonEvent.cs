﻿using Intersect.Editor.Localization;
using Intersect.Framework.Core.GameObjects.Events;
using Intersect.Framework.Core.GameObjects.Events.Commands;

namespace Intersect.Editor.Forms.Editors.Events.Event_Commands;

public partial class EventCommandStartCommonEvent : UserControl
{
    private readonly FrmEvent mEventEditor;

    private StartCommmonEventCommand mMyCommand;

    public EventCommandStartCommonEvent(StartCommmonEventCommand refCommand, FrmEvent editor)
    {
        InitializeComponent();
        mMyCommand = refCommand;
        mEventEditor = editor;
        InitLocalization();

        cmbEvent.Items.Clear();
        cmbEvent.Items.AddRange(EventDescriptor.Names);

        cmbEvent.SelectedIndex = EventDescriptor.ListIndex(refCommand.EventId);
        chkAllInInstance.Checked = refCommand.AllInInstance;
        chkOverworldOverride.Checked = refCommand.AllowInOverworld;
        chkOverworldOverride.Enabled = chkAllInInstance.Checked;
    }

    private void InitLocalization()
    {
        grpCommonEvent.Text = Strings.EventStartCommonEvent.title;
        lblCommonEvent.Text = Strings.EventStartCommonEvent.label;
        chkAllInInstance.Text = Strings.EventStartCommonEvent.AllInInstance;
        chkOverworldOverride.Text = Strings.EventStartCommonEvent.AllowInOverworld;

        ToolTip overworldWarningTooltip = new ToolTip()
        {
            InitialDelay = 1000,
            ReshowDelay = 500,
        };

        overworldWarningTooltip.SetToolTip(chkOverworldOverride, Strings.EventStartCommonEvent.OverworldOverrideTooltip.ToString());

        btnSave.Text = Strings.EventStartCommonEvent.okay;
        btnCancel.Text = Strings.EventStartCommonEvent.cancel;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        mMyCommand.EventId = EventDescriptor.IdFromList(cmbEvent.SelectedIndex);
        mMyCommand.AllInInstance = chkAllInInstance.Checked;
        mMyCommand.AllowInOverworld = chkAllInInstance.Checked && chkOverworldOverride.Checked;
        mEventEditor.FinishCommandEdit();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        mEventEditor.CancelCommandEdit();
    }

    private void chkAllInInstance_CheckedChanged(object sender, EventArgs e)
    {
        if (!chkAllInInstance.Checked)
        {
            chkOverworldOverride.Checked = false;
        }

        chkOverworldOverride.Enabled = chkAllInInstance.Checked;
    }
}
