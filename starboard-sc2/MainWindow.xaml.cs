// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard
{
    using System.Windows;

    using Starboard.Scoreboard;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary> Window controlling the scoreboard display </summary>
        private readonly ScoreboardDisplay display = new ScoreboardDisplay();

        /// <summary> The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        /// <summary> Initializes a new instance of the <see cref="MainWindow"/> class. </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindowClosing;

            this.display.SetViewModel(this.viewModel);
            this.scoreboardPreview.ViewModel = this.viewModel;

            this.desiredWidth = (int)(SystemParameters.PrimaryScreenWidth * .36);
        }

        /// <summary> Shuts down the application when the main window closes. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private static void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary> Shows/Hides the display when the "Show" button is clicked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ShowClicked(object sender, RoutedEventArgs e)
        {
            if (this.display.IsVisible)
            {
                this.display.Hide();
            }
            else
            {
                this.display.ViewboxWidth = this.desiredWidth;
                this.display.Show();
            }
        }

        private void WindowMovableChecked(object sender, RoutedEventArgs e)
        {
            this.display.IsWindowMovable = true;
        }

        private void WindowMovableUnchecked(object sender, RoutedEventArgs e)
        {
            this.display.IsWindowMovable = false;
        }

        private void ResetPositionClicked(object sender, RoutedEventArgs e)
        {
            this.display.ResetPosition();
        }
    }
}
