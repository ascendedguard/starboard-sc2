// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsPanelViewModel.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   The view model for the settings panel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.ViewModel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Starboard.Model;
    using Starboard.MVVM;
    using Starboard.Sockets;
    using Starboard.Sockets.Commands;
    using Starboard.View;

    /// <summary>
    /// The settings panel view model.
    /// </summary>
    public class SettingsPanelViewModel : ObservableObject
    {
        #region Constants and Fields

        /// <summary> A reference to the view model for the scoreboard controls. </summary>
        private readonly ScoreboardControlPanelViewModel controlViewModel;

        /// <summary>   The desired width of the viewbox in the display window, based on the screen resolution. </summary>
        private readonly int desiredWidth;

        /// <summary> The application settings. </summary>
        private readonly Settings settings;

        /// <summary> Whether the display show allow transparency. </summary>
        private bool allowTransparency;

        /// <summary> String displayed on the network connect button. </summary>
        private string connectionButtonString = "Enable Remote";

        /// <summary> Whether networking is currently enabled. </summary>
        private bool isNetworkingEnabled;

        /// <summary> Whether the scoreboard can be moved. </summary>
        private bool isWindowMovable;

        /// <summary> The network connection. </summary>
        private ScoreboardNetwork network;

        /// <summary> The port number for the network. </summary>
        private int portNumber = 12000;

        /// <summary> The backing field for the reset position command. </summary>
        private ICommand resetPositionCommand;

        /// <summary> The backing field for the reset size command. </summary>
        private ICommand resetSizeCommand;

        /// <summary> The backing field for the toggle networking command. </summary>
        private ICommand toggleNetworkingCommand;

        /// <summary> The transparency level of the scoreboard. </summary>
        private double transparencyLevel;

        /// <summary> The viewbox width. </summary>
        private int viewboxWidth;

        /// <summary> The maximum width allowed for the viewbox. </summary>
        private int widthMaximum;

        /// <summary> The minimum width allowed for the viewbox. </summary>
        private int widthMinimum;

        /// <summary> Backing property for the SaveImageCommand property. </summary>
        private ICommand saveImageCommand;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref="SettingsPanelViewModel"/> class. </summary>
        /// <param name="settings"> The application settings. </param>
        /// <param name="controlViewModel"> The scoreboard controlpanel view model. </param>
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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a command executed when the Save Image button is clicked.
        /// </summary>
        public ICommand SaveImageCommand
        {
            get
            {
                return this.saveImageCommand ?? (this.saveImageCommand = new RelayCommand(this.SaveImage));
            }
        }

        /// <summary> Gets or sets a value indicating whether transparency is allowed. </summary>
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
                this.RaisePropertyChanged("AllowTransparency");
            }
        }

        /// <summary> Gets the string to be displayed on the connection button. </summary>
        public string ConnectionButtonString
        {
            get
            {
                return this.connectionButtonString;
            }

            private set
            {
                this.connectionButtonString = value;
                this.RaisePropertyChanged("ConnectionButtonString");
            }
        }

        /// <summary> Gets or sets a value indicating whether networking is enabled. </summary>
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

                this.RaisePropertyChanged("IsNetworkingEnabled");
            }
        }

        /// <summary> Gets or sets a value indicating whether the display window can be moved. </summary>
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
                this.RaisePropertyChanged("IsWindowMovable");
            }
        }

        /// <summary> Gets or sets the network port number. </summary>
        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }

            set
            {
                this.portNumber = value;
                this.RaisePropertyChanged("PortNumber");
            }
        }

        /// <summary> Gets a command to reset the scoreboard position to it's default. </summary>
        public ICommand ResetPositionCommand
        {
            get
            {
                return this.resetPositionCommand ?? (this.resetPositionCommand = new RelayCommand(ResetPosition));
            }
        }

        /// <summary> Gets a command to reset the scoreboard size. </summary>
        public ICommand ResetSizeCommand
        {
            get
            {
                return this.resetSizeCommand ?? (this.resetSizeCommand = new RelayCommand(this.ResetSize));
            }
        }

        /// <summary> Gets a command to toggle whether networking is enabled. </summary>
        public ICommand ToggleNetworkingCommand
        {
            get
            {
                return this.toggleNetworkingCommand
                       ?? (this.toggleNetworkingCommand = new RelayCommand(this.ToggleNetworking));
            }
        }

        /// <summary> Gets or sets the display's transparency level. </summary>
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
                this.RaisePropertyChanged("TransparencyLevel");
            }
        }

        /// <summary> Gets or sets the width of the display viewbox. </summary>
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
                this.RaisePropertyChanged("ViewboxWidth");
            }
        }

        /// <summary> Gets or sets the maximum width of the viewbox. </summary>
        public int WidthMaximum
        {
            get
            {
                return this.widthMaximum;
            }

            set
            {
                this.widthMaximum = value;
                this.RaisePropertyChanged("WidthMaximum");
            }
        }

        /// <summary> Gets or sets the minimum width of the viewbox. </summary>
        public int WidthMinimum
        {
            get
            {
                return this.widthMinimum;
            }

            set
            {
                this.widthMinimum = value;
                this.RaisePropertyChanged("WidthMinimum");
            }
        }

        #endregion

        #region Public Methods

        /// <summary> Closes the active network connection. </summary>
        public void CloseNetworkConnections()
        {
            if (this.network != null)
            {
                this.network.StopListen = true;
            }
        }

        /// <summary> Handles all incoming network packets. </summary>
        /// <param name="packet"> The packet. </param>
        /// <param name="socket"> The active socket. </param>
        /// <param name="senderRemote"> The sender where the packet originated. </param>
        /// <exception cref="System.Net.Sockets.SocketException"> Thrown if a response failed to send. </exception>
        public void HandleRemoteCommands(ScoreboardPacket packet, Socket socket, EndPoint senderRemote)
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

                    case CommandType.IncrementPlayerScore:
                        {
                            var cmd = command as EmptyPacketCommand;

                            if (cmd != null)
                            {
                                if (cmd.Player == 1)
                                {
                                    this.controlViewModel.Scoreboard.Player1.Score++;
                                }
                                else if (cmd.Player == 2)
                                {
                                    this.controlViewModel.Scoreboard.Player2.Score++;
                                }
                            }

                            break;
                        }

                    case CommandType.RetrievePlayerInformation:
                        {
                            var cmd = command as EmptyPacketCommand;

                            if (cmd != null)
                            {
                                var commands = new List<IScoreboardPacketCommand>();
                                var player = cmd.Player == 1
                                             ? this.controlViewModel.Scoreboard.Player1
                                             : this.controlViewModel.Scoreboard.Player2;

                                commands.Add(
                                    new StringPacketCommand
                                        {
                                            Command = CommandType.UpdatePlayerName, 
                                            Player = cmd.Player, 
                                            Data = player.Name
                                        });

                                commands.Add(
                                    new BytePacketCommand
                                        {
                                            Command = CommandType.UpdatePlayerRace, 
                                            Data = (byte)player.Race, 
                                            Player = cmd.Player
                                        });

                                commands.Add(
                                    new BytePacketCommand
                                        {
                                            Command = CommandType.UpdatePlayerColor, 
                                            Data = (byte)player.Color, 
                                            Player = cmd.Player
                                        });

                                commands.Add(
                                    new Int32PacketCommand 
                                        {
                                            Command = CommandType.UpdatePlayerScore, 
                                            Data = player.Score, 
                                            Player = cmd.Player
                                        });

                                var p = new ScoreboardPacket { Commands = commands.ToArray() };

                                socket.SendTo(p.ToBytes(), senderRemote);
                            }

                            break;
                        }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary> Resets the positon of the display window. </summary>
        private static void ResetPosition()
        {
            MainWindowViewModel.DisplayWindow.ResetPosition();
        }

        /// <summary> Saves the rendered scoreboard to an image when clicked. </summary>
        private void SaveImage()
        {
            if (MainWindowViewModel.DisplayWindow.IsVisible == false)
            {
                MessageBox.Show(
                    "Scoreboard must be showing on-screen in order to save to an image.",
                    "Error: Scoreboard Hidden",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            const string Filter = "PNG Image (*.png)|*.png|" +
                                  "TIFF Image (*.tiff)|*.tiff|" +
                                  "All Files (*.*)|*.*";

            var sfd = new Microsoft.Win32.SaveFileDialog { Filter = Filter };

            if (sfd.ShowDialog() == true)
            {
                var filename = sfd.FileName;

                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                var viewBox = MainWindowViewModel.DisplayWindow.rootGrid;

                var bmp = new RenderTargetBitmap(
                    (int)viewBox.RenderSize.Width, (int)viewBox.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(viewBox);

                BitmapEncoder encoder;

                var extUpper = Path.GetExtension(filename);

                if (extUpper == null)
                {
                    // Could happen if they choose all files and didn't type an extension.
                    MessageBox.Show(
                        "Unrecognized Image Format",
                        "Error: Format Unrecognized",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                string ext = extUpper.ToLower();

                if (ext.Equals(".png"))
                {
                    encoder = new PngBitmapEncoder();
                }
                else if (ext.Equals(".jpg"))
                {
                    encoder = new JpegBitmapEncoder { QualityLevel = 95 };
                }
                else if (ext.Equals(".tiff"))
                {
                    encoder = new TiffBitmapEncoder();
                }
                else if (ext.Equals(".gif"))
                {
                    encoder = new GifBitmapEncoder();
                }
                else if (ext.Equals(".bmp"))
                {
                    encoder = new BmpBitmapEncoder();
                }
                else
                {
                    MessageBox.Show(
                        "Unrecognized Image Format",
                        "Error: Format Unrecognized",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                using (var stream = new FileStream(filename, FileMode.Create))
                {
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    encoder.Save(stream);
                }
            }
        }

        /// <summary> Resets the size of the viewbox to default. </summary>
        private void ResetSize()
        {
            this.ViewboxWidth = this.desiredWidth;
        }

        /// <summary> Sets whether transparency is allowed. </summary>
        /// <param name="value"> Indicates whether transparency is allowed. </param>
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

        /// <summary> Toggles whether networking is currently enabled. </summary>
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

        #endregion
    }
}