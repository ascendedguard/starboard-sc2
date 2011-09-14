// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerView.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Interaction logic for PlayerView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System.Windows;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView
    {
        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "PlayerView" /> class. </summary>
        public PlayerView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary> Increment's the player's score by 1. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void IncrementPlayerScore(object sender, RoutedEventArgs e)
        {
            var player = this.DataContext as Player;

            if (player != null)
            {
                player.Score++;
            }
        }

        #endregion
    }
}