namespace Starboard.View
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for TimedTextEditGroupControl.xaml
    /// </summary>
    public partial class TimedTextEditGroupControl
    {
        public TimedTextEditGroupControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<TimedText> ViewModel
        {
            get
            {
                return this.DataContext as ObservableCollection<TimedText>;
            }
        }

        private void AddTextClicked(object sender, RoutedEventArgs e)
        {
            var text = new TimedText();
            var ctrl = new TimedTextControl { TimedText = text };

            ctrl.RowDeleted += this.RowDeleted;

            lstItems.Items.Add(ctrl);
            this.ViewModel.Add(text);
        }
            
        /// <summary> Handles when a TimedTextControl requests to be removed. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void RowDeleted(object sender, EventArgs e)
            {
                var ctrl = (TimedTextControl)sender;

                this.ViewModel.Remove(ctrl.TimedText);
                this.lstItems.Items.Remove(ctrl);

            ctrl.RowDeleted -= this.RowDeleted;
            }
    }
}
