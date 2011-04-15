// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimedText.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Simple class containing a Time, in seconds, and associated Text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Model
{
    using System.ComponentModel;

    /// <summary> Simple class containing a Time, in seconds, and associated Text. </summary>
    public class TimedText : INotifyPropertyChanged
    {
        /// <summary> Time delay to show the text, in seconds. </summary>
        private int time = 10;

        /// <summary> Message text </summary>
        private string text = string.Empty;

        /// <summary> PropertyChanged event, indiciating a class Property has changed. </summary>
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

        /// <summary> Triggers the PropertyChanged event for the requested property. </summary>
        /// <param name="property"> The property name. </param>
        private void OnPropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
