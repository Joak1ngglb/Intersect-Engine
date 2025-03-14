﻿namespace Intersect.Client.Framework.Gwen.Control;


/// <summary>
///     Simple message box.
/// </summary>
public partial class MessageBox : WindowControl
{

    private readonly Button mButton;

    private readonly Label mLabel; // should be rich label with maxwidth = parent

    /// <summary>
    ///     Invoked when the message box has been dismissed.
    /// </summary>
    public GwenEventHandler<EventArgs> Dismissed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageBox" /> class.
    /// </summary>
    /// <param name="parent">Parent control.</param>
    /// <param name="text">Message to display.</param>
    /// <param name="caption">Window caption.</param>
    public MessageBox(Base parent, string text, string caption = "") : base(parent, caption, true)
    {
        DeleteOnClose = true;

        mLabel = new Label(_innerPanel);
        mLabel.Text = text;
        mLabel.Margin = Margin.Five;
        mLabel.Dock = Pos.Top;
        mLabel.TextAlign = Pos.Center;

        mButton = new Button(_innerPanel);
        mButton.Text = "OK"; // todo: parametrize buttons
        mButton.Clicked += OnClose;
        mButton.Clicked += DismissedHandler;
        mButton.Margin = Margin.Five;
        mButton.SetSize(50, 20);

        Align.Center(this);
    }

    private void DismissedHandler(Base control, EventArgs args)
    {
        if (Dismissed != null)
        {
            Dismissed.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Lays out the control's interior according to alignment, padding, dock etc.
    /// </summary>
    /// <param name="skin">Skin to use.</param>
    protected override void Layout(Skin.Base skin)
    {
        base.Layout(skin);

        Align.PlaceDownLeft(mButton, mLabel, 10);
        Align.CenterHorizontally(mButton);
        _innerPanel.SizeToChildren();
        _innerPanel.Height += 10;
        SizeToChildren();
    }

    public void SetTextScale(float scale)
    {
        mLabel.SetTextScale(scale);
    }

}
