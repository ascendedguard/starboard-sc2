// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControlViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ScoreboardControlViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Starboard.Scoreboard
{
    using System.Windows;

    using Starboard.Model;

    public class ScoreboardControlViewModel : DependencyObject
    {
        private Player player1 = new Player();

        private Player player2 = new Player();

        public Player Player1
        {
            get { return player1; }
        }

        public Player Player2
        {
            get { return player2; }
        }

        /// <summary> DependencyProperty for the MatchupType property. </summary>
        public static readonly DependencyProperty MatchupTypeProperty =
            DependencyProperty.Register("MatchupType", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata("King of the Hill"));

        /// <summary> Gets or sets the matchup type, the bottom line displayed on the matchup. </summary>
        public string MatchupType
        {
            get { return (string)GetValue(MatchupTypeProperty); }
            set { SetValue(MatchupTypeProperty, value); }
        }

        public string SubbarLine1
        {
            get { return (string)GetValue(SubbarLine1Property); }
            set { SetValue(SubbarLine1Property, value); }
        }

        public static readonly DependencyProperty SubbarLine1Property =
            DependencyProperty.Register("SubbarLine1", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        public string SubbarLine2
        {
            get { return (string)GetValue(SubbarLine2Property); }
            set { SetValue(SubbarLine2Property, value); }
        }

        public static readonly DependencyProperty SubbarLine2Property =
            DependencyProperty.Register("SubbarLine2", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        public string SubbarLine3
        {
            get { return (string)GetValue(SubbarLine3Property); }
            set { SetValue(SubbarLine3Property, value); }
        }

        public static readonly DependencyProperty SubbarLine3Property =
            DependencyProperty.Register("SubbarLine3", typeof(string), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(string.Empty));

        public int SubbarTime1
        {
            get { return (int)GetValue(SubbarTime1Property); }
            set { SetValue(SubbarTime1Property, value); }
        }

        public static readonly DependencyProperty SubbarTime1Property =
            DependencyProperty.Register("SubbarTime1", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        public int SubbarTime2
        {
            get { return (int)GetValue(SubbarTime2Property); }
            set { SetValue(SubbarTime2Property, value); }
        }

        public static readonly DependencyProperty SubbarTime2Property =
            DependencyProperty.Register("SubbarTime2", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        public int SubbarTime3
        {
            get { return (int)GetValue(SubbarTime3Property); }
            set { SetValue(SubbarTime3Property, value); }
        }

        public static readonly DependencyProperty SubbarTime3Property =
            DependencyProperty.Register("SubbarTime3", typeof(int), typeof(ScoreboardControlViewModel), new UIPropertyMetadata(10));

        
    }
}
