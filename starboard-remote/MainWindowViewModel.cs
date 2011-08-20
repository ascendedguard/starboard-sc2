// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   ViewModel for the main application window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Remote
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Windows;
    using System.Windows.Input;

    using Starboard.Model;
    using Starboard.MVVM;
    using Starboard.Sockets;
    using Starboard.Sockets.Commands;

    /// <summary>
    /// ViewModel for the main application window.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        private PropertyObserver<Player> player1Observer;
        private PropertyObserver<Player> player2Observer;

        public MainWindowViewModel()
        {
            this.IsConnected = false;
        
            this.player1Observer =
                new PropertyObserver<Player>(this.Player1)
                    .RegisterHandler(n => n.Name, this.Player1NameChanged)
                    .RegisterHandler(n => n.Color, this.Player1ColorChanged)
                    .RegisterHandler(n => n.Race, this.Player1RaceChanged)
                    .RegisterHandler(n => n.Score, this.Player1ScoreChanged);

            this.player2Observer =
                new PropertyObserver<Player>(this.Player2)
                    .RegisterHandler(n => n.Name, this.Player2NameChanged)
                    .RegisterHandler(n => n.Color, this.Player2ColorChanged)
                    .RegisterHandler(n => n.Race, this.Player2RaceChanged)
                    .RegisterHandler(n => n.Score, this.Player2ScoreChanged);

        }

        private ICommand connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return this.connectCommand ?? (this.connectCommand = new RelayCommand(this.Connect));
            }
        }

        public string ConnectionString { get; private set; }
        public bool AreConnectionFieldsEnabled { get; private set; }

        private bool isConnected;

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }

            set
            {
                this.isConnected = value;

                this.ConnectionString = this.IsConnected ? "Disconnect" : "Connect";
                this.AreConnectionFieldsEnabled = !value;

                RaisePropertyChanged("AreConnectionFieldsEnabled");
                RaisePropertyChanged("ConnectionString");
                RaisePropertyChanged("IsConnected");
            }
        }

        private ScoreboardUpdater scoreboardUpdater;

        private void Connect()
        {
            if (this.IsConnected)
            {
                this.scoreboardUpdater = null;
                this.IsConnected = false;
                return;
            }

            var updater = this.GetUpdater();

            this.IsConnected = updater.Ping();

            if (this.IsConnected == false)
            {
                MessageBox.Show(
                    "Failed to connect to remote scoreboard.",
                    "Connection failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                this.scoreboardUpdater = updater;
            }
        }

        private void Player1ScoreChanged(Player obj)
        {  
            var commands = new List<IScoreboardPacketCommand>();

            commands.Add(new Int32PacketCommand { Command = CommandType.UpdatePlayerScore, Player = 1, Data = Player1.Score });

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player1RaceChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>
                {
                    new BytePacketCommand
                        {
                            Command = CommandType.UpdatePlayerRace, Player = 1, Data = (byte)this.Player1.Race 
                        }
                };

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player1ColorChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>
                {
                    new BytePacketCommand
                        {
                            Command = CommandType.UpdatePlayerColor, Player = 1, Data = (byte)this.Player1.Color 
                        }
                };

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player1NameChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>
                {
                    new StringPacketCommand
                        {
                            Command = CommandType.UpdatePlayerName, Player = 1, Data = this.Player1.Name 
                        }
                };

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player2ScoreChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>
                {
                    new Int32PacketCommand
                        {
                            Command = CommandType.UpdatePlayerScore, Player = 2, Data = this.Player2.Score 
                        }
                };

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player2RaceChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>();

            commands.Add(new BytePacketCommand { Command = CommandType.UpdatePlayerRace, Player = 2, Data = (byte)Player2.Race });

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player2ColorChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>();

            commands.Add(new BytePacketCommand { Command = CommandType.UpdatePlayerColor, Player = 2, Data = (byte)Player2.Color });

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private void Player2NameChanged(Player obj)
        {
            var commands = new List<IScoreboardPacketCommand>();

            commands.Add(new StringPacketCommand { Command = CommandType.UpdatePlayerName, Player = 2, Data = Player2.Name });

            var packet = new ScoreboardPacket { Commands = commands.ToArray() };

            this.scoreboardUpdater.SendPacket(packet);
        }

        private ScoreboardUpdater GetUpdater()
        {
            var nums = this.IPAddressText.Split('.');

            var ip = new byte[4];

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    ip[i] = byte.Parse(nums[i]);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "An invalid IP address was entered. Fields can only contain numbers.",
                    "Error: IP Address",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show(
                    "An invalid IP address was entered. Values must be between 0 and 255.",
                    "Error: IP Address",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            var endPoint = new IPEndPoint(new IPAddress(ip), this.Port);

            return new ScoreboardUpdater { EndPoint = endPoint };
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
                RaisePropertyChanged("MyProperty");
            }
        }
        

        private int port = 12000;

        public int Port
        {
            get
            {
                return this.port;
            }

            set
            {
                this.port = value;
                RaisePropertyChanged("Port");
            }
        }
        

        private string ipAddressText;

        public string IPAddressText
        {
            get
            {
                return this.ipAddressText;
            }

            set
            {
                this.ipAddressText = value;
                RaisePropertyChanged("IPAddressText");
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

        private void ToggleAnnouncement()
        {
            var packet = new ScoreboardPacket { Commands = new[] { new EmptyPacketCommand { Command = CommandType.ToggleAnnouncement } } };
            this.scoreboardUpdater.SendPacket(packet);
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
            var packet = new ScoreboardPacket { Commands = new[] { new EmptyPacketCommand { Command = CommandType.SwapPlayer } } };

            this.scoreboardUpdater.SendPacket(packet);

        }

        private void ToggleSubbar()
        {
            var packet = new ScoreboardPacket { Commands = new[] { new EmptyPacketCommand { Command = CommandType.ToggleSubbar } } };

            this.scoreboardUpdater.SendPacket(packet);

        }

        private void ToggleScoreboardVisible()
        {
            var packet = new ScoreboardPacket { Commands = new[] { new EmptyPacketCommand { Command = CommandType.ShowScoreboard } } };

            this.scoreboardUpdater.SendPacket(packet);

        }

        private Player player1 = new Player();

        public Player Player1
        {
            get
            {
                return this.player1;
            }
        }
                
        private Player player2 = new Player();

        public Player Player2
        {
            get
            {
                return this.player2;
            }
        }
    }
}
