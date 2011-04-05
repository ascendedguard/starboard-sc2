// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControlViewModel.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Defines the ScoreboardControlViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Starboard.Scoreboard
{
    using System.Windows;

    using Starboard.Model;

    /// <summary> ViewModel controlling all the information necessary for databinding the scoreboards. </summary>
    public class ScoreboardControlViewModel : DependencyObject
    {
        /// <summary> DependencyProperty for the MatchupType property. </summary>
        public static readonly DependencyProperty MatchupTypeProperty =
            DependencyProperty.Register("MatchupType", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata("King of the Hill"));

        /// <summary> DependencyProperty for the SubbarLine1 property. </summary>
        public static readonly DependencyProperty SubbarLine1Property =
            DependencyProperty.Register("SubbarLine1", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        /// <summary> DependencyProperty for the SubbarLine2 property. </summary>
        public static readonly DependencyProperty SubbarLine2Property =
            DependencyProperty.Register("SubbarLine2", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        /// <summary> DependencyProperty for the SubbarLine3 property. </summary>
        public static readonly DependencyProperty SubbarLine3Property =
            DependencyProperty.Register("SubbarLine3", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        /// <summary> DependencyProperty for the SubbarTime1 property. </summary>
        public static readonly DependencyProperty SubbarTime1Property =
            DependencyProperty.Register("SubbarTime1", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        /// <summary> DependencyProperty for the SubbarTime2 property. </summary>
        public static readonly DependencyProperty SubbarTime2Property =
            DependencyProperty.Register("SubbarTime2", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        /// <summary> DependencyProperty for the SubbarTime3 property. </summary>
        public static readonly DependencyProperty SubbarTime3Property =
            DependencyProperty.Register("SubbarTime3", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        /// <summary> Holds our first player, which is initialized on creation. </summary>
        private readonly Player player1 = new Player();

        /// <summary> Holds our second player, which is initialized on creation. </summary>
        private readonly Player player2 = new Player();

        /// <summary> Gets the first player's information. </summary>
        public Player Player1
        {
            get { return this.player1; }
        }

        /// <summary> Gets the second player's information. </summary>
        public Player Player2
        {
            get { return this.player2; }
        }

        /// <summary> Gets or sets the matchup type, the bottom line displayed on the matchup. </summary>
        public string MatchupType
        {
            get { return (string)GetValue(MatchupTypeProperty); }
            set { SetValue(MatchupTypeProperty, value); }
        }

        /// <summary> Gets or sets the first line of text to show in the subbar. </summary>
        public string SubbarLine1
        {
            get { return (string)GetValue(SubbarLine1Property); }
            set { SetValue(SubbarLine1Property, value); }
        }

        /// <summary> Gets or sets the second line of text to show in the subbar. </summary>
        public string SubbarLine2
        {
            get { return (string)GetValue(SubbarLine2Property); }
            set { SetValue(SubbarLine2Property, value); }
        }

        /// <summary> Gets or sets the third line of text to show in the subbar. </summary>
        public string SubbarLine3
        {
            get { return (string)GetValue(SubbarLine3Property); }
            set { SetValue(SubbarLine3Property, value); }
        }

        /// <summary> Gets or sets the time, in seconds, to display the first line of text. </summary>
        public int SubbarTime1
        {
            get { return (int)GetValue(SubbarTime1Property); }
            set { SetValue(SubbarTime1Property, value); }
        }

        /// <summary> Gets or sets the time, in seconds, to display the second line of text. </summary>
        public int SubbarTime2
        {
            get { return (int)GetValue(SubbarTime2Property); }
            set { SetValue(SubbarTime2Property, value); }
        }

        /// <summary> Gets or sets the time, in seconds, to display the third line of text. </summary>
        public int SubbarTime3
        {
            get { return (int)GetValue(SubbarTime3Property); }
            set { SetValue(SubbarTime3Property, value); }
        }

        public string AnnouncementText
        {
            get { return (string)GetValue(AnnouncementTextProperty); }
            set { SetValue(AnnouncementTextProperty, value); }
        }

        public static readonly DependencyProperty AnnouncementTextProperty =
            DependencyProperty.Register("AnnouncementText", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        public bool IsAnnouncementTextShowing
        {
            get { return (bool)GetValue(IsAnnouncementTextShowingProperty); }
            set { SetValue(IsAnnouncementTextShowingProperty, value); }
        }

        public static readonly DependencyProperty IsAnnouncementTextShowingProperty =
            DependencyProperty.Register("IsAnnouncementTextShowing", typeof(bool), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(false));

        
    }
}
