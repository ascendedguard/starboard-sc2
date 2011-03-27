using System.Windows;

namespace Starboard.Scoreboard
{
    /// <summary>
    /// Interaction logic for ScoreboardDisplay.xaml
    /// </summary>
    public partial class ScoreboardDisplay : Window
    {
        public ScoreboardDisplay()
        {
            InitializeComponent();

            this.Loaded += ScoreboardDisplay_Loaded;

        }

        void ScoreboardDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            var leftAdjust = this.Width / 2.0;
            var left = SystemParameters.PrimaryScreenWidth / 2.0 - leftAdjust;

            this.Left = left;
            this.Top = 10;
        }

        public void SetViewModel(ScoreboardControlViewModel vm)
        {
            scoreboardControl.ViewModel = vm;
        }

        public double ViewboxWidth
        {
            get { return viewBox.Width; }
            set { viewBox.Width = value; }
        }
    }
}
