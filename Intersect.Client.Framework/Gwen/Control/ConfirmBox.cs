namespace Intersect.Client.Framework.Gwen.Control
{
    /// <summary>
    ///     Simple confirmation box with Yes/No options and result handling.
    /// </summary>
    public partial class ConfirmBox : WindowControl
    {
        private readonly Label mLabel;
        private readonly Button mYesButton;
        private readonly Button mNoButton;

        /// <summary>
        ///     Possible results of the confirmation.
        /// </summary>
        public enum ConfirmResult { Yes, No, Closed }

        /// <summary>
        ///     EventArgs for confirmation result.
        /// </summary>
        public class ConfirmEventArgs : EventArgs
        {
            public ConfirmResult Result { get; }
            public ConfirmEventArgs(ConfirmResult result) { Result = result; }
        }

        /// <summary>
        ///     Event fired when the user makes a choice or closes the dialog.
        /// </summary>
        public event GwenEventHandler<ConfirmEventArgs> Confirmed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfirmBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        /// <param name="text">Message to display.</param>
        /// <param name="caption">Window caption.</param>
        public ConfirmBox(Base parent, string text, string caption = "") : base(parent, caption, true)
        {
            DeleteOnClose = true;
            Padding = new Padding(10, 10, 10, 10);

            // Message label
            mLabel = new Label(mInnerPanel)
            {
                Text = text,
                Margin = Margin.Five,
                Dock = Pos.Top,
                Alignment = Pos.Center
            };
            mLabel.SetTextColor(Color.White, Label.ControlState.Normal);
            mLabel.FontName = "sourcesansproblack";
            mLabel.FontSize = 14;
            mLabel.TextPadding = new Padding(10, 10, 10, 10);
            // "Sí" button
            mYesButton = new Button(mInnerPanel)
            {
                Text = "Yes",
                Margin = Margin.Five
            };
            mYesButton.SetSize(80, 30);
            mYesButton.FontName = "sourcesansproblack";
            mYesButton.FontSize = 14;
            mYesButton.SetTextColor(Color.White, Label.ControlState.Normal);
            mYesButton.Clicked += (s, e) => OnButtonClicked(ConfirmResult.Yes);

            // "No" button
            mNoButton = new Button(mInnerPanel)
            {
                Text = "No",
                Margin = Margin.Five
            };
            mNoButton.SetSize(80, 30);
            mNoButton.FontName = "sourcesansproblack";
            mNoButton.FontSize = 14;
            mNoButton.SetTextColor(Color.White, Label.ControlState.Normal);
            mNoButton.Clicked += (s, e) => OnButtonClicked(ConfirmResult.No);


            // Center the dialog
            Align.Center(this);
        }

        /// <summary>
        ///     Internal handler for button clicks.
        /// </summary>
        private void OnButtonClicked(ConfirmResult result)
        {
            // Invoke event with custom EventArgs
            Confirmed?.Invoke(this, new ConfirmEventArgs(result));
            Close();
        }

        /// <summary>
        ///     Handle the window close button (X).
        /// </summary>
        protected override void CloseButtonPressed(Base sender, EventArgs args)
        {
            base.CloseButtonPressed(sender, args);
            Confirmed?.Invoke(this, new ConfirmEventArgs(ConfirmResult.Closed));
        }

        /// <summary>
        ///     Layout the control's interior.
        /// </summary>
        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            Align.PlaceDownLeft(mYesButton, mLabel, 10);
            Align.CenterHorizontally(mYesButton);

            Align.PlaceDownLeft(mNoButton, mYesButton, 5);
            Align.CenterHorizontally(mNoButton);

            mInnerPanel.SizeToChildren();
            mInnerPanel.Height += 20;
            SizeToChildren();
        }

        /// <summary>
        ///     Adjust the scale of the message text.
        /// </summary>
        public void SetTextScale(float scale)
        {
            mLabel.SetTextScale(scale);
        }
    }
}
