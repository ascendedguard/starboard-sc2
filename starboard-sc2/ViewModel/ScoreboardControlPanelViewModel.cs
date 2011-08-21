namespace Starboard.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Starboard.Model;
    using Starboard.MVVM;

    public class ScoreboardControlPanelViewModel : ObservableObject
    {
        public ScoreboardControlPanelViewModel(ScoreboardControlViewModel scoreboard)
        {
            this.scoreboard = scoreboard;
        }

        private ScoreboardControlViewModel scoreboard;

        public ScoreboardControlViewModel Scoreboard
        {
            get
            {
                return this.scoreboard;
            }
        }

        private ICommand toggleScoreboardVisibleCommand;
        public ICommand ToggleScoreboardVisibleCommand
        {
            get
            {
                return this.toggleScoreboardVisibleCommand ?? (this.toggleScoreboardVisibleCommand = new RelayCommand(this.ToggleScoreboardVisible));
            }
        }

        private ICommand toggleAnnouncementCommand;
        public ICommand ToggleAnnouncementCommand
        {
            get
            {
                return this.toggleAnnouncementCommand ?? (this.toggleAnnouncementCommand = new RelayCommand(this.ToggleAnnouncement));
            }
        }

        public void ToggleAnnouncement()
        {
            this.Scoreboard.IsAnnouncementShowing = !this.Scoreboard.IsAnnouncementShowing;
        }

        private ICommand toggleSubbarCommand;
        public ICommand ToggleSubbarCommand
        {
            get
            {
                return this.toggleSubbarCommand ?? (this.toggleSubbarCommand = new RelayCommand(this.ToggleSubbar));
            }
        }

        private ICommand swapPlayersCommand;
        public ICommand SwapPlayersCommand
        {
            get
            {
                return this.swapPlayersCommand ?? (this.swapPlayersCommand = new RelayCommand(this.SwapPlayers));
            }
        }

        public void SwapPlayers()
        {
            var player2 = (Player)this.Scoreboard.Player1.Clone();
            var player1 = (Player)this.Scoreboard.Player2.Clone();

            this.Scoreboard.Player1.Name = player1.Name;
            this.Scoreboard.Player1.Color = player1.Color;
            this.Scoreboard.Player1.Score = player1.Score;
            this.Scoreboard.Player1.Race = player1.Race;

            this.Scoreboard.Player2.Name = player2.Name;
            this.Scoreboard.Player2.Color = player2.Color;
            this.Scoreboard.Player2.Score = player2.Score;
            this.Scoreboard.Player2.Race = player2.Race;
        }

        public void ToggleSubbar()
        {
            this.Scoreboard.IsSubbarShowing = !this.Scoreboard.IsSubbarShowing;
        }

        public void ToggleScoreboardVisible()
        {
            var display = MainWindowViewModel.DisplayWindow;
            
            if (display.IsVisible)
            {
                display.Hide();
            }
            else
            {
                display.Show();
            }
        }

    }
}
