// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardDisplay.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Interaction logic for ScoreboardDisplay.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media.Animation;

    using Starboard.Model;

    /// <summary>
    /// Interaction logic for ScoreboardDisplay.xaml
    /// </summary>
    public partial class ScoreboardDisplay
    {
        /// <summary> The opacity used by the scoreboard when visible. The maximum value used during transitions. </summary>
        private double maxOpacity = 1;

        private ScoreboardControlViewModel scoreboard = null;

        /// <summary> Initializes a new instance of the <see cref="ScoreboardDisplay"/> class. </summary>
        public ScoreboardDisplay()
        {
            InitializeComponent();

            this.InitializePositionOnLoad = true;

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

        /// <summary> Gets or sets a value indicating whether the window should reset positions when first loaded. </summary>
        public bool InitializePositionOnLoad { get; set; }

        /// <summary> Gets or sets the maximum opacity used by the scoreboard. </summary>
        public double MaxOpacity
        {
            get
            {
                return this.maxOpacity;
            }

            set
            {
                this.maxOpacity = value;
                if (scoreboardControl.IsVisible)
                {
                    // Applying the opacity without an animation results in no change. Is there a better way?
                    var animation = new DoubleAnimation(value, new Duration(new TimeSpan(0, 0, 0, 0)));
                    scoreboardControl.BeginAnimation(OpacityProperty, animation);
                }
            }
        }

        /// <summary> Resets the position of the window to the default location, centered on the primary monitor with a 10px offset from top. </summary>
        public void ResetPosition()
        {
            if (this.IsMeasureValid == false)
            {
                this.UpdateLayout();
                this.Measure(new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight));
            }

            var leftAdjust = this.Width / 2.0;
            
            var left = (SystemParameters.PrimaryScreenWidth / 2.0) - leftAdjust;

            this.Left = left;
            this.Top = 10.0;
        }

        /// <summary> Sets the viewmodel for the window to another instance of ScoreboardControlViewModel </summary>
        /// <param name="vm"> The viewModel to apply. </param>
        public void SetViewModel(ScoreboardControlViewModel vm)
        {
            scoreboardControl.DataContext = vm;
            this.scoreboard = vm;
        }

        /// <summary> Overrides the original Show function to support a fade-in effect in cases where transparency is used. </summary>
        public new void Show()
        {
            if (this.AllowsTransparency)
            {
                scoreboardControl.Opacity = 0;
            }

            base.Show();

            if (this.AllowsTransparency)
            {
                var animation = new DoubleAnimation(this.MaxOpacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                scoreboardControl.BeginAnimation(OpacityProperty, animation);
            }
        }

        /// <summary> Overrides the original Hide function to support fading in cases where transparency is used. </summary>
        public new void Hide()
        {
            if (this.AllowsTransparency)
            {
                var animation = new DoubleAnimation(0, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                Action hideAction = base.Hide;
                animation.Completed += (sender, e) => hideAction();
                scoreboardControl.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                base.Hide();
            }
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

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            Debug.WriteLine(e.SystemKey);
            Debug.WriteLine(e.Key);

            base.OnPreviewKeyDown(e);
        }

        /// <summary> Overrides the OnKeyDown event to handled hotkeys for this window. </summary>
        /// <param name="e"> The event arguments. </param>
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            /*
            Debug.WriteLine(e.SystemKey);
            Debug.WriteLine(e.Key);
            */
            if (e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                var result = ApplyHotkey(e.Key, this.scoreboard.Player1);
                e.Handled = result;
            }

            if (e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Alt && e.SystemKey != System.Windows.Input.Key.LeftAlt)
            {
                var result = ApplyHotkey(e.SystemKey, this.scoreboard.Player2);
                e.Handled = result;
            }
            else if (e.SystemKey == System.Windows.Input.Key.LeftAlt)
            {

            }
            else
            {
                int i = 0;
                i++;
            }

            //base.OnKeyDown(e);
        }

        /// <summary> Applies the hotkey for the attached key to the attached player. </summary>
        /// <param name="key"> The key which was pressed in the hotkey sequence. </param>
        /// <param name="player"> The player to apply the change to. </param>
        /// <returns> Whether the key pressed was a valid hotkey, and was applied to the player. </returns>
        private static bool ApplyHotkey(System.Windows.Input.Key key, Player player)
        {
            var handled = true;

            switch (key)
            {
                case System.Windows.Input.Key.P:
                    player.Race = Race.Protoss;
                    break;
                case System.Windows.Input.Key.T:
                    player.Race = Race.Terran;
                    break;
                case System.Windows.Input.Key.Z:
                    player.Race = Race.Zerg;
                    break;
                case System.Windows.Input.Key.R:
                    player.Race = Race.Random;
                    break;
                case System.Windows.Input.Key.D1:
                case System.Windows.Input.Key.NumPad1:
                    player.Color = PlayerColor.Red;
                    break;
                case System.Windows.Input.Key.D2:
                case System.Windows.Input.Key.NumPad2:
                    player.Color = PlayerColor.Blue;
                    break;
                case System.Windows.Input.Key.D3:
                case System.Windows.Input.Key.NumPad3:
                    player.Color = PlayerColor.Teal;
                    break;
                case System.Windows.Input.Key.D4:
                case System.Windows.Input.Key.NumPad4:
                    player.Color = PlayerColor.Purple;
                    break;
                case System.Windows.Input.Key.D5:
                case System.Windows.Input.Key.NumPad5:
                    player.Color = PlayerColor.Yellow;
                    break;
                case System.Windows.Input.Key.D6:
                case System.Windows.Input.Key.NumPad6:
                    player.Color = PlayerColor.Orange;
                    break;
                case System.Windows.Input.Key.D7:
                case System.Windows.Input.Key.NumPad7:
                    player.Color = PlayerColor.Green;
                    break;
                case System.Windows.Input.Key.D8:
                case System.Windows.Input.Key.NumPad8:
                    player.Color = PlayerColor.LightPink;
                    break;
                case System.Windows.Input.Key.OemPlus:
                case System.Windows.Input.Key.Add:
                    player.Score++;
                    break;
                case System.Windows.Input.Key.OemMinus:
                case System.Windows.Input.Key.Subtract:
                    player.Score--;
                    break;
                default:
                    handled = false;
                    break;
            }

            return handled;
        }

        /// <summary> Resets the position of the window after it has completed loading. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event parameters. </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (this.InitializePositionOnLoad)
            {
                this.ResetPosition();                
            }
        }
    }
}
