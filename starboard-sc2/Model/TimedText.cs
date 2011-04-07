namespace Starboard.Model
{
    using System.ComponentModel;

    public class TimedText : INotifyPropertyChanged
    {
        /// <summary> Time delay to show the text, in seconds. </summary>
        private int time = 10;

        /// <summary> Message text </summary>
        private string text = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Gets or sets the text to be displayed. </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            { 
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }

        /// <summary> Gets or sets the time to display the text, in seconds. </summary>
        public int Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.OnPropertyChanged("Time");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
