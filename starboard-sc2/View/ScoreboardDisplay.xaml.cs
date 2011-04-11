// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardDisplay.xaml.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Interaction logic for ScoreboardDisplay.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System.Windows;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for ScoreboardDisplay.xaml
    /// </summary>
    public partial class ScoreboardDisplay
    {
        /// <summary> Initializes a new instance of the <see cref="ScoreboardDisplay"/> class. </summary>
        public ScoreboardDisplay()
        {
            InitializeComponent();

            this.Loaded += this.WindowLoaded;
            this.IsWindowMovable = false;
        }

        /// <summary> Gets or sets the width of the viewbox used to render the scoreboard. </summary>
        public double ViewboxWidth
        {
            get { return viewBox.Width; }
            set { viewBox.Width = value; }
        }

        /// <summary> Gets or sets a value indicating whether the window can be dragged. </summary>
        public bool IsWindowMovable { get; set; }

        /// <summary> Resets the position of the window to the default location, centered on the primary monitor with a 10px offset from top. </summary>
        public void ResetPosition()
        {
            var leftAdjust = this.Width / 2.0;
            var left = (SystemParameters.PrimaryScreenWidth / 2.0) - leftAdjust;

            this.Left = left;
            this.Top = 10;   
        }

        /// <summary> Sets the viewmodel for the window to another instance of ScoreboardControlViewModel </summary>
        /// <param name="vm"> The viewModel to apply. </param>
        public void SetViewModel(ScoreboardControlViewModel vm)
        {
            scoreboardControl.ViewModel = vm;
        }

        /// <summary> Allows the window to be dragged if IsWindowMovable has been set. </summary>
        /// <param name="e"> The event arguments. </param>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.IsWindowMovable)
            {
                this.DragMove();
            }
        }

        /// <summary> Resets the position of the window after it has completed loading. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event parameters. </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.ResetPosition();
        }

        private class HotkeyChanges
        {
            public bool changeRace = false;
            public bool changeColor = false;
            public bool changeScore = false;

            public Race race = Race.Unknown;
            public PlayerColor color = PlayerColor.Unknown;

            public int scoreChange = 0;
        }

        private HotkeyChanges GetKeyChanges(System.Windows.Input.Key key)
        {
            var changes = new HotkeyChanges();

            switch (key)
            {
                case System.Windows.Input.Key.P:
                    changes.changeRace = true;
                    changes.race = Race.Protoss;
                    break;
                case System.Windows.Input.Key.T:
                    changes.changeRace = true;
                    changes.race = Race.Terran;
                    break;
                case System.Windows.Input.Key.Z:
                    changes.changeRace = true;
                    changes.race = Race.Zerg;
                    break;
                case System.Windows.Input.Key.R:
                    changes.changeRace = true;
                    changes.race = Race.Random;
                    break;
                case System.Windows.Input.Key.D1:
                case System.Windows.Input.Key.NumPad1:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Red;
                    break;
                case System.Windows.Input.Key.D2:
                case System.Windows.Input.Key.NumPad2:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Blue;
                    break;
                case System.Windows.Input.Key.D3:
                case System.Windows.Input.Key.NumPad3:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Teal;
                    break;
                case System.Windows.Input.Key.D4:
                case System.Windows.Input.Key.NumPad4:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Purple;
                    break;
                case System.Windows.Input.Key.D5:
                case System.Windows.Input.Key.NumPad5:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Yellow;
                    break;
                case System.Windows.Input.Key.D6:
                case System.Windows.Input.Key.NumPad6:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Orange;
                    break;
                case System.Windows.Input.Key.D7:
                case System.Windows.Input.Key.NumPad7:
                    changes.changeColor = true;
                    changes.color = PlayerColor.Green;
                    break;
                case System.Windows.Input.Key.D8:
                case System.Windows.Input.Key.NumPad8:
                    changes.changeColor = true;
                    changes.color = PlayerColor.LightPink;
                    break;
                case System.Windows.Input.Key.OemPlus:
                case System.Windows.Input.Key.Add:
                    changes.changeScore = true;
                    changes.scoreChange = 1;
                    break;
                case System.Windows.Input.Key.OemMinus:
                case System.Windows.Input.Key.Subtract:
                    changes.changeScore = true;
                    changes.scoreChange = -1;
                    break;
            }

            return changes;
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                var changes = GetKeyChanges(e.Key);

                if (changes.changeColor)
                {
                    scoreboardControl.ViewModel.Player1.Color = changes.color;
                    e.Handled = true;
                }

                if (changes.changeRace)
                {
                    scoreboardControl.ViewModel.Player1.Race = changes.race;
                    e.Handled = true;
                }

                if (changes.changeScore)
                {
                    scoreboardControl.ViewModel.Player1.Score += changes.scoreChange;
                    e.Handled = true;                    
                }
            }

            if (e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Alt)
            {
                var changes = GetKeyChanges(e.SystemKey);

                if (changes.changeColor)
                {
                    scoreboardControl.ViewModel.Player2.Color = changes.color;
                    e.Handled = true;
                }

                if (changes.changeRace)
                {
                    scoreboardControl.ViewModel.Player2.Race = changes.race;
                    e.Handled = true;
                }

                if (changes.changeScore)
                {
                    scoreboardControl.ViewModel.Player2.Score += changes.scoreChange;
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }
    }
}
