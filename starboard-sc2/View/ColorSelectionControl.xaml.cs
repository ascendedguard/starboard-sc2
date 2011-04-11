namespace Starboard.View
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for ColorSelectionControl.xaml
    /// </summary>
    public partial class ColorSelectionControl : INotifyPropertyChanged
    {
        /// <summary> Initializes a new instance of the <see cref="ColorSelectionControl"/> class. </summary>
        public ColorSelectionControl()
        {
            InitializeComponent();
        }

        public PlayerColor SelectedColor
        {
            get { return (PlayerColor)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(PlayerColor), typeof(ColorSelectionControl), new UIPropertyMetadata(PlayerColor.Unknown, colorChanged));

        private static void colorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (ColorSelectionControl)d;
            var color = (PlayerColor)e.NewValue;

            foreach (RadioButton control in userControl.gridBase.Children)
            {
                if ((PlayerColor)control.Content == color)
                {
                    control.IsChecked = true;
                    break;
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedColor = (PlayerColor)btn.Content;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(
                    this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
