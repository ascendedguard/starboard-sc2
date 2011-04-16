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
    using Starboard.MVVM;

    /// <summary> Simple class containing a Time, in seconds, and associated Text. </summary>
    public class TimedText : ObservableObject
    {
        /// <summary> Time delay to show the text, in seconds. </summary>
        private int time = 10;

        /// <summary> Message text </summary>
        private string text = string.Empty;

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
                this.RaisePropertyChanged("Text");
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
                this.time = value < 1 ? 1 : value;

                this.RaisePropertyChanged("Time");
            }
        }
    }
}
