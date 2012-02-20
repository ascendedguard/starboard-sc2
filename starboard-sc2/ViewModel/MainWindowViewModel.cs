// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   The main window view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.ViewModel
{
    using System.Windows;

    using Starboard.MVVM;
    using Starboard.View;

    /// <summary> The view model for the main application window. </summary>
    public class MainWindowViewModel : ObservableObject
    {
        #region Constants and Fields

        /// <summary>
        /// Settings files stored to the registry for retaining last-used settings
        /// </summary>
        private readonly Settings settings = Settings.Load();

        /// <summary>
        /// The scoreboard view model used for the actual scoreboard.
        /// </summary>
        private readonly ScoreboardControlViewModel scoreboard = new ScoreboardControlViewModel();

        /// <summary>
        /// The scoreboard control view model.
        /// </summary>
        private readonly ScoreboardControlPanelViewModel scoreboardControlViewModel;

        /// <summary>
        /// The settings panel view model.
        /// </summary>
        private readonly SettingsPanelViewModel settingsPanelViewModel;

        /// <summary>
        ///   Window controlling the scoreboard display
        /// </summary>
        private static ScoreboardDisplay display = new ScoreboardDisplay();

        /// <summary>
        /// Whether the settings panel is currently visible.
        /// </summary>
        private bool isSettingsVisible;

        /// <summary>
        /// Value describing whether the main settings panel is currently showing. Used for databinding.
        /// </summary>
        private bool isMainSettingsVisible;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref="MainWindowViewModel"/> class. </summary>
        public MainWindowViewModel()
        {
            this.scoreboardControlViewModel = new ScoreboardControlPanelViewModel(this.Scoreboard);
            this.settingsPanelViewModel = new SettingsPanelViewModel(this.settings, this.scoreboardControlViewModel);
            display.SetViewModel(this.Scoreboard);
            this.IsMainSettingsVisible = true;

            if (this.settings.Position.X != 0 || this.settings.Position.Y != 0)
            {
                display.InitializePositionOnLoad = false;
                display.SetValue(Window.TopProperty, this.settings.Position.Y);
                display.SetValue(Window.LeftProperty, this.settings.Position.X);
            }
        }

        /// <summary>
        /// Gets the view model for the scoreboard control panel (main screen).
        /// </summary>
        public ScoreboardControlPanelViewModel ScoreboardControlPanelViewModel
        {
            get
            {
                return this.scoreboardControlViewModel;
            }
        }

        /// <summary>
        /// Gets the view model for the settings panel.
        /// </summary>
        public SettingsPanelViewModel SettingsPanelViewModel
        {
            get
            {
                return this.settingsPanelViewModel;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>   Gets or sets the active display window being used. </summary>
        public static ScoreboardDisplay DisplayWindow
        {
            get
            {
                return display;
            }

            set
            {
                display = value;
            }
        }

        /// <summary> Gets or sets a value indicating whether the settings button is pressed, and the settings tab is visible. </summary>
        public bool IsSettingsVisible
        {
            get
            {
                return this.isSettingsVisible;
            }

            set
            {
                this.isSettingsVisible = value;
                this.RaisePropertyChanged("IsSettingsVisible");

                this.IsMainSettingsVisible = !value;
            }
        }

        public bool IsMainSettingsVisible
        {
            get
            {
                return this.isMainSettingsVisible;
            }

            set
            {
                this.isMainSettingsVisible = value;
                this.RaisePropertyChanged("IsMainSettingsVisible");
            }
        }

        /// <summary> Gets the scoreboard view model. </summary>
        public ScoreboardControlViewModel Scoreboard
        {
            get
            {
                return this.scoreboard;
            }
        }

        #endregion

        #region Public Methods

        /// <summary> Closes the active network connection. </summary>
        public void CloseNetwork()
        {
            this.settingsPanelViewModel.CloseNetworkConnections();
        }

        /// <summary> Saves the active settings to the registry. </summary>
        public void SaveSettings()
        {
            this.settings.Size = this.settingsPanelViewModel.ViewboxWidth;
            this.settings.Position = new Point(DisplayWindow.Left, DisplayWindow.Top);
            
            try
            {
                this.settings.AllowTransparency = DisplayWindow.AllowsTransparency;
            }
            catch (System.InvalidOperationException)
            {
                // Shouldn't happen, but if it did, default to the application default.
                this.settings.AllowTransparency = false;
            }

            this.settings.Save();
        }

        #endregion
    }
}