namespace Starboard.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using Starboard.Model;
    using Starboard.MVVM;
    using Starboard.Sockets;
    using Starboard.Sockets.Commands;
    using Starboard.View;

    public class SettingsPanelViewModel : ObservableObject
    {
        private Settings settings;

        private ScoreboardControlPanelViewModel controlViewModel;

        /// <summary> The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        public SettingsPanelViewModel(Settings settings, ScoreboardControlPanelViewModel controlViewModel)
        {
            this.controlViewModel = controlViewModel;
            this.settings = settings;

            this.desiredWidth = this.settings.Size;

            this.WidthMinimum = (int)(SystemParameters.PrimaryScreenWidth * .10);
            this.WidthMaximum = (int)(SystemParameters.PrimaryScreenWidth * .60);
            this.ViewboxWidth = this.desiredWidth;

            this.AllowTransparency = this.settings.AllowTransparency;
            this.TransparencyLevel = this.settings.WindowTransparency;
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
                MainWindowViewModel.DisplayWindow.Opacity = value;
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
                MainWindowViewModel.DisplayWindow.IsWindowMovable = value;
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
            var opacity = MainWindowViewModel.DisplayWindow.MaxOpacity;
            var showing = MainWindowViewModel.DisplayWindow.IsVisible;

            var left = MainWindowViewModel.DisplayWindow.Left;
            var top = MainWindowViewModel.DisplayWindow.Top;

            MainWindowViewModel.DisplayWindow.Close();
            MainWindowViewModel.DisplayWindow = null;

            MainWindowViewModel.DisplayWindow = new ScoreboardDisplay { AllowsTransparency = value };
            MainWindowViewModel.DisplayWindow.SetViewModel(this.controlViewModel.Scoreboard);

            MainWindowViewModel.DisplayWindow.MaxOpacity = opacity;
            MainWindowViewModel.DisplayWindow.ViewboxWidth = this.ViewboxWidth;

            MainWindowViewModel.DisplayWindow.IsWindowMovable = this.IsWindowMovable;

            // Retain the previous position settings.
            if (double.IsNaN(left) == false && double.IsNaN(top) == false)
            {
                MainWindowViewModel.DisplayWindow.InitializePositionOnLoad = false;
                MainWindowViewModel.DisplayWindow.SetValue(Window.TopProperty, top);
                MainWindowViewModel.DisplayWindow.SetValue(Window.LeftProperty, left);
            }

            if (showing)
            {
                MainWindowViewModel.DisplayWindow.Show();
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
                MainWindowViewModel.DisplayWindow.ViewboxWidth = value;
                RaisePropertyChanged("ViewboxWidth");
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

        public void CloseNetworkConnections()
        {
            if (this.network != null)
            {
                this.network.StopListen = true;
            }
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
                                    this.controlViewModel.Scoreboard.Player1.Name = cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Name = cmd.Data;
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
                                    this.controlViewModel.Scoreboard.Player1.Score = cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Score = cmd.Data;
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
                                    this.controlViewModel.Scoreboard.Player1.Color = (PlayerColor)cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Color = (PlayerColor)cmd.Data;
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
                                    this.controlViewModel.Scoreboard.Player1.Race = (Race)cmd.Data;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Race = (Race)cmd.Data;
                                }
                            }

                            break;
                        }

                    case CommandType.ToggleAnnouncement:
                        this.controlViewModel.ToggleAnnouncement();
                        break;

                    case CommandType.ToggleSubbar:
                        this.controlViewModel.ToggleSubbar();
                        break;

                    case CommandType.SwapPlayer:
                        this.controlViewModel.SwapPlayers();
                        break;

                    case CommandType.ShowScoreboard:
                        this.controlViewModel.ToggleScoreboardVisible();
                        break;

                    case CommandType.HideScoreboard:
                        this.controlViewModel.ToggleScoreboardVisible();
                        break;

                    case CommandType.ResetPlayer:
                        {
                            var cmd = command as EmptyPacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.controlViewModel.Scoreboard.Player1.Reset();
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Reset();
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

        private ScoreboardNetwork network;

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
            MainWindowViewModel.DisplayWindow.ResetPosition();
        }
    }
}
