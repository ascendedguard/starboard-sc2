// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Starboard">
//   Copright 2011
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Model
{
    using System.Windows;

    public class Player : Freezable
    {
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(Player), new UIPropertyMetadata(0));

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Player), new UIPropertyMetadata("Player"));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(PlayerColor), typeof(Player), new UIPropertyMetadata(PlayerColor.Unknown, ColorChangedCallback));

        public static readonly DependencyProperty RaceProperty =
            DependencyProperty.Register("Race", typeof(Race), typeof(Player), new UIPropertyMetadata(Race.Terran));

        public event DependencyPropertyChangedEventHandler ColorChanged;

        private static void ColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Player player = (Player)d;

            if (player.ColorChanged != null)
            {
                player.ColorChanged(d, e);
            }
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public PlayerColor Color
        {
            get { return (PlayerColor)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Race Race
        {
            get { return (Race)GetValue(RaceProperty); }
            set { SetValue(RaceProperty, value); }
        }
        
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Player();
        }
    }
}
