namespace Starboard.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for RaceSelectionControl.xaml
    /// </summary>
    public partial class RaceSelectionControl
    {
        public RaceSelectionControl()
        {
            InitializeComponent();
        }

        public Race SelectedRace
        {
            get { return (Race)GetValue(SelectedRaceProperty); }
            set { SetValue(SelectedRaceProperty, value); }
        }

        public static readonly DependencyProperty SelectedRaceProperty =
            DependencyProperty.Register("SelectedRace", typeof(Race), typeof(RaceSelectionControl), new UIPropertyMetadata(Race.Terran, raceChanged));

        private static void raceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (RaceSelectionControl)d;
            var race = (Race)e.NewValue;

            foreach (RadioButton control in userControl.gridBase.Children)
            {
                if ((Race)control.Content == race)
                {
                    control.IsChecked = true;
                    break;
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedRace = (Race)btn.Content;
        }
    }
}
