namespace Starboard.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for ColorSelectionControl.xaml
    /// </summary>
    public partial class ColorSelectionControl
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
            DependencyProperty.Register("SelectedColor", typeof(PlayerColor), typeof(ColorSelectionControl), new UIPropertyMetadata(PlayerColor.Unknown));

        

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedColor = (PlayerColor)btn.Content;
        }
    }
}
