// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Model for a Player, containing the player's name, color, race and score, implemented as a DependencyObject.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Model
{
    using System.Windows;

    /// <summary> Model for a Player, containing the player's name, color, race and score, implemented as a DependencyObject. </summary>
    public class Player : Freezable
    {
        /// <summary> DependencyProperty for the Name property. </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Player), new UIPropertyMetadata("Player"));

        /// <summary> DependencyProperty for the Color property. </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(PlayerColor), typeof(Player), new UIPropertyMetadata(PlayerColor.Unknown, ColorChangedCallback));

        /// <summary> DependencyProperty for the Race property. </summary>
        public static readonly DependencyProperty RaceProperty =
            DependencyProperty.Register("Race", typeof(Race), typeof(Player), new UIPropertyMetadata(Race.Terran));

        /// <summary> DependencyProperty for the Score property. </summary>
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        /// <summary> Triggers when the player's color changes. </summary>
        public event DependencyPropertyChangedEventHandler ColorChanged;

        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the player color.
        /// </summary>
        public PlayerColor Color
        {
            get { return (PlayerColor)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the race of the player.
        /// </summary>
        public Race Race
        {
            get { return (Race)GetValue(RaceProperty); }
            set { SetValue(RaceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current score.
        /// </summary>
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        /// <summary> Implementation for a Freezable object. </summary>
        /// <returns> A new instance of the Player class. </returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Player();
        }

        /// <summary> Triggers the event saying the PlayerColor has changed, for the ScoreboardControl object. </summary>
        /// <param name="d"> The dependency object the change has occured to. </param>
        /// <param name="e"> The event arguments containing the new color value. </param>
        private static void ColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var player = (Player)d;

            if (player.ColorChanged != null)
            {
                player.ColorChanged(d, e);
            }
        }
    }
}
