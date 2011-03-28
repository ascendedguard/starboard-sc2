// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardDisplay.xaml.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Interaction logic for ScoreboardDisplay.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ScoreboardDisplay.xaml
    /// </summary>
    public partial class ScoreboardDisplay
    {
        /// <summary> Initializes a new instance of the <see cref="ScoreboardDisplay"/> class. </summary>
        public ScoreboardDisplay()
        {
            InitializeComponent();

            this.Loaded += this.WindowLoaded;
            this.IsWindowMovable = false;
        }

        /// <summary> Gets or sets the width of the viewbox used to render the scoreboard. </summary>
        public double ViewboxWidth
        {
            get { return viewBox.Width; }
            set { viewBox.Width = value; }
        }

        /// <summary> Gets or sets a value indicating whether the window can be dragged. </summary>
        public bool IsWindowMovable { get; set; }

        /// <summary> Resets the position of the window to the default location, centered on the primary monitor with a 10px offset from top. </summary>
        public void ResetPosition()
        {
            var leftAdjust = this.Width / 2.0;
            var left = (SystemParameters.PrimaryScreenWidth / 2.0) - leftAdjust;

            this.Left = left;
            this.Top = 10;   
        }

        /// <summary> Sets the viewmodel for the window to another instance of ScoreboardControlViewModel </summary>
        /// <param name="vm"> The viewModel to apply. </param>
        public void SetViewModel(ScoreboardControlViewModel vm)
        {
            scoreboardControl.ViewModel = vm;
        }

        /// <summary> Allows the window to be dragged if IsWindowMovable has been set. </summary>
        /// <param name="e"> The event arguments. </param>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.IsWindowMovable)
            {
                this.DragMove();
            }
        }

        /// <summary> Resets the position of the window after it has completed loading. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event parameters. </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.ResetPosition();
        }
    }
}
