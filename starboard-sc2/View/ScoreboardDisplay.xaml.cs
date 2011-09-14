// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardDisplay.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   View for the scoreboard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using Starboard.Model;
    using Starboard.ViewModel;

    /// <summary>
    /// Interaction logic for ScoreboardDisplay.xaml
    /// </summary>
    public partial class ScoreboardDisplay
    {
        #region Constants and Fields

        /// <summary> The opacity used by the scoreboard when visible. The maximum value used during transitions. </summary>
        private double maxOpacity = 1;

        /// <summary> ViewModel for the scoreboard. </summary>
        private ScoreboardControlViewModel scoreboard;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "ScoreboardDisplay" /> class. </summary>
        public ScoreboardDisplay()
        {
            this.InitializeComponent();

            this.InitializePositionOnLoad = true;

            this.Loaded += this.WindowLoaded;
            this.IsWindowMovable = false;
        }

        #endregion

        #region Public Properties

        /// <summary> Gets or sets a value indicating whether the window should reset positions when first loaded. </summary>
        public bool InitializePositionOnLoad { get; set; }

        /// <summary> Gets or sets a value indicating whether the window can be dragged. </summary>
        public bool IsWindowMovable { get; set; }

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
                if (this.scoreboardControl.IsVisible)
                {
                    // Applying the opacity without an animation results in no change. Is there a better way?
                    var animation = new DoubleAnimation(value, new Duration(new TimeSpan(0, 0, 0, 0)));
                    this.scoreboardControl.BeginAnimation(OpacityProperty, animation);
                }
            }
        }

        /// <summary> Gets or sets the width of the viewbox used to render the scoreboard. </summary>
        public double ViewboxWidth
        {
            get
            {
                return this.viewBox.Width;
            }

            set
            {
                this.viewBox.Width = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary> Overrides the original Hide function to support fading in cases where transparency is used. </summary>
        public new void Hide()
        {
            if (this.AllowsTransparency)
            {
                var animation = new DoubleAnimation(0, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                Action hideAction = base.Hide;
                animation.Completed += (sender, e) => hideAction();
                this.scoreboardControl.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                base.Hide();
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
        /// <param name="vm"> The viewModel to apply.  </param>
        public void SetViewModel(ScoreboardControlViewModel vm)
        {
            this.scoreboardControl.DataContext = vm;
            this.scoreboard = vm;
        }

        /// <summary> Overrides the original Show function to support a fade-in effect in cases where transparency is used. </summary>
        public new void Show()
        {
            if (this.AllowsTransparency)
            {
                this.scoreboardControl.Opacity = 0;
            }

            base.Show();

            if (this.AllowsTransparency)
            {
                var animation = new DoubleAnimation(this.MaxOpacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                this.scoreboardControl.BeginAnimation(OpacityProperty, animation);
            }
        }

        #endregion

        #region Methods

        /// <summary> Overrides the OnKeyDown event to handled hotkeys for this window. </summary>
        /// <param name="e"> The event arguments.  </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                // Change Player 1's Name
                this.CreatePlayerChangeField("Player1.Name");
            }
            else if (e.Key == Key.F2)
            {
                // Change Player 2's Name
                this.CreatePlayerChangeField("Player2.Name");
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                var result = ApplyHotkey(e.Key, this.scoreboard.Player1);
                e.Handled = result;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt
                && e.SystemKey != Key.LeftAlt)
            {
                var result = ApplyHotkey(e.SystemKey, this.scoreboard.Player2);
                e.Handled = result;
            }
        }

        /// <summary> Allows the window to be dragged if IsWindowMovable has been set. </summary>
        /// <param name="e"> The event arguments.  </param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.IsWindowMovable)
            {
                this.DragMove();
            }
        }

        /// <summary> Applies the hotkey for the attached key to the attached player. </summary>
        /// <param name="key"> The key which was pressed in the hotkey sequence.  </param>
        /// <param name="player"> The player to apply the change to.  </param>
        /// <returns> Whether the key pressed was a valid hotkey, and was applied to the player.  </returns>
        private static bool ApplyHotkey(Key key, Player player)
        {
            var handled = true;

            switch (key)
            {
                case Key.P:
                    player.Race = Race.Protoss;
                    break;
                case Key.T:
                    player.Race = Race.Terran;
                    break;
                case Key.Z:
                    player.Race = Race.Zerg;
                    break;
                case Key.R:
                    player.Race = Race.Random;
                    break;
                case Key.D1:
                case Key.NumPad1:
                    player.Color = PlayerColor.Red;
                    break;
                case Key.D2:
                case Key.NumPad2:
                    player.Color = PlayerColor.Blue;
                    break;
                case Key.D3:
                case Key.NumPad3:
                    player.Color = PlayerColor.Teal;
                    break;
                case Key.D4:
                case Key.NumPad4:
                    player.Color = PlayerColor.Purple;
                    break;
                case Key.D5:
                case Key.NumPad5:
                    player.Color = PlayerColor.Yellow;
                    break;
                case Key.D6:
                case Key.NumPad6:
                    player.Color = PlayerColor.Orange;
                    break;
                case Key.D7:
                case Key.NumPad7:
                    player.Color = PlayerColor.Green;
                    break;
                case Key.D8:
                case Key.NumPad8:
                    player.Color = PlayerColor.LightPink;
                    break;
                case Key.OemPlus:
                case Key.Add:
                    player.Score++;
                    break;
                case Key.OemMinus:
                case Key.Subtract:
                    player.Score--;
                    break;
                default:
                    handled = false;
                    break;
            }

            return handled;
        }

        /// <summary> Creates a text field for typing the player name directly into the scoreboard. Clears the player name upon the hotkey press. </summary>
        /// <param name="binding"> The binding to apply to the TextBox.  </param>
        private void CreatePlayerChangeField(string binding)
        {
            var field = new TextBox();
            field.SetBinding(
                TextBox.TextProperty, 
                new Binding(binding)
                    {
                       Source = this.scoreboard, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged 
                    });
            field.Width = 50;
            field.Height = 20;

            field.LostFocus += (sender, e) => this.rootGrid.Children.Remove(field);
            field.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter || e.Key == Key.Return
                        || e.Key == Key.Escape)
                    {
                        this.rootGrid.Children.Remove(field);
                    }
                };

            this.rootGrid.Children.Insert(0, field);

            field.Focus();
            field.Text = string.Empty;
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

        #endregion
    }
}