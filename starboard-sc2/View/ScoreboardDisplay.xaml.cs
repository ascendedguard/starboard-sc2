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
            this.IsWindowMovable = false;
        }

        void ScoreboardDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            var leftAdjust = this.Width / 2.0;
            var left = (SystemParameters.PrimaryScreenWidth / 2.0) - leftAdjust;

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

        public bool IsWindowMovable { get; set; }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.IsWindowMovable)
            {
                this.DragMove();
            }
        }
    }
}
