// -----------------------------------------------------------------------
// <copyright file="ScoreboardNetwork.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    using Starboard.Sockets.Commands;
    using Starboard.ViewModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreboardNetwork
    {
        private Dispatcher dispatcher;

        public ScoreboardNetwork()
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        private bool stopListen = true;
        public bool StopListen
        {
            get
            {
                return this.stopListen;
            }

            set
            {
                this.stopListen = value;

                if (this.stopListen)
                {
                    this.IsListening = false;
                }
            }
        }

        public int PortNumber { get; set; }

        private Thread listenThread;

        public bool IsListening { get; private set; }

        public void Listen(SettingsPanelViewModel sb)
        {
            if (this.listenThread != null)
            {
                if (this.listenThread.IsAlive)
                {
                    this.StopListen = true;

                    while (this.listenThread.IsAlive)
                    {
                        Thread.Sleep(20);
                    }
                }
            }

            this.StopListen = false;

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            var endPoints = new List<IPEndPoint>();

            foreach (var entry in hostEntry.AddressList)
            {
                if (entry.AddressFamily == AddressFamily.InterNetwork)
                {
                    endPoints.Add(new IPEndPoint(entry, this.PortNumber));
                    break;
                }
            }

            if (endPoints.Count == 0)
            {
                MessageBox.Show("No valid networks were found to connect to.");
                return;
            }

            var socket = new Socket(endPoints[0].Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp) { ReceiveTimeout = 5000 };
            foreach (var e in endPoints)
            {
                socket.Bind(e);
            }

            this.listenThread = new Thread(
                new ThreadStart(
                    delegate
                    {
                        while (this.StopListen == false)
                        {
                            var inBuffer = new byte[1000];
                            EndPoint senderRemote = new IPEndPoint(IPAddress.Any, 0);

                            try
                            {
                                socket.ReceiveFrom(inBuffer, ref senderRemote);
                            }
                            catch (SocketException ex)
                            {
                                int errorCode = ex.ErrorCode;
                                continue;
                            }

                            var packet = ScoreboardPacket.Parse(inBuffer);

                            foreach (var cmd in packet.Commands)
                            {
                                if (cmd.Command == CommandType.Ping)
                                {
                                    var pongCmd = new EmptyPacketCommand { Command = CommandType.Pong };
                                    var pongPacket = new ScoreboardPacket() { Commands = new[] { pongCmd } };
                                    
                                    socket.SendTo(pongPacket.ToBytes(), senderRemote);
                                }
                            }

                            this.dispatcher.BeginInvoke((Action)(() => sb.HandleRemoteCommands(packet, socket, senderRemote)));
                        }

                        socket.Close();
                        socket.Dispose();
                    }));

            this.listenThread.Start();
            this.IsListening = true;
        }
    }
}
