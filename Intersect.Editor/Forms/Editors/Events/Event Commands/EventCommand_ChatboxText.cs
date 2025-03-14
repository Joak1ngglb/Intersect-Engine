using Intersect.Editor.General;
using Intersect.Editor.Localization;
using Intersect.Enums;
using Intersect.Framework.Core.GameObjects.Events.Commands;
using Intersect.Utilities;

namespace Intersect.Editor.Forms.Editors.Events.Event_Commands;


public partial class EventCommandChatboxText : UserControl
{

    private readonly FrmEvent mEventEditor;

    private AddChatboxTextCommand mMyCommand;

    public EventCommandChatboxText(AddChatboxTextCommand refCommand, FrmEvent editor)
    {
        InitializeComponent();
        mMyCommand = refCommand;
        mEventEditor = editor;
        InitLocalization();
        txtAddText.Text = mMyCommand.Text;
        cmbColor.Items.Clear();
        foreach (Color.ChatColor color in Enum.GetValues(typeof(Color.ChatColor)))
        {
            cmbColor.Items.Add(Globals.GetColorName(color));
        }

        cmbColor.SelectedIndex = cmbColor.Items.IndexOf(mMyCommand.Color);
        if (cmbColor.SelectedIndex == -1)
        {
            cmbColor.SelectedIndex = 0;
        }

        cmbChannel.SelectedIndex = (int) mMyCommand.Channel;
        chkShowChatBubble.Checked = mMyCommand.ShowChatBubble;

        chkShowChatBubbleInProximity.Checked = mMyCommand.ShowChatBubbleInProximity;
    }

    private void InitLocalization()
    {
        grpChatboxText.Text = Strings.EventChatboxText.title;
        lblText.Text = Strings.EventChatboxText.text;
        lblColor.Text = Strings.EventChatboxText.color;
        lblChannel.Text = Strings.EventChatboxText.channel;
        lblCommands.Text = Strings.EventChatboxText.commands;
        cmbChannel.Items.Clear();
        for (var i = 0; i < Strings.EventChatboxText.channels.Count; i++)
        {
            cmbChannel.Items.Add(Strings.EventChatboxText.channels[i]);
        }
        chkShowChatBubble.Text = Strings.EventChatboxText.ShowChatBubble;
        chkShowChatBubbleInProximity.Text = Strings.EventChatboxText.ShowChatBubbleInProximity;

        btnSave.Text = Strings.EventChatboxText.okay;
        btnCancel.Text = Strings.EventChatboxText.cancel;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        mMyCommand.Text = txtAddText.Text;
        mMyCommand.Color = cmbColor.Text;
        mMyCommand.Channel = (ChatboxChannel) cmbChannel.SelectedIndex;
        mMyCommand.ShowChatBubble = chkShowChatBubble.Checked;
        mMyCommand.ShowChatBubbleInProximity = chkShowChatBubbleInProximity.Checked;
        mEventEditor.FinishCommandEdit();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        mEventEditor.CancelCommandEdit();
    }

    private void lblCommands_Click(object sender, EventArgs e)
    {
        BrowserUtils.Open("http://www.ascensiongamedev.com/community/topic/749-event-text-variables/");
    }

}
