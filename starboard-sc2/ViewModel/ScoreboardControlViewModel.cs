// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControlViewModel.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   ViewModel controlling all the information necessary for databinding the scoreboards.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using Starboard.Model;

    /// <summary> ViewModel controlling all the information necessary for databinding the scoreboards. </summary>
    public class ScoreboardControlViewModel : DependencyObject
    {
        /// <summary> DependencyProperty for the MatchupType property. </summary>
        public static readonly DependencyProperty MatchupTypeProperty =
            DependencyProperty.Register("MatchupType", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata("King of the Hill"));

        /// <summary> DependencyProperty for the IsAnnouncementShowing property. </summary>
        public static readonly DependencyProperty IsAnnouncementShowingProperty =
            DependencyProperty.Register("IsAnnouncementShowing", typeof(bool), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(false));

        /// <summary> DependencyProperty for the IsSubbarShowing property. </summary>
        public static readonly DependencyProperty IsSubbarShowingProperty =
            DependencyProperty.Register("IsSubbarShowing", typeof(bool), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(false));

        /// <summary> Holds our first player, which is initialized on creation. </summary>
        private readonly Player player1 = new Player();

        /// <summary> Holds our second player, which is initialized on creation. </summary>
        private readonly Player player2 = new Player();

        /// <summary> Contains the array of TimedText objects defining the subbar text. </summary>
        private readonly ObservableCollection<TimedText> subbarText = new ObservableCollection<TimedText>();

        /// <summary> Contains the array of TimedText objects defining the announcement text. </summary>
        private readonly ObservableCollection<TimedText> announcementText = new ObservableCollection<TimedText>();

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

        /// <summary> Gets the array containing the subbar text fields. </summary>
        public ObservableCollection<TimedText> SubbarText
        {
            get
            {
                return this.subbarText;
            }
        }

        /// <summary> Gets the array containing the announcement text fields. </summary>
        public ObservableCollection<TimedText> AnnouncementText
        {
            get
            {
                return this.announcementText;
            }
        }

        /// <summary> Gets or sets a value indicating whether the announcement is showing. </summary>
        public bool IsAnnouncementShowing
        {
            get { return (bool)GetValue(IsAnnouncementShowingProperty); }
            set { SetValue(IsAnnouncementShowingProperty, value); }
        }

        /// <summary> Gets or sets a value indicating whether the subbar is showing. </summary>
        public bool IsSubbarShowing
        {
            get { return (bool)GetValue(IsSubbarShowingProperty); }
            set { SetValue(IsSubbarShowingProperty, value); }
        }
    }
}
