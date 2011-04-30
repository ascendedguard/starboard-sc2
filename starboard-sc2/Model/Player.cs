// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Model for a Player, containing the player's name, color, race and score.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Model
{
    using Starboard.MVVM;

    /// <summary> Model for a Player, containing the player's name, color, race and score.</summary>
    public class Player : ObservableObject
    {
        /// <summary> The player's name, defaulting to "Player". </summary>
        private string name = "Player";

        /// <summary> The player's color, default to "Unknown" </summary>
        private PlayerColor color = PlayerColor.Unknown;

        /// <summary> The player's race, defaulting to terran. </summary>
        private Race race = Race.Unknown;

        /// <summary> The current score, starting at 0. </summary>
        private int score;

        /// <summary> Gets or sets the name of the player. </summary>
        public string Name
        {
            get 
            { 
                return this.name; 
            }

            set
            {
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary> Gets or sets the color of the player. </summary>
        public PlayerColor Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                RaisePropertyChanged("Color");
            }
        }

        /// <summary> Gets or sets the race of the player. </summary>
        public Race Race
        {
            get
            {
                return this.race;
            }

            set
            {
                this.race = value;
                RaisePropertyChanged("Race");
            }
        }

        /// <summary> Gets or sets the player's current score. </summary>
        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                this.score = value < 0 ? 0 : value;
                RaisePropertyChanged("Score");
            }
        }
    }
}
