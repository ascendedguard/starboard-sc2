using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Starboard.View
{
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
            DependencyProperty.Register("SelectedRace", typeof(Race), typeof(RaceSelectionControl), new UIPropertyMetadata(Race.Terran));

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedRace = (Race)btn.Content;
        }
    }
}
