// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControlViewModel.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   ViewModel controlling all the information necessary for databinding the scoreboards.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.ViewModel
{
    using System.Collections.ObjectModel;

    using Starboard.Model;
    using Starboard.MVVM;

    /// <summary>
    /// ViewModel controlling all the information necessary for databinding the scoreboards.
    /// </summary>
    public class ScoreboardControlViewModel : ObservableObject
    {
        #region Constants and Fields

        /// <summary>
        ///   Contains the array of TimedText objects defining the announcement text.
        /// </summary>
        private readonly ObservableCollection<TimedText> announcementText = new ObservableCollection<TimedText>();

        /// <summary>
        ///   Holds our first player, which is initialized on creation.
        /// </summary>
        private readonly Player player1 = new Player();

        /// <summary>
        ///   Holds our second player, which is initialized on creation.
        /// </summary>
        private readonly Player player2 = new Player();

        /// <summary>
        ///   Contains the array of TimedText objects defining the subbar text.
        /// </summary>
        private readonly ObservableCollection<TimedText> subbarText = new ObservableCollection<TimedText>();

        /// <summary>
        ///   Backing property for the IsAnnouncementShowing property.
        /// </summary>
        private bool isAnnouncementShowing;

        /// <summary>
        ///   Backing property for the isSubbarShowing property.
        /// </summary>
        private bool isSubbarShowing;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the array containing the announcement text fields.
        /// </summary>
        public ObservableCollection<TimedText> AnnouncementText
        {
            get
            {
                return this.announcementText;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the announcement bar is showing.
        /// </summary>
        public bool IsAnnouncementShowing
        {
            get
            {
                return this.isAnnouncementShowing;
            }

            set
            {
                this.isAnnouncementShowing = value;
                this.RaisePropertyChanged("IsAnnouncementShowing");
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the subbar is showing.
        /// </summary>
        public bool IsSubbarShowing
        {
            get
            {
                return this.isSubbarShowing;
            }

            set
            {
                this.isSubbarShowing = value;
                this.RaisePropertyChanged("IsSubbarShowing");
            }
        }

        /// <summary>
        ///   Gets the first player's information.
        /// </summary>
        public Player Player1
        {
            get
            {
                return this.player1;
            }
        }

        /// <summary>
        ///   Gets the second player's information.
        /// </summary>
        public Player Player2
        {
            get
            {
                return this.player2;
            }
        }

        /// <summary>
        ///   Gets the array containing the subbar text fields.
        /// </summary>
        public ObservableCollection<TimedText> SubbarText
        {
            get
            {
                return this.subbarText;
            }
        }

        public void SwapPlayers()
        {
            var player2 = (Player)this.Player1.Clone();
            var player1 = (Player)this.Player2.Clone();

            this.Player1.Name = player1.Name;
            this.Player1.Color = player1.Color;
            this.Player1.Score = player1.Score;
            this.Player1.Race = player1.Race;

            this.Player2.Name = player2.Name;
            this.Player2.Color = player2.Color;
            this.Player2.Score = player2.Score;
            this.Player2.Race = player2.Race;
        }

        #endregion
    }
}