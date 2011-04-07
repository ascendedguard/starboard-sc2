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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Starboard.Model;

    /// <summary> ViewModel controlling all the information necessary for databinding the scoreboards. </summary>
    public class ScoreboardControlViewModel : DependencyObject
    {
        /// <summary> DependencyProperty for the MatchupType property. </summary>
        public static readonly DependencyProperty MatchupTypeProperty =
            DependencyProperty.Register("MatchupType", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata("King of the Hill"));

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

        public ObservableCollection<TimedText> SubbarText = new ObservableCollection<TimedText>();
        public ObservableCollection<TimedText> AnnouncementText = new ObservableCollection<TimedText>();
        /*
        public string AnnouncementText
        {
            get { return (string)GetValue(AnnouncementTextProperty); }
            set { SetValue(AnnouncementTextProperty, value); }
        }

        public static readonly DependencyProperty AnnouncementTextProperty =
            DependencyProperty.Register("AnnouncementText", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));
        */
        public bool IsAnnouncementShowing
        {
            get { return (bool)GetValue(IsAnnouncementShowingProperty); }
            set { SetValue(IsAnnouncementShowingProperty, value); }
        }

        public static readonly DependencyProperty IsAnnouncementShowingProperty =
            DependencyProperty.Register("IsAnnouncementShowing", typeof(bool), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(false));

        public bool IsSubbarShowing
        {
            get { return (bool)GetValue(IsSubbarShowingProperty); }
            set { SetValue(IsSubbarShowingProperty, value); }
        }

        public static readonly DependencyProperty IsSubbarShowingProperty =
            DependencyProperty.Register("IsSubbarShowing", typeof(bool), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(false));

        
    }
}
