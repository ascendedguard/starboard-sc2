// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using Starboard.Model;
    using Starboard.Scoreboard;
    using Starboard.View;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary> The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        /// <summary> Window controlling the scoreboard display </summary>
        private ScoreboardDisplay display = new ScoreboardDisplay();

        /// <summary> Initializes a new instance of the <see cref="MainWindow"/> class. </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindowClosing;

            this.display.SetViewModel(this.viewModel);
            this.scoreboardPreview.DataContext = this.viewModel;

            this.desiredWidth = (int)(SystemParameters.PrimaryScreenWidth * .36);
            this.display.ViewboxWidth = this.desiredWidth;

            this.sldrSize.Minimum = (int)(SystemParameters.PrimaryScreenWidth * .10);
            this.sldrSize.Maximum = (int)(SystemParameters.PrimaryScreenWidth * .60);
            this.sldrSize.DataContext = this.display;
            this.sldrSize.SetBinding(RangeBase.ValueProperty, "ViewboxWidth");

            this.txtBuild.Text = string.Format("Build: {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        /// <summary> Shuts down the application when the main window closes. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private static void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary> Shows/Hides the display when the "Show" button is clicked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ShowClicked(object sender, RoutedEventArgs e)
        {
            if (this.display.IsVisible)
            {
                this.display.Hide();
            }
            else
            {
                this.display.Show();
            }
        }

        /// <summary> Enables the display to be moved when the "Is Window Movable" checkbox is checked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void WindowMovableChecked(object sender, RoutedEventArgs e)
        {
            this.display.IsWindowMovable = true;
        }

        /// <summary> Disables the display to be moved when the "Is Window Movable" checkbox is checked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void WindowMovableUnchecked(object sender, RoutedEventArgs e)
        {
            this.display.IsWindowMovable = false;
        }

        /// <summary> Resets the position of the scoreboard when the Reset button is clicked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ResetPositionClicked(object sender, RoutedEventArgs e)
        {
            this.display.ResetPosition();
        }

        /// <summary> Handles when the Show Announcement button is clicked. Toggles whether announcements are showing. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ShowAnnouncementClicked(object sender, RoutedEventArgs e)
        {
            this.viewModel.IsAnnouncementShowing = !this.viewModel.IsAnnouncementShowing;
        }

        /// <summary> Handles when the Show Subbar button is clicked. Toggles whether the subbar is showing. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ShowSubbarClicked(object sender, RoutedEventArgs e)
        {
            this.viewModel.IsSubbarShowing = !this.viewModel.IsSubbarShowing;
        }

        /// <summary> Adds a new TimedTextControl to the Subbar list. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AddSubbarClicked(object sender, RoutedEventArgs e)
        {
            var text = new TimedText();
            var ctrl = new TimedTextControl { TimedText = text };

            ctrl.RowDeleted += this.SubbarRowDeleted;

            this.viewModel.SubbarText.Add(text);
            spSubbar.Items.Add(ctrl);
        }

        /// <summary> Handles when a TimedTextControl requests to be removed. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void SubbarRowDeleted(object sender, EventArgs e)
        {
            var ctrl = (TimedTextControl)sender;

            this.viewModel.SubbarText.Remove(ctrl.TimedText);
            ctrl.RowDeleted -= this.SubbarRowDeleted;
            spSubbar.Items.Remove(ctrl);
        }

        /// <summary> Adds a new TimedTextControl to the Announcement list. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AddAnnouncementClicked(object sender, RoutedEventArgs e)
        {
            var text = new TimedText();
            var ctrl = new TimedTextControl { TimedText = text };

            ctrl.RowDeleted += this.AnnouncementRowDeleted;

            this.viewModel.AnnouncementText.Add(text);
            lbxAnnouncements.Items.Add(ctrl);
        }

        /// <summary> Handles when a TimedTextControl requests to be removed. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AnnouncementRowDeleted(object sender, EventArgs e)
        {
            var ctrl = (TimedTextControl)sender;

            this.viewModel.AnnouncementText.Remove(ctrl.TimedText);
            ctrl.RowDeleted -= this.SubbarRowDeleted;
            lbxAnnouncements.Items.Remove(ctrl);
        }

        /// <summary> Resets the Size slider to it's default value. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ResetSize(object sender, RoutedEventArgs e)
        {
            this.sldrSize.Value = this.desiredWidth;
        }

        /// <summary> Sets the binding of the transparency slider to the new display context. </summary>
        private void BindToTransparencySlider()
        {
            this.sldrTransparency.DataContext = this.display;
            this.sldrTransparency.SetBinding(RangeBase.ValueProperty, new Binding("MaxOpacity"));
        }
        
        /// <summary> Recreates the scoreboard display to allow transparency. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void TransparencyOptionChecked(object sender, RoutedEventArgs e)
        {
            var opacity = this.display.MaxOpacity;
            var showing = this.display.IsVisible;
            var size = this.sldrSize.Value;

            var left = this.display.Left;
            var top = this.display.Top;

            this.display.Close();
            this.display = null;

            this.display = new ScoreboardDisplay { AllowsTransparency = true };
            this.display.SetViewModel(this.viewModel);

            this.display.MaxOpacity = opacity;

            this.sldrTransparency.IsEnabled = true;
            this.BindToTransparencySlider();

            this.sldrSize.DataContext = this.display;
            this.sldrSize.Value = size;
            
            // Retain the previous position settings.
            this.display.InitializePositionOnLoad = false;
            this.display.IsWindowMovable = cbxWindowMovable.IsChecked == true;
            this.display.SetValue(TopProperty, top);
            this.display.SetValue(LeftProperty, left);

            if (showing)
            {
                this.display.Show();
            }
        }

        /// <summary> Recreates the scoreboard display to turn off transparency. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void TransparencyOptionUnchecked(object sender, RoutedEventArgs e)
        {
            var opacity = this.display.MaxOpacity;
            var showing = this.display.IsVisible;
            var size = this.sldrSize.Value;

            var left = this.display.Left;
            var top = this.display.Top;

            this.display.Close();
            this.display = null;

            this.display = new ScoreboardDisplay { AllowsTransparency = false };
            this.display.SetViewModel(this.viewModel);

            this.display.MaxOpacity = opacity;
            this.sldrTransparency.IsEnabled = false;

            this.sldrSize.DataContext = this.display;
            this.sldrSize.Value = size;

            // Retain the previous position settings.
            this.display.InitializePositionOnLoad = false;
            this.display.IsWindowMovable = cbxWindowMovable.IsChecked == true;
            this.display.SetValue(TopProperty, top);
            this.display.SetValue(LeftProperty, left);

            if (showing)
            {
                this.display.Show();
            }
        }
    }
}
