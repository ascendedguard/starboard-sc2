using System;
using System.Windows;

namespace Starboard.View
{
    using Starboard.Model;

    /// <summary>
    /// Interaction logic for TimedTextControl.xaml
    /// </summary>
    public partial class TimedTextControl
    {
        private TimedText timedText;

        /// <summary> Initializes a new instance of the <see cref="TimedTextControl"/> class. </summary>
        public TimedTextControl()
        {
            InitializeComponent();
        }

        public event EventHandler RowDeleted;

        public TimedText TimedText 
        { 
            get
            {
                return this.timedText;
            }  
        
            set
            {
                this.timedText = value;
                gridBase.DataContext = value;
            }
        }

        private void RemoveRowClicked(object sender, RoutedEventArgs e)
        {
            if (this.RowDeleted != null)
            {
                this.RowDeleted(this, EventArgs.Empty);
            }
        }
    }
}
