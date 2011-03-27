// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
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
        private int desiredWidth;

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += this.MainWindow_Closing;

            this.display.SetViewModel(this.viewModel);
            this.scoreboardPreview.ViewModel = this.viewModel;

            this.desiredWidth = (int)(SystemParameters.PrimaryScreenWidth * .36);
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.display.IsVisible)
            {
                this.display.Close();
            }
        }

        private ScoreboardDisplay display = new ScoreboardDisplay();

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
    }
}
