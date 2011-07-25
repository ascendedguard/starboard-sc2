// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using Starboard.Model;
    using Starboard.MVVM;
    using Starboard.Sockets;
    using Starboard.Sockets.Commands;
    using Starboard.View;
    using Starboard.ViewModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary> Settings files stored to the registry for retaining last-used settings </summary>
        private readonly Settings settings = Settings.Load();

        /// <summary> The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        /// <summary> Window controlling the scoreboard display </summary>
        private ScoreboardDisplay display = new ScoreboardDisplay();

        private ScoreboardNetwork network;

        public MainWindowViewModel()
        {
            this.display.SetViewModel(this.Scoreboard);

            this.AllowTransparency = this.settings.AllowTransparency;
            this.TransparencyLevel = this.settings.WindowTransparency;

            this.desiredWidth = this.settings.Size;

            if (this.settings.Position.X != 0 || this.settings.Position.Y != 0)
            {
                this.display.InitializePositionOnLoad = false;
                this.display.SetValue(Window.TopProperty, this.settings.Position.Y);
                this.display.SetValue(Window.LeftProperty, this.settings.Position.X);
            }

            this.WidthMinimum = (int)(SystemParameters.PrimaryScreenWidth * .10);
            this.WidthMaximum = (int)(SystemParameters.PrimaryScreenWidth * .60);
            this.ViewboxWidth = this.desiredWidth;
        }

        private ICommand toggleNetworkingCommand;
        public ICommand ToggleNetworkingCommand
        {
            get
            {
                return this.toggleNetworkingCommand ?? (this.toggleNetworkingCommand = new RelayCommand(this.ToggleNetworking));
            }
        }

        public void HandleRemoteCommands(ScoreboardPacket packet)
        {
            foreach (var command in packet.Commands)
            {
                switch (command.Command)
                {
                    case CommandType.UpdatePlayerName:
                        {
                            var cmd = command as StringPacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.Scoreboard.Player1.Name = cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.Scoreboard.Player2.Name = cmd.Data;
                                }
                            }

                            break;
                        }

                    case CommandType.UpdatePlayerScore:
                        {
                            var cmd = command as Int32PacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.Scoreboard.Player1.Score = cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.Scoreboard.Player2.Score = cmd.Data;
                                }
                            }

                            break;
                        }

                    case CommandType.UpdatePlayerColor:
                        {
                            var cmd = command as BytePacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.Scoreboard.Player1.Color = (PlayerColor)cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.Scoreboard.Player2.Color = (PlayerColor)cmd.Data;
                                }
                            }

                            break;
                        }

                    case CommandType.UpdatePlayerRace:
                        {
                            var cmd = command as BytePacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.Scoreboard.Player1.Race = (Race)cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.Scoreboard.Player2.Race = (Race)cmd.Data;
                                }
                            }

                            break;
                        }

                    case CommandType.ToggleAnnouncement:
                        this.ToggleAnnouncement();
                        break;

                    case CommandType.ToggleSubbar:
                        this.ToggleSubbar();
                        break;

                    case CommandType.SwapPlayer:
                        this.SwapPlayers();
                        break;

                    case CommandType.ShowScoreboard:
                        this.ToggleScoreboardVisible();
                        break;

                    case CommandType.HideScoreboard:
                        this.ToggleScoreboardVisible();
                        break;

                    case CommandType.ResetPlayer:
                        {
                            var cmd = command as EmptyPacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.Scoreboard.Player1.Reset();
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.Scoreboard.Player2.Reset();
                                }
                            }

                            break;
                        }
                }
            }
        }

        private string connectionButtonString = "Enable Remote";

        public string ConnectionButtonString
        {
            get
            {
                return this.connectionButtonString;
            }

            private set
            {
                this.connectionButtonString = value;
                RaisePropertyChanged("ConnectionButtonString");
            }
        }

        private bool isNetworkingEnabled;

        public bool IsNetworkingEnabled
        {
            get
            {
                return this.isNetworkingEnabled;
            }

            set
            {
                this.isNetworkingEnabled = value;

                this.ConnectionButtonString = value ? "Disable Remote" : "Enable Remote";

                RaisePropertyChanged("IsNetworkingEnabled");
            }
        }

        private int portNumber = 12000;

        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }

            set
            {
                this.portNumber = value;
                RaisePropertyChanged("PortNumber");
            }
        }
        
        
        private void ToggleNetworking()
        {
            if (this.network == null)
            {
                this.network = new ScoreboardNetwork();
            }

            if (this.network.IsListening)
            {
                this.network.StopListen = true;
                this.IsNetworkingEnabled = false;
            }
            else
            {
                if (this.PortNumber <= 0)
                {
                    MessageBox.Show("Must specify a valid port number.", "Invalid Port", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.network.PortNumber = this.PortNumber;
                this.network.Listen(this);
                this.IsNetworkingEnabled = true;
            }
        }

        private ICommand resetSizeCommand;
        public ICommand ResetSizeCommand
        {
            get
            {
                return this.resetSizeCommand ?? (this.resetSizeCommand = new RelayCommand(this.ResetSize));
            }
        }

        private void ResetSize()
        {
            this.ViewboxWidth = this.desiredWidth;
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

        private void ToggleAnnouncement()
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

        private void SwapPlayers()
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

        private void ToggleSubbar()
        {
            this.Scoreboard.IsSubbarShowing = !this.Scoreboard.IsSubbarShowing;
        }

        private void ToggleScoreboardVisible()
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

        private ICommand resetPositionCommand;
        public ICommand ResetPositionCommand
        {
            get
            {
                return this.resetPositionCommand ?? (this.resetPositionCommand = new RelayCommand(this.ResetPosition));
            }
        }

        private void ResetPosition()
        {
            this.display.ResetPosition();
        }

        public void CloseNetworkConnections()
        {
            if (this.network != null)
            {
                this.network.StopListen = true;
            }
        }

        public void SaveSettings()
        {
            this.settings.Size = this.ViewboxWidth;
            this.settings.Position = new Point(this.display.Left, this.display.Top);
            this.settings.AllowTransparency = this.display.AllowsTransparency;
            this.settings.WindowTransparency = this.TransparencyLevel;
            this.settings.Save();
        }

        private double transparencyLevel;

        public double TransparencyLevel
        {
            get
            {
                return this.transparencyLevel;
            }

            set
            {
                this.transparencyLevel = value;
                this.display.Opacity = value;
                RaisePropertyChanged("TransparencyLevel");
            }
        }

        private bool isWindowMovable;

        public bool IsWindowMovable
        {
            get
            {
                return this.isWindowMovable;
            }

            set
            {
                this.isWindowMovable = value;
                this.display.IsWindowMovable = value;
                RaisePropertyChanged("IsWindowMovable");
            }
        }
        

        private bool allowTransparency;

        public bool AllowTransparency
        {
            get
            {
                return this.allowTransparency;
            }

            set
            {
                this.allowTransparency = value;
                this.SetTransparency(value);
                RaisePropertyChanged("AllowTransparency");
            }
        }

        private void SetTransparency(bool value)
        {
            var opacity = this.display.MaxOpacity;
            var showing = this.display.IsVisible;

            var left = this.display.Left;
            var top = this.display.Top;

            this.display.Close();
            this.display = null;

            this.display = new ScoreboardDisplay { AllowsTransparency = value };
            this.display.SetViewModel(this.Scoreboard);

            this.display.MaxOpacity = opacity;
            this.display.ViewboxWidth = this.ViewboxWidth;

            this.display.IsWindowMovable = this.IsWindowMovable;

            // Retain the previous position settings.
            if (double.IsNaN(left) == false && double.IsNaN(top) == false)
            {
                this.display.InitializePositionOnLoad = false;
                this.display.SetValue(Window.TopProperty, top);
                this.display.SetValue(Window.LeftProperty, left);
            }

            if (showing)
            {
                this.display.Show();
            }
        }

        private ScoreboardControlViewModel scoreboard = new ScoreboardControlViewModel();

        public ScoreboardControlViewModel Scoreboard
        {
            get
            {
                return this.scoreboard;
            }
        }

        private int viewboxWidth;

        public int ViewboxWidth
        {
            get
            {
                return this.viewboxWidth;
            }

            set
            {
                this.viewboxWidth = value;
                this.display.ViewboxWidth = value;
                RaisePropertyChanged("ViewboxWidth");
            }
        }
        

        private int widthMaximum;

        public int WidthMaximum
        {
            get
            {
                return this.widthMaximum;
            }

            set
            {
                this.widthMaximum = value;
                RaisePropertyChanged("WidthMaximum");
            }
        }
        

        private int widthMinimum;

        public int WidthMinimum
        {
            get
            {
                return this.widthMinimum;
            }

            set
            {
                this.widthMinimum = value;
                RaisePropertyChanged("WidthMinimum");
            }
        }
    }
}
