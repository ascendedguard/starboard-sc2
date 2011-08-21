namespace Starboard
{
    using System.Windows;

    using Starboard.MVVM;
    using Starboard.View;
    using Starboard.ViewModel;

    public class MainWindowViewModel : ObservableObject
    {
        /// <summary> Settings files stored to the registry for retaining last-used settings </summary>
        private readonly Settings settings = Settings.Load();

        /// <summary> Window controlling the scoreboard display </summary>
        private static ScoreboardDisplay display = new ScoreboardDisplay();

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

        public void CloseNetwork()
        {
            this.settingsPanelViewModel.CloseNetworkConnections();
        }

        private ObservableObject activeViewModel;

        public ObservableObject ActiveViewModel
        {
            get
            {
                return this.activeViewModel;
            }

            set
            {
                this.activeViewModel = value;
                RaisePropertyChanged("ActiveViewModel");
            }
        }

        private ScoreboardControlPanelViewModel scoreboardControlViewModel;

        private SettingsPanelViewModel settingsPanelViewModel;

        public MainWindowViewModel()
        {
            this.scoreboardControlViewModel = new ScoreboardControlPanelViewModel(this.Scoreboard);
            this.settingsPanelViewModel = new SettingsPanelViewModel(this.settings, this.scoreboardControlViewModel);
            display.SetViewModel(this.Scoreboard);

            if (this.settings.Position.X != 0 || this.settings.Position.Y != 0)
            {
                display.InitializePositionOnLoad = false;
                display.SetValue(Window.TopProperty, this.settings.Position.Y);
                display.SetValue(Window.LeftProperty, this.settings.Position.X);
            }

            this.ActiveViewModel = this.scoreboardControlViewModel;
        }


        public void SaveSettings()
        {
            this.settings.Size = this.settingsPanelViewModel.ViewboxWidth;
            this.settings.Position = new Point(MainWindowViewModel.DisplayWindow.Left, MainWindowViewModel.DisplayWindow.Top);
            this.settings.AllowTransparency = MainWindowViewModel.DisplayWindow.AllowsTransparency;

            this.settings.Save();
        }

        private ScoreboardControlViewModel scoreboard = new ScoreboardControlViewModel();

        public ScoreboardControlViewModel Scoreboard
        {
            get
            {
                return this.scoreboard;
            }
        }

        private bool isSettingsVisible;

        public bool IsSettingsVisible
        {
            get
            {
                return this.isSettingsVisible;
            }

            set
            {
                this.isSettingsVisible = value;
                RaisePropertyChanged("IsSettingsVisible");

                if (value)
                {
                    this.ActiveViewModel = this.settingsPanelViewModel;
                }
                else
                {
                    this.ActiveViewModel = this.scoreboardControlViewModel;
                }
            }
        }
        
    }
}
