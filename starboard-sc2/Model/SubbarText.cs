// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubbarText.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Defines a segment of text to be displayed on the subbar of the scoreboard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AscendStudio.Model
{
    using System.Windows;

    /// <summary> Defines a segment of text to be displayed on the subbar of the scoreboard </summary>
    public class SubbarText : DependencyObject
    {
        /// <summary> DependencyProperty for the Text property. </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SubbarText), new UIPropertyMetadata(string.Empty));

        /// <summary> DependencyProperty for the Time property. </summary>
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(int), typeof(SubbarText), new UIPropertyMetadata(30));

        /// <summary> Gets or sets the Text to be displayed in the subbar. </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary> Gets or sets the time duration, in seconds, to display the subbar text. </summary>
        public int Time
        {
            get { return (int)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
    }
}
