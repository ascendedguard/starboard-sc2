// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Starboard">
//   Copyright 2011
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
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using Starboard.Model;
    using Starboard.Scoreboard;
    using Starboard.View;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary> Window controlling the scoreboard display </summary>
        private readonly ScoreboardDisplay display = new ScoreboardDisplay();

        /// <summary> The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        /// <summary> Initializes a new instance of the <see cref="MainWindow"/> class. </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindowClosing;

            this.display.SetViewModel(this.viewModel);
            this.scoreboardPreview.ViewModel = this.viewModel;

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
                this.display.ViewboxWidth = this.desiredWidth;
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

        private void btnShowAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.IsAnnouncementShowing = !this.viewModel.IsAnnouncementShowing;
        }

        private void btnShowSubbar_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.IsSubbarShowing = !this.viewModel.IsSubbarShowing;
        }

        private void AddSubbarClicked(object sender, RoutedEventArgs e)
        {
            TimedText text = new TimedText();
            var ctrl = new TimedTextControl { TimedText = text };

            ctrl.RowDeleted += this.ctrl_RowDeleted;

            this.viewModel.SubbarText.Add(text);
            spSubbar.Items.Add(ctrl);
        }

        void ctrl_RowDeleted(object sender, EventArgs e)
        {
            var ctrl = (TimedTextControl)sender;

            this.viewModel.SubbarText.Remove(ctrl.TimedText);
            ctrl.RowDeleted -= this.ctrl_RowDeleted;
            spSubbar.Items.Remove(ctrl);
        }

        private void AddAnnouncementClicked(object sender, RoutedEventArgs e)
        {
            TimedText text = new TimedText();
            var ctrl = new TimedTextControl() { TimedText = text };

            ctrl.RowDeleted += this.AnnouncementRowDeleted;

            this.viewModel.AnnouncementText.Add(text);
            lbxAnnouncements.Items.Add(ctrl);
        }

        private void AnnouncementRowDeleted(object sender, EventArgs e)
        {
            var ctrl = (TimedTextControl)sender;

            this.viewModel.AnnouncementText.Remove(ctrl.TimedText);
            ctrl.RowDeleted -= this.ctrl_RowDeleted;
            lbxAnnouncements.Items.Remove(ctrl);
        }
    }
}
